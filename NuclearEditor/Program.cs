using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace NuclearEditor;

class Program
{
    static void Main(string[] args)
    {
        using (var editor = new EditorWrapper(GameWindowSettings.Default, new NativeWindowSettings
               {
                   NumberOfSamples = 4
               }))
        {
            editor.VSync = VSyncMode.On;
            editor.Run();
        }
    }
}
