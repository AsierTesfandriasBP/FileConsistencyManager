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
            btnStart = new Button();
            cmbFilter = new ComboBox();
            dgvResults = new DataGridView();
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
            ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new Point(12, 601);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(156, 32);
            btnStart.TabIndex = 0;
            btnStart.Text = "Scan starten";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // cmbFilter
            // 
            cmbFilter.FormattingEnabled = true;
            cmbFilter.Location = new Point(962, 600);
            cmbFilter.Name = "cmbFilter";
            cmbFilter.Size = new Size(250, 33);
            cmbFilter.TabIndex = 1;
            cmbFilter.SelectedIndexChanged += cmbFilter_SelectedIndexChanged;
            // 
            // dgvResults
            // 
            dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResults.Location = new Point(12, 76);
            dgvResults.Name = "dgvResults";
            dgvResults.RowHeadersWidth = 62;
            dgvResults.Size = new Size(1200, 429);
            dgvResults.TabIndex = 2;
            dgvResults.CellFormatting += dgvResults_CellFormatting;
            dgvResults.SelectionChanged += dgvResults_SelectionChanged;
            // 
            // lblCmbFilterTitle
            // 
            lblCmbFilterTitle.AutoSize = true;
            lblCmbFilterTitle.Location = new Point(962, 572);
            lblCmbFilterTitle.Name = "lblCmbFilterTitle";
            lblCmbFilterTitle.Size = new Size(54, 25);
            lblCmbFilterTitle.TabIndex = 3;
            lblCmbFilterTitle.Text = "Filter:";
            // 
            // btnIgnore
            // 
            btnIgnore.Location = new Point(12, 511);
            btnIgnore.Name = "btnIgnore";
            btnIgnore.Size = new Size(156, 34);
            btnIgnore.TabIndex = 4;
            btnIgnore.Text = "Ignorieren";
            btnIgnore.UseVisualStyleBackColor = true;
            // 
            // btnArchive
            // 
            btnArchive.Location = new Point(174, 511);
            btnArchive.Name = "btnArchive";
            btnArchive.Size = new Size(156, 34);
            btnArchive.TabIndex = 6;
            btnArchive.Text = "Archivieren";
            btnArchive.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(962, 511);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(250, 34);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "Löschen [Permanent]";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // pbProgress
            // 
            pbProgress.Location = new Point(174, 601);
            pbProgress.Name = "pbProgress";
            pbProgress.Size = new Size(782, 32);
            pbProgress.TabIndex = 8;
            // 
            // cmbLanguage
            // 
            cmbLanguage.FormattingEnabled = true;
            cmbLanguage.Location = new Point(962, 37);
            cmbLanguage.Name = "cmbLanguage";
            cmbLanguage.Size = new Size(250, 33);
            cmbLanguage.TabIndex = 9;
            cmbLanguage.SelectedIndexChanged += cmbLanguage_SelectedIndexChanged;
            // 
            // lblCmbLanguageTitle
            // 
            lblCmbLanguageTitle.AutoSize = true;
            lblCmbLanguageTitle.Location = new Point(962, 9);
            lblCmbLanguageTitle.Name = "lblCmbLanguageTitle";
            lblCmbLanguageTitle.Size = new Size(79, 25);
            lblCmbLanguageTitle.TabIndex = 10;
            lblCmbLanguageTitle.Text = "Sprache:";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(174, 573);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 25);
            lblStatus.TabIndex = 11;
            // 
            // lblMissing
            // 
            lblMissing.AutoSize = true;
            lblMissing.Location = new Point(12, 9);
            lblMissing.Name = "lblMissing";
            lblMissing.Size = new Size(128, 25);
            lblMissing.TabIndex = 12;
            lblMissing.Text = "Missing Files: /";
            // 
            // lblOrphan
            // 
            lblOrphan.AutoSize = true;
            lblOrphan.Location = new Point(12, 40);
            lblOrphan.Name = "lblOrphan";
            lblOrphan.Size = new Size(127, 25);
            lblOrphan.TabIndex = 13;
            lblOrphan.Text = "Orphan Files: /";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1224, 645);
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
            Controls.Add(dgvResults);
            Controls.Add(cmbFilter);
            Controls.Add(btnStart);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FileConsistencyManager -|- Analyse- und Bereinigungstool";
            ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStart;
        private ComboBox cmbFilter;
        private DataGridView dgvResults;
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
    }
}
