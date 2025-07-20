namespace VRCGPUTool
{
    internal class GpuStatus(
        string Name,
        string UUID,
        int PLimit,
        int PLimitMin,
        int PLimitMax,
        int PLimitDefault,
        int CoreLoad,
        int CoreTemp,
        int PowerDraw,
        int CoreClock,
        int MemoryClock
        )
    {
        public string Name { get; private set; } = Name;
        public string UUID { get; private set; } = UUID;
        public int PLimit { get; private set; } = PLimit;
        public int PLimitMin { get; private set; } = PLimitMin;
        public int PLimitMax { get; private set; } = PLimitMax;
        public int PLimitDefault { get; private set; } = PLimitDefault;
        public int CoreLoad { get; private set; } = CoreLoad;
        public int CoreTemp { get; private set; } = CoreTemp;
        public int PowerDraw { get; private set; } = PowerDraw;
        public int CoreClock { get; private set; } = CoreClock;
        public int MemoryClock { get; private set; } = MemoryClock;
    }
}
