using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;
using Npgsql;

namespace IpqcDB
{
    public class TfSqlPqm
    {
        NpgsqlConnection connection;
        string conStringPqmDb = string.Empty;
        static string conStringPqmDbP2 = @"Server=192.168.193.2;Port=5432;User Id=pqm;Password=dbuser;Database=pqmdb; CommandTimeout=100; Timeout=100;";
        static string conStringPqmDbP4 = @"Server=192.168.193.4;Port=5432;User Id=pqm;Password=dbuser;Database=pqmdb; CommandTimeout=100; Timeout=100;";

        // ＰＱＭテーブルへの一括登録
        public bool sqlMultipleInsertMeasurementToPqmTable(string model, string process, string inspect, 
            DateTime lot, DateTime inspectdate, string line, DataTable dt, double upper, double lower)
        {
            int res1;
            bool res2 = false;

            conStringPqmDb = decideConnectionString(model);
            connection = new NpgsqlConnection(conStringPqmDb);
            connection.Open();
            NpgsqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                string serno = string.Empty;
                string site = "NCVC";
                string factory = "QA";
                string tjudge = "0";
                string tstatus = "IPQC";
                double inspectdata = 0; //= double.Parse(dt.Rows[0]["m1"].ToString());
                string judge = "0";
                string pqmTable1 = decideReferenceTable(model, lot);
                string pqmTable2 = pqmTable1 + "data"; 

                // ＤＢテーブルに既に存在するデータを削除し、登録。ただし例外発生時、削除も登録もロールバック。
                // 削除①：
                string sql1 = "delete from " + pqmTable1 + " where " + 
                    "model ='" + model + "' and " +
                    "process ='" + process + "' and " +
                    "lot ='" + lot + "' and " +
                    "inspectdate ='" + inspectdate + "' and " +
                    "line ='" + line + "'";
                System.Diagnostics.Debug.Print(sql1);
                NpgsqlCommand command1 = new NpgsqlCommand(sql1, connection);
                command1.ExecuteNonQuery();

                // 削除②：
                string sql2 = "delete from " + pqmTable2 + " where " +
                    "inspect ='" + inspect + "' and " +
                    "lot ='" + lot + "' and " +
                    "inspectdate ='" + inspectdate + "'";
                System.Diagnostics.Debug.Print(sql2);
                NpgsqlCommand command2 = new NpgsqlCommand(sql2, connection);
                command2.ExecuteNonQuery();

                // 登録①：
                string sql3 = "INSERT INTO " + pqmTable1 + " (serno, lot, model, site, factory, line, process, inspectdate, tjudge, tstatus) " +
                    "VALUES (:serno, :lot, :model, :site, :factory, :line, :process, :inspectdate, :tjudge, :tstatus)";
                NpgsqlCommand command3 = new NpgsqlCommand(sql3, connection);

                command3.Parameters.Add(new NpgsqlParameter("serno", NpgsqlTypes.NpgsqlDbType.Varchar));
                command3.Parameters.Add(new NpgsqlParameter("lot", NpgsqlTypes.NpgsqlDbType.Varchar));
                command3.Parameters.Add(new NpgsqlParameter("model", NpgsqlTypes.NpgsqlDbType.Varchar));
                command3.Parameters.Add(new NpgsqlParameter("site", NpgsqlTypes.NpgsqlDbType.Varchar));
                command3.Parameters.Add(new NpgsqlParameter("factory", NpgsqlTypes.NpgsqlDbType.Varchar));
                command3.Parameters.Add(new NpgsqlParameter("line", NpgsqlTypes.NpgsqlDbType.Varchar));
                command3.Parameters.Add(new NpgsqlParameter("process", NpgsqlTypes.NpgsqlDbType.Varchar));
                command3.Parameters.Add(new NpgsqlParameter("inspectdate", NpgsqlTypes.NpgsqlDbType.TimestampTZ));
                command3.Parameters.Add(new NpgsqlParameter("tjudge", NpgsqlTypes.NpgsqlDbType.Varchar));
                command3.Parameters.Add(new NpgsqlParameter("tstatus", NpgsqlTypes.NpgsqlDbType.Varchar));

                string[] fldAry = { "m1", "m2", "m3", "m4", "m5" };
                int k = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // 天秤用、２つの値の差異のみ登録する
                    string qcuser = dt.Rows[i]["qc_user"].ToString();
                    if (qcuser == "1. Upper" || qcuser == "2. Lower") continue;

                    for (int j = 0; j < 5; j++)
                    {
                        object value = dt.Rows[i][(fldAry[j])];
                        if (value == DBNull.Value) continue;

                        inspectdata = (double)value;
                        if (inspectdata >= lower && inspectdata <= upper) 
                            tjudge = "0";
                        else  
                            tjudge = "1";

                        command3.Parameters[0].Value = k.ToString();
                        command3.Parameters[1].Value = lot;
                        command3.Parameters[2].Value = model;
                        command3.Parameters[3].Value = site;
                        command3.Parameters[4].Value = factory;
                        command3.Parameters[5].Value = line;
                        command3.Parameters[6].Value = process;
                        command3.Parameters[7].Value = inspectdate;
                        command3.Parameters[8].Value = tjudge;
                        command3.Parameters[9].Value = tstatus;
                        k++;

                        res1 = command3.ExecuteNonQuery();
                        if (res1 == -1) res2 = true;
                    }
                }

                // 登録②：
                string sql4 = "INSERT INTO " + pqmTable2 + " (serno, lot, inspectdate, inspect, inspectdata, judge) " +
                    "VALUES (:serno, :lot, :inspectdate, :inspect, :inspectdata, :judge)";
                NpgsqlCommand command4 = new NpgsqlCommand(sql4, connection);

                command4.Parameters.Add(new NpgsqlParameter("serno", NpgsqlTypes.NpgsqlDbType.Varchar));
                command4.Parameters.Add(new NpgsqlParameter("lot", NpgsqlTypes.NpgsqlDbType.Varchar));
                command4.Parameters.Add(new NpgsqlParameter("inspectdate", NpgsqlTypes.NpgsqlDbType.TimestampTZ));
                command4.Parameters.Add(new NpgsqlParameter("inspect", NpgsqlTypes.NpgsqlDbType.Varchar));
                command4.Parameters.Add(new NpgsqlParameter("inspectdata", NpgsqlTypes.NpgsqlDbType.Double));
                command4.Parameters.Add(new NpgsqlParameter("judge", NpgsqlTypes.NpgsqlDbType.Varchar));

                k = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // 天秤用、２つの値の差異のみ登録する
                    string qcuser = dt.Rows[i]["qc_user"].ToString();
                    if (qcuser == "1. Upper" || qcuser == "2. Lower") continue;

                    for (int j = 0; j < 5; j++)
                    {
                        object value = dt.Rows[i][(fldAry[j])];
                        if (value == DBNull.Value) continue;

                        inspectdata = (double)value;
                        if (inspectdata >= lower && inspectdata <= upper)
                            judge = "0";
                        else
                            judge = "1";

                        command4.Parameters[0].Value = k.ToString();
                        command4.Parameters[1].Value = lot;
                        command4.Parameters[2].Value = inspectdate;
                        command4.Parameters[3].Value = inspect;
                        command4.Parameters[4].Value = inspectdata;
                        command4.Parameters[5].Value = judge;
                        k++;

                        res1 = command4.ExecuteNonQuery();
                        if (res1 == -1) res2 = true;
                    }
                }

                if (!res2)
                {
                    transaction.Commit();
                    connection.Close();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    MessageBox.Show("Not successful!", "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    connection.Close();
                    transaction.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show("Not successful!" + System.Environment.NewLine + ex.Message
                                , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return false;
            }
        }

        // ＰＱＭテーブルの一括削除
        public void sqlDeleteFromPqmTable(string model, string process, string inspect,
            DateTime lot, DateTime inspectdate, string line)
        {
            conStringPqmDb = decideConnectionString(model);
            connection = new NpgsqlConnection(conStringPqmDb);
            connection.Open();
            NpgsqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                string pqmTable1 = decideReferenceTable(model, lot);
                string pqmTable2 = pqmTable1 + "data";

                // ＤＢテーブルに既に存在するデータを削除し、登録。ただし例外発生時、削除も登録もロールバック。
                // 削除①：
                string sql1 = "delete from " + pqmTable1 + " where " +
                    "model ='" + model + "' and " +
                    "process ='" + process + "' and " +
                    "lot ='" + lot + "' and " +
                    "inspectdate ='" + inspectdate + "' and " +
                    "line ='" + line + "'";
                System.Diagnostics.Debug.Print(sql1);
                NpgsqlCommand command1 = new NpgsqlCommand(sql1, connection);
                command1.ExecuteNonQuery();

                // 削除②：
                string sql2 = "delete from " + pqmTable2 + " where " +
                    "inspect ='" + inspect + "' and " +
                    "lot ='" + lot + "' and " +
                    "inspectdate ='" + inspectdate + "'";
                System.Diagnostics.Debug.Print(sql2);
                NpgsqlCommand command2 = new NpgsqlCommand(sql2, connection);
                command2.ExecuteNonQuery();

                transaction.Commit();
                connection.Close();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show("Not successful!" + System.Environment.NewLine + ex.Message
                                , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
            }
        }

        // サブプロシージャ：参照すべきＰＱＭテーブル名を特定する
        private string decideReferenceTable(string model, DateTime month)
        {
            string tablekey = model.ToLower();            
            string pqmTable = tablekey + month.ToString("yyyyMM");
            return pqmTable;
        }

        // サブプロシージャ：参照すべき接続文字列を　ＰＱＭテーブル名を特定する
        private string decideConnectionString(string model)
        {
            string sql = "select dbplace from tbl_model_dbplace where model='" + model + "'";
            System.Diagnostics.Debug.Print(sql);
            TfSQL tf = new TfSQL();
            string dbplace = tf.sqlExecuteScalarString(sql);

            if (dbplace == "HW2") 
                conStringPqmDb = conStringPqmDbP2;
            else if (dbplace == "CAR") 
                conStringPqmDb = conStringPqmDbP4;

            return conStringPqmDb;
        }
    }
}
