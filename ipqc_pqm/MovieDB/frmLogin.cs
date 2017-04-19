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
        // �R���X�g���N�^
        public frmLogin()
        {
            InitializeComponent();
        }

        // ���[�h���̏����i�R���{�{�b�N�X�ɁA�I�[�g�R���v���[�g�@�\�̒ǉ��j
        private void Form5_Load(object sender, EventArgs e)
        {
            string sql = "select DISTINCT qcuser FROM qc_user ORDER BY qcuser";
            TfSQL tf = new TfSQL();
            tf.getComboBoxData(sql, ref cmbUserName);
        }

        // ���[�U�[���O�C�����A�p�X���[�h�ƃ��O�C����Ԃ̊m�F�i�Q�d���O�C���̖h�~�j
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

                    // ���O�C����Ԃ��s�q�t�d�֕ύX
                    sql = "UPDATE qc_user SET loginstatus=true WHERE qcuser='" + user + "'";
                    bool res = tf.sqlExecuteNonQuery(sql, false);

                    // �q�t�H�[��Form1��\�����A�f���Q�[�g�C�x���g��ǉ��F 
                    frmItem f1 = new frmItem();
                    f1.RefreshEvent += delegate(object sndr, EventArgs excp)
                    {
                        // Form1�����ہA���O�C����Ԃ��e�`�k�r�d�֕ύX���A���t�H�[��Form5������
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

        // ���O�C���{�^���̉����ɉ����A�G���^�[�L�[�ł����O�C���ł���
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



