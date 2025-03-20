using System;
using System.Drawing;
namespace WinForms.Ribbon
{
    partial class RibbonItems
    {
        UI_HSBCOLOR _backgroundDefault;
        UI_HSBCOLOR _highlightDefault;
        UI_HSBCOLOR _textDefault;
        UI_HSBCOLOR _backgroundCurrent;
        UI_HSBCOLOR _highlightCurrent;
        UI_HSBCOLOR _textCurrent;

        public void Init()
        {
            ToggleDark.ToggleChanged += ToggleDark_ToggleChanged;
            ButtonDefaultColors.Click += ButtonDefaultColors_Click;
        }

        private void ButtonDefaultColors_Click(object sender, EventArgs e)
        {
            Ribbon.SetBackgroundColor(_backgroundDefault);
            Ribbon.SetHighlightColor(_highlightDefault);
            Ribbon.SetTextColor(_textDefault);
            _backgroundCurrent = _backgroundDefault;
            _highlightCurrent = _highlightDefault;
            _textCurrent = _textDefault;
        }

        private void ToggleDark_ToggleChanged(object sender, EventArgs e)
        {
            Ribbon.SetDarkModeRibbon(ToggleDark.BooleanValue);
            if (!ToggleDark.BooleanValue)
            {
                Ribbon.SetBackgroundColor(_backgroundCurrent);
                Ribbon.SetHighlightColor(_highlightCurrent);
                Ribbon.SetTextColor(_textCurrent);
            }
        }

        public void Load()
        {
            _backgroundDefault = Ribbon.GetBackgroundColor();
            _highlightDefault = Ribbon.GetHighlightColor();
            _textDefault = Ribbon.GetTextColor();
            // set ribbon colors
            //Ribbon.SetColors(Color.Wheat, Color.IndianRed, Color.BlueViolet);
            Ribbon.SetBackgroundColor(_backgroundCurrent = new UI_HSBCOLOR(Color.Wheat));
            Ribbon.SetHighlightColor(_highlightCurrent = new UI_HSBCOLOR(Color.IndianRed));
            Ribbon.SetTextColor(_textCurrent = new UI_HSBCOLOR(Color.BlueViolet));
        }

    }
}
