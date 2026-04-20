namespace FileConsistencyManager
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            btnStart = new Button();
            cmbFilter = new ComboBox();
            lblCmbFilterTitle = new Label();
            btnArchive = new Button();
            btnDelete = new Button();
            pbProgress = new ProgressBar();
            cmbLanguage = new ComboBox();
            lblCmbLanguageTitle = new Label();
            lblStatus = new Label();
            lblMissing = new Label();
            lblOrphan = new Label();
            lblMissingCount = new Label();
            lblOrphanCount = new Label();
            dgvResults = new DataGridView();
            tip = new ToolTip(components);
            statusStrip1 = new StatusStrip();
            lblConnectionLabel = new ToolStripStatusLabel();
            lblEntriesFound = new ToolStripStatusLabel();
            lblEntriesFoundCount = new ToolStripStatusLabel();
            btnSettings = new Button();
            lblExistsCount = new Label();
            lblExists = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new Point(12, 542);
            btnStart.Margin = new Padding(2, 2, 2, 2);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(155, 29);
            btnStart.TabIndex = 0;
            btnStart.Text = "Start scan";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // cmbFilter
            // 
            cmbFilter.FormattingEnabled = true;
            cmbFilter.Location = new Point(907, 551);
            cmbFilter.Margin = new Padding(2, 2, 2, 2);
            cmbFilter.Name = "cmbFilter";
            cmbFilter.Size = new Size(177, 23);
            cmbFilter.TabIndex = 1;
            cmbFilter.SelectedIndexChanged += cmbFilter_SelectedIndexChanged;
            // 
            // lblCmbFilterTitle
            // 
            lblCmbFilterTitle.AutoSize = true;
            lblCmbFilterTitle.Location = new Point(907, 534);
            lblCmbFilterTitle.Margin = new Padding(2, 0, 2, 0);
            lblCmbFilterTitle.Name = "lblCmbFilterTitle";
            lblCmbFilterTitle.Size = new Size(36, 15);
            lblCmbFilterTitle.TabIndex = 3;
            lblCmbFilterTitle.Text = "Filter:";
            // 
            // btnArchive
            // 
            btnArchive.Location = new Point(729, 500);
            btnArchive.Margin = new Padding(2, 2, 2, 2);
            btnArchive.Name = "btnArchive";
            btnArchive.Size = new Size(174, 29);
            btnArchive.TabIndex = 6;
            btnArchive.Text = "Archive";
            btnArchive.UseVisualStyleBackColor = true;
            btnArchive.Click += btnArchive_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(907, 500);
            btnDelete.Margin = new Padding(2, 2, 2, 2);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(176, 29);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "Delete [Permanent]";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // pbProgress
            // 
            pbProgress.Location = new Point(172, 549);
            pbProgress.Margin = new Padding(2, 2, 2, 2);
            pbProgress.Name = "pbProgress";
            pbProgress.Size = new Size(732, 22);
            pbProgress.TabIndex = 8;
            // 
            // cmbLanguage
            // 
            cmbLanguage.FormattingEnabled = true;
            cmbLanguage.Location = new Point(911, 28);
            cmbLanguage.Margin = new Padding(2, 2, 2, 2);
            cmbLanguage.Name = "cmbLanguage";
            cmbLanguage.Size = new Size(173, 23);
            cmbLanguage.TabIndex = 9;
            cmbLanguage.SelectedIndexChanged += cmbLanguage_SelectedIndexChanged;
            // 
            // lblCmbLanguageTitle
            // 
            lblCmbLanguageTitle.AutoSize = true;
            lblCmbLanguageTitle.Location = new Point(911, 11);
            lblCmbLanguageTitle.Margin = new Padding(2, 0, 2, 0);
            lblCmbLanguageTitle.Name = "lblCmbLanguageTitle";
            lblCmbLanguageTitle.Size = new Size(62, 15);
            lblCmbLanguageTitle.TabIndex = 10;
            lblCmbLanguageTitle.Text = "Language:";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(172, 532);
            lblStatus.Margin = new Padding(2, 0, 2, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 15);
            lblStatus.TabIndex = 11;
            // 
            // lblMissing
            // 
            lblMissing.AutoSize = true;
            lblMissing.Location = new Point(41, 20);
            lblMissing.Margin = new Padding(2, 0, 2, 0);
            lblMissing.Name = "lblMissing";
            lblMissing.Size = new Size(106, 15);
            lblMissing.TabIndex = 12;
            lblMissing.Text = "Entries in Database";
            // 
            // lblOrphan
            // 
            lblOrphan.AutoSize = true;
            lblOrphan.Location = new Point(41, 35);
            lblOrphan.Margin = new Padding(2, 0, 2, 0);
            lblOrphan.Name = "lblOrphan";
            lblOrphan.Size = new Size(88, 15);
            lblOrphan.TabIndex = 13;
            lblOrphan.Text = "Files in Filepath";
            // 
            // lblMissingCount
            // 
            lblMissingCount.AutoSize = true;
            lblMissingCount.Location = new Point(10, 20);
            lblMissingCount.Margin = new Padding(2, 0, 2, 0);
            lblMissingCount.Name = "lblMissingCount";
            lblMissingCount.Size = new Size(12, 15);
            lblMissingCount.TabIndex = 14;
            lblMissingCount.Text = "/";
            // 
            // lblOrphanCount
            // 
            lblOrphanCount.AutoSize = true;
            lblOrphanCount.Location = new Point(10, 35);
            lblOrphanCount.Margin = new Padding(2, 0, 2, 0);
            lblOrphanCount.Name = "lblOrphanCount";
            lblOrphanCount.Size = new Size(12, 15);
            lblOrphanCount.TabIndex = 15;
            lblOrphanCount.Text = "/";
            // 
            // dgvResults
            // 
            dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResults.Location = new Point(12, 53);
            dgvResults.Name = "dgvResults";
            dgvResults.RowHeadersWidth = 62;
            dgvResults.Size = new Size(1071, 442);
            dgvResults.TabIndex = 16;
            dgvResults.CellFormatting += dgvResults_CellFormatting;
            dgvResults.SelectionChanged += dgvResults_SelectionChanged;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblConnectionLabel, lblEntriesFound, lblEntriesFoundCount });
            statusStrip1.Location = new Point(0, 583);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1092, 22);
            statusStrip1.TabIndex = 18;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblConnectionLabel
            // 
            lblConnectionLabel.Name = "lblConnectionLabel";
            lblConnectionLabel.Size = new Size(933, 17);
            lblConnectionLabel.Spring = true;
            lblConnectionLabel.Text = "Connected to: MSSQL-Server Database";
            lblConnectionLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEntriesFound
            // 
            lblEntriesFound.Name = "lblEntriesFound";
            lblEntriesFound.Size = new Size(132, 17);
            lblEntriesFound.Text = "Entries in DataGridView:";
            lblEntriesFound.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblEntriesFoundCount
            // 
            lblEntriesFoundCount.Name = "lblEntriesFoundCount";
            lblEntriesFoundCount.Size = new Size(12, 17);
            lblEntriesFoundCount.Text = "/";
            // 
            // btnSettings
            // 
            btnSettings.Location = new Point(12, 500);
            btnSettings.Margin = new Padding(2, 2, 2, 2);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(156, 29);
            btnSettings.TabIndex = 19;
            btnSettings.Text = "Config Settings";
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += btnSettings_Click;
            // 
            // lblExistsCount
            // 
            lblExistsCount.AutoSize = true;
            lblExistsCount.Location = new Point(10, 5);
            lblExistsCount.Margin = new Padding(2, 0, 2, 0);
            lblExistsCount.Name = "lblExistsCount";
            lblExistsCount.Size = new Size(12, 15);
            lblExistsCount.TabIndex = 20;
            lblExistsCount.Text = "/";
            // 
            // lblExists
            // 
            lblExists.AutoSize = true;
            lblExists.Location = new Point(41, 5);
            lblExists.Margin = new Padding(2, 0, 2, 0);
            lblExists.Name = "lblExists";
            lblExists.Size = new Size(202, 15);
            lblExists.TabIndex = 21;
            lblExists.Text = "Entries/Files in Database and Filepath";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1092, 605);
            Controls.Add(lblExists);
            Controls.Add(lblExistsCount);
            Controls.Add(btnSettings);
            Controls.Add(statusStrip1);
            Controls.Add(dgvResults);
            Controls.Add(lblOrphanCount);
            Controls.Add(lblMissingCount);
            Controls.Add(lblOrphan);
            Controls.Add(lblMissing);
            Controls.Add(lblStatus);
            Controls.Add(lblCmbLanguageTitle);
            Controls.Add(cmbLanguage);
            Controls.Add(pbProgress);
            Controls.Add(btnDelete);
            Controls.Add(btnArchive);
            Controls.Add(lblCmbFilterTitle);
            Controls.Add(cmbFilter);
            Controls.Add(btnStart);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            HelpButton = true;
            Margin = new Padding(2, 2, 2, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FileConsistencyManager";
            ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStart;
        private ComboBox cmbFilter;
        private Label lblCmbFilterTitle;
        private Button button2;
        private Button btnArchive;
        private Button btnDelete;
        private ProgressBar pbProgress;
        private ComboBox cmbLanguage;
        private Label lblCmbLanguageTitle;
        private Label lblStatus;
        private Label lblMissing;
        private Label lblOrphan;
        private Label lblMissingCount;
        private Label lblOrphanCount;
        private DataGridView dgvResults;
        private ToolTip tip;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblConnectionLabel;
        private Button btnSettings;
        private ToolStripStatusLabel lblEntriesFound;
        private ToolStripStatusLabel lblEntriesFoundCount;
        private Label lblExistsCount;
        private Label lblExists;
    }
}
