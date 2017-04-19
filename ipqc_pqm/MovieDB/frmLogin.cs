using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb; 
using System.Security.Permissions;

namespace IpqcDB
{
    public partial class frmLogin : Form
    {
        // コンストラクタ
        public frmLogin()
        {
            InitializeComponent();
        }

        // ロード時の処理（コンボボックスに、オートコンプリート機能の追加）
        private void Form5_Load(object sender, EventArgs e)
        {
            string sql = "select DISTINCT qcuser FROM qc_user ORDER BY qcuser";
            TfSQL tf = new TfSQL();
            tf.getComboBoxData(sql, ref cmbUserName);
        }

        // ユーザーログイン時、パスワードとログイン状態の確認（２重ログインの防止）
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            string sql = null;
            string user = null;
            string pass = null;
            bool login = false;

            user = cmbUserName.Text;

            if (user != null) 
            {
                TfSQL tf = new TfSQL();

                sql = "select pass FROM qc_user WHERE qcuser='" + user + "'";
                pass = tf.sqlExecuteScalarString(sql);

                sql = "select loginstatus FROM qc_user WHERE qcuser='" + user + "'";
                login = tf.sqlExecuteScalarBool(sql); 

                if (pass == txtPassword.Text)
                {
                    if (login) 
                    { 
                        DialogResult reply = MessageBox.Show("This user account is currently used by other user," + System.Environment.NewLine +
                            "or the log out last time had a problem." + System.Environment.NewLine + "Do you log in with this account ?", 
                            "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (reply == DialogResult.No)
                        {
                            return;
                        }
                    }

                    // ログイン状態をＴＲＵＥへ変更
                    sql = "UPDATE qc_user SET loginstatus=true WHERE qcuser='" + user + "'";
                    bool res = tf.sqlExecuteNonQuery(sql, false);

                    // 子フォームForm1を表示し、デレゲートイベントを追加： 
                    frmItem f1 = new frmItem();
                    f1.RefreshEvent += delegate(object sndr, EventArgs excp)
                    {
                        // Form1を閉じる際、ログイン状態をＦＡＬＳＥへ変更し、当フォームForm5も閉じる
                        sql = "UPDATE qc_user SET loginstatus=false WHERE qcuser='" + user + "'";
                        res = tf.sqlExecuteNonQuery(sql, false);
                        this.Close();
                    };
                    f1.updateControls(user);
                    f1.Show();
                    this.Visible = false;
                }
                else if(pass != txtPassword.Text)
                {
                    MessageBox.Show("Password does not match", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // ログインボタンの押下に加え、エンターキーでもログインできる
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string serialNo = txtPassword.Text;
                if (serialNo != String.Empty)
                {
                    btnLogIn_Click(sender, e);
                }
            }
        }
    }
}



