using ImGuiNET;
using NuclearGame.Input;
using Veldrid;
using Veldrid.Sdl2;

namespace NuclearEditor;

public class Editor
{
    private readonly Sdl2Window _window;
    private readonly GraphicsDevice _graphicsDevice;

    private CommandList _commandList;
    
    // Editor
    //private Game _game;
    
    private ImGuiRenderer _imGuiRenderer;
    
    public Editor(Sdl2Window window, GraphicsDevice graphicsDevice)
    {
        _window = window;
        _graphicsDevice = graphicsDevice;
    }
    
    public void Create()
    {
        _imGuiRenderer = new ImGuiRenderer(_graphicsDevice, _graphicsDevice.SwapchainFramebuffer.OutputDescription, 
            _window.Width, _window.Height);
        
        var factory = _graphicsDevice.ResourceFactory;
        _commandList = factory.CreateCommandList();
    }
    
    public void Render()
    {
        ImGui.ShowDemoWindow();
        
        _commandList.Begin();
        _commandList.SetFramebuffer(_graphicsDevice.SwapchainFramebuffer);
        _commandList.ClearColorTarget(0, RgbaFloat.LightGrey);
        _imGuiRenderer.Render(_graphicsDevice, _commandList);
        _commandList.End();

        _graphicsDevice.SubmitCommands(_commandList);
        _graphicsDevice.SwapBuffers();
    }

    public void Update(float delta)
    {
        _imGuiRenderer.Update(delta, InputTracker.FrameSnapshot);
    }

    public void Resize()
    {
        var width = _window.Width;
        var height = _window.Height;

        _graphicsDevice.ResizeMainWindow((uint) width, (uint) height);
        _imGuiRenderer.WindowResized(width, height);
    }
    
    public void Destroy()
    {
        _imGuiRenderer.Dispose();
        _graphicsDevice.Dispose();
    }
}