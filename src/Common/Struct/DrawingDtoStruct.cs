using System;

namespace Common.Struct
{
    public readonly ref struct DrawingDtoStruct
    {
        public int YStart { get; }
        public int? YEnd { get; }
        public int XStart { get; }
        public int? XEnd { get; }
        public ReadOnlySpan<byte> Color { get; }

        public DrawingDtoStruct(in ReadOnlySpan<byte> color)
        {
            YStart = default;
            YEnd = default;
            XStart = default;
            XEnd = default;
            Color = color;
        }

        public DrawingDtoStruct(int yStart, int? yEnd, 
            int xStart, int? xEnd, in ReadOnlySpan<byte> color)
        {
            YStart = yStart;
            YEnd = yEnd;
            XStart = xStart;
            XEnd = xEnd;
            Color = color;
        }
    }
}
