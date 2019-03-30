using System;

namespace Common.Struct
{
    public readonly ref struct DrawingServiceStruct
    {
        private readonly int imageWidth;
        private readonly int imageHeight;
        private readonly int bytesPerPixel;
        private readonly Span<byte> imageBuffer;

        public DrawingServiceStruct(in Span<byte> imageBuffer, int imageWidth, 
            int imageHeght, int bytesPerPixel)
        {
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeght;
            this.bytesPerPixel = bytesPerPixel;
            this.imageBuffer = imageBuffer;
        }

        public ReadOnlySpan<byte> GetImageBuffer()
        {
            ReadOnlySpan<byte> imgBuffer = imageBuffer;
            return imgBuffer;
        }

        private void PlotPixel(int x, int y, byte redValue,
            byte greenValue, byte blueValue)
        {
            var offset = ((imageWidth * bytesPerPixel) * y) + (x * bytesPerPixel);
            imageBuffer[offset] = blueValue;
            imageBuffer[++offset] = greenValue;
            imageBuffer[++offset] = redValue;
            // Fixed alpha value (No transparency)
            imageBuffer[++offset] = 0xFF;
        }

        public void Draw(in DrawingDtoStruct pos)
        {
            if (pos.YEnd > imageHeight || pos.XEnd > imageWidth)
            {
                return;
            }

            for (int y = pos.YStart; y <= (pos.YEnd ?? imageHeight); y++)
            {
                for (int x = pos.XStart; x <= (pos.XEnd ?? imageWidth); x++)
                {
                    PlotPixel(x, y, pos.Color[0], pos.Color[1], pos.Color[2]);
                }
            }
        }

        public long ReplaceColor(in ReadOnlySpan<byte> oldColor, in ReadOnlySpan<byte> newColor)
        {
            long totalReplacements = 0;
            for (int i = imageBuffer.Length - 1; i > 0; i -= bytesPerPixel)
            {
                if (imageBuffer[i] == 0xFF // the alpha value
                    && imageBuffer[i - 1] == oldColor[0]
                    && imageBuffer[i - 2] == oldColor[1]
                    && imageBuffer[i - 3] == oldColor[2])
                {
                    imageBuffer[i] = 0xFF; // the alpha value
                    imageBuffer[i - 1] = newColor[0];
                    imageBuffer[i - 2] = newColor[1];
                    imageBuffer[i - 3] = newColor[2];
                    totalReplacements++;
                }
            }
            return totalReplacements;
        }

        public long GetColorFrequency(in ReadOnlySpan<byte> color)
        {
            long result = 0;
            for (int i = imageBuffer.Length - 1; i > 0; i -= bytesPerPixel)
            {
                if (imageBuffer[i] == 0xFF // the alpha value
                    && imageBuffer[i - 1] == color[0]
                    && imageBuffer[i - 2] == color[1]
                    && imageBuffer[i - 3] == color[2])
                {
                    result++;
                }
            }
            return result;
        }

    }
}
