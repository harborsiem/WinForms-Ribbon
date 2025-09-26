using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinForms.Ribbon;

namespace UIRibbonTools
{
    internal partial class ColorFrame : UserControl
    {
        private ColorSelection _selection;
        private Color _color;
        private WhichColor _whichColor;
        private Action<UI_HSBCOLOR> _setRibbonColor;
        private bool _suspendEvent;
        private Padding _upDownMargin;
        private Padding _buttonMargin;
        int diffHeight;
        //private string[] _colorsText = { "Red", "Green", "Blue" };
        //private string[] _hsbText = { "Hue", "Sat.", "Bright." };

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public UI_HSBCOLOR HsbDefault { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public UI_HSBCOLOR HsbSelected { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public UI_HSBCOLOR HsbCurrent { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public ColorSelection ColorSelection
        {
            get { return _selection; }
            set
            {
                if (value == _selection)
                    return;
                _selection = value;
                SetUpDownsText();
                SetUpDownsValue(HsbCurrent);
                SetHsbOrColor();
            }
        }

        public ColorFrame()
        {
            InitializeComponent();
            Load += ColorFrame_Load;
            upDown_RorH.ValueChanged += ColorOrHsb_ValueChanged;
            upDown_GorS.ValueChanged += ColorOrHsb_ValueChanged;
            upDown_BorB.ValueChanged += ColorOrHsb_ValueChanged;
            this.setColorButton.Click += SetColorButton_Click;
#if FixHighDpi
            if (DeviceDpi != 96) //Workaround for wrong Margins of NumericUpDown
            {
                Size thisSize = this.Size;
                Padding upDownMargin = upDown_RorH.Margin;
                Padding margin = setColorButton.Margin;
                diffHeight = (upDownMargin.Top + upDownMargin.Bottom - margin.Top - margin.Bottom) * 3;
                int diffWidth = (upDownMargin.Left + upDownMargin.Right - margin.Left - margin.Right) * 3;
                //frameLayout.SuspendLayout();
                //this.SuspendLayout();
                upDown_RorH.Margin = margin;
                upDown_GorS.Margin = margin;
                upDown_BorB.Margin = margin;
                //frameLayout.Size = thisSize;
                //frameLayout.ResumeLayout(false);
                //frameLayout.PerformLayout();
                //int endPositionY = hsbOrColorText.Location.Y + hsbOrColorText.Size.Height;
                ////this.Size = new Size(thisSize.Width - diffWidth, endPositionY + margin.Bottom);
                //this.ResumeLayout(false);
                //this.PerformLayout();
                //this.Parent.Height = this.Parent.Height - diffHeight;
            }
#endif
            _upDownMargin = upDown_RorH.Margin;
            _buttonMargin = setColorButton.Margin;
        }

        public void Init(UI_HSBCOLOR hsbColorDefault, WhichColor whichColor, Action<UI_HSBCOLOR> setRibbonColor)
        {
            _whichColor = whichColor;
            _setRibbonColor = setRibbonColor;
            HsbDefault = hsbColorDefault;
            HsbCurrent = hsbColorDefault;
            HsbSelected = hsbColorDefault;
            Color color = HsbCurrent.ToColor();
            _color = color;
            colorPanel.BackColor = color;
            ColorSelection = ColorSelection.Color;
            //SetUpDownsText();
            //SetUpDownsValue(HsbCurrent);
            //SetHsbOrColor();
        }

        private void SetUpDownsValue(UI_HSBCOLOR hsb)
        {
            _suspendEvent = true;
            switch (ColorSelection)
            {
                case ColorSelection.Color:
                    Color color = hsb.ToColor();
                    upDown_RorH.Value = color.R;
                    upDown_GorS.Value = color.G;
                    upDown_BorB.Value = color.B;
                    break;
                case ColorSelection.Hsb:
                    upDown_RorH.Value = hsb.Hue;
                    upDown_GorS.Value = hsb.Saturation;
                    upDown_BorB.Value = hsb.Brightness;
                    break;
                default:
                    break;
            }
            _color = hsb.ToColor();
            colorPanel.BackColor = _color;
            _suspendEvent = false;
        }

        private void SetUpDownsText()
        {
            switch (ColorSelection)
            {
                case ColorSelection.Color:
                    label1.Text = "Red";
                    label2.Text = "Green";
                    label3.Text = "Blue";
                    break;
                case ColorSelection.Hsb:
                    label1.Text = "Hue";
                    label2.Text = "Sat.";
                    label3.Text = "Bright.";
                    break;
                default:
                    break;
            }
        }

        private void SetHsbOrColor()
        {
            switch (ColorSelection)
            {
                case ColorSelection.Color:
                    hsbOrColorText.Text = GetHsbText();
                    break;
                case ColorSelection.Hsb:
                    hsbOrColorText.Text = GetColorText();
                    break;
                default:
                    break;
            }
        }

        public void SetDefaultColor()
        {
            SetUpDownsValue(HsbDefault);
            SetThisColor(HsbDefault);
        }

        private void ColorFrame_Load(object sender, EventArgs e)
        {
            //frameLayout.BackColor = Color.Red;
            //this.BackColor = Color.Green;
#if FixHighDpi
            if (DeviceDpi != 96) //Workaround for wrong Margins of NumericUpDown
            {
                Size thisSize = this.Size;
                frameLayout.Size = thisSize;
                this.Parent.Height = this.Parent.Height - diffHeight;
            }
            //if (DeviceDpi != 96) //Workaround for wrong Margins of NumericUpDown
            //{
            //    MessageBox.Show(_upDownMargin.ToString());
            //    Size thisSize = this.Size;
            //    Padding upDownMargin = upDown_RorH.Margin;
            //    Padding margin = setColorButton.Margin;
            //    int diffHeight = (upDownMargin.Top + upDownMargin.Bottom - margin.Top - margin.Bottom) * 3;
            //    int diffWidth = (upDownMargin.Left + upDownMargin.Right - margin.Left - margin.Right) * 3;
            //    frameLayout.SuspendLayout();
            //    this.SuspendLayout();
            //    upDown_RorH.Margin = margin;
            //    upDown_GorS.Margin = margin;
            //    upDown_BorB.Margin = margin;
            //    frameLayout.Size = thisSize;
            //    frameLayout.ResumeLayout(false);
            //    frameLayout.PerformLayout();
            //    int endPositionY = hsbOrColorText.Location.Y + hsbOrColorText.Size.Height;
            //    this.Size = new Size(thisSize.Width - diffWidth, endPositionY + margin.Bottom);
            //    this.ResumeLayout(false);
            //    this.PerformLayout();
            //    this.Parent.Height = this.Parent.Height - diffHeight;
            //}
#endif
        }

        private void ColorOrHsb_ValueChanged(object sender, EventArgs e)
        {
            if (_suspendEvent)
                return;
            Color color;
            UI_HSBCOLOR hsb;
            if (ColorSelection == ColorSelection.Color)
            {
                color = Color.FromArgb((int)upDown_RorH.Value, (int)upDown_GorS.Value, (int)upDown_BorB.Value);
                hsb = new UI_HSBCOLOR(color);
            }
            else
            {
                hsb = new UI_HSBCOLOR((byte)upDown_RorH.Value, (byte)upDown_GorS.Value, (byte)upDown_BorB.Value);
                color = hsb.ToColor();
            }
            HsbSelected = hsb;
            _color = color;
            colorPanel.BackColor = color;
            //SetHsbOrColor();
        }

        private void SetColorButton_Click(object sender, EventArgs e)
        {
            SetThisColor(new UI_HSBCOLOR(_color));
        }

        public void SetThisColor(UI_HSBCOLOR hsbColor)
        {
            _setRibbonColor(HsbCurrent = hsbColor);
            //_ribbon.SetBackgroundColor(HsbCurrent = hsbColor);
            SetHsbOrColor();
        }

        private string GetHsbText()
        {
            //return "HSB: 0x" + HsbCurrent.Value.ToString("X8");
            return "HSB: " + HsbCurrent.Hue + ", " + HsbCurrent.Saturation + ", " + HsbCurrent.Brightness;
        }

        private string GetColorText()
        {
            return "RGB: " + _color.R + ", " + _color.G + ", " + _color.B;
        }
    }

    public enum ColorSelection
    {
        None,
        Color,
        Hsb
    }

    public enum WhichColor
    {
        Background,
        Highlight,
        Text,
        AppButton
    }
}
