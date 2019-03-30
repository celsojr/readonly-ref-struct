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
        private const int IMAGE_WIDTH = 251;
        private const int IMAGE_HEIGHT = 251;
        private const int BYTES_PER_PIXEL = 4; // RGBA

        private readonly int bufferSize = ((IMAGE_WIDTH * IMAGE_HEIGHT) * BYTES_PER_PIXEL) + (IMAGE_WIDTH * BYTES_PER_PIXEL) + BYTES_PER_PIXEL;

        private readonly string desktop = GetFolderPath(SpecialFolder.Desktop);

        private readonly DrawingServiceClass classService;

        public DrawingClass()
        {
            classService = new DrawingServiceClass(new byte[bufferSize], IMAGE_WIDTH, IMAGE_HEIGHT, BYTES_PER_PIXEL);
        }

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

            Save(classService.ImageBuffer);
        }

        public unsafe void Save(byte[] imageBuffer)
        {
            fixed (byte* ptr = imageBuffer)
            {
                var format = PixelFormat.Format32bppRgb;
                using (var bmp = new Bitmap(IMAGE_WIDTH, IMAGE_HEIGHT, CalculateStride(format),
                    format, new IntPtr(ptr)))
                {
                    var guid = Guid.NewGuid().ToString("n");
                    var fileName = Combine(desktop, $"{guid.Substring(0, 5)}.png");

                    bmp.Save(fileName);
                }
            }
        }

        private int CalculateStride(PixelFormat format)
        {
            int bitsPerPixel = ((int)format & 0xff00) >> 8;
            int bytesPerPixel = (bitsPerPixel + 7) / 8;

            return 4 * ((IMAGE_WIDTH * bytesPerPixel + 3) / 4);
        }

    }
}
