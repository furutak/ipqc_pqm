﻿namespace IpqcDB
{
    partial class frmManual
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManual));
            this.dgvBuffer = new System.Windows.Forms.DataGridView();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUsl = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLsl = new System.Windows.Forms.TextBox();
            this.dtpLotTo = new System.Windows.Forms.DateTimePicker();
            this.dtpLotFrom = new System.Windows.Forms.DateTimePicker();
            this.txtInspect = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnMeasure = new System.Windows.Forms.Button();
            this.txtLine = new System.Windows.Forms.TextBox();
            this.dtpLotInput = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.txtModel = new System.Windows.Forms.TextBox();
            this.txtProcess = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuffer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBuffer
            // 
            this.dgvBuffer.AllowUserToAddRows = false;
            this.dgvBuffer.AllowUserToDeleteRows = false;
            this.dgvBuffer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBuffer.Location = new System.Drawing.Point(13, 144);
            this.dgvBuffer.Name = "dgvBuffer";
            this.dgvBuffer.RowTemplate.Height = 21;
            this.dgvBuffer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvBuffer.Size = new System.Drawing.Size(897, 154);
            this.dgvBuffer.TabIndex = 10;
            this.dgvBuffer.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvBuffer_CellBeginEdit);
            this.dgvBuffer.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBuffer_CellEndEdit);
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(466, 102);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(90, 22);
            this.btnRegister.TabIndex = 12;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(804, 102);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 22);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgvHistory
            // 
            this.dgvHistory.AllowUserToAddRows = false;
            this.dgvHistory.AllowUserToDeleteRows = false;
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Location = new System.Drawing.Point(13, 314);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.ReadOnly = true;
            this.dgvHistory.RowTemplate.Height = 21;
            this.dgvHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvHistory.Size = new System.Drawing.Size(897, 359);
            this.dgvHistory.TabIndex = 10;
            this.dgvHistory.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHistory_CellContentClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(562, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "USL: ";
            // 
            // txtUsl
            // 
            this.txtUsl.Enabled = false;
            this.txtUsl.Location = new System.Drawing.Point(608, 54);
            this.txtUsl.Name = "txtUsl";
            this.txtUsl.Size = new System.Drawing.Size(83, 19);
            this.txtUsl.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(562, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 12);
            this.label6.TabIndex = 17;
            this.label6.Text = "LSL: ";
            // 
            // txtLsl
            // 
            this.txtLsl.Enabled = false;
            this.txtLsl.Location = new System.Drawing.Point(608, 21);
            this.txtLsl.Name = "txtLsl";
            this.txtLsl.Size = new System.Drawing.Size(83, 19);
            this.txtLsl.TabIndex = 16;
            // 
            // dtpLotTo
            // 
            this.dtpLotTo.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtpLotTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLotTo.Location = new System.Drawing.Point(248, 54);
            this.dtpLotTo.Name = "dtpLotTo";
            this.dtpLotTo.ShowUpDown = true;
            this.dtpLotTo.Size = new System.Drawing.Size(120, 19);
            this.dtpLotTo.TabIndex = 37;
            // 
            // dtpLotFrom
            // 
            this.dtpLotFrom.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtpLotFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLotFrom.Location = new System.Drawing.Point(89, 54);
            this.dtpLotFrom.Name = "dtpLotFrom";
            this.dtpLotFrom.ShowUpDown = true;
            this.dtpLotFrom.Size = new System.Drawing.Size(120, 19);
            this.dtpLotFrom.TabIndex = 38;
            // 
            // txtInspect
            // 
            this.txtInspect.Enabled = false;
            this.txtInspect.Location = new System.Drawing.Point(446, 21);
            this.txtInspect.Name = "txtInspect";
            this.txtInspect.Size = new System.Drawing.Size(83, 19);
            this.txtInspect.TabIndex = 26;
            // 
            // txtUser
            // 
            this.txtUser.Enabled = false;
            this.txtUser.Location = new System.Drawing.Point(792, 21);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(65, 19);
            this.txtUser.TabIndex = 27;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(392, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 12);
            this.label2.TabIndex = 28;
            this.label2.Text = "Line: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(721, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 29;
            this.label3.Text = "User: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(221, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 12);
            this.label1.TabIndex = 31;
            this.label1.Text = "to: ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(51, 59);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 12);
            this.label8.TabIndex = 32;
            this.label8.Text = "from: ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(392, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 12);
            this.label9.TabIndex = 33;
            this.label9.Text = "Inspect: ";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(20, 109);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(27, 12);
            this.label12.TabIndex = 34;
            this.label12.Text = "Lot: ";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(585, 102);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(90, 22);
            this.btnSearch.TabIndex = 40;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnMeasure
            // 
            this.btnMeasure.Location = new System.Drawing.Point(362, 102);
            this.btnMeasure.Name = "btnMeasure";
            this.btnMeasure.Size = new System.Drawing.Size(90, 22);
            this.btnMeasure.TabIndex = 41;
            this.btnMeasure.Text = "Measure New";
            this.btnMeasure.UseVisualStyleBackColor = true;
            this.btnMeasure.Click += new System.EventHandler(this.btnMeasure_Click);
            // 
            // txtLine
            // 
            this.txtLine.Enabled = false;
            this.txtLine.Location = new System.Drawing.Point(446, 54);
            this.txtLine.Name = "txtLine";
            this.txtLine.Size = new System.Drawing.Size(83, 19);
            this.txtLine.TabIndex = 26;
            // 
            // dtpLotInput
            // 
            this.dtpLotInput.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtpLotInput.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLotInput.Location = new System.Drawing.Point(89, 105);
            this.dtpLotInput.Name = "dtpLotInput";
            this.dtpLotInput.ShowUpDown = true;
            this.dtpLotInput.Size = new System.Drawing.Size(120, 19);
            this.dtpLotInput.TabIndex = 38;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 12);
            this.label4.TabIndex = 32;
            this.label4.Text = "input: ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(27, 12);
            this.label10.TabIndex = 34;
            this.label10.Text = "Lot: ";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(691, 102);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(90, 22);
            this.btnExport.TabIndex = 40;
            this.btnExport.Text = "Excel Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // txtModel
            // 
            this.txtModel.Enabled = false;
            this.txtModel.Location = new System.Drawing.Point(89, 21);
            this.txtModel.Name = "txtModel";
            this.txtModel.Size = new System.Drawing.Size(83, 19);
            this.txtModel.TabIndex = 42;
            // 
            // txtProcess
            // 
            this.txtProcess.Enabled = false;
            this.txtProcess.Location = new System.Drawing.Point(266, 21);
            this.txtProcess.Name = "txtProcess";
            this.txtProcess.Size = new System.Drawing.Size(83, 19);
            this.txtProcess.TabIndex = 43;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(20, 24);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 12);
            this.label13.TabIndex = 44;
            this.label13.Text = "Model: ";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(212, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(52, 12);
            this.label11.TabIndex = 45;
            this.label11.Text = "Process: ";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(257, 102);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 22);
            this.btnDelete.TabIndex = 41;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(922, 686);
            this.Controls.Add(this.txtModel);
            this.Controls.Add(this.txtProcess);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnMeasure);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dtpLotTo);
            this.Controls.Add(this.dtpLotInput);
            this.Controls.Add(this.dtpLotFrom);
            this.Controls.Add(this.txtLine);
            this.Controls.Add(this.txtInspect);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtLsl);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtUsl);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.dgvHistory);
            this.Controls.Add(this.dgvBuffer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form6";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Measurement Data";
            this.Load += new System.EventHandler(this.Form6_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuffer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvBuffer;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgvHistory;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtUsl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtLsl;
        private System.Windows.Forms.DateTimePicker dtpLotTo;
        private System.Windows.Forms.DateTimePicker dtpLotFrom;
        private System.Windows.Forms.TextBox txtInspect;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnMeasure;
        private System.Windows.Forms.TextBox txtLine;
        private System.Windows.Forms.DateTimePicker dtpLotInput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TextBox txtModel;
        private System.Windows.Forms.TextBox txtProcess;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnDelete;

    }
}

