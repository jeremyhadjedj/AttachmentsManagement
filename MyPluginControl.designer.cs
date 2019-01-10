namespace AttachmentsManagement
{
    partial class MyPluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Btn_RefreshCRM = new System.Windows.Forms.ToolStripButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.EmailsInCrmCount = new System.Windows.Forms.Label();
            this.NotesInCrmCount = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.inCRM_Panel = new System.Windows.Forms.Panel();
            this.Btn_MoveNotesToCrm = new System.Windows.Forms.Button();
            this.Btn_MoveEmailsToCrm = new System.Windows.Forms.Button();
            this.Btn_MoveNotesFromCrm = new System.Windows.Forms.Button();
            this.Btn_MoveEmailsFromCrm = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.EmailsOutOfCrmCount = new System.Windows.Forms.Label();
            this.NotesOutOfCrmCount = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.CheckedEntities = new System.Windows.Forms.Label();
            this.TitleZone1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStripMenu.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.inCRM_Panel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.TitleZone1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tssSeparator1,
            this.Btn_RefreshCRM});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripMenu.Size = new System.Drawing.Size(1747, 32);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.Image = global::AttachmentsManagement.Properties.Resources.Close;
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.White;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(153, 29);
            this.tsbClose.Text = "Close this tool";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // Btn_RefreshCRM
            // 
            this.Btn_RefreshCRM.Image = global::AttachmentsManagement.Properties.Resources.dynamics365logo;
            this.Btn_RefreshCRM.ImageTransparentColor = System.Drawing.Color.White;
            this.Btn_RefreshCRM.Name = "Btn_RefreshCRM";
            this.Btn_RefreshCRM.Size = new System.Drawing.Size(230, 29);
            this.Btn_RefreshCRM.Text = "Refresh Data From CRM";
            this.Btn_RefreshCRM.Click += new System.EventHandler(this.Btn_RefreshCRM_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Controls.Add(this.panel4);
            this.flowLayoutPanel1.Controls.Add(this.inCRM_Panel);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.panel5);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(34, 74);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1658, 475);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.tableLayoutPanel1);
            this.panel4.Controls.Add(this.panel1);
            this.panel4.Location = new System.Drawing.Point(50, 120);
            this.panel4.Margin = new System.Windows.Forms.Padding(50, 120, 0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(614, 216);
            this.panel4.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.EmailsInCrmCount, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.NotesInCrmCount, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(-1, 57);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(614, 158);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label4.Location = new System.Drawing.Point(25, 25);
            this.label4.Margin = new System.Windows.Forms.Padding(25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(223, 29);
            this.label4.TabIndex = 0;
            this.label4.Text = "Emails Attachments\r\n";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label5.Location = new System.Drawing.Point(25, 104);
            this.label5.Margin = new System.Windows.Forms.Padding(25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(219, 29);
            this.label5.TabIndex = 1;
            this.label5.Text = "Notes Attachments";
            // 
            // EmailsInCrmCount
            // 
            this.EmailsInCrmCount.AutoSize = true;
            this.EmailsInCrmCount.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.EmailsInCrmCount.Location = new System.Drawing.Point(479, 25);
            this.EmailsInCrmCount.Margin = new System.Windows.Forms.Padding(50, 25, 25, 25);
            this.EmailsInCrmCount.Name = "EmailsInCrmCount";
            this.EmailsInCrmCount.Size = new System.Drawing.Size(28, 29);
            this.EmailsInCrmCount.TabIndex = 2;
            this.EmailsInCrmCount.Text = "0";
            // 
            // NotesInCrmCount
            // 
            this.NotesInCrmCount.AutoSize = true;
            this.NotesInCrmCount.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.NotesInCrmCount.Location = new System.Drawing.Point(479, 104);
            this.NotesInCrmCount.Margin = new System.Windows.Forms.Padding(50, 25, 25, 25);
            this.NotesInCrmCount.Name = "NotesInCrmCount";
            this.NotesInCrmCount.Size = new System.Drawing.Size(28, 29);
            this.NotesInCrmCount.TabIndex = 3;
            this.NotesInCrmCount.Text = "0";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(-1, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(614, 59);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Light", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(518, 54);
            this.label2.TabIndex = 0;
            this.label2.Text = "Attachments in Dynamics 365";
            // 
            // inCRM_Panel
            // 
            this.inCRM_Panel.Controls.Add(this.Btn_MoveNotesToCrm);
            this.inCRM_Panel.Controls.Add(this.Btn_MoveEmailsToCrm);
            this.inCRM_Panel.Controls.Add(this.Btn_MoveNotesFromCrm);
            this.inCRM_Panel.Controls.Add(this.Btn_MoveEmailsFromCrm);
            this.inCRM_Panel.Location = new System.Drawing.Point(714, 120);
            this.inCRM_Panel.Margin = new System.Windows.Forms.Padding(50, 120, 0, 50);
            this.inCRM_Panel.Name = "inCRM_Panel";
            this.inCRM_Panel.Size = new System.Drawing.Size(230, 216);
            this.inCRM_Panel.TabIndex = 0;
            // 
            // Btn_MoveNotesToCrm
            // 
            this.Btn_MoveNotesToCrm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(140)))), ((int)(((byte)(255)))));
            this.Btn_MoveNotesToCrm.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_MoveNotesToCrm.ForeColor = System.Drawing.Color.White;
            this.Btn_MoveNotesToCrm.Location = new System.Drawing.Point(0, 178);
            this.Btn_MoveNotesToCrm.Margin = new System.Windows.Forms.Padding(0);
            this.Btn_MoveNotesToCrm.Name = "Btn_MoveNotesToCrm";
            this.Btn_MoveNotesToCrm.Size = new System.Drawing.Size(230, 39);
            this.Btn_MoveNotesToCrm.TabIndex = 3;
            this.Btn_MoveNotesToCrm.Text = "Move notes to CRM";
            this.Btn_MoveNotesToCrm.UseVisualStyleBackColor = false;
            this.Btn_MoveNotesToCrm.Click += new System.EventHandler(this.Btn_MoveNotesToCrm_Click);
            // 
            // Btn_MoveEmailsToCrm
            // 
            this.Btn_MoveEmailsToCrm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(140)))), ((int)(((byte)(255)))));
            this.Btn_MoveEmailsToCrm.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_MoveEmailsToCrm.ForeColor = System.Drawing.Color.White;
            this.Btn_MoveEmailsToCrm.Location = new System.Drawing.Point(0, 99);
            this.Btn_MoveEmailsToCrm.Margin = new System.Windows.Forms.Padding(0);
            this.Btn_MoveEmailsToCrm.Name = "Btn_MoveEmailsToCrm";
            this.Btn_MoveEmailsToCrm.Size = new System.Drawing.Size(230, 39);
            this.Btn_MoveEmailsToCrm.TabIndex = 2;
            this.Btn_MoveEmailsToCrm.Text = "Move emails to CRM";
            this.Btn_MoveEmailsToCrm.UseVisualStyleBackColor = false;
            this.Btn_MoveEmailsToCrm.Click += new System.EventHandler(this.Btn_MoveEmailsToCrm_Click);
            // 
            // Btn_MoveNotesFromCrm
            // 
            this.Btn_MoveNotesFromCrm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(140)))), ((int)(((byte)(255)))));
            this.Btn_MoveNotesFromCrm.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_MoveNotesFromCrm.ForeColor = System.Drawing.Color.White;
            this.Btn_MoveNotesFromCrm.Location = new System.Drawing.Point(0, 138);
            this.Btn_MoveNotesFromCrm.Margin = new System.Windows.Forms.Padding(0);
            this.Btn_MoveNotesFromCrm.Name = "Btn_MoveNotesFromCrm";
            this.Btn_MoveNotesFromCrm.Size = new System.Drawing.Size(230, 39);
            this.Btn_MoveNotesFromCrm.TabIndex = 1;
            this.Btn_MoveNotesFromCrm.Text = "Move notes to Azure";
            this.Btn_MoveNotesFromCrm.UseVisualStyleBackColor = false;
            this.Btn_MoveNotesFromCrm.Click += new System.EventHandler(this.Btn_MoveNotesFromCrm_Click);
            // 
            // Btn_MoveEmailsFromCrm
            // 
            this.Btn_MoveEmailsFromCrm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(140)))), ((int)(((byte)(255)))));
            this.Btn_MoveEmailsFromCrm.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_MoveEmailsFromCrm.ForeColor = System.Drawing.Color.White;
            this.Btn_MoveEmailsFromCrm.Location = new System.Drawing.Point(0, 59);
            this.Btn_MoveEmailsFromCrm.Margin = new System.Windows.Forms.Padding(0);
            this.Btn_MoveEmailsFromCrm.Name = "Btn_MoveEmailsFromCrm";
            this.Btn_MoveEmailsFromCrm.Size = new System.Drawing.Size(230, 39);
            this.Btn_MoveEmailsFromCrm.TabIndex = 0;
            this.Btn_MoveEmailsFromCrm.Text = "Move emails to Azure";
            this.Btn_MoveEmailsFromCrm.UseVisualStyleBackColor = false;
            this.Btn_MoveEmailsFromCrm.Click += new System.EventHandler(this.Btn_MoveEmailsFromCrm_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.tableLayoutPanel2);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Location = new System.Drawing.Point(994, 120);
            this.panel2.Margin = new System.Windows.Forms.Padding(50, 120, 0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(614, 216);
            this.panel2.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.EmailsOutOfCrmCount, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.NotesOutOfCrmCount, 1, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(-1, 57);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(614, 158);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label6.Location = new System.Drawing.Point(25, 25);
            this.label6.Margin = new System.Windows.Forms.Padding(25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(223, 29);
            this.label6.TabIndex = 0;
            this.label6.Text = "Emails Attachments";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label7.Location = new System.Drawing.Point(25, 104);
            this.label7.Margin = new System.Windows.Forms.Padding(25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(219, 29);
            this.label7.TabIndex = 1;
            this.label7.Text = "Notes Attachments";
            // 
            // EmailsOutOfCrmCount
            // 
            this.EmailsOutOfCrmCount.AutoSize = true;
            this.EmailsOutOfCrmCount.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.EmailsOutOfCrmCount.Location = new System.Drawing.Point(479, 25);
            this.EmailsOutOfCrmCount.Margin = new System.Windows.Forms.Padding(50, 25, 25, 25);
            this.EmailsOutOfCrmCount.Name = "EmailsOutOfCrmCount";
            this.EmailsOutOfCrmCount.Size = new System.Drawing.Size(28, 29);
            this.EmailsOutOfCrmCount.TabIndex = 2;
            this.EmailsOutOfCrmCount.Text = "0";
            // 
            // NotesOutOfCrmCount
            // 
            this.NotesOutOfCrmCount.AutoSize = true;
            this.NotesOutOfCrmCount.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.NotesOutOfCrmCount.Location = new System.Drawing.Point(479, 104);
            this.NotesOutOfCrmCount.Margin = new System.Windows.Forms.Padding(50, 25, 25, 25);
            this.NotesOutOfCrmCount.Name = "NotesOutOfCrmCount";
            this.NotesOutOfCrmCount.Size = new System.Drawing.Size(28, 29);
            this.NotesOutOfCrmCount.TabIndex = 3;
            this.NotesOutOfCrmCount.Text = "0";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(-1, -1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(614, 59);
            this.panel3.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Light", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(511, 54);
            this.label3.TabIndex = 0;
            this.label3.Text = "Attachments moved to Azure";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.CheckedEntities);
            this.panel5.Location = new System.Drawing.Point(50, 386);
            this.panel5.Margin = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1558, 85);
            this.panel5.TabIndex = 3;
            // 
            // CheckedEntities
            // 
            this.CheckedEntities.AllowDrop = true;
            this.CheckedEntities.AutoSize = true;
            this.CheckedEntities.Location = new System.Drawing.Point(0, 0);
            this.CheckedEntities.Margin = new System.Windows.Forms.Padding(0);
            this.CheckedEntities.Name = "CheckedEntities";
            this.CheckedEntities.Size = new System.Drawing.Size(0, 20);
            this.CheckedEntities.TabIndex = 0;
            // 
            // TitleZone1
            // 
            this.TitleZone1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(140)))), ((int)(((byte)(255)))));
            this.TitleZone1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TitleZone1.Controls.Add(this.label1);
            this.TitleZone1.Location = new System.Drawing.Point(34, 74);
            this.TitleZone1.Name = "TitleZone1";
            this.TitleZone1.Size = new System.Drawing.Size(1658, 70);
            this.TitleZone1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Light", 22F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(755, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "Manage your organization attachments\r\n";
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.TitleZone1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.toolStripMenu);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(1747, 840);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.inCRM_Panel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.TitleZone1.ResumeLayout(false);
            this.TitleZone1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel TitleZone1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel inCRM_Panel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripButton Btn_RefreshCRM;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label EmailsInCrmCount;
        private System.Windows.Forms.Label NotesInCrmCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label EmailsOutOfCrmCount;
        private System.Windows.Forms.Label NotesOutOfCrmCount;
        private System.Windows.Forms.Button Btn_MoveEmailsFromCrm;
        private System.Windows.Forms.Button Btn_MoveNotesFromCrm;
        private System.Windows.Forms.Button Btn_MoveEmailsToCrm;
        private System.Windows.Forms.Button Btn_MoveNotesToCrm;
        private System.Windows.Forms.Label CheckedEntities;
    }
}
