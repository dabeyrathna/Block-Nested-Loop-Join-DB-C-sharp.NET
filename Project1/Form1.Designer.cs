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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtxFrom = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtWhere = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbCurrServer = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(264, 359);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(158, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Run Query";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtSelect
            // 
            this.txtSelect.Location = new System.Drawing.Point(112, 43);
            this.txtSelect.Name = "txtSelect";
            this.txtSelect.Size = new System.Drawing.Size(539, 20);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Query";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(119, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Outer relation";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(469, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Inner relation";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(58, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "SELECT";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(58, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "FROM";
            // 
            // txtxFrom
            // 
            this.txtxFrom.Location = new System.Drawing.Point(112, 76);
            this.txtxFrom.Name = "txtxFrom";
            this.txtxFrom.Size = new System.Drawing.Size(539, 20);
            this.txtxFrom.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(58, 113);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "WHERE";
            // 
            // txtWhere
            // 
            this.txtWhere.Location = new System.Drawing.Point(112, 110);
            this.txtWhere.Name = "txtWhere";
            this.txtWhere.Size = new System.Drawing.Size(539, 20);
            this.txtWhere.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(697, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Current Server";
            // 
            // cmbCurrServer
            // 
            this.cmbCurrServer.FormattingEnabled = true;
            this.cmbCurrServer.Items.AddRange(new object[] {
            "NYC",
            "OMA",
            "HOU",
            "SFO"});
            this.cmbCurrServer.Location = new System.Drawing.Point(700, 42);
            this.cmbCurrServer.Name = "cmbCurrServer";
            this.cmbCurrServer.Size = new System.Drawing.Size(121, 21);
            this.cmbCurrServer.TabIndex = 7;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(28, 406);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(623, 150);
            this.dataGridView1.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 568);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cmbCurrServer);
            this.Controls.Add(this.txtWhere);
            this.Controls.Add(this.txtxFrom);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstInner);
            this.Controls.Add(this.lstOuter);
            this.Controls.Add(this.txtSelect);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Block Nested Loop Join";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtSelect;
        private System.Windows.Forms.ListBox lstOuter;
        private System.Windows.Forms.ListBox lstInner;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtxFrom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtWhere;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbCurrServer;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

