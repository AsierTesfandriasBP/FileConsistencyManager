using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static FileConsistencyManager.Models.CustomMessageBoxTypes;
using FileConsistencyManager.Localization;

namespace FileConsistencyManager
{
    internal partial class CustomMessageBoxForm : Form
    {
        public CustomDialogResult Result { get; private set; } = CustomDialogResult.None;
        private Localize _localization;

        public CustomMessageBoxForm(string message, string title, CustomMessageBoxButtons buttons, CustomMessageBoxIcon icon, Localize lang)
        {
            InitializeComponent();

            SetupTableLayoutPanel();

            // for pressing the enter or escape key
            //this.AcceptButton = btnOK;
            //this.CancelButton = btnCancel;

            lblMessage.Text = message;
            _localization = lang;
            this.BackColor = Color.White;
            pnlButtons.BackColor = Color.FromArgb(240, 240, 240);

            SetupIconAndTitle(icon, title, lang);
            SetupButtons(buttons);
        }

        private void SetupTableLayoutPanel()
        {
            tblPanel.BringToFront();
        }

        private void SetupIconAndTitle(CustomMessageBoxIcon icon, string title, Localize lang)
        {
            switch (icon)
            {
                case CustomMessageBoxIcon.Information:
                    picIcon.Image = SystemIcons.Information.ToBitmap();
                    this.Text = lang.GetContent("CustomTextInformation", lang.GetCurrentLanguage());
                    break;
                case CustomMessageBoxIcon.Warning:
                    picIcon.Image = SystemIcons.Warning.ToBitmap();
                    this.Text = lang.GetContent("CustomTextWarning", lang.GetCurrentLanguage());
                    break;
                case CustomMessageBoxIcon.Error:
                    picIcon.Image = SystemIcons.Error.ToBitmap();
                    this.Text = lang.GetContent("CustomTextError", lang.GetCurrentLanguage());
                    break;
                default:
                    picIcon.Visible = false;
                    this.Text = title;
                    break;
            }
        }

        private void AddButton(string text, CustomDialogResult result)
        {
            var btn = new Button
            {
                Text = text,
                AutoSize = true,
                MinimumSize = new Size(120, 30),
                Margin = new Padding(5)
            };

            btn.Click += (s, e) =>
            {
                Result = result;
                this.Close();
            };

            pnlButtons.Controls.Add(btn);
        }

        private void SetupButtons(CustomMessageBoxButtons buttons)
        {
            string lang = _localization.GetCurrentLanguage();

            pnlButtons.Controls.Clear();

            switch (buttons)
            {
                case CustomMessageBoxButtons.OK:
                    AddButton(_localization.GetContent("CustomTextOK", lang), CustomDialogResult.OK);
                    break;

                case CustomMessageBoxButtons.OKCancel:
                    AddButton(_localization.GetContent("CustomTextOK", lang), CustomDialogResult.OK);
                    AddButton(_localization.GetContent("CustomTextCancel", lang), CustomDialogResult.Cancel);
                    break;

                case CustomMessageBoxButtons.YesNo:
                    AddButton(_localization.GetContent("CustomTextYes", lang), CustomDialogResult.Yes);
                    AddButton(_localization.GetContent("CustomTextNo", lang), CustomDialogResult.No);
                    break;

                case CustomMessageBoxButtons.YesNoCancel:
                    AddButton(_localization.GetContent("CustomTextYes", lang), CustomDialogResult.Yes);
                    AddButton(_localization.GetContent("CustomTextNo", lang), CustomDialogResult.No);
                    AddButton(_localization.GetContent("CustomTextCancel", lang), CustomDialogResult.Cancel);
                    break;
            }
        }
    }

    // Helper class to show the custom message box easily
    internal static class CustomMessageBox
    {
        public static CustomDialogResult Show(
            string message,
            string title,
            CustomMessageBoxButtons buttons = CustomMessageBoxButtons.OK,
            CustomMessageBoxIcon icon = CustomMessageBoxIcon.None,
            Localize lang = null)
        {
            using (var form = new CustomMessageBoxForm(message, title, buttons, icon, lang))
            {
                form.ShowDialog();
                return form.Result;
            }
        }
    }
}
