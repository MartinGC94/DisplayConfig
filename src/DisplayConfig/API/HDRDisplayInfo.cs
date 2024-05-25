namespace MartinGC94.DisplayConfig.API
{
    public sealed class HDRDisplayInfo
    {
        public uint DisplayId;
        public bool HDREnabled { get; }
        public uint SdrWhiteLevel { get; }

        internal HDRDisplayInfo(ColorInfo colorInfo, uint displayId)
        {
            DisplayId = displayId;
            HDREnabled = colorInfo.AdvancedColorEnabled;
            SdrWhiteLevel = colorInfo.SDRWhiteLevel;
        }
    }
}