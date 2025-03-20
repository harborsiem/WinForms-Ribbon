using System;

namespace WinForms.Ribbon
{
    partial class RibbonItems
    {
        public void Init()
        {

            ButtonDropA.Click += new EventHandler<EventArgs>(_buttonDropA_Click);
            Spinner.RepresentativeString = "2.50 m";
        }

        void _buttonDropA_Click(object sender, EventArgs e)
        {
            InitSpinner();
        }

        private void InitSpinner()
        {
            Spinner.DecimalPlaces = 2;
            Spinner.DecimalValue = 1.8M;
            Spinner.TooltipTitle = "Height";
            Spinner.TooltipDescription = "Enter height in meters.";
            Spinner.MaxValue = 2.5M;
            Spinner.MinValue = 0;
            Spinner.Increment = 0.01M;
            Spinner.FormatString = " m";
            Spinner.Label = "Height:";
        }

        public void Load()
        {
        }

    }
}
