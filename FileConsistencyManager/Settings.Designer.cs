namespace FileConsistencyManager
{
    partial class Settings
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
            tbServer = new TextBox();
            lblServer = new Label();
            lblDatabase = new Label();
            lblUserId = new Label();
            lblPassword = new Label();
            lblArchivePath = new Label();
            tbDatabase = new TextBox();
            tbUserId = new TextBox();
            tbPassword = new TextBox();
            tbArchivePath = new TextBox();
            lblLanguage = new Label();
            lblConnectionTitle = new Label();
            btnSave = new Button();
            lblPathTitle = new Label();
            btnConnectionTest = new Button();
            lblLanguageTitle = new Label();
            cmbLanguage = new ComboBox();
            SuspendLayout();
            // 
            // tbServer
            // 
            tbServer.Location = new Point(24, 56);
            tbServer.Margin = new Padding(2, 2, 2, 2);
            tbServer.Name = "tbServer";
            tbServer.Size = new Size(351, 23);
            tbServer.TabIndex = 0;
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(24, 40);
            lblServer.Margin = new Padding(2, 0, 2, 0);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(42, 15);
            lblServer.TabIndex = 1;
            lblServer.Text = "Server:";
            // 
            // lblDatabase
            // 
            lblDatabase.AutoSize = true;
            lblDatabase.Location = new Point(24, 83);
            lblDatabase.Margin = new Padding(2, 0, 2, 0);
            lblDatabase.Name = "lblDatabase";
            lblDatabase.Size = new Size(58, 15);
            lblDatabase.TabIndex = 2;
            lblDatabase.Text = "Database:";
            // 
            // lblUserId
            // 
            lblUserId.AutoSize = true;
            lblUserId.Location = new Point(24, 127);
            lblUserId.Margin = new Padding(2, 0, 2, 0);
            lblUserId.Name = "lblUserId";
            lblUserId.Size = new Size(44, 15);
            lblUserId.TabIndex = 3;
            lblUserId.Text = "UserID:";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(24, 169);
            lblPassword.Margin = new Padding(2, 0, 2, 0);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(60, 15);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Password:";
            // 
            // lblArchivePath
            // 
            lblArchivePath.AutoSize = true;
            lblArchivePath.Location = new Point(24, 304);
            lblArchivePath.Margin = new Padding(2, 0, 2, 0);
            lblArchivePath.Name = "lblArchivePath";
            lblArchivePath.Size = new Size(77, 15);
            lblArchivePath.TabIndex = 5;
            lblArchivePath.Text = "Archive Path:";
            // 
            // tbDatabase
            // 
            tbDatabase.Location = new Point(24, 100);
            tbDatabase.Margin = new Padding(2, 2, 2, 2);
            tbDatabase.Name = "tbDatabase";
            tbDatabase.Size = new Size(351, 23);
            tbDatabase.TabIndex = 6;
            // 
            // tbUserId
            // 
            tbUserId.Location = new Point(24, 143);
            tbUserId.Margin = new Padding(2, 2, 2, 2);
            tbUserId.Name = "tbUserId";
            tbUserId.Size = new Size(351, 23);
            tbUserId.TabIndex = 7;
            // 
            // tbPassword
            // 
            tbPassword.Location = new Point(24, 186);
            tbPassword.Margin = new Padding(2, 2, 2, 2);
            tbPassword.Name = "tbPassword";
            tbPassword.PasswordChar = '*';
            tbPassword.Size = new Size(351, 23);
            tbPassword.TabIndex = 8;
            // 
            // tbArchivePath
            // 
            tbArchivePath.Location = new Point(24, 320);
            tbArchivePath.Margin = new Padding(2, 2, 2, 2);
            tbArchivePath.Name = "tbArchivePath";
            tbArchivePath.Size = new Size(351, 23);
            tbArchivePath.TabIndex = 9;
            // 
            // lblLanguage
            // 
            lblLanguage.AutoSize = true;
            lblLanguage.Location = new Point(24, 388);
            lblLanguage.Margin = new Padding(2, 0, 2, 0);
            lblLanguage.Name = "lblLanguage";
            lblLanguage.Size = new Size(62, 15);
            lblLanguage.TabIndex = 10;
            lblLanguage.Text = "Language:";
            // 
            // lblConnectionTitle
            // 
            lblConnectionTitle.AutoSize = true;
            lblConnectionTitle.Font = new Font("Segoe UI", 12F);
            lblConnectionTitle.Location = new Point(24, 5);
            lblConnectionTitle.Margin = new Padding(2, 0, 2, 0);
            lblConnectionTitle.Name = "lblConnectionTitle";
            lblConnectionTitle.Size = new Size(157, 21);
            lblConnectionTitle.TabIndex = 12;
            lblConnectionTitle.Text = "Database Connection";
            // 
            // btnSave
            // 
            btnSave.Font = new Font("Segoe UI", 12F);
            btnSave.Location = new Point(24, 447);
            btnSave.Margin = new Padding(2, 2, 2, 2);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(350, 35);
            btnSave.TabIndex = 13;
            btnSave.Text = "Save General Settings";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += SaveCurrentSettings;
            // 
            // lblPathTitle
            // 
            lblPathTitle.AutoSize = true;
            lblPathTitle.Font = new Font("Segoe UI", 12F);
            lblPathTitle.Location = new Point(24, 278);
            lblPathTitle.Margin = new Padding(2, 0, 2, 0);
            lblPathTitle.Name = "lblPathTitle";
            lblPathTitle.Size = new Size(94, 21);
            lblPathTitle.TabIndex = 14;
            lblPathTitle.Text = "Default Path";
            // 
            // btnConnectionTest
            // 
            btnConnectionTest.Font = new Font("Segoe UI", 12F);
            btnConnectionTest.Location = new Point(24, 227);
            btnConnectionTest.Margin = new Padding(2, 2, 2, 2);
            btnConnectionTest.Name = "btnConnectionTest";
            btnConnectionTest.Size = new Size(350, 35);
            btnConnectionTest.TabIndex = 15;
            btnConnectionTest.Text = "Test Connection";
            btnConnectionTest.UseVisualStyleBackColor = true;
            btnConnectionTest.Click += TestConnection;
            // 
            // lblLanguageTitle
            // 
            lblLanguageTitle.AutoSize = true;
            lblLanguageTitle.Font = new Font("Segoe UI", 12F);
            lblLanguageTitle.Location = new Point(24, 363);
            lblLanguageTitle.Margin = new Padding(2, 0, 2, 0);
            lblLanguageTitle.Name = "lblLanguageTitle";
            lblLanguageTitle.Size = new Size(132, 21);
            lblLanguageTitle.TabIndex = 16;
            lblLanguageTitle.Text = "Default Language";
            // 
            // cmbLanguage
            // 
            cmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLanguage.FormattingEnabled = true;
            cmbLanguage.Items.AddRange(new object[] { "en", "de" });
            cmbLanguage.Location = new Point(24, 405);
            cmbLanguage.Margin = new Padding(2, 2, 2, 2);
            cmbLanguage.Name = "cmbLanguage";
            cmbLanguage.Size = new Size(351, 23);
            cmbLanguage.TabIndex = 17;
            // 
            // Settings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(399, 490);
            Controls.Add(cmbLanguage);
            Controls.Add(lblLanguageTitle);
            Controls.Add(btnConnectionTest);
            Controls.Add(lblPathTitle);
            Controls.Add(btnSave);
            Controls.Add(lblConnectionTitle);
            Controls.Add(lblLanguage);
            Controls.Add(tbArchivePath);
            Controls.Add(tbPassword);
            Controls.Add(tbUserId);
            Controls.Add(tbDatabase);
            Controls.Add(lblArchivePath);
            Controls.Add(lblPassword);
            Controls.Add(lblUserId);
            Controls.Add(lblDatabase);
            Controls.Add(lblServer);
            Controls.Add(tbServer);
            Margin = new Padding(2, 2, 2, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Settings";
            Text = "FileConsistencyManager";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbServer;
        private Label lblServer;
        private Label lblDatabase;
        private Label lblUserId;
        private Label lblPassword;
        private Label lblArchivePath;
        private TextBox tbDatabase;
        private TextBox tbUserId;
        private TextBox tbPassword;
        private TextBox tbArchivePath;
        private Label lblLanguage;
        private Label lblConnectionTitle;
        private Button btnSave;
        private Label lblPathTitle;
        private Button btnConnectionTest;
        private Label lblLanguageTitle;
        private ComboBox cmbLanguage;
    }
}