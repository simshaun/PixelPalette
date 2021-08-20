namespace PixelPalette
{
    public class EphemeralData
    {
        public bool DebugMode { get; set; }
    }

    public static class EphemeralState
    {
        public static readonly EphemeralData Data;

        static EphemeralState()
        {
            Data = new EphemeralData();
        }
    }
}