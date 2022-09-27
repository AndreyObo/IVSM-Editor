using IVSMlib.VsmCanvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace IVSMlib.Utils
{
    public static class ImportManager
    {
        public enum ImgFormat {PNG, JPG, BMP }

        private static string file_name;
        private static ImgFormat format;

        public static void SetFileName(string name)
        {
            file_name = name;
        }

        public static void SetFormat(ImgFormat fmt)
        {
            format = fmt;
        }

        public static void ImportImage(VsmCustomCanvas FieldCanvas, int width, Int32 height, Int32Rect crop_rect)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap(width,
            height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(FieldCanvas);

            var crop = new CroppedBitmap(rtb, crop_rect);
           
            switch(format)
            {
                case ImgFormat.PNG:
                    CreatePNG(crop);
                    break;
                case ImgFormat.JPG:
                    CreateJPG(crop);
                    break;
                case ImgFormat.BMP:
                    CreateBMP(crop);
                    break;
            }
           
        }

        private static void CreatePNG(CroppedBitmap crop)
        {
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(crop));

            string f_name = file_name + ".png";

            using (var fs = System.IO.File.OpenWrite(f_name))
            {
                pngEncoder.Save(fs);
            }
        }

        private static void CreateJPG(CroppedBitmap crop)
        {
            BitmapEncoder pngEncoder = new JpegBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(crop));

            string f_name = file_name + ".jpg";

            using (var fs = System.IO.File.OpenWrite(f_name))
            {
                pngEncoder.Save(fs);
            }
        }

        private static void CreateBMP(CroppedBitmap crop)
        {
            BitmapEncoder pngEncoder = new BmpBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(crop));

            string f_name = file_name + ".bmp";

            using (var fs = System.IO.File.OpenWrite(f_name))
            {
                pngEncoder.Save(fs);
            }
        }
    }
}
