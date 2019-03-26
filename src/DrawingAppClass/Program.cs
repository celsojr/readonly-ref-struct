using System;
using System.IO;
using Common.Class;
using System.Drawing;
using System.Drawing.Imaging;

namespace DrawingAppClass
{
    using static Path;
    using static Environment;

    class Program
    {
        static void Main()
        {
            var d = new DrawingClass();

            d.DrawUsingClasses();

            //Console.ReadKey(true);
        }
    }

    public class DrawingClass
    {
        private const int ImageSize = 250;

        private readonly DrawingServiceClass classService = new DrawingServiceClass(ImageSize);

        private static readonly string desktop =
            GetFolderPath(SpecialFolder.Desktop);

        public void DrawUsingClasses()
        {
            var whiteBackground = new DrawingDtoClass
            {
                Color = new byte[] { 0xFF, 0xFF, 0xFF }
            };

            var red = new DrawingDtoClass
            {
                YStart = 10,
                YEnd = 120,
                XStart = 10,
                XEnd = 120,
                Color = new byte[] { 0xF3, 0x65, 0x23 }
            };

            var green = new DrawingDtoClass
            {
                YStart = 10,
                YEnd = 120,
                XStart = 130,
                XEnd = 240,
                Color = new byte[] { 0x8D, 0xC7, 0x3F }
            };

            var blue = new DrawingDtoClass
            {
                YStart = 130,
                YEnd = 240,
                XStart = 10,
                XEnd = 120,
                Color = new byte[] { 0x00, 0xAD, 0xEF }
            };

            var yellow = new DrawingDtoClass
            {
                YStart = 130,
                YEnd = 240,
                XStart = 130,
                XEnd = 240,
                Color = new byte[] { 0xFF, 0xC2, 0x0F }
            };

            classService.Draw(whiteBackground);
            classService.Draw(red);
            classService.Draw(green);
            classService.Draw(blue);
            classService.Draw(yellow);

            Save();
        }

        public unsafe void Save()
        {
            fixed (byte* ptr = classService.ImageBuffer)
            {
                using (var bmp = new Bitmap(ImageSize, ImageSize, ImageSize * 4,
                    PixelFormat.Format32bppRgb, new IntPtr(ptr)))
                {
                    var guid = Guid.NewGuid().ToString("n");
                    var fileName = Combine(desktop, $"{guid.Substring(0, 5)}.png");

                    bmp.Save(fileName);
                }
            }
        }

    }
}
