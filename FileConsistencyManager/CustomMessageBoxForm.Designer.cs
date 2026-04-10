namespace FileConsistencyManager
{
    partial class CustomMessageBoxForm
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
            pnlButtons = new FlowLayoutPanel();
            lblMessage = new Label();
            picIcon = new PictureBox();
            tblPanel = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)picIcon).BeginInit();
            tblPanel.SuspendLayout();
            SuspendLayout();
            // 
            // pnlButtons
            // 
            tblPanel.SetColumnSpan(pnlButtons, 2);
            pnlButtons.Dock = DockStyle.Fill;
            pnlButtons.FlowDirection = FlowDirection.RightToLeft;
            pnlButtons.Location = new Point(3, 158);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new Size(556, 44);
            pnlButtons.TabIndex = 0;
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Dock = DockStyle.Fill;
            lblMessage.Font = new Font("Segoe UI", 9F);
            lblMessage.Location = new Point(87, 0);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(472, 155);
            lblMessage.TabIndex = 1;
            lblMessage.Text = "Test";
            lblMessage.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // picIcon
            // 
            picIcon.Dock = DockStyle.Fill;
            picIcon.Location = new Point(10, 10);
            picIcon.Margin = new Padding(10);
            picIcon.Name = "picIcon";
            picIcon.Size = new Size(64, 135);
            picIcon.SizeMode = PictureBoxSizeMode.CenterImage;
            picIcon.TabIndex = 2;
            picIcon.TabStop = false;
            // 
            // tblPanel
            // 
            tblPanel.ColumnCount = 2;
            tblPanel.ColumnStyles.Add(new ColumnStyle());
            tblPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblPanel.Controls.Add(lblMessage, 1, 0);
            tblPanel.Controls.Add(picIcon, 0, 0);
            tblPanel.Controls.Add(pnlButtons, 0, 1);
            tblPanel.Dock = DockStyle.Fill;
            tblPanel.Location = new Point(0, 0);
            tblPanel.Name = "tblPanel";
            tblPanel.RowCount = 2;
            tblPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblPanel.RowStyles.Add(new RowStyle());
            tblPanel.Size = new Size(562, 205);
            tblPanel.TabIndex = 3;
            // 
            // CustomMessageBoxForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(562, 205);
            Controls.Add(tblPanel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CustomMessageBoxForm";
            StartPosition = FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)picIcon).EndInit();
            tblPanel.ResumeLayout(false);
            tblPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel pnlButtons;
        private Label lblMessage;
        private PictureBox picIcon;
        private TableLayoutPanel tblPanel;
    }
}