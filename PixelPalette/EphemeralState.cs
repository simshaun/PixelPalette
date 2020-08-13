namespace PixelPalette
{
    public class EphemeralData
    {
        public bool DebugMode { get; set; } = false;
    }

    public static class EphemeralState
    {
        public static EphemeralData Data;

        static EphemeralState()
        {
            Data = new EphemeralData();
        }
    }
}