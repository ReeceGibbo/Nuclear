using System.Numerics;
using Veldrid;
using Veldrid.Sdl2;

namespace NuclearGame.Input;

public static class InputTracker
{
    private static readonly HashSet<Key> CurrentlyPressedKeys = new();
    private static readonly HashSet<Key> NewKeysThisFrame = new();

    private static readonly HashSet<MouseButton> CurrentlyPressedMouseButtons = new();
    private static readonly HashSet<MouseButton> NewMouseButtonsThisFrame = new();

    public static Vector2 MousePosition;
    public static Vector2 MouseDelta;
    public static InputSnapshot FrameSnapshot { get; private set; }

    public static bool GetKey(Key key)
    {
        return CurrentlyPressedKeys.Contains(key);
    }

    public static bool GetKeyDown(Key key)
    {
        return NewKeysThisFrame.Contains(key);
    }

    public static bool GetMouseButton(MouseButton button)
    {
        return CurrentlyPressedMouseButtons.Contains(button);
    }

    public static bool GetMouseButtonDown(MouseButton button)
    {
        return NewMouseButtonsThisFrame.Contains(button);
    }

    public static void UpdateFrameInput(InputSnapshot snapshot, Sdl2Window window)
    {
        FrameSnapshot = snapshot;
        NewKeysThisFrame.Clear();
        NewMouseButtonsThisFrame.Clear();

        MousePosition = snapshot.MousePosition;
        MouseDelta = window.MouseDelta;
        for (var i = 0; i < snapshot.KeyEvents.Count; i++)
        {
            var ke = snapshot.KeyEvents[i];
            if (ke.Down)
                KeyDown(ke.Key);
            else
                KeyUp(ke.Key);
        }

        for (var i = 0; i < snapshot.MouseEvents.Count; i++)
        {
            var me = snapshot.MouseEvents[i];
            if (me.Down)
                MouseDown(me.MouseButton);
            else
                MouseUp(me.MouseButton);
        }
    }

    private static void MouseUp(MouseButton mouseButton)
    {
        CurrentlyPressedMouseButtons.Remove(mouseButton);
        NewMouseButtonsThisFrame.Remove(mouseButton);
    }

    private static void MouseDown(MouseButton mouseButton)
    {
        if (CurrentlyPressedMouseButtons.Add(mouseButton)) NewMouseButtonsThisFrame.Add(mouseButton);
    }

    private static void KeyUp(Key key)
    {
        CurrentlyPressedKeys.Remove(key);
        NewKeysThisFrame.Remove(key);
    }

    private static void KeyDown(Key key)
    {
        if (CurrentlyPressedKeys.Add(key)) NewKeysThisFrame.Add(key);
    }
}