using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinForms.Ribbon
{
    partial class RibbonItems
    {
        UI_HSBCOLOR _backgroundDefault;
        UI_HSBCOLOR _highlightDefault;
        UI_HSBCOLOR _textDefault;
        UI_HSBCOLOR _appButtonColorDefault;
        UI_HSBCOLOR _backgroundCurrent;
        UI_HSBCOLOR _highlightCurrent;
        UI_HSBCOLOR _textCurrent;
        UI_HSBCOLOR _appButtonColorCurrent;
        bool _hasAppButtonColor;

        public void Init()
        {
            ToggleDark.ToggleChanged += ToggleDark_ToggleChanged;
            ButtonDefaultColors.Click += ButtonDefaultColors_Click;
            ButtonColor.Click += ButtonColor_Click;
            Ribbon.ViewCreated += Ribbon_ViewCreated;
        }

        private void ButtonColor_Click(object sender, EventArgs e)
        {
            // set ribbon colors
            Ribbon.SetBackgroundColor(_backgroundCurrent = new UI_HSBCOLOR(Color.Wheat));
            Ribbon.SetHighlightColor(_highlightCurrent = new UI_HSBCOLOR(Color.IndianRed));
            Ribbon.SetTextColor(_textCurrent = new UI_HSBCOLOR(Color.BlueViolet));
            if (_hasAppButtonColor)
            {
                Ribbon.SetApplicationButtonColor(_appButtonColorCurrent = new UI_HSBCOLOR(Color.BlueViolet));
            }
        }

        private void Ribbon_ViewCreated(object sender, EventArgs e)
        {
            _backgroundDefault = Ribbon.GetBackgroundColor();
            _highlightDefault = Ribbon.GetHighlightColor();
            _textDefault = Ribbon.GetTextColor();
            try
            {
                _appButtonColorDefault = Ribbon.GetApplicationButtonColor();
                _hasAppButtonColor = true;
            }
            catch (NotSupportedException)
            {
                _hasAppButtonColor = false;
            }
        }

        private void ButtonDefaultColors_Click(object sender, EventArgs e)
        {
            Ribbon.SetBackgroundColor(_backgroundDefault);
            Ribbon.SetHighlightColor(_highlightDefault);
            Ribbon.SetTextColor(_textDefault);
            if (_hasAppButtonColor)
            {
                Ribbon.SetApplicationButtonColor(_appButtonColorDefault);
            }
            _backgroundCurrent = _backgroundDefault;
            _highlightCurrent = _highlightDefault;
            _textCurrent = _textDefault;
            _appButtonColorCurrent = _appButtonColorDefault;
        }

        private void ToggleDark_ToggleChanged(object sender, EventArgs e)
        {
            Ribbon.SetDarkModeRibbon(ToggleDark.BooleanValue);
            if (ToggleDark.BooleanValue)
            {
                ButtonColor.Enabled = false;
                ButtonDefaultColors.Enabled = false;
            }
            else
            {
                ButtonColor.Enabled = true;
                ButtonDefaultColors.Enabled = true;
            }
        }

        public void Load()
        {
#if NET10_0_OR_GREATER
            if (Application.IsDarkModeEnabled)
            {
                ToggleDark.BooleanValue = true;
                ButtonColor.Enabled = false;
                ButtonDefaultColors.Enabled = false;
            }
#endif
        }

    }
}
