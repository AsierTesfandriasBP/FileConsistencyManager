using System;
using System.Drawing;
using System.Windows.Forms;
using FileConsistencyManager.Localization;

namespace FileConsistencyManager.UI
{
    internal static class CustomMessageBoxEx
    {
        // Show overload similar to MessageBox.Show
        public static DialogResult Show(
            string text,
            string caption = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1,
            Localize lang = null,
            IWin32Window owner = null)
        {
            lang ??= new Localize("en");

            using (var form = new Form())
            {
                form.Text = caption ?? string.Empty;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.ShowInTaskbar = false;
                form.ClientSize = new Size(420, 140);
                form.Font = SystemFonts.MessageBoxFont;

                var padding = 12;

                // Icon
                var pic = new PictureBox
                {
                    Location = new Point(padding, padding),
                    Size = new Size(32, 32),
                    SizeMode = PictureBoxSizeMode.CenterImage
                };

                switch (icon)
                {
                    case MessageBoxIcon.Information:
                        pic.Image = SystemIcons.Information.ToBitmap();
                        break;
                    case MessageBoxIcon.Warning:
                        pic.Image = SystemIcons.Warning.ToBitmap();
                        break;
                    case MessageBoxIcon.Error:
                        pic.Image = SystemIcons.Error.ToBitmap();
                        break;
                    case MessageBoxIcon.Question:
                        pic.Image = SystemIcons.Question.ToBitmap();
                        break;
                    default:
                        pic.Visible = false;
                        pic.Size = new Size(0, 0);
                        break;
                }

                form.Controls.Add(pic);

                // Label for text
                var lbl = new Label
                {
                    AutoSize = false,
                    Location = new Point(padding + pic.Width + (pic.Visible ? 8 : 0), padding),
                    Size = new Size(form.ClientSize.Width - (padding * 2) - (pic.Visible ? pic.Width + 8 : 0), 64),
                    Text = text ?? string.Empty,
                };

                form.Controls.Add(lbl);

                // Buttons panel
                var btnPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.RightToLeft,
                    Dock = DockStyle.Bottom,
                    Padding = new Padding(padding, 8, padding, padding),
                    AutoSize = false,
                    Height = 48
                };

                form.Controls.Add(btnPanel);

                // Helper to get localized text
                string GetText(string key, string fallback)
                {
                    try
                    {
                        return lang.GetContent(key, lang.GetCurrentLanguage());
                    }
                    catch
                    {
                        return fallback;
                    }
                }

                // Create buttons based on MessageBoxButtons
                void AddButton(string textBtn, DialogResult dr)
                {
                    var b = new Button
                    {
                        Text = textBtn,
                        AutoSize = true,
                        DialogResult = dr,
                        Margin = new Padding(6, 0, 0, 0),
                        Padding = new Padding(8, 4, 8, 4),
                        MinimumSize = new Size(80, 28)
                    };
                    btnPanel.Controls.Add(b);
                    // set Accept/Cancel mapping
                    if (dr == DialogResult.OK)
                        form.AcceptButton = b;
                    if (dr == DialogResult.Cancel)
                        form.CancelButton = b;
                }

                // Map MessageBoxButtons
                switch (buttons)
                {
                    case MessageBoxButtons.OK:
                        AddButton(GetText("CustomTextOK", "OK"), DialogResult.OK);
                        break;
                    case MessageBoxButtons.OKCancel:
                        AddButton(GetText("CustomTextOK", "OK"), DialogResult.OK);
                        AddButton(GetText("CustomTextCancel", "Cancel"), DialogResult.Cancel);
                        break;
                    case MessageBoxButtons.YesNo:
                        AddButton(GetText("CustomTextYes", "Yes"), DialogResult.Yes);
                        AddButton(GetText("CustomTextNo", "No"), DialogResult.No);
                        break;
                    case MessageBoxButtons.YesNoCancel:
                        AddButton(GetText("CustomTextYes", "Yes"), DialogResult.Yes);
                        AddButton(GetText("CustomTextNo", "No"), DialogResult.No);
                        AddButton(GetText("CustomTextCancel", "Cancel"), DialogResult.Cancel);
                        break;
                    default:
                        AddButton(GetText("CustomTextOK", "OK"), DialogResult.OK);
                        break;
                }

                // Default button handling: set AcceptButton/CancelButton to match defaultButton
                // MessageBoxDefaultButton.Button1 -> first button (right-most in our right-to-left flow)
                // We'll map accordingly
                if (defaultButton != MessageBoxDefaultButton.Button1)
                {
                    try
                    {
                        Control[] btns = new Control[btnPanel.Controls.Count];
                        btnPanel.Controls.CopyTo(btns, 0);
                        // btns[0] is the right-most because of RightToLeft flow
                        int idx = 0;
                        if (defaultButton == MessageBoxDefaultButton.Button2) idx = Math.Min(1, btns.Length - 1);
                        if (defaultButton == MessageBoxDefaultButton.Button3) idx = Math.Min(2, btns.Length - 1);
                        if (btns.Length > idx && btns[idx] is Button defBtn)
                        {
                            form.AcceptButton = defBtn as Button;
                        }
                    }
                    catch { }
                }

                // Adjust form height based on text
                var neededHeight = lbl.PreferredHeight + pic.Height + 48 + padding * 2;
                form.ClientSize = new Size(form.ClientSize.Width, Math.Max(form.ClientSize.Height, neededHeight));

                // Show dialog
                if (owner != null)
                    return form.ShowDialog(owner);
                else
                    return form.ShowDialog();
            }
        }
    }
}
