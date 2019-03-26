using System;

namespace Common.Struct
{
    public readonly ref struct DrawingServiceStruct
    {
        private readonly int imageSize;
        private readonly Span<byte> imageBuffer;

        public DrawingServiceStruct(in Span<byte> imageBuffer, int imageSize)
        {
            this.imageSize = imageSize;
            this.imageBuffer = imageBuffer;
        }

        public byte[] GetImageBuffer()
        {
            Span<byte> imgBuffer = new byte[imageBuffer.Length];

            if (!imageBuffer.TryCopyTo(imgBuffer))
            {
                throw new InvalidOperationException();
            }

            return imgBuffer.ToArray();
        }

        private void PlotPixel(int x, int y, byte redValue,
            byte greenValue, byte blueValue)
        {
            var offset = ((250 * 4) * y) + (x * 4);
            imageBuffer[offset] = blueValue;
            imageBuffer[++offset] = greenValue;
            imageBuffer[++offset] = redValue;
            // Fixed alpha value (No transparency)
            imageBuffer[++offset] = 0xFF;
        }

        public void Draw(in DrawingDtoStruct pos)
        {
            if (pos.YEnd > imageSize || pos.XEnd > imageSize)
            {
                return;
            }

            for (int y = pos.YStart; y <= (pos.YEnd ?? imageSize); y++)
            {
                for (int x = pos.XStart; x <= (pos.XEnd ?? imageSize); x++)
                {
                    PlotPixel(x, y, pos.Color[0], pos.Color[1], pos.Color[2]);
                }
            }
        }

        public long ReplaceColor(in ReadOnlySpan<byte> oldColor, in ReadOnlySpan<byte> newColor)
        {
            long totalReplacements = 0;
            for (int i = imageBuffer.Length - 1; i > 0; i -= 4)
            {
                var lastIndex = i - 3;
                if (lastIndex < 0)
                {
                    break;
                }

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
            for (int i = imageBuffer.Length - 1; i > 0; i -= 4)
            {
                var lastIndex = i - 3;
                if (lastIndex < 0)
                {
                    break;
                }

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
