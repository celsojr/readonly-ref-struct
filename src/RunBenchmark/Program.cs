using System;
using Common.Class;
using Common.Struct;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;

namespace RunBenchmark
{
    class Program
    {
        static void Main() => BenchmarkRunner.Run<MethodCallCompare>();
    }

    [CoreJob, MemoryDiagnoser]
    public class MethodCallCompare
    {
        private const int ImageSize = 250;

        private readonly DrawingServiceClass classService = new DrawingServiceClass(ImageSize);

        private readonly byte[] whiteColor = new byte[] { 0xFF, 0xFF, 0xFF };
        private readonly byte[] redColor = new byte[] { 0xF3, 0x65, 0x23 };
        private readonly byte[] greenColor = new byte[] { 0x8D, 0xC7, 0x3F };
        private readonly byte[] blueColor = new byte[] { 0x00, 0xAD, 0xEF };
        private readonly byte[] yellowColor = new byte[] { 0xFF, 0xC2, 0x0F };

        [Benchmark(Baseline = true)]
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

            //Save();

        }

        [Benchmark]
        public void DrawUsingStructs()
        {
            Span<byte> imageBuffer = stackalloc byte[ImageSize * 4 * 1024];
            var serv = new DrawingServiceStruct(imageBuffer, ImageSize);

            Span<byte> colorBuffer = stackalloc byte[3];

            colorBuffer = whiteColor;
            serv.Draw(new DrawingDtoStruct(colorBuffer));

            colorBuffer = redColor;
            serv.Draw( new DrawingDtoStruct(10, 120, 10, 120, colorBuffer));

            colorBuffer = greenColor;
            serv.Draw(new DrawingDtoStruct(10, 120, 10, 120, colorBuffer));

            colorBuffer = blueColor;
            serv.Draw(new DrawingDtoStruct(10, 120, 10, 120, colorBuffer));

            colorBuffer = yellowColor;
            serv.Draw(new DrawingDtoStruct(10, 120, 10, 120, colorBuffer));

            //Save();
        }
    }
}
