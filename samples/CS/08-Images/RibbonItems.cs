using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WinForms.Ribbon
{
    partial class RibbonItems
    {
        private bool exitOn = false;

        public void Init()
        {
            ButtonDropA.Click += new EventHandler<EventArgs>(_buttonDropA_ExecuteEvent);
            ButtonDropB.Click += new EventHandler<EventArgs>(_buttonDropB_ExecuteEvent);
        }

        unsafe void _buttonDropA_ExecuteEvent(object sender, EventArgs e)
        {
            // load bitmap from file
            Bitmap bitmap = new System.Drawing.Bitmap(@"..\..\..\Res\Drop32.bmp");
            //bitmap.MakeTransparent();

            // set large image property
            ButtonDropA.LargeImage = new UIImage(Ribbon, bitmap).UIImageHandle; //.ConvertToUIImage(bitmap);
        }

        unsafe void _buttonDropB_ExecuteEvent(object sender, EventArgs e)
        {
            List<int> supportedImageSizes = new List<int>() { 32, 48, 64 };

            Bitmap bitmap;
            StringBuilder bitmapFileName = new StringBuilder();

            int selectedImageSize;
            if (supportedImageSizes.Contains(SystemInformation.IconSize.Width))
            {
                selectedImageSize = SystemInformation.IconSize.Width;
            }
            else
            {
                selectedImageSize = 32;
            }

            exitOn = !exitOn;
            string exitStatus = exitOn ? "on" : "off";

            bitmapFileName.AppendFormat(@"..\..\..\Res\Exit{0}{1}.bmp", exitStatus, selectedImageSize);

            bitmap = new System.Drawing.Bitmap(bitmapFileName.ToString());
            bitmap.MakeTransparent();

            ButtonDropB.LargeImage = new UIImage(Ribbon, bitmap).UIImageHandle; //.ConvertToUIImage(bitmap);
        }

        public void Load()
        {
        }

    }
}
