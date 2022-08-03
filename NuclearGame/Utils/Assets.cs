﻿namespace NuclearGame.Utils;

internal static class Assets
{
    private static readonly string SAssetRoot = Path.Combine(AppContext.BaseDirectory, "Assets");

    internal static string GetPath(string assetPath)
    {
        return Path.Combine(SAssetRoot, assetPath);
    }
}