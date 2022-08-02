using System.Numerics;
using ImGuiNET;
using NuclearGame;
using OpenTK.Graphics.OpenGL4;
using OpenTK.ImGui;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Vector2 = System.Numerics.Vector2;

namespace NuclearEditor;

public class Editor
{

    private GameWindow _window;
    private Game _game;
    
    private ImGuiController _controller;
    private OpenGlFramebuffer? gameFramebuffer;

    private Vector2 viewportPanelSize;

    public Editor(GameWindow gameWindow)
    {
        _window = gameWindow;
    }
    
    public void Load()
    {
        _controller = new ImGuiController(_window.Size.X, _window.Size.Y);

        gameFramebuffer = new OpenGlFramebuffer(800, 800);
        gameFramebuffer.Generate();
        
        gameFramebuffer.Bind();
        _game = new Game(_window);
        _game.Load();
        gameFramebuffer.Unbind();
    }

    private int counter = 0;
    private double frameTime = 0d;
    
    public void Render(FrameEventArgs e)
    {
        counter++;
        if (counter == 100)
        {
            frameTime = e.Time;
            counter = 0;
        }

        _controller.Update(_window, (float)e.Time);

        gameFramebuffer.Bind();
        _game.Render(e);
        gameFramebuffer.Unbind();

        GL.Viewport(0, 0, _window.Size.X, _window.Size.Y);
        GL.ClearColor(new Color4(0, 32, 48, 255));
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

        ImGui.Begin("Viewport");
        var panelSize = ImGui.GetContentRegionAvail();
        if (viewportPanelSize != panelSize)
        {
            gameFramebuffer.Resize((int) panelSize.X, (int) panelSize.Y);
            _game.EditorResize((int)panelSize.X, (int)panelSize.Y);
            viewportPanelSize = panelSize;
        }
        ImGui.Image(gameFramebuffer.GetColorAttachment(), viewportPanelSize, new Vector2(0, 1), new Vector2(1, 0));
        ImGui.End();

        ImGui.Begin("Render Stats");
        ImGui.Text($"MS: {frameTime * 1000f}");
        ImGui.End();
        
        ImGui.ShowDemoWindow();
        _controller.Render();
    }

    public void Update(FrameEventArgs e)
    {
    }

    public void Resize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, e.Width, e.Height);
        _controller.WindowResized(e.Width, e.Height);
    }
    
    public void Destroy()
    {
        _controller.Dispose();
        _game.Destroy();
    }

    public void TextInput(TextInputEventArgs e)
    {
        _controller.PressChar((char)e.Unicode);
    }

    public void MouseWheel(MouseWheelEventArgs e)
    {
        _controller.MouseScroll(e.Offset);
    }
}