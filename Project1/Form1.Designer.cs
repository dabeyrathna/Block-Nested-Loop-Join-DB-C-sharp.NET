namespace Project1
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtSelect = new System.Windows.Forms.TextBox();
            this.lstOuter = new System.Windows.Forms.ListBox();
            this.lstInner = new System.Windows.Forms.ListBox();
            this.lblOuter = new System.Windows.Forms.Label();
            this.lblInner = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtxFrom = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtWhere = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbCurrServer = new System.Windows.Forms.ComboBox();
            this.gridOut = new System.Windows.Forms.DataGridView();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lstSite1 = new System.Windows.Forms.ListBox();
            this.lstSite2 = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdo4 = new System.Windows.Forms.RadioButton();
            this.rdo3 = new System.Windows.Forms.RadioButton();
            this.rdo2 = new System.Windows.Forms.RadioButton();
            this.rdo1 = new System.Windows.Forms.RadioButton();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstOutBuffer = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridOut)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(257, 359);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(158, 41);
            this.button1.TabIndex = 0;
            this.button1.Text = "Run Query";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtSelect
            // 
            this.txtSelect.Location = new System.Drawing.Point(71, 21);
            this.txtSelect.Name = "txtSelect";
            this.txtSelect.Size = new System.Drawing.Size(389, 20);
            this.txtSelect.TabIndex = 1;
            // 
            // lstOuter
            // 
            this.lstOuter.FormattingEnabled = true;
            this.lstOuter.Location = new System.Drawing.Point(28, 170);
            this.lstOuter.Name = "lstOuter";
            this.lstOuter.Size = new System.Drawing.Size(311, 173);
            this.lstOuter.TabIndex = 2;
            // 
            // lstInner
            // 
            this.lstInner.FormattingEnabled = true;
            this.lstInner.Location = new System.Drawing.Point(345, 170);
            this.lstInner.Name = "lstInner";
            this.lstInner.Size = new System.Drawing.Size(306, 173);
            this.lstInner.TabIndex = 2;
            // 
            // lblOuter
            // 
            this.lblOuter.AutoSize = true;
            this.lblOuter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOuter.Location = new System.Drawing.Point(108, 147);
            this.lblOuter.Name = "lblOuter";
            this.lblOuter.Size = new System.Drawing.Size(105, 20);
            this.lblOuter.TabIndex = 4;
            this.lblOuter.Text = "Outer relation";
            // 
            // lblInner
            // 
            this.lblInner.AutoSize = true;
            this.lblInner.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInner.Location = new System.Drawing.Point(458, 147);
            this.lblInner.Name = "lblInner";
            this.lblInner.Size = new System.Drawing.Size(102, 20);
            this.lblInner.TabIndex = 4;
            this.lblInner.Text = "Inner relation";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "SELECT";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "FROM";
            // 
            // txtxFrom
            // 
            this.txtxFrom.Location = new System.Drawing.Point(71, 54);
            this.txtxFrom.Name = "txtxFrom";
            this.txtxFrom.Size = new System.Drawing.Size(389, 20);
            this.txtxFrom.TabIndex = 5;
            this.txtxFrom.Text = "customer";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "WHERE";
            // 
            // txtWhere
            // 
            this.txtWhere.Location = new System.Drawing.Point(71, 88);
            this.txtWhere.Multiline = true;
            this.txtWhere.Name = "txtWhere";
            this.txtWhere.Size = new System.Drawing.Size(389, 45);
            this.txtWhere.TabIndex = 6;
            this.txtWhere.Text = "customer = depositor";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(491, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Server";
            // 
            // cmbCurrServer
            // 
            this.cmbCurrServer.FormattingEnabled = true;
            this.cmbCurrServer.Items.AddRange(new object[] {
            "NYC",
            "OMA",
            "HOU",
            "SFO"});
            this.cmbCurrServer.Location = new System.Drawing.Point(530, 16);
            this.cmbCurrServer.Name = "cmbCurrServer";
            this.cmbCurrServer.Size = new System.Drawing.Size(81, 21);
            this.cmbCurrServer.TabIndex = 7;
            this.cmbCurrServer.SelectedIndexChanged += new System.EventHandler(this.cmbCurrServer_SelectedIndexChanged);
            // 
            // gridOut
            // 
            this.gridOut.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridOut.Location = new System.Drawing.Point(28, 406);
            this.gridOut.Name = "gridOut";
            this.gridOut.Size = new System.Drawing.Size(623, 276);
            this.gridOut.TabIndex = 8;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(28, 379);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(108, 21);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtLog
            // 
            this.txtLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(1083, 19);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(267, 614);
            this.txtLog.TabIndex = 10;
            // 
            // lstSite1
            // 
            this.lstSite1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSite1.FormattingEnabled = true;
            this.lstSite1.ItemHeight = 15;
            this.lstSite1.Location = new System.Drawing.Point(19, 62);
            this.lstSite1.Name = "lstSite1";
            this.lstSite1.Size = new System.Drawing.Size(152, 409);
            this.lstSite1.TabIndex = 11;
            // 
            // lstSite2
            // 
            this.lstSite2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSite2.FormattingEnabled = true;
            this.lstSite2.ItemHeight = 15;
            this.lstSite2.Location = new System.Drawing.Point(170, 62);
            this.lstSite2.Name = "lstSite2";
            this.lstSite2.Size = new System.Drawing.Size(217, 409);
            this.lstSite2.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(31, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 20);
            this.label2.TabIndex = 13;
            this.label2.Text = "Send to site 2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(187, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 20);
            this.label3.TabIndex = 14;
            this.label3.Text = "Recieved to site 1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstSite1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lstSite2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(670, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(407, 490);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Semi - JOIN";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.txtSelect);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cmbCurrServer);
            this.groupBox2.Controls.Add(this.txtxFrom);
            this.groupBox2.Controls.Add(this.txtWhere);
            this.groupBox2.Location = new System.Drawing.Point(28, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(623, 139);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Query";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rdo4);
            this.groupBox4.Controls.Add(this.rdo3);
            this.groupBox4.Controls.Add(this.rdo2);
            this.groupBox4.Controls.Add(this.rdo1);
            this.groupBox4.Location = new System.Drawing.Point(490, 40);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(121, 83);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            // 
            // rdo4
            // 
            this.rdo4.AutoSize = true;
            this.rdo4.Location = new System.Drawing.Point(32, 62);
            this.rdo4.Name = "rdo4";
            this.rdo4.Size = new System.Drawing.Size(62, 17);
            this.rdo4.TabIndex = 3;
            this.rdo4.TabStop = true;
            this.rdo4.Text = "Query 4";
            this.rdo4.UseVisualStyleBackColor = true;
            // 
            // rdo3
            // 
            this.rdo3.AutoSize = true;
            this.rdo3.Location = new System.Drawing.Point(32, 44);
            this.rdo3.Name = "rdo3";
            this.rdo3.Size = new System.Drawing.Size(62, 17);
            this.rdo3.TabIndex = 2;
            this.rdo3.TabStop = true;
            this.rdo3.Text = "Query 3";
            this.rdo3.UseVisualStyleBackColor = true;
            this.rdo3.CheckedChanged += new System.EventHandler(this.rdo3_CheckedChanged);
            // 
            // rdo2
            // 
            this.rdo2.AutoSize = true;
            this.rdo2.Location = new System.Drawing.Point(32, 26);
            this.rdo2.Name = "rdo2";
            this.rdo2.Size = new System.Drawing.Size(62, 17);
            this.rdo2.TabIndex = 1;
            this.rdo2.TabStop = true;
            this.rdo2.Text = "Query 2";
            this.rdo2.UseVisualStyleBackColor = true;
            this.rdo2.CheckedChanged += new System.EventHandler(this.rdo2_CheckedChanged);
            // 
            // rdo1
            // 
            this.rdo1.AutoSize = true;
            this.rdo1.Location = new System.Drawing.Point(32, 8);
            this.rdo1.Name = "rdo1";
            this.rdo1.Size = new System.Drawing.Size(62, 17);
            this.rdo1.TabIndex = 0;
            this.rdo1.TabStop = true;
            this.rdo1.Text = "Query 1";
            this.rdo1.UseVisualStyleBackColor = true;
            this.rdo1.CheckedChanged += new System.EventHandler(this.rdo1_CheckedChanged);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(1112, 640);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(133, 30);
            this.btnClearLog.TabIndex = 17;
            this.btnClearLog.Text = "Clear Log";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(142, 379);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(108, 21);
            this.btnExit.TabIndex = 18;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lstOutBuffer);
            this.groupBox3.Location = new System.Drawing.Point(670, 508);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(407, 162);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Out Buffer";
            // 
            // lstOutBuffer
            // 
            this.lstOutBuffer.FormattingEnabled = true;
            this.lstOutBuffer.Location = new System.Drawing.Point(27, 31);
            this.lstOutBuffer.Name = "lstOutBuffer";
            this.lstOutBuffer.Size = new System.Drawing.Size(360, 108);
            this.lstOutBuffer.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 722);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lblInner);
            this.Controls.Add(this.lblOuter);
            this.Controls.Add(this.lstInner);
            this.Controls.Add(this.lstOuter);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gridOut);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Block Nested Loop Join";
            ((System.ComponentModel.ISupportInitialize)(this.gridOut)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtSelect;
        private System.Windows.Forms.ListBox lstOuter;
        private System.Windows.Forms.ListBox lstInner;
        private System.Windows.Forms.Label lblOuter;
        private System.Windows.Forms.Label lblInner;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtxFrom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtWhere;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbCurrServer;
        private System.Windows.Forms.DataGridView gridOut;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ListBox lstSite1;
        private System.Windows.Forms.ListBox lstSite2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lstOutBuffer;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rdo4;
        private System.Windows.Forms.RadioButton rdo3;
        private System.Windows.Forms.RadioButton rdo2;
        private System.Windows.Forms.RadioButton rdo1;
    }
}

