namespace Common.Class
{
    public class DrawingServiceClass
    {
        public byte[] ImageBuffer { get; }

        private readonly int imageSize;

        public DrawingServiceClass() { }

        public DrawingServiceClass(int imageSize)
        {
            this.imageSize = imageSize;
            ImageBuffer = new byte[imageSize * 4 * 1024];
        }

        private void PlotPixel(int x, int y, byte redValue,
            byte greenValue, byte blueValue)
        {
            var offset = ((imageSize * 4) * y) + (x * 4);
            ImageBuffer[offset] = blueValue;
            ImageBuffer[++offset] = greenValue;
            ImageBuffer[++offset] = redValue;
            // Fixed alpha value (No transparency)
            ImageBuffer[++offset] = 0xFF;
        }

        public void Draw(DrawingDtoClass pos)
        {
            if (pos.YEnd > imageSize || pos.XEnd > imageSize)
            {
                return;
            }

            byte defaultColor = 0x00;

            for (int y = pos.YStart; y <= (pos.YEnd ?? imageSize); y++)
            {
                for (int x = pos.XStart; x <= (pos.XEnd ?? imageSize); x++)
                {
                    PlotPixel(
                        x, 
                        y,
                        pos.Color?[0] ?? defaultColor,
                        pos.Color?[1] ?? defaultColor,
                        pos.Color?[2] ?? defaultColor
                    );
                }
            }
        }

        public long ReplaceColor(byte[] oldColor, byte[] newColor)
        {
            long totalReplacements = 0;
            for (int i = ImageBuffer.Length - 1; i > 0; i -= 4)
            {
                var lastIndex = i - 3;
                if (lastIndex < 0)
                {
                    break;
                }

                if (ImageBuffer[i] == 0xFF // the alpha value
                    && ImageBuffer[i - 1] == oldColor[0]
                    && ImageBuffer[i - 2] == oldColor[1]
                    && ImageBuffer[i - 3] == oldColor[2])
                {
                    ImageBuffer[i] = 0xFF; // the alpha value
                    ImageBuffer[i - 1] = newColor[0];
                    ImageBuffer[i - 2] = newColor[1];
                    ImageBuffer[i - 3] = newColor[2];
                    totalReplacements++;
                }
            }
            return totalReplacements;
        }

        public long GetColorFrequency(byte[] color)
        {
            long result = 0;
            for (int i = ImageBuffer.Length - 1; i > 0; i -= 4)
            {
                var lastIndex = i - 3;
                if (lastIndex < 0)
                {
                    break;
                }

                if (ImageBuffer[i] == 0xFF // the alpha value
                    && ImageBuffer[i - 1] == color[0]
                    && ImageBuffer[i - 2] == color[1]
                    && ImageBuffer[i - 3] == color[2])
                {
                    result++;
                }
            }
            return result;
        }

    }
}
