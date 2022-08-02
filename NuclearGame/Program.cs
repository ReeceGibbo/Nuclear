using OpenTK.Windowing.Desktop;

namespace NuclearGame
{
    class Program
    {
        private static void Main(string[] args)
        {
            using (var game = new GameWrapper(GameWindowSettings.Default, new NativeWindowSettings
                   {
                       NumberOfSamples = 8
                   }))
            {
                game.Run();
            }
        }
    }
}