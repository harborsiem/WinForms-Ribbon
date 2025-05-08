using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Svg;
using System.Windows.Controls;

namespace UIRibbonTools
{
    class SvgConverter : BaseConverter
    {
        private static readonly int[] sizes = new int[] { 16, 20, 24, 32, 40, 48, 64 };

        public SvgConverter(string outputPath, OutputSelector selected) : base(outputPath, selected)
        {
        }

        protected override void OpenSave(string fileName)
        {
            string bmpFile = Path.Combine(_outputPath, _fileNameWithoutExtension);
            SvgDocument svgDocument = SvgDocument.Open(fileName);
            for (int i = 0; i < sizes.Length; i++)
            {
                int size = sizes[i];
                svgDocument.Height = size;
                svgDocument.Width = size;
                string outputFile = bmpFile + "_" + (sizes[i]).ToString() + ".bmp";
                using (Bitmap bitmap = svgDocument.Draw())
                {
                    if (bitmap != null)
                    {
                        SaveImage(bitmap, outputFile);
                    }
                }
            }
        }

        private void SaveImage(Bitmap bitmap, string path)
        {
            if (_outputSelected == OutputSelector.Bitmap)
                AlphaBitmap.SetTransparentRGB(bitmap, TransparentRGBColor);
            Save(bitmap, path);
        }
    }
}
