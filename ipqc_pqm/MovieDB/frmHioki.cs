using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Security.Permissions;
using System.Collections;
using Npgsql;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks; 

namespace IpqcDB
{
    public partial class frmHioki : Form
    {
        //�e�t�H�[��Form1�փC�x���g������A������A�f���Q�[�g�ϐ�
        public delegate void RefreshEventHandler(object sender, EventArgs e);
        public event RefreshEventHandler RefreshEvent;

        // �f�[�^��M�C�x���g�p�A�f���Q�[�g�ϐ�
        private delegate void Delegate_RcvDataToBufferDataTable(string data);

        //�f�[�^�O���b�h�r���[�{�^���̈ʒu�ƃ}�b�`����f�[�^�e�[�u���̃A�h���X�i�u�c�A�g���j
        int vAdr;
        int hAdr;

        //�f�[�^�O���b�h�r���[�p�{�^��
        DataGridViewButtonColumn Open;

        //���̑��A�񃍁[�J���ϐ�
        string command;
        double upp;
        double low;
        bool editMode;
        DataTable dtBuffer;
        DataTable dtHistory;
        DataTable dtUpLowIns;
        int clmSet = 5;
        int rowSet = 1;

        // �R���X�g���N�^
        public frmHioki()
        {
            InitializeComponent();
        }

        // ���[�h���̏���
        private void Form3_Load(object sender, EventArgs e)
        {
            // ���t�H�[���̕\���ꏊ���w��
            this.Left = 300;
            this.Top = 15;

            // �c�`�s�d�s�h�l�d�o�h�b�j�d�q���P�O���O�̓��t�ɂ���
            dtpSet10daysBefore(dtpLotFrom);

            // �c�`�s�d�s�h�l�d�o�h�b�j�d�q�̕��ȉ���؂�グ��
            dtpRoundUpHour(dtpLotTo);

            // �c�`�s�d�s�h�l�d�o�h�b�j�d�q�̕��ȉ���������
            dtpRounddownHour(dtpLotInput);

            // ���m���ꂽ�P�ڂ̃V���A���|�[�g��I�����A�I�[�v������
            initializePort();

            // �e�폈���p�̃e�[�u���𐶐����ăf�[�^��ǂݍ���
            dtBuffer = new DataTable();
            defineBufferAndHistoryTable(ref dtBuffer);
            dtHistory = new DataTable();
            defineBufferAndHistoryTable(ref dtHistory);
            readDtHistory(ref dtHistory);
            dtUpLowIns = new DataTable();
            setLimitSetAndCommand(ref dtUpLowIns);

            // �O���b�g�r���[�̍X�V
            updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);

            // �O���b�g�r���[�E�[�Ƀ{�^����ǉ��i����̂݁j
            addButtonsToDataGridView(dgvHistory);
        }

        // �T�u�v���V�[�W���F�e�t�H�[���ŌĂяo���A�e�t�H�[���̏����A�e�L�X�g�{�b�N�X�֊i�[���Ĉ����p��
        //�i���[�h�P�F�{���E�ҏW�A���[�h�Q�F�V�K�o�^�j
        public void updateControls(string model, string process, string inspect, string line, string user)
        {
            txtModel.Text = model;
            txtProcess.Text = process;
            txtInspect.Text = inspect;
            txtLine.Text = line;
            txtUser.Text = user;
        }

        // �T�u�v���V�[�W���F�c�a����̂c�s�g�h�r�s�n�q�x�ւ̓ǂݍ���
        private void readDtHistory(ref DataTable dt)
        {
            dt.Clear();

            string model = txtModel.Text;
            string process = txtProcess.Text;
            string inspect = txtInspect.Text;
            DateTime lotFrom = dtpLotFrom.Value;
            DateTime lotTo = dtpLotTo.Value;
            string line = txtLine.Text;

            string sql = "select inspect, lot, inspectdate, line, qc_user, " +
                                "m1, m2, m3, m4, m5, x, r FROM tbl_measure_history " + 
                         "WHERE model = '" + model + "' AND " +
                                "process = '" + process + "' AND " +
                                "inspect = '" + inspect + "' AND " +
                                "lot >= '" + lotFrom.ToString() + "' AND " +
                                "lot <= '" + lotTo.ToString() + "' AND " +
                                "line = '" + line + "' " +
                         "order by lot, inspectdate";

            System.Diagnostics.Debug.Print(sql);
            TfSQL tf = new TfSQL();
            tf.sqlDataAdapterFillDatatable(sql, ref dt);
        }

        // �T�u�v���V�[�W���F�����e�[�u���̒�`
        private void defineBufferAndHistoryTable(ref DataTable dt)
        {
            dt.Columns.Add("inspect", Type.GetType("System.String"));
            dt.Columns.Add("lot", Type.GetType("System.DateTime"));
            dt.Columns.Add("inspectdate", Type.GetType("System.DateTime"));
            dt.Columns.Add("line", Type.GetType("System.String"));
            dt.Columns.Add("qc_user", Type.GetType("System.String"));
            dt.Columns.Add("m1", Type.GetType("System.Double"));
            dt.Columns.Add("m2", Type.GetType("System.Double"));
            dt.Columns.Add("m3", Type.GetType("System.Double"));
            dt.Columns.Add("m4", Type.GetType("System.Double"));
            dt.Columns.Add("m5", Type.GetType("System.Double"));
            dt.Columns.Add("x", Type.GetType("System.Double"));
            dt.Columns.Add("r", Type.GetType("System.Double"));
        }

        // �T�u�v���V�[�W���F����E�����A�s�Z�b�g�E��Z�b�g�A�R�}���h�A�̐ݒ�
        private void setLimitSetAndCommand(ref DataTable dt)
        {
            dt.Clear();
            string sql = "select upper, lower, clm_set, row_set, instrument from tbl_measure_item_2 " + 
                "where model = '" + txtModel.Text + "' and " +
                      "inspect = '" + txtInspect.Text + "'";
            System.Diagnostics.Debug.Print(sql);
            TfSQL tf = new TfSQL();
            tf.sqlDataAdapterFillDatatable(sql, ref dt);

            upp = (double)dt.Rows[0]["upper"];
            txtUsl.Text = upp.ToString();
            low = (double)dt.Rows[0]["lower"];
            txtLsl.Text = low.ToString();
            rowSet = (int)dt.Rows[0]["row_set"];
            clmSet = (int)dt.Rows[0]["clm_set"];
        }

        // �T�u�v���V�[�W���F�f�[�^�O���b�g�r���[�̍X�V
        private void updateDataGripViews(DataTable dt1, DataTable dt2, ref DataGridView dgv1, ref DataGridView dgv2)
        {
            // �f�[�^�O���b�g�r���[�ւc�s�`�`�s�`�a�k�d���i�[
            dgv1.DataSource = dt1;
            dgv1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dgv2.DataSource = dt2;
            dgv2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // �X�y�b�N�O�̃Z�����}�[�L���O����
            colorHistoryViewBySpec(dtHistory, ref dgvHistory);

            // ��ԉ��̍s��\������
            if (dgv2.Rows.Count >= 1)
                dgv2.FirstDisplayedScrollingRowIndex = dgv2.Rows.Count - 1;
        }

        // �����{�^���������A�f�[�^��ǂݍ��݁A�c�`�s�`�f�q�h�c�u�h�d�v���X�V����
        private void btnSearch_Click(object sender, EventArgs e)
        {
            readDtHistory(ref dtHistory);
            updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);
        }

        // �T�u�T�u�v���V�[�W���F�O���b�g�r���[�E�[�Ƀ{�^����ǉ�
        private void addButtonsToDataGridView(DataGridView dgv)
        {
            Open = new DataGridViewButtonColumn();
            Open.Text = "Open";
            Open.UseColumnTextForButtonValue = true;
            Open.Width = 45;
            dgv.Columns.Add(Open);

            if (txtUser.Text == "user9")
            {
                btnDelete.Visible = true;
            }
        }

        // ����l�̐V�K�o�^
        private void btnMeasure_Click(object sender, EventArgs e)
        {
            // �ҏW���[�h�t���O�������A�o�^�E�C���{�^�����u�o�^�v�̕\���ɂ���
            editMode = false;
            btnRegister.Text = "Register";
            dtpLotInput.Enabled = true;

            // �g�h�r�s�n�q�x�f�[�^�O���b�h�r���[�̃}�[�L���O���N���A����
            colorViewReset(ref dgvHistory);
            colorViewReset(ref dgvBuffer);

            // �V�K�o�^�p�o�b�t�@�[�e�[�u���A�o�b�t�@�[�O���b�g�r���[������������
            dtBuffer.Clear();

            // �Z�b�g�����̍s�̐ݒ�
            for (int i = 1; i <= rowSet; i++)
            {
                DataRow dr = dtBuffer.NewRow();
                dr["inspect"] = txtInspect.Text;
                dr["lot"] = dtpLotInput.Value;
                dr["inspectdate"] = DateTime.Now;
                dr["line"] = txtLine.Text;
                dr["qc_user"] = txtUser.Text;
                dtBuffer.Rows.Add(dr);
            }

            // �O���b�g�r���[�̍X�V
            updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);
        }

        // ��������l�̏C��
        private void dgvHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int curRow = int.Parse(e.RowIndex.ToString());

            if (dgvHistory.Columns[e.ColumnIndex] == Open && curRow >= 0)
            {
                // �ҏW���[�h�t���O�𗧂āA�o�^�E�C���{�^�����u�C���v�̕\���ɂ���
                editMode = true;
                btnRegister.Text = "Update";
                dtpLotInput.Enabled = false;

                // �V�K�o�^�p�o�b�t�@�[�e�[�u���A�o�b�t�@�[�O���b�g�r���[�����������A�{�^���ɑΉ�����l���i�[����
                dtBuffer.Clear();

                string sql = "select inspect, lot, inspectdate, line, qc_user, " +
                                    "m1, m2, m3, m4, m5 FROM tbl_measure_history WHERE " +
                             "model = '" + txtModel.Text + "' AND " +
                             "inspect = '" + dgvHistory["inspect", curRow].Value.ToString() + "' AND " +
                             "lot = '" + (DateTime)dgvHistory["lot", curRow].Value + "' AND " +
                             "inspectdate = '" + (DateTime)dgvHistory["inspectdate", curRow].Value + "' AND " +
                             "line = '" + dgvHistory["line", curRow].Value.ToString() + "' " +
                             "order by qc_user";
                System.Diagnostics.Debug.Print(sql);
                TfSQL tf = new TfSQL();
                tf.sqlDataAdapterFillDatatable(sql, ref dtBuffer);

                // �O���b�g�r���[�̍X�V
                updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);

                // �ύX�^�[�Q�b�g�s��\������
                if (dgvHistory.Rows.Count >= 1)
                    dgvHistory.FirstDisplayedScrollingRowIndex = curRow;

                // �T�u�v���V�[�W���F�ҏW���̍s���}�[�L���O����
                colorViewForEdit(ref dgvHistory, curRow);
                colorViewForEdit(ref dgvBuffer, 0);
            }
        }

        // �T�u�v���V�[�W���F�ҏW���̍s���}�[�L���O����
        private void colorViewForEdit(ref DataGridView dgv, int row)
        {
            if (dgv.Rows.Count == 0) return;

            int rowCount = dgv.RowCount;
            int clmCount = dgv.ColumnCount;
            DateTime inspectdate = (DateTime)dgv["inspectdate", row].Value;

            for (int i = 0; i < rowCount; ++i)
            {
                if ((DateTime)dgv["inspectdate", i].Value == inspectdate)
                {
                    for (int j = 0; j < clmCount; ++j)
                        dgv[j, i].Style.BackColor = Color.Yellow;
                }
                else
                {
                    for (int k = 0; k < clmCount; ++k)
                        dgv[k, i].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                }
            }
        }

        // �T�u�v���V�[�W���F�}�[�L���O���N���A����
        private void colorViewReset(ref DataGridView dgv)
        {
            int rowCount = dgv.RowCount;
            int clmCount = dgv.ColumnCount;

            for (int i = 0; i < rowCount; ++i)
            {
                for (int k = 0; k < clmCount; ++k)
                    dgv[k, i].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
            }
        }

        // �T�u�v���V�[�W���F�X�y�b�N�O�̃Z�����}�[�L���O����i�����e�[�u���j
        private void colorHistoryViewBySpec(DataTable dt, ref DataGridView dgv)
        {
            int rowCount = dgv.RowCount;
            int clmStart = 5;
            int clmEnd = 9;

            for (int i = 0; i < rowCount; ++i)
            {
                for (int j = clmStart; j <= clmEnd; ++j)
                {
                    double m = 0;
                    bool b = double.TryParse(dt.Rows[i][j].ToString(), out m);
                    if (m >= low && m <= upp)
                        dgv[j, i].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                    else
                        dgv[j, i].Style.BackColor = Color.Red;
                }
            }
        }

        // �T�u�v���V�[�W���F�X�y�b�N�O�̃Z�����}�[�L���O����i�ꎞ�e�[�u���j
        private void colorBufferViewBySpec(double value, ref DataGridView dgv)
        {
            // �Ԓl�̊i�[��f�[�^�e�[�u���A�Z���Ԓn��񃍁[�J���ϐ��֊i�[
            if (value >= low && value <= upp)
                dgv[hAdr, vAdr].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
            else
                dgv[hAdr, vAdr].Style.BackColor = Color.Red;
        }

        // ����l�̎�荞�݂��I�������A�f�[�^�x�[�X�֓o�^����
        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (dtBuffer.Rows.Count <= 0) return;

            string model = txtModel.Text;
            string process = txtProcess.Text;
            string inspect = txtInspect.Text;
            DateTime lot = DateTime.Parse(dtBuffer.Rows[0]["lot"].ToString()); ;
            DateTime inspectdate = DateTime.Parse(dtBuffer.Rows[0]["inspectdate"].ToString()); ;
            string line = txtLine.Text;

            // �u�@�b�t�@�[�e�[�u�����ŁA���ςƃ����W���v�Z����
            calculateAverageAndRangeInDataTable(ref dtBuffer);

            // �h�o�p�b�c�a ���藚���e�[�u���ɓo�^����
            TfSQL tf = new TfSQL();
            bool res = tf.sqlMultipleInsert(model, process, inspect, lot, inspectdate, line, dtBuffer);

            if (res)
            {
                // �o�b�N�O���E���h�ło�p�l�e�[�u���ɓo�^����
                DataTable dtTemp = new DataTable();
                dtTemp = dtBuffer.Copy();
                registerMeasurementToPqmTable(dtTemp);

                // �o�^�ς̏�Ԃ��A���t�H�[���ɕ\������
                dtBuffer.Clear();
                readDtHistory(ref dtHistory);
                updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);

                // �ҏW���[�h�t���O�𗧂āA�o�^�E�C���{�^�����u�o�^�v�̕\���ɖ߂�
                editMode = false;
                btnRegister.Text = "Register";
                dtpLotInput.Enabled = true;
            }
        }

        // �T�u�v���V�[�W��: �f�[�^�e�[�u�������ŁA���ςƃ����W�i�ő�|�ŏ��j�����߁A�i�[����
        private void calculateAverageAndRangeInDataTable(ref DataTable dt)
        {
            if (dt.Rows.Count == 0) return;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                double[] ary = new double[5];
                double max = double.MinValue;
                double min = double.MaxValue;
                double sum = 0;
                double avg = 0;
                int cnt = 0;
                string idx = string.Empty;

                for (int j = 0; j < 5; j++)
                {
                    idx = "m" + (j + 1);
                    if (!string.IsNullOrEmpty(dt.Rows[i][idx].ToString()))
                    {
                        ary[j] = (double)dt.Rows[i][idx];
                        if (max < ary[j]) max = ary[j];
                        if (min > ary[j]) min = ary[j];
                        sum += ary[j];
                        cnt += 1;
                    }
                }
                avg = sum / cnt;
                dt.Rows[i]["x"] = Math.Round(avg, 4);
                dt.Rows[i]["r"] = Math.Abs(max - min);
            }
        }

        // �T�u�v���V�[�W��: �o�p�l�e�[�u���ւ̓o�^�i�o�b�N�O���E���h�����j
        private void registerMeasurementToPqmTable(DataTable dt)
        {
            var task = Task.Factory.StartNew(() =>
            {
                string model = txtModel.Text;
                string process = txtProcess.Text;
                string inspect = txtInspect.Text;
                DateTime lot = DateTime.Parse(dt.Rows[0]["lot"].ToString());
                DateTime inspectdate = DateTime.Parse(dt.Rows[0]["inspectdate"].ToString());
                string line = txtLine.Text;

                TfSqlPqm Tfc = new TfSqlPqm();
                Tfc.sqlMultipleInsertMeasurementToPqmTable(model, process, inspect, lot, inspectdate, line, dt, upp, low);
            });
        }

        // �폜�{�^���������̏���
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dtBuffer.Rows.Count <= 0) return;

            DialogResult result = MessageBox.Show("Do you really want to delete the selected row?",
                "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.No)
            {
                MessageBox.Show("Delete process was canceled.",
                "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            }
            else if (result == DialogResult.Yes)
            {
                // �f�[�^�̍폜
                string sql = "delete from tbl_measure_history where " +
                    "model='" + txtModel.Text + "' and " +
                    "inspect='" + txtInspect.Text + "' and " +
                    "lot ='" + dtBuffer.Rows[0]["lot"] + "' and " +
                    "inspectdate ='" + dtBuffer.Rows[0]["inspectdate"] + "' and " +
                    "line ='" + txtLine.Text + "'";

                System.Diagnostics.Debug.Print(sql);
                TfSQL tf = new TfSQL();
                int res = tf.sqlExecuteNonQueryInt(sql, false);

                // �o�b�N�O���E���h�ło�p�l�e�[�u�����̍폜
                DataTable dtTemp = new DataTable();
                dtTemp = dtBuffer.Copy();
                deleteFromPqmTable(dtTemp);

                // �V�K�o�^�p�o�b�t�@�[�e�[�u���A�o�b�t�@�[�O���b�g�r���[������������
                dtBuffer.Clear();

                // �폜��e�[�u���̍ēǂݍ���
                readDtHistory(ref dtHistory);

                // �g�h�r�s�n�q�x�f�[�^�O���b�h�r���[�̃}�[�L���O���N���A����
                colorViewReset(ref dgvHistory);

                // �O���b�g�r���[�̍X�V
                updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);

                // �ҏW���[�h�t���O�������A�o�^�E�C���{�^�����u�o�^�v�̕\���ɂ���
                editMode = false;
                btnRegister.Text = "Register";
                dtpLotInput.Enabled = true;
            }
        }

        // �T�u�v���V�[�W��: �o�p�l�e�[�u���ł̍폜�i�o�b�N�O���E���h�����j
        private void deleteFromPqmTable(DataTable dt)
        {
            var task = Task.Factory.StartNew(() =>
            {
                string model = txtModel.Text;
                string process = txtProcess.Text;
                string inspect = txtInspect.Text;
                DateTime lot = DateTime.Parse(dt.Rows[0]["lot"].ToString());
                DateTime inspectdate = DateTime.Parse(dt.Rows[0]["inspectdate"].ToString());
                string line = txtLine.Text;

                TfSqlPqm Tfc = new TfSqlPqm();
                Tfc.sqlDeleteFromPqmTable(model, process, inspect, lot, inspectdate, line);
            });
        }

        // �L�����Z���{�^���������̏���
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        // �f�[�^��M�����������Ƃ��̃C�x���g�����i�f���Q�[�g���j
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //! �V���A���|�[�g���I�[�v�����Ă��Ȃ��ꍇ�A�������s��Ȃ�.
            if (serialPort1.IsOpen == false) return;

            try
            {
                //�@�X���[�v�����邱�ƂŁA�P�Z�b�g�̐M���̑��M���I���܂ő҂�
                Thread.Sleep(100);

                //! ��M�R�}���h��ǂݍ��݁A�f���Q�[�g����
                string cmd = serialPort1.ReadExisting();
                Invoke(new Delegate_RcvDataToBufferDataTable(RcvDataToBufferDataTable), new Object[] { cmd });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // �f�[�^��M�����������Ƃ��̃C�x���g�����i�f���Q�[�g��j
        private void RcvDataToBufferDataTable(string cmd)
        {
            vAdr = dgvBuffer.CurrentCell.RowIndex;
            hAdr = dgvBuffer.CurrentCell.ColumnIndex;

            // �l�̃e�L�X�g���A�c�n�t�a�k�d�ɕϊ����Ăa�t�e�e�d�q�s�`�a�k�d�֊i�[����
            string replaced = cmd.Replace("\r\n", string.Empty);
            double value = 0;
            double.TryParse(replaced, out value);
            dtBuffer.Rows[vAdr][hAdr] = value;

            // �O���b�g�r���[�̍X�V
            updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);

            // �X�y�b�N�O�̃Z�����}�[�L���O����i�ꎞ�e�[�u���j
            colorBufferViewBySpec(value, ref dgvBuffer);
        }

        // �T�u�v���V�[�W��: ���m���ꂽ�P�ڂ̃V���A���|�[�g��I�����A�I�[�v������
        private void initializePort()
        {
            // ���p�\�ȃV���A���|�[�g���̔z����擾����
            string[] PortList = SerialPort.GetPortNames();

            // �V���A���|�[�g�����R���{�{�b�N�X�ɃZ�b�g����
            cmbPortName.Items.Clear();

            foreach (string PortName in PortList)
                cmbPortName.Items.Add(PortName);

            // ���m���ꂽ�|�[�g�̂P�ڂ�I�����A�R���{�{�b�N�X�ύX�C�x���g�ɂ��A�V���A���|�[�g���I�[�v������
            if (cmbPortName.Items.Count > 0)
                cmbPortName.SelectedIndex = 0;
        }

        // �|�[�g�I��p�R���{�{�b�N�X�ύX���A�V���A���|�[�g���I�[�v������
        private void cmbPortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //! ���ɃI�[�v�����Ă���|�[�g������ꍇ�́A�������s��Ȃ�
            if (serialPort1.IsOpen) return;

            //! �I�[�v������V���A���|�[�g���R���{�{�b�N�X������o��.
            serialPort1.PortName = cmbPortName.SelectedItem.ToString();
            serialPort1.BaudRate = 9600;
            serialPort1.DataBits = 8;
            serialPort1.Parity = Parity.None;
            serialPort1.StopBits = StopBits.Two;
            serialPort1.Encoding = Encoding.ASCII;

            try
            {
                serialPort1.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // �_�C�A���O�̏I�������F�V���A���|�[�g���I�[�v�����Ă���ꍇ�A�N���[�Y����
        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1.IsOpen) serialPort1.Close();
        }

        // �T�u�T�u�v���V�[�W���F�c�`�s�d�s�h�l�d�o�h�b�j�d�q���P�O���O�̓��t�ɂ���
        private void dtpSet10daysBefore(DateTimePicker dtp)
        {
            DateTime dt = dtp.Value.Date.AddDays(-10);
            dtp.Value = dt;
        }

        // �T�u�T�u�v���V�[�W���F�c�`�s�d�s�h�l�d�o�h�b�j�d�q�̕��ȉ���؂�グ��
        private void dtpRoundUpHour(DateTimePicker dtp)
        {
            DateTime dt = dtp.Value;
            int hour = dt.Hour;
            int minute = dt.Minute;
            int second = dt.Second;
            int millisecond = dt.Millisecond;
            dtp.Value = dt.AddHours(1).AddMinutes(-minute).AddSeconds(-second).AddMilliseconds(-millisecond);
        }

        // �T�u�T�u�v���V�[�W���F�c�`�s�d�s�h�l�d�o�h�b�j�d�q�̕��ȉ���������
        private void dtpRounddownHour(DateTimePicker dtp)
        {
            DateTime dt = dtp.Value;
            int hour = dt.Hour;
            int minute = dt.Minute;
            int second = dt.Second;
            int millisecond = dt.Millisecond;
            dtp.Value = dt.AddMinutes(-minute).AddSeconds(-second).AddMilliseconds(-millisecond);
        }

        // �w�q�O���t�쐬�{�^���������̏���  
        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelClass xl = new ExcelClass();

            xl.ExportToExcelWithXrChart
            (
                returnXrChartData()
            );
        }

        // �T�u�T�u�v���V�[�W���F�w�q�Ǘ��}�p�f�[�^�e�[�u���̐���      
        private DataTable returnXrChartData()
        {
            DataTable dt = new DataTable();
            dt = ((DataTable)dgvHistory.DataSource).Copy();
            dt.Columns.Add("llim", Type.GetType("System.Double"));
            dt.Columns.Add("ulim", Type.GetType("System.Double"));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["llim"] = double.Parse(txtLsl.Text);
                dt.Rows[i]["ulim"] = double.Parse(txtUsl.Text);
            }

            return dt;
        }
    }
}