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
    partial class SplitButtonGalleryFrame : GalleryFrame
    {
        private static Image sample = ImageManager.SplitButtonGallerySample();

        public SplitButtonGalleryFrame()
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

            LabelHeader.Text = "  Split Button Gallery Properties";
            LabelHeader.ImageIndex = 16;
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
