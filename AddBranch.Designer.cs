namespace BMP_Console {
    partial class AddBranch {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btcancel = new System.Windows.Forms.Button();
            this.btsave = new System.Windows.Forms.Button();
            this.cbagencies = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbcontactphone = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbcontactemail = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbcontactname = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbBranchID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbpostalcode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbaddress2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbaddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbbranchname = new System.Windows.Forms.TextBox();
            this.ckUse = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckUse);
            this.groupBox1.Controls.Add(this.btcancel);
            this.groupBox1.Controls.Add(this.btsave);
            this.groupBox1.Controls.Add(this.cbagencies);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.tbcontactphone);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.tbcontactemail);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tbcontactname);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.tbBranchID);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbpostalcode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbaddress2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbaddress);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbbranchname);
            this.groupBox1.Location = new System.Drawing.Point(10, 11);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(562, 240);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Branch Details";
            // 
            // btcancel
            // 
            this.btcancel.Location = new System.Drawing.Point(281, 203);
            this.btcancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btcancel.Name = "btcancel";
            this.btcancel.Size = new System.Drawing.Size(136, 28);
            this.btcancel.TabIndex = 22;
            this.btcancel.Text = "Cancel and Exit";
            this.btcancel.UseVisualStyleBackColor = true;
            this.btcancel.Click += new System.EventHandler(this.btcancel_Click);
            // 
            // btsave
            // 
            this.btsave.Location = new System.Drawing.Point(461, 203);
            this.btsave.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btsave.Name = "btsave";
            this.btsave.Size = new System.Drawing.Size(82, 28);
            this.btsave.TabIndex = 21;
            this.btsave.Text = "Save";
            this.btsave.UseVisualStyleBackColor = true;
            this.btsave.Click += new System.EventHandler(this.btsave_Click);
            // 
            // cbagencies
            // 
            this.cbagencies.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbagencies.FormattingEnabled = true;
            this.cbagencies.Location = new System.Drawing.Point(375, 171);
            this.cbagencies.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbagencies.Name = "cbagencies";
            this.cbagencies.Size = new System.Drawing.Size(169, 21);
            this.cbagencies.TabIndex = 20;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(371, 154);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 13);
            this.label11.TabIndex = 19;
            this.label11.Text = "Agency Name";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(2, 15);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Branch Name";
            // 
            // tbcontactphone
            // 
            this.tbcontactphone.Location = new System.Drawing.Point(277, 123);
            this.tbcontactphone.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbcontactphone.Name = "tbcontactphone";
            this.tbcontactphone.Size = new System.Drawing.Size(267, 20);
            this.tbcontactphone.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(277, 106);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(118, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Contact Phone Number";
            // 
            // tbcontactemail
            // 
            this.tbcontactemail.Location = new System.Drawing.Point(279, 74);
            this.tbcontactemail.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbcontactemail.Name = "tbcontactemail";
            this.tbcontactemail.Size = new System.Drawing.Size(265, 20);
            this.tbcontactemail.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(277, 58);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Contact Email";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(2, 15);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "label7";
            // 
            // tbcontactname
            // 
            this.tbcontactname.Location = new System.Drawing.Point(277, 34);
            this.tbcontactname.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbcontactname.Name = "tbcontactname";
            this.tbcontactname.Size = new System.Drawing.Size(267, 20);
            this.tbcontactname.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(274, 18);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Contact Name";
            // 
            // tbBranchID
            // 
            this.tbBranchID.Enabled = false;
            this.tbBranchID.Location = new System.Drawing.Point(90, 171);
            this.tbBranchID.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbBranchID.Name = "tbBranchID";
            this.tbBranchID.Size = new System.Drawing.Size(72, 20);
            this.tbBranchID.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(88, 155);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "# of Branches";
            // 
            // tbpostalcode
            // 
            this.tbpostalcode.Location = new System.Drawing.Point(5, 172);
            this.tbpostalcode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbpostalcode.Name = "tbpostalcode";
            this.tbpostalcode.Size = new System.Drawing.Size(72, 20);
            this.tbpostalcode.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 155);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Postal Code";
            // 
            // tbaddress2
            // 
            this.tbaddress2.Location = new System.Drawing.Point(4, 123);
            this.tbaddress2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbaddress2.Name = "tbaddress2";
            this.tbaddress2.Size = new System.Drawing.Size(260, 20);
            this.tbaddress2.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 106);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Address 2";
            // 
            // tbaddress
            // 
            this.tbaddress.Location = new System.Drawing.Point(4, 75);
            this.tbaddress.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbaddress.Name = "tbaddress";
            this.tbaddress.Size = new System.Drawing.Size(260, 20);
            this.tbaddress.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 58);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Address";
            // 
            // tbbranchname
            // 
            this.tbbranchname.Location = new System.Drawing.Point(4, 34);
            this.tbbranchname.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbbranchname.Name = "tbbranchname";
            this.tbbranchname.Size = new System.Drawing.Size(260, 20);
            this.tbbranchname.TabIndex = 1;
            // 
            // ckUse
            // 
            this.ckUse.AutoSize = true;
            this.ckUse.Location = new System.Drawing.Point(187, 172);
            this.ckUse.Name = "ckUse";
            this.ckUse.Size = new System.Drawing.Size(167, 17);
            this.ckUse.TabIndex = 23;
            this.ckUse.Text = "Use Branch Name for Checks";
            this.ckUse.UseVisualStyleBackColor = true;
            // 
            // AddBranch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(584, 263);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "AddBranch";
            this.Text = "AddBranch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddBranch_FormClosing);
            this.Load += new System.EventHandler(this.AddBranch_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbpostalcode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbaddress2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbaddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbbranchname;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbBranchID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbagencies;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbcontactphone;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbcontactemail;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbcontactname;
        private System.Windows.Forms.Button btcancel;
        private System.Windows.Forms.Button btsave;
        private System.Windows.Forms.CheckBox ckUse;
    }
}