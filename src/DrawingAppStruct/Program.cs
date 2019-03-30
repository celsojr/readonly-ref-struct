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
        private const int IMAGE_WIDTH = 251;
        private const int IMAGE_HEIGHT = 251;
        private const int BYTES_PER_PIXEL = 4; // RGBA

        private readonly int bufferSize = ((IMAGE_WIDTH * IMAGE_HEIGHT) * BYTES_PER_PIXEL) + (IMAGE_WIDTH * BYTES_PER_PIXEL) + BYTES_PER_PIXEL;

        private readonly string desktop = GetFolderPath(SpecialFolder.Desktop);

        public void DrawUsingStructs()
        {
            Span<byte> imageBuffer = stackalloc byte[bufferSize];
            var serv = new DrawingServiceStruct(imageBuffer, IMAGE_WIDTH, IMAGE_HEIGHT, BYTES_PER_PIXEL);

            Span<byte> colorBuffer = stackalloc byte[3];

            //white
            colorBuffer[0] = 0xFF;
            colorBuffer[1] = 0xFF;
            colorBuffer[2] = 0xFF;
            serv.Draw(new DrawingDtoStruct(colorBuffer));

            //red
            colorBuffer[0] = 0xF3;
            colorBuffer[1] = 0x65;
            colorBuffer[2] = 0x23;
            serv.Draw(new DrawingDtoStruct(10, 120, 10, 120, colorBuffer));

            //green
            colorBuffer[0] = 0x8D;
            colorBuffer[1] = 0xC7;
            colorBuffer[2] = 0x3F;
            serv.Draw(new DrawingDtoStruct(10, 120, 130, 240, colorBuffer));

            //blue
            colorBuffer[0] = 0x00;
            colorBuffer[1] = 0xAD;
            colorBuffer[2] = 0xEF;
            serv.Draw(new DrawingDtoStruct(130, 240, 10, 120, colorBuffer));

            //yellow
            colorBuffer[0] = 0xFF;
            colorBuffer[1] = 0xC2;
            colorBuffer[2] = 0x0F;
            serv.Draw(new DrawingDtoStruct(130, 240, 130, 240, colorBuffer));

            Save(serv.GetImageBuffer().ToArray());
        }

        public unsafe void Save(byte[] imageBuffer)
        {
            fixed (byte* ptr = imageBuffer)
            {
                var format = PixelFormat.Format32bppRgb;
                using (var bmp = new Bitmap(IMAGE_WIDTH, IMAGE_HEIGHT, CalculateStride(format),
                    format, new IntPtr(ptr)))
                {
                    var guid = Guid.NewGuid().ToString("n").AsSpan();
                    var fileName = Combine(desktop, $"{guid.Slice(0, 5).ToString()}.png");

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
