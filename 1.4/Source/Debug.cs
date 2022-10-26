namespace VisibleRaidPoints
{
    public static class Debug
    {
        public static void Log(string message)
        {
#if DEBUG
            Verse.Log.Message($"[{VisibleRaidPointsMod.PACKAGE_NAME}] {message}");
#endif
        }
    }
}
