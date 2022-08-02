using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace NuclearEditor;

public class EditorWrapper : GameWindow
{
    private readonly Editor Editor;
    
    public EditorWrapper(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        Editor = new Editor(this);
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        Console.WriteLine(GL.GetString(StringName.Version));
        Editor.Load();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);
        Editor.Render(e);
        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);
        Editor.Update(e);
    }
    
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        Editor.Resize(e);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        Editor.Destroy();
    }
    
    protected override void OnTextInput(TextInputEventArgs e)
    {
        base.OnTextInput(e);
        Editor.TextInput(e);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);
        Editor.MouseWheel(e);
    }
}