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
            btnIgnore = new Button();
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
            lblStatusCount = new Label();
            tip = new ToolTip(components);
            statusStrip1 = new StatusStrip();
            lblConnectionLabel = new ToolStripStatusLabel();
            btnSettings = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new Point(16, 637);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(156, 38);
            btnStart.TabIndex = 0;
            btnStart.Text = "Scan starten";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // cmbFilter
            // 
            cmbFilter.FormattingEnabled = true;
            cmbFilter.Location = new Point(956, 637);
            cmbFilter.Name = "cmbFilter";
            cmbFilter.Size = new Size(251, 33);
            cmbFilter.TabIndex = 1;
            cmbFilter.SelectedIndexChanged += cmbFilter_SelectedIndexChanged;
            // 
            // lblCmbFilterTitle
            // 
            lblCmbFilterTitle.AutoSize = true;
            lblCmbFilterTitle.Location = new Point(956, 608);
            lblCmbFilterTitle.Name = "lblCmbFilterTitle";
            lblCmbFilterTitle.Size = new Size(54, 25);
            lblCmbFilterTitle.TabIndex = 3;
            lblCmbFilterTitle.Text = "Filter:";
            // 
            // btnIgnore
            // 
            btnIgnore.Location = new Point(16, 542);
            btnIgnore.Name = "btnIgnore";
            btnIgnore.Size = new Size(156, 48);
            btnIgnore.TabIndex = 4;
            btnIgnore.Text = "Ignorieren";
            btnIgnore.UseVisualStyleBackColor = true;
            btnIgnore.Click += btnIgnore_Click;
            // 
            // btnArchive
            // 
            btnArchive.Location = new Point(177, 542);
            btnArchive.Name = "btnArchive";
            btnArchive.Size = new Size(156, 48);
            btnArchive.TabIndex = 6;
            btnArchive.Text = "Archivieren";
            btnArchive.UseVisualStyleBackColor = true;
            btnArchive.Click += btnArchive_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(957, 542);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(251, 48);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "Löschen [Permanent]";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // pbProgress
            // 
            pbProgress.Location = new Point(177, 638);
            pbProgress.Name = "pbProgress";
            pbProgress.Size = new Size(773, 37);
            pbProgress.TabIndex = 8;
            // 
            // cmbLanguage
            // 
            cmbLanguage.FormattingEnabled = true;
            cmbLanguage.Location = new Point(961, 37);
            cmbLanguage.Name = "cmbLanguage";
            cmbLanguage.Size = new Size(245, 33);
            cmbLanguage.TabIndex = 9;
            cmbLanguage.SelectedIndexChanged += cmbLanguage_SelectedIndexChanged;
            // 
            // lblCmbLanguageTitle
            // 
            lblCmbLanguageTitle.AutoSize = true;
            lblCmbLanguageTitle.Location = new Point(961, 8);
            lblCmbLanguageTitle.Name = "lblCmbLanguageTitle";
            lblCmbLanguageTitle.Size = new Size(79, 25);
            lblCmbLanguageTitle.TabIndex = 10;
            lblCmbLanguageTitle.Text = "Sprache:";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(207, 610);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 25);
            lblStatus.TabIndex = 11;
            // 
            // lblMissing
            // 
            lblMissing.AutoSize = true;
            lblMissing.Location = new Point(61, 15);
            lblMissing.Name = "lblMissing";
            lblMissing.Size = new Size(144, 25);
            lblMissing.TabIndex = 12;
            lblMissing.Text = "Files in Database";
            // 
            // lblOrphan
            // 
            lblOrphan.AutoSize = true;
            lblOrphan.Location = new Point(61, 47);
            lblOrphan.Name = "lblOrphan";
            lblOrphan.Size = new Size(132, 25);
            lblOrphan.TabIndex = 13;
            lblOrphan.Text = "Files in Filepath";
            // 
            // lblMissingCount
            // 
            lblMissingCount.AutoSize = true;
            lblMissingCount.Location = new Point(17, 15);
            lblMissingCount.Name = "lblMissingCount";
            lblMissingCount.Size = new Size(19, 25);
            lblMissingCount.TabIndex = 14;
            lblMissingCount.Text = "/";
            // 
            // lblOrphanCount
            // 
            lblOrphanCount.AutoSize = true;
            lblOrphanCount.Location = new Point(17, 47);
            lblOrphanCount.Name = "lblOrphanCount";
            lblOrphanCount.Size = new Size(19, 25);
            lblOrphanCount.TabIndex = 15;
            lblOrphanCount.Text = "/";
            // 
            // dgvResults
            // 
            dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResults.Location = new Point(17, 83);
            dgvResults.Margin = new Padding(4, 5, 4, 5);
            dgvResults.Name = "dgvResults";
            dgvResults.RowHeadersWidth = 62;
            dgvResults.Size = new Size(1190, 450);
            dgvResults.TabIndex = 16;
            dgvResults.CellFormatting += dgvResults_CellFormatting;
            dgvResults.SelectionChanged += dgvResults_SelectionChanged;
            // 
            // lblStatusCount
            // 
            lblStatusCount.AutoSize = true;
            lblStatusCount.Location = new Point(179, 610);
            lblStatusCount.Margin = new Padding(4, 0, 4, 0);
            lblStatusCount.Name = "lblStatusCount";
            lblStatusCount.Size = new Size(0, 25);
            lblStatusCount.TabIndex = 17;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblConnectionLabel });
            statusStrip1.Location = new Point(0, 705);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 20, 0);
            statusStrip1.Size = new Size(1220, 32);
            statusStrip1.TabIndex = 18;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblConnectionLabel
            // 
            lblConnectionLabel.Name = "lblConnectionLabel";
            lblConnectionLabel.Size = new Size(106, 25);
            lblConnectionLabel.Text = "Connected: ";
            // 
            // btnSettings
            // 
            btnSettings.Location = new Point(649, 12);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(171, 41);
            btnSettings.TabIndex = 19;
            btnSettings.Text = "Settings";
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += btnSettings_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1220, 737);
            Controls.Add(btnSettings);
            Controls.Add(statusStrip1);
            Controls.Add(lblStatusCount);
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
            Controls.Add(btnIgnore);
            Controls.Add(lblCmbFilterTitle);
            Controls.Add(cmbFilter);
            Controls.Add(btnStart);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            HelpButton = true;
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
        private Button btnIgnore;
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
        private Label lblStatusCount;
        private ToolTip tip;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblConnectionLabel;
        private Button btnSettings;
    }
}
