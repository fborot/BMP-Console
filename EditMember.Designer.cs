namespace BMP_Console {
    partial class EditMember {
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
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbMID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbLN = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbPN = new System.Windows.Forms.TextBox();
            this.btSearch = new System.Windows.Forms.Button();
            this.gbDetails = new System.Windows.Forms.GroupBox();
            this.lFound = new System.Windows.Forms.Label();
            this.gbSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.lFound);
            this.gbSearch.Controls.Add(this.btSearch);
            this.gbSearch.Controls.Add(this.tbPN);
            this.gbSearch.Controls.Add(this.label3);
            this.gbSearch.Controls.Add(this.tbLN);
            this.gbSearch.Controls.Add(this.label2);
            this.gbSearch.Controls.Add(this.tbMID);
            this.gbSearch.Controls.Add(this.label1);
            this.gbSearch.Location = new System.Drawing.Point(13, 13);
            this.gbSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbSearch.Size = new System.Drawing.Size(1708, 98);
            this.gbSearch.TabIndex = 0;
            this.gbSearch.TabStop = false;
            this.gbSearch.Text = "Enter Search Criteria:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Member ID";
            // 
            // tbMID
            // 
            this.tbMID.Location = new System.Drawing.Point(12, 54);
            this.tbMID.Name = "tbMID";
            this.tbMID.Size = new System.Drawing.Size(215, 26);
            this.tbMID.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(267, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Last Name";
            // 
            // tbLN
            // 
            this.tbLN.Location = new System.Drawing.Point(259, 54);
            this.tbLN.Name = "tbLN";
            this.tbLN.Size = new System.Drawing.Size(215, 26);
            this.tbLN.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(496, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Phone Number";
            // 
            // tbPN
            // 
            this.tbPN.Location = new System.Drawing.Point(500, 54);
            this.tbPN.Name = "tbPN";
            this.tbPN.Size = new System.Drawing.Size(215, 26);
            this.tbPN.TabIndex = 5;
            // 
            // btSearch
            // 
            this.btSearch.Location = new System.Drawing.Point(1543, 54);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(158, 34);
            this.btSearch.TabIndex = 6;
            this.btSearch.Text = "Search";
            this.btSearch.UseVisualStyleBackColor = true;
            // 
            // gbDetails
            // 
            this.gbDetails.Location = new System.Drawing.Point(13, 119);
            this.gbDetails.Name = "gbDetails";
            this.gbDetails.Size = new System.Drawing.Size(1707, 769);
            this.gbDetails.TabIndex = 1;
            this.gbDetails.TabStop = false;
            this.gbDetails.Text = "Member Details";
            // 
            // lFound
            // 
            this.lFound.AutoSize = true;
            this.lFound.Location = new System.Drawing.Point(807, 54);
            this.lFound.Name = "lFound";
            this.lFound.Size = new System.Drawing.Size(55, 20);
            this.lFound.TabIndex = 7;
            this.lFound.Text = "Found";
            this.lFound.Visible = false;
            // 
            // EditMember
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1732, 900);
            this.Controls.Add(this.gbDetails);
            this.Controls.Add(this.gbSearch);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "EditMember";
            this.Text = "EditMember";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditMember_FormClosing);
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.Label lFound;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.TextBox tbPN;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbLN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbDetails;
    }
}