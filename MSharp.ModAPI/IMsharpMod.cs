namespace MSharp.ModAPI
{
    public interface IMsharpMod
    {
        void OnStart();
        void OnTick();
        void OnEvent(string evnt, object? payload = null);
    }
}
