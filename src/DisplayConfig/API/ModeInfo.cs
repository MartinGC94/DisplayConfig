using MartinGC94.DisplayConfig.Native.Structs;

namespace MartinGC94.DisplayConfig.API
{
    public sealed class ModeInfo
    {
        public uint Width { get; }
        public uint Height { get; }
        public double RefreshRate { get; }

        internal ModeInfo(DISPLAYCONFIG_SOURCE_MODE sourceMode, DISPLAYCONFIG_VIDEO_SIGNAL_INFO targetVideo)
        {
            Width = sourceMode.width;
            Height = sourceMode.height;
            RefreshRate = targetVideo.vSyncFreq.AsDouble();
        }

        public override string ToString()
        {
            return $"{Width}x{Height}@{RefreshRate} Hz";
        }
    }
}