using Veldrid;
using Veldrid.StartupUtilities;

namespace NuclearGame;

public class GameWrapper
{
    public GameWrapper()
    {
        var windowInfo = new WindowCreateInfo
        {
            X = 50,
            Y = 50,
            WindowWidth = 852,
            WindowHeight = 480,
            WindowInitialState = WindowState.Normal,
            WindowTitle = "Nuclear Engine v0.1"
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

        var game = new Game(window, graphicsDevice);

        game.Create();
        window.Resized += game.Resize;

        while (window.Exists)
        {
            window.PumpEvents();

            if (window.Exists)
            {
                game.Update();
                game.Render();
            }
        }
        
        game.Destroy();
    }
}