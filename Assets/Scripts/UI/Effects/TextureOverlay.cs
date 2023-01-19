/*using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[UnityEngine.Scripting.Preserve]
[Serializable, VolumeComponentMenu("Custom/TextureOverlay")]*/
/*public sealed class TextureOverlay : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    [Header("Texture")]
    [Tooltip("Texture Overlay")]
    public TextureParameter texture = new TextureParameter(null);
    [Tooltip("Tiling")]
    public Vector2Parameter tiling = new Vector2Parameter(new Vector2(1,1));
    [Tooltip("Offset")]
    public Vector2Parameter offset = new Vector2Parameter(new Vector2(0, 0));
    [Tooltip("Keep Aspect Ratio")]
    public BoolParameter keepAspectRatio = new BoolParameter(false);

    [Header("Alpha Cutout")]
    [Tooltip("Active")]
    public BoolParameter alphaIsTransparent = new BoolParameter(true);

    public bool IsActive()
    {
        throw new NotImplementedException();
    }
}*/

/*[UnityEngine.Scripting.Preserve]
public sealed class TextureOverlayRenderer : PostProcessEffectRenderer<TextureOverlay>
{*//*
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/TextureOverlay"));

        var imageTexture = settings.texture.value == null
                ? RuntimeUtilities.transparentTexture
                : settings.texture.value;

        sheet.properties.SetTexture("_Image", imageTexture);
        sheet.properties.SetVector("_Tiling", settings.tiling);
        sheet.properties.SetVector("_Offset", settings.offset);
        sheet.properties.SetInt("_KeepAspectRatio", BoolToInt(settings.keepAspectRatio));
        sheet.properties.SetInt("_AlphaIsTransparent", BoolToInt(settings.alphaIsTransparent));
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }

    private int BoolToInt(bool b)
    {
        return b ? 1 : 0;
    }*//*
}*/

