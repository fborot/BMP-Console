namespace BMP_Console {
    partial class ShowMembers {
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgmembers = new System.Windows.Forms.DataGridView();
            this.btDelete = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showTransactionsHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showPaymentStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.disableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.lFound = new System.Windows.Forms.Label();
            this.btSearch = new System.Windows.Forms.Button();
            this.tbPN = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbLN = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgmembers)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.gbSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgmembers);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(7, 122);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.groupBox1.Size = new System.Drawing.Size(1383, 598);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Members List";
            // 
            // dgmembers
            // 
            this.dgmembers.AllowUserToAddRows = false;
            this.dgmembers.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dgmembers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgmembers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgmembers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgmembers.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dgmembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgmembers.Location = new System.Drawing.Point(8, 20);
            this.dgmembers.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.dgmembers.MultiSelect = false;
            this.dgmembers.Name = "dgmembers";
            this.dgmembers.RowHeadersVisible = false;
            this.dgmembers.RowHeadersWidth = 51;
            this.dgmembers.RowTemplate.Height = 24;
            this.dgmembers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgmembers.Size = new System.Drawing.Size(1367, 568);
            this.dgmembers.TabIndex = 0;
            this.dgmembers.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgmembers_CellFormatting);
            this.dgmembers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgmembers_CellValueChanged);
            this.dgmembers.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgmembers_MouseUp);
            // 
            // btDelete
            // 
            this.btDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btDelete.Location = new System.Drawing.Point(594, 724);
            this.btDelete.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(221, 31);
            this.btDelete.TabIndex = 1;
            this.btDelete.Text = "Delete User";
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.Enabled = false;
            this.btCancel.Location = new System.Drawing.Point(873, 724);
            this.btCancel.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(221, 31);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "Cancel and Exit";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Enabled = false;
            this.btSave.Location = new System.Drawing.Point(1171, 724);
            this.btSave.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(219, 31);
            this.btSave.TabIndex = 3;
            this.btSave.Text = "Save changes";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showTransactionsHistoryToolStripMenuItem,
            this.showPaymentStatusToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripSeparator1,
            this.disableToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(247, 106);
            // 
            // showTransactionsHistoryToolStripMenuItem
            // 
            this.showTransactionsHistoryToolStripMenuItem.Name = "showTransactionsHistoryToolStripMenuItem";
            this.showTransactionsHistoryToolStripMenuItem.Size = new System.Drawing.Size(246, 24);
            this.showTransactionsHistoryToolStripMenuItem.Text = "Show TransactionsHistory";
            this.showTransactionsHistoryToolStripMenuItem.Click += new System.EventHandler(this.showTransactionsHistoryToolStripMenuItem_Click);
            // 
            // showPaymentStatusToolStripMenuItem
            // 
            this.showPaymentStatusToolStripMenuItem.Name = "showPaymentStatusToolStripMenuItem";
            this.showPaymentStatusToolStripMenuItem.Size = new System.Drawing.Size(246, 24);
            this.showPaymentStatusToolStripMenuItem.Text = "Show Payment Status";
            this.showPaymentStatusToolStripMenuItem.Click += new System.EventHandler(this.showPaymentStatusToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(243, 6);
            // 
            // disableToolStripMenuItem
            // 
            this.disableToolStripMenuItem.ForeColor = System.Drawing.Color.Red;
            this.disableToolStripMenuItem.Name = "disableToolStripMenuItem";
            this.disableToolStripMenuItem.Size = new System.Drawing.Size(246, 24);
            this.disableToolStripMenuItem.Text = "Disable Member";
            this.disableToolStripMenuItem.Click += new System.EventHandler(this.disableToolStripMenuItem_Click);
            // 
            // gbSearch
            // 
            this.gbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSearch.Controls.Add(this.lFound);
            this.gbSearch.Controls.Add(this.btSearch);
            this.gbSearch.Controls.Add(this.tbPN);
            this.gbSearch.Controls.Add(this.label3);
            this.gbSearch.Controls.Add(this.tbLN);
            this.gbSearch.Controls.Add(this.label2);
            this.gbSearch.Controls.Add(this.tbMID);
            this.gbSearch.Controls.Add(this.label1);
            this.gbSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbSearch.Location = new System.Drawing.Point(8, 14);
            this.gbSearch.Margin = new System.Windows.Forms.Padding(5);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Padding = new System.Windows.Forms.Padding(5);
            this.gbSearch.Size = new System.Drawing.Size(1383, 93);
            this.gbSearch.TabIndex = 4;
            this.gbSearch.TabStop = false;
            this.gbSearch.Text = "Enter Search Criteria:";
            // 
            // lFound
            // 
            this.lFound.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lFound.AutoSize = true;
            this.lFound.Location = new System.Drawing.Point(1047, 53);
            this.lFound.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lFound.Name = "lFound";
            this.lFound.Size = new System.Drawing.Size(55, 20);
            this.lFound.TabIndex = 7;
            this.lFound.Text = "Found";
            this.lFound.Visible = false;
            // 
            // btSearch
            // 
            this.btSearch.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btSearch.Location = new System.Drawing.Point(1166, 41);
            this.btSearch.Margin = new System.Windows.Forms.Padding(4);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(198, 35);
            this.btSearch.TabIndex = 6;
            this.btSearch.Text = "Search";
            this.btSearch.UseVisualStyleBackColor = true;
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // tbPN
            // 
            this.tbPN.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbPN.Location = new System.Drawing.Point(588, 53);
            this.tbPN.Margin = new System.Windows.Forms.Padding(4);
            this.tbPN.Name = "tbPN";
            this.tbPN.Size = new System.Drawing.Size(268, 27);
            this.tbPN.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(583, 19);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Phone Number";
            // 
            // tbLN
            // 
            this.tbLN.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbLN.Location = new System.Drawing.Point(287, 53);
            this.tbLN.Margin = new System.Windows.Forms.Padding(4);
            this.tbLN.Name = "tbLN";
            this.tbLN.Size = new System.Drawing.Size(268, 27);
            this.tbLN.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(297, 19);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Last Name";
            // 
            // tbMID
            // 
            this.tbMID.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbMID.Location = new System.Drawing.Point(9, 53);
            this.tbMID.Margin = new System.Windows.Forms.Padding(4);
            this.tbMID.Name = "tbMID";
            this.tbMID.Size = new System.Drawing.Size(268, 27);
            this.tbMID.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Member ID";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(246, 24);
            this.toolStripMenuItem1.Text = "Edit Notes";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // ShowMembers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1404, 762);
            this.Controls.Add(this.gbSearch);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btDelete);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "ShowMembers";
            this.Text = "ShowMembers";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShowMembers_FormClosing);
            this.Load += new System.EventHandler(this.ShowMembers_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgmembers)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgmembers;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showTransactionsHistoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showPaymentStatusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.Label lFound;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.TextBox tbPN;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbLN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}