using NuclearGame.Input;
using Veldrid;
using Veldrid.StartupUtilities;

namespace NuclearEditor;

public class EditorWrapper
{
    public EditorWrapper()
    {
        var windowInfo = new WindowCreateInfo
        {
            X = 50,
            Y = 50,
            WindowWidth = 852,
            WindowHeight = 480,
            WindowInitialState = WindowState.Normal,
            WindowTitle = "Nuclear Editor v1.0"
        };

        var graphicsDeviceOptions =
            new GraphicsDeviceOptions(false, null, false,
                ResourceBindingModel.Improved, true, true, true);

#if DEBUG
        graphicsDeviceOptions.Debug = true;
#endif
        
        VeldridStartup.CreateWindowAndGraphicsDevice(
            windowInfo,
            graphicsDeviceOptions,
            GraphicsBackend.OpenGL,
            out var window,
            out var graphicsDevice
        );

        var editor = new Editor(window, graphicsDevice);

        editor.Create();
        window.Resized += editor.Resize;
        
        while (window.Exists)
        {
            var snapshot = window.PumpEvents();

            if (window.Exists)
            {
                InputTracker.UpdateFrameInput(snapshot, window);
                editor.Update(1f);
                editor.Render();
            }
        }
        
        editor.Destroy();
    }
}