namespace MartinGC94.DisplayConfig.API
{
    public sealed class DisplayResolution
    {
        public uint Width { get; }
        public uint Height { get; }

        public DisplayResolution(uint width, uint height)
        {
            Width = width;
            Height = height;
        }
    }
}