using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(ColorBlindCorrectionRenderer), PostProcessEvent.AfterStack, "Custom/ColorBlindCorrection")]
public sealed class ColorBlindCorrection : PostProcessEffectSettings
{
    [Range(0, 6)]
    public IntParameter mode = new() { value = 0 };
}

public class ColorBlindCorrectionRenderer : PostProcessEffectRenderer<ColorBlindCorrection>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/ColorBlindCorrection"));
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, settings.mode.value);
    }
}
