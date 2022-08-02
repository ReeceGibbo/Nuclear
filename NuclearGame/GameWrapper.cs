using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace NuclearGame;

public class GameWrapper : GameWindow
{

    private readonly Game Game;
    
    public GameWrapper(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        Game = new Game(this);
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        Console.WriteLine(GL.GetString(StringName.Version));
        Game.Load();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);
        Game.Render(e);
        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);
        Game.Update(e, KeyboardState.GetSnapshot(), MouseState.GetSnapshot());
    }
    
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        Game.Resize(e);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        Game.Destroy();
    }
}