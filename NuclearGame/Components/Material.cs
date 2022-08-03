using NuclearGame.Components.Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace NuclearGame.Components;

public struct Material
{
    public string shader;
    public string diffuseTexture;

    private TextureData TextureData;
    private int shaderPointer;

    public void LoadTexture()
    {
        var image = Image.Load<Rgba32>(diffuseTexture);

        if (image.TryGetSinglePixelSpan(out var pixelSpan))
        {
            TextureData = new TextureData
            {
                Width = image.Width,
                Height = image.Height,
                PixelData = pixelSpan.ToArray()
            };
        }
    }

    public TextureData GetTextureData()
    {
        return TextureData;
    }
}