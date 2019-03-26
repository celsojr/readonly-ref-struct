using System;
using System.IO;
using Common.Struct;
using System.Drawing;
using System.Drawing.Imaging;

namespace DrawingAppStruct
{
    using static Path;
    using static Environment;

    class Program
    {
        static void Main()
        {
            var d = new DrawingStruct();

            d.DrawUsingStructs();

            //Console.ReadKey(true);
        }
    }

    public class DrawingStruct
    {
        private const int ImageSize = 250;
        private readonly string desktop = GetFolderPath(SpecialFolder.Desktop);

        private readonly byte[] whiteColor = new byte[] { 0xFF, 0xFF, 0xFF };
        private readonly byte[] redColor = new byte[] { 0xF3, 0x65, 0x23 };
        private readonly byte[] greenColor = new byte[] { 0x8D, 0xC7, 0x3F };
        private readonly byte[] blueColor = new byte[] { 0x00, 0xAD, 0xEF };
        private readonly byte[] yellowColor = new byte[] { 0xFF, 0xC2, 0x0F };

        public void DrawUsingStructs()
        {
            Span<byte> imageBuffer = stackalloc byte[ImageSize * 4 * 1024];
            var serv = new DrawingServiceStruct(imageBuffer, ImageSize);

            Span<byte> colorBuffer = stackalloc byte[3];

            colorBuffer = whiteColor;
            serv.Draw(new DrawingDtoStruct(colorBuffer));

            colorBuffer = redColor;
            serv.Draw(new DrawingDtoStruct(10, 120, 10, 120, colorBuffer));

            colorBuffer = greenColor;
            serv.Draw(new DrawingDtoStruct(10, 120, 130, 240, colorBuffer));

            colorBuffer = blueColor;
            serv.Draw(new DrawingDtoStruct(130, 240, 10, 120, colorBuffer));

            colorBuffer = yellowColor;
            serv.Draw(new DrawingDtoStruct(130, 240, 130, 240, colorBuffer));

            Save(serv.GetImageBuffer());
        }

        public unsafe void Save(byte[] imageBuffer)
        {
            fixed (byte* ptr = imageBuffer)
            {
                using (var bmp = new Bitmap(ImageSize, ImageSize, ImageSize * 4,
                    PixelFormat.Format32bppRgb, new IntPtr(ptr)))
                {
                    var guid = Guid.NewGuid().ToString("n").AsSpan();
                    var fileName = Combine(desktop, $"{guid.Slice(0, 5).ToString()}.png");

                    bmp.Save(fileName);
                }
            }
        }

    }
}
