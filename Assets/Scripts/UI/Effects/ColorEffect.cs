using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(ColorEffectRenderer), PostProcessEvent.AfterStack, "Custom/ColorEffect")]
public sealed class ColorEffect : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Grayscale effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };
    public FloatParameter intensity = new FloatParameter { value = 0.5f };
    public ColorParameter effectColor = new ColorParameter {value = Color.white };
}

public sealed class ColorEffectRenderer : PostProcessEffectRenderer<ColorEffect>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/ColorEffect"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        sheet.properties.SetColor("_Intensity", Color.HSVToRGB(0,0, settings.intensity));
        sheet.properties.SetColor("_EffectColor", settings.effectColor);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
