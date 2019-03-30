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
        private const int IMAGE_WIDTH = 250;
        private const int IMAGE_HEIGHT = 250;
        private const int BYTES_PER_PIXEL = 4; // RGBA

        private readonly int buffer = ((IMAGE_WIDTH * IMAGE_HEIGHT) * BYTES_PER_PIXEL) + (IMAGE_WIDTH * BYTES_PER_PIXEL) + BYTES_PER_PIXEL;

        private readonly DrawingServiceClass classService;

        public MethodCallCompare()
        {
            classService = new DrawingServiceClass(new byte[buffer], IMAGE_WIDTH, IMAGE_HEIGHT, BYTES_PER_PIXEL);
        }

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

            //Save(classService.ImageBuffer);

        }

        [Benchmark]
        public void DrawUsingStructs()
        {
            Span<byte> imageBuffer = stackalloc byte[buffer];
            var serv = new DrawingServiceStruct(imageBuffer, IMAGE_WIDTH, IMAGE_HEIGHT, BYTES_PER_PIXEL);

            Span<byte> colorBuffer = stackalloc byte[3];

            colorBuffer[0] = 0xFF;
            colorBuffer[1] = 0xFF;
            colorBuffer[2] = 0xFF;

            serv.Draw(new DrawingDtoStruct(colorBuffer));

            colorBuffer[0] = 0xF3;
            colorBuffer[1] = 0x65;
            colorBuffer[2] = 0x23;

            serv.Draw(new DrawingDtoStruct(10, 120, 10, 120, colorBuffer));

            colorBuffer[0] = 0x8D;
            colorBuffer[1] = 0xC7;
            colorBuffer[2] = 0x3F;
            
            serv.Draw(new DrawingDtoStruct(10, 120, 10, 120, colorBuffer));

            colorBuffer[0] = 0x00;
            colorBuffer[1] = 0xAD;
            colorBuffer[2] = 0xEF;
            
            serv.Draw(new DrawingDtoStruct(10, 120, 10, 120, colorBuffer));

            colorBuffer[0] = 0xFF;
            colorBuffer[1] = 0xC2;
            colorBuffer[2] = 0x0F;
            
            serv.Draw(new DrawingDtoStruct(10, 120, 10, 120, colorBuffer));

            //Save(serv.GetImageBuffer());
        }
    }
}
