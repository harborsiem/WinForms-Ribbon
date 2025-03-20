using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinForms.Ribbon
{
    partial class RibbonItems
    {
        public void Init()
        {
            ButtonListColors.Click += new EventHandler<EventArgs>(_buttonListColors_ExecuteEvent);
        }

        void _buttonListColors_ExecuteEvent(object sender, EventArgs e)
        {
            Color[] colors = DropDownColorPickerThemeColors.ThemeColors;
            string[] colorsTooltips = DropDownColorPickerThemeColors.ThemeColorsTooltips;

            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

            for (int i = 0; i < colors.Length; ++i)
            {
                stringBuilder.AppendFormat("{0} = {1}\n", colorsTooltips[i], colors[i].ToString());
            }

            MessageBox.Show(stringBuilder.ToString());
        }

        private void InitDropDownColorPickers()
        {
            // common properties
            DropDownColorPickerThemeColors.Label = "Theme Colors";
            DropDownColorPickerThemeColors.ColorChanged += new EventHandler<ColorPickerEventArgs>(_themeColors_ExecuteEvent);

            // set labels
            DropDownColorPickerThemeColors.AutomaticColorLabel = "My Automatic";
            DropDownColorPickerThemeColors.MoreColorsLabel = "My More Colors";
            DropDownColorPickerThemeColors.NoColorLabel = "My No Color";
            DropDownColorPickerThemeColors.RecentColorsCategoryLabel = "My Recent Colors";
            DropDownColorPickerThemeColors.StandardColorsCategoryLabel = "My Standard Colors";
            DropDownColorPickerThemeColors.ThemeColorsCategoryLabel = "My Theme Colors";

            // set colors
            DropDownColorPickerThemeColors.ThemeColorsTooltips = new string[] { "yellow", "green", "red", "blue" };
            DropDownColorPickerThemeColors.ThemeColors = new Color[] { Color.Yellow, Color.Green, Color.Red, Color.Blue };
        }

        void _themeColors_ExecuteEvent(object sender, ColorPickerEventArgs e)
        {
            if (e.ColorType == SwatchColorType.NoColor)
            {
                MessageBox.Show("Selected color is NoColor" + Environment.NewLine + "Selected color is " + DropDownColorPickerThemeColors.Color.ToString());
            }
            else
            {
                MessageBox.Show("Selected color is " + e.RGBColor.ToString());
            }
            //MessageBox.Show("Selected color is " + DropDownColorPickerThemeColors.Color.ToString());
        }

        public void Load()
        {
            InitDropDownColorPickers();
        }

    }
}
