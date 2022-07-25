namespace FoenixCore.Processor.Generic
{
    public static class SimulatorCommands
    {
        /// <summary>
        /// Tell the CoreCpu thread to pause until an interrupt is received
        /// </summary>
        public const int WaitForInterrupt = 0;
        /// <summary>
        /// Tell the display to refresh the screen. The screen will refresh at the next 60Hz interval.
        /// </summary>
        public const int RefreshDisplay = 1;
    }
}
