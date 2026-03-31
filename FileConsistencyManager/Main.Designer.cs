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
            lb_cmbFilterTitle = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new Point(12, 406);
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
            cmbFilter.Location = new Point(538, 406);
            cmbFilter.Name = "cmbFilter";
            cmbFilter.Size = new Size(250, 33);
            cmbFilter.TabIndex = 1;
            cmbFilter.SelectedIndexChanged += cmbFilter_SelectedIndexChanged;
            // 
            // dgvResults
            // 
            dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResults.Location = new Point(12, 43);
            dgvResults.Name = "dgvResults";
            dgvResults.RowHeadersWidth = 62;
            dgvResults.Size = new Size(776, 313);
            dgvResults.TabIndex = 2;
            // 
            // lb_cmbFilterTitle
            // 
            lb_cmbFilterTitle.AutoSize = true;
            lb_cmbFilterTitle.Location = new Point(538, 378);
            lb_cmbFilterTitle.Name = "lb_cmbFilterTitle";
            lb_cmbFilterTitle.Size = new Size(54, 25);
            lb_cmbFilterTitle.TabIndex = 3;
            lb_cmbFilterTitle.Text = "Filter:";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lb_cmbFilterTitle);
            Controls.Add(dgvResults);
            Controls.Add(cmbFilter);
            Controls.Add(btnStart);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FileConsistencyManager";
            ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStart;
        private ComboBox cmbFilter;
        private DataGridView dgvResults;
        private Label lb_cmbFilterTitle;
    }
}
