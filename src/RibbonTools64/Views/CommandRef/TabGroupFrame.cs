using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UIRibbonTools
{
    [DesignTimeVisible(false)]
    partial class TabGroupFrame : CommandRefObjectFrame
    {
        private static Image sample = ImageManager.TabGroupSample();

        public TabGroupFrame()
        {
            bool designtime = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            if (designtime)
                InitializeComponent();
        }

        protected override void InitComponentStep1()
        {
            if (components == null)
                components = new Container();

            base.InitComponentStep1();
        }

        protected override void InitSuspend()
        {

            base.InitSuspend();
        }

        protected override void InitComponentStep2()
        {
            base.InitComponentStep2();

            LabelHeader.Text = "  Tab Group Properties";
            LabelHeader.ImageIndex = 22;
        }

        protected override void InitComponentStep3()
        {

            base.InitComponentStep3();
        }

        protected override void InitResume()
        {

            base.InitResume();
        }

        protected override void InitEvents()
        {
            base.InitEvents();
        }

        protected override void InitTooltips(IContainer components)
        {
            base.InitTooltips(components);
        }

        protected override Image SetImageSample()
        {
            return sample;
        }
    }
}
