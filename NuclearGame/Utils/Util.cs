namespace NuclearGame.Utils;

public static class Util
{
    public static bool UseReverseDepth = false;
    public static bool IsClipSpaceYInverted = false;

    public static float DegreesToRadians(float degrees)
    {
        return (float) ((Math.PI / 180f) * degrees);
    }
}