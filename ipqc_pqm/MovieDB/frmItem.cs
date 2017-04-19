using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Security.Permissions;
using Npgsql;

namespace IpqcDB
{
    public partial class frmItem : Form
    {
        //�e�t�H�[��Form5�ցA�C�x���g������A���i�f���Q�[�g�j
        public delegate void RefreshEventHandler(object sender, EventArgs e);
        public event RefreshEventHandler RefreshEvent;

        //�f�[�^�e�[�u��
        DataTable dtInspectItems;
        DataTable dtLine;

        //�f�[�^�O���b�h�r���[�p�{�^��
        DataGridViewButtonColumn line1;
        DataGridViewButtonColumn line2;
        DataGridViewButtonColumn line3;

        //���̑��񃍁[�J���ϐ�
        bool load_cmb = true;

        // �R���X�g���N�^
        public frmItem()
        {
            InitializeComponent();
        }

        // ���[�h���̏���
        private void Form1_Load(object sender, EventArgs e)
        {
            //�t�H�[���̏ꏊ���w��
            this.Left = 0;
            this.Top = 0;
            dtInspectItems = new DataTable();
            dtLine = new DataTable();
            defineItemTable(ref dtInspectItems);
            defineLineTable(ref dtLine);
            getComboListFromDB(ref cmbModel);
            updateDataGripViews(ref dgvMeasureItem, true);
            load_cmb = false;
            if (txtUser.Text == "user9") btnEditMaster.Enabled = true;
        }

        // �T�u�v���V�[�W���F�f�[�^�O���b�g�r���[�̍X�V�B�e�t�H�[���ŌĂяo���A�e�t�H�[���̏��������p��
        public void updateControls(string user)
        {
            txtUser.Text = user;
        }

        // �T�u�v���V�[�W���F�������ڃe�[�u�����`����
        private void defineItemTable(ref DataTable dt)
        {
            dt.Columns.Add("no", Type.GetType("System.String"));
            dt.Columns.Add("model", Type.GetType("System.String"));
            dt.Columns.Add("process", Type.GetType("System.String"));
            dt.Columns.Add("inspect", Type.GetType("System.String"));
            dt.Columns.Add("description", Type.GetType("System.String"));
            dt.Columns.Add("instrument", Type.GetType("System.String"));
        }

        // �T�u�v���V�[�W���F���C���e�[�u�����`����
        private void defineLineTable(ref DataTable dt)
        {
            dt.Columns.Add("model", Type.GetType("System.String"));
            dt.Columns.Add("line", Type.GetType("System.String"));
        }

        // �T�u�v���V�[�W���F�^���R���{�{�b�N�X�֌�����荞��
        public void getComboListFromDB(ref ComboBox cmb)
        {
            string sql = "select model from tbl_model_dbplace order by model";
            System.Diagnostics.Debug.Print(sql);
            TfSQL tf = new TfSQL();
            tf.getComboBoxData(sql, ref cmb);

            if (cmbModel.Items.Count > 0)
                cmbModel.SelectedIndex = 0;
        }

        // �T�u�v���V�[�W���F�f�[�^�O���b�g�r���[�̍X�V
        public void updateDataGripViews(ref DataGridView dgv, bool load)
        {
            dtInspectItems.Clear();
            string model = cmbModel.Text;
            string sql = "select no, model, process, inspect, description, instrument from tbl_measure_item_2 where model='"
                + model + "' order by no, process, inspect";
            System.Diagnostics.Debug.Print(sql);
            TfSQL tf = new TfSQL();
            tf.sqlDataAdapterFillDatatable(sql, ref dtInspectItems);

            // �f�[�^�O���b�g�r���[�ւc�s�`�`�s�`�a�k�d���i�[
            dgv.DataSource = dtInspectItems;

            // �O���b�g�r���[�E�[�Ƀ{�^����ǉ�
            addButtonsToDataGridView(dgv);

            // �񕝂̒���
            //dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.Columns["no"].Width = 50;
            dgv.Columns["model"].Width = 100;
            dgv.Columns["process"].Width = 50;
            dgv.Columns["inspect"].Width = 100;
            dgv.Columns["description"].Width = 600;
            dgv.Columns["instrument"].Width = 80;
         }
        
        // �T�u�T�u�v���V�[�W���F�O���b�g�r���[�E�[�Ƀ{�^����ǉ�
        private void addButtonsToDataGridView(DataGridView dgv)
        {
            dtLine.Clear();
            string model = cmbModel.Text;
            string sql = "select line FROM tbl_model_line where model='" + model + "' order by line";
            System.Diagnostics.Debug.Print(sql);
            TfSQL tf = new TfSQL();
            tf.sqlDataAdapterFillDatatable(sql, ref dtLine);

            if (dtLine.Rows.Count == 0) return;

            if (line1 != null)
            {
                dgv.Columns.Remove(line1);
                line1 = null;
            }

            if (line2 != null)
            {
                dgv.Columns.Remove(line2);
                line2 = null;
            }

            if (line3 != null)
            {
                dgv.Columns.Remove(line3);
                line3 = null;
            } 

            if (dtLine.Rows.Count >= 1)
            {
                line1 = new DataGridViewButtonColumn();
                line1.Name = "line";
                line1.Text = dtLine.Rows[0]["line"].ToString();
                line1.UseColumnTextForButtonValue = true;
                line1.Width = 45;
                dgv.Columns.Add(line1);
            }

            if (dtLine.Rows.Count >= 2)
            {
                line2 = new DataGridViewButtonColumn();
                line2.Name = "line";
                line2.Text = dtLine.Rows[1]["line"].ToString();
                line2.UseColumnTextForButtonValue = true;
                line2.Width = 45;
                dgv.Columns.Add(line2);            
            }

            if (dtLine.Rows.Count >= 3)
            {
                line3 = new DataGridViewButtonColumn();
                line3.Name = "line";
                line3.Text = dtLine.Rows[2]["line"].ToString();
                line3.UseColumnTextForButtonValue = true;
                line3.Width = 45;
                dgv.Columns.Add(line3);
            }
        }

        // �^���R���{�{�b�N�X�ύX���̏���
        private void cmbModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load_cmb) return; //���[�h���͏������s��Ȃ�

            updateDataGripViews(ref dgvMeasureItem, false);
        }

        // �O���b�g�r���[��̃{�^���N���b�N���A�t�H�[���Q�E�R�E�S���J���i�t�H�[���Q�̓f���Q�[�g����j
        private void dgvBoxId_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (!(senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)) return;

            //�I�������{�^���ɊY���������ێ�����
            int curRow = int.Parse(e.RowIndex.ToString());
            string model = dgvMeasureItem["model", curRow].Value.ToString();
            string process = dgvMeasureItem["process", curRow].Value.ToString();
            string inspect = dgvMeasureItem["inspect", curRow].Value.ToString();
            string line = ((DataGridViewButtonColumn)senderGrid.Columns[e.ColumnIndex]).Text;
            string instrument = dgvMeasureItem["instrument", curRow].Value.ToString();
            string user = txtUser.Text;

            //�����ɊY������t�H�[�����J��
            if (instrument == "mmx" || instrument == "mmy")
            {
                frmMmAuto fA = new frmMmAuto();
                fA.updateControls(model, process, inspect, line, user);
                fA.Show();
            }
            else if (instrument == "push" || instrument == "pull")
            {
                frmPushPull fP = new frmPushPull();
                fP.updateControls(model, process, inspect, line, user);
                fP.Show();
            }
            else if (instrument == "hr-20")
            {
                frmScale fS = new frmScale();
                fS.updateControls(model, process, inspect, line, user);
                fS.Show();
            }
            else if (instrument == "hiohm")
            {
                frmHioki fH = new frmHioki();
                fH.updateControls(model, process, inspect, line, user);
                fH.Show();
            }
            else
            {
                frmManual fM = new frmManual();
                fM.updateControls(model, process, inspect, line, user);
                fM.Show();
            }
        }

        //Form1�����ہA��\���ɂȂ��Ă���e�t�H�[��Form5�����
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //�e�t�H�[��Form5�����悤�A�f���Q�[�g�C�x���g�𔭐�������
            this.RefreshEvent(this, new EventArgs());
        }

        // �L�����Z���{�^���������̏���
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        // �������ڃ}�X�^�[�ҏW�p�t�H�[���̌Ăяo��
        private void btnEditMaster_Click(object sender, EventArgs e)
        {
            if (txtUser.Text != "user9") return;
            if (TfGeneral.checkOpenFormExists("frmItemMaster")) return;

            frmItemMaster fM = new frmItemMaster(cmbModel.Text);
            //�q�C�x���g���L���b�`���āA�f�[�^�O���b�h���X�V����
            fM.RefreshEvent += delegate (object sndr, EventArgs excp)
            {
                updateDataGripViews(ref dgvMeasureItem, false);
            };
            fM.Show();
        }
    }
}