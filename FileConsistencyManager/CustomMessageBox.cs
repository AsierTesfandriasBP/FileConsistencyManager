using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using FileConsistencyManager.Localization;

namespace FileConsistencyManager
{
    public partial class CustomMessageBox : Form
    {
        private Localize _localize;
        private Label _lblMessage;
        private PictureBox _picIcon;
        private FlowLayoutPanel _pnlButtons;

        public CustomMessageBox()
        {
            InitializeComponent();
        }

        public CustomMessageBox(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, Localize localize)
            : this()
        {
            _localize = localize ?? new Localize("en");
            Setup(message, title, buttons, icon, defaultButton);
        }

        private void Setup(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            this.Text = title ?? string.Empty;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
            this.Font = SystemFonts.MessageBoxFont;

            var padding = 12;

            _picIcon = new PictureBox
            {
                Location = new Point(padding, padding),
                Size = new Size(32, 32),
                SizeMode = PictureBoxSizeMode.CenterImage
            };

            switch (icon)
            {
                case MessageBoxIcon.Information:
                    _picIcon.Image = SystemIcons.Information.ToBitmap();
                    break;
                case MessageBoxIcon.Warning:
                    _picIcon.Image = SystemIcons.Warning.ToBitmap();
                    break;
                case MessageBoxIcon.Error:
                    _picIcon.Image = SystemIcons.Error.ToBitmap();
                    break;
                case MessageBoxIcon.Question:
                    _picIcon.Image = SystemIcons.Question.ToBitmap();
                    break;
                default:
                    _picIcon.Visible = false;
                    _picIcon.Size = new Size(0, 0);
                    break;
            }

            // Use a TableLayoutPanel to reliably layout icon and text side-by-side
            var contentPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Top,
                Padding = new Padding(padding, padding, padding, 0),
                AutoSize = true,
            };

            int iconArea = _picIcon.Visible ? (_picIcon.Width + 8) : 0;
            // Column 0 reserves space for icon, column 1 takes remaining space
            contentPanel.ColumnStyles.Add(new ColumnStyle(_picIcon.Visible ? SizeType.Absolute : SizeType.Absolute, iconArea));
            contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            _lblMessage = new Label
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                MaximumSize = new Size(600 - (padding * 2) - iconArea, 0),
                Text = message ?? string.Empty,
            };

            // add controls into the contentPanel
            contentPanel.Controls.Add(_picIcon, 0, 0);
            contentPanel.Controls.Add(_lblMessage, 1, 0);

            this.Controls.Add(contentPanel);

            _pnlButtons = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Bottom,
                Padding = new Padding(padding, 8, padding, padding),
                AutoSize = false,
                Height = 48,
                WrapContents = false
            };

            this.Controls.Add(_pnlButtons);

            string lang = _localize.GetCurrentLanguage();
            string GetText(string key, string fallback) => _localize.GetContent(key, lang) ?? fallback;

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
                _pnlButtons.Controls.Add(b);
                if (dr == DialogResult.OK) this.AcceptButton = b;
                if (dr == DialogResult.Cancel) this.CancelButton = b;
            }

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

            // default button mapping
            if (defaultButton != MessageBoxDefaultButton.Button1)
            {
                try
                {
                    var btnList = _pnlButtons.Controls.Cast<Control>().ToArray();
                    int idx = 0;
                    if (defaultButton == MessageBoxDefaultButton.Button2) idx = Math.Min(1, btnList.Length - 1);
                    if (defaultButton == MessageBoxDefaultButton.Button3) idx = Math.Min(2, btnList.Length - 1);
                    if (btnList.Length > idx && btnList[idx] is Button defBtn) this.AcceptButton = defBtn as Button;
                }
                catch { }
            }

            // Measure buttons total width
            int buttonsTotalWidth = 0;
            int gapBetweenButtons = 6; // matches button margin
            foreach (Control c in _pnlButtons.Controls)
            {
                if (c is Button b)
                {
                    var preferred = b.GetPreferredSize(Size.Empty);
                    buttonsTotalWidth += preferred.Width + gapBetweenButtons;
                }
            }
            buttonsTotalWidth += _pnlButtons.Padding.Left + _pnlButtons.Padding.Right;

            int maxTextWidth = Math.Min(600 - (padding * 2) - iconArea, Math.Max(120, buttonsTotalWidth));
            var textSize = TextRenderer.MeasureText(_lblMessage.Text, _lblMessage.Font, new Size(maxTextWidth, int.MaxValue), TextFormatFlags.WordBreak);

            int finalWidth = padding * 2 + iconArea + Math.Max(textSize.Width, buttonsTotalWidth);
            finalWidth = Math.Min(finalWidth, 600);
            finalWidth = Math.Max(finalWidth, 260);

            int finalHeight = padding + Math.Max(_picIcon.Height, textSize.Height) + _pnlButtons.Height + padding;

            this.ClientSize = new Size(finalWidth, finalHeight);

            // adjust label max width so text wraps correctly
            _lblMessage.MaximumSize = new Size(finalWidth - (padding * 2) - iconArea, int.MaxValue);
            var wrapped = TextRenderer.MeasureText(_lblMessage.Text, _lblMessage.Font, new Size(_lblMessage.MaximumSize.Width, int.MaxValue), TextFormatFlags.WordBreak);
            _lblMessage.Size = new Size(_lblMessage.MaximumSize.Width, wrapped.Height);
        }

        // Static helper to mimic MessageBox usage and include localization
        public static DialogResult Show(string message, string caption = "", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1, Localize lang = null, IWin32Window owner = null)
        {
            lang ??= new Localize("en");
            using (var form = new CustomMessageBox(message, caption, buttons, icon, defaultButton, lang))
            {
                if (owner != null)
                    return form.ShowDialog(owner);
                return form.ShowDialog();
            }
        }
    }
}
