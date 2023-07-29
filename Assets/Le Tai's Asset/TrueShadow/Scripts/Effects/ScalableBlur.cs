using UnityEngine;
using UnityEngine.Rendering;

namespace LeTai.Effects
{
public class ScalableBlur : IBlurAlgorithm
{
    Material           material;
    ScalableBlurConfig config;

    const int BLUR_PASS      = 0;
    const int CROP_BLUR_PASS = 1;

    Material Material
    {
        get
        {
            if (material == null)
                Material = new Material(Shader.Find("Hidden/EfficientBlur"));

            return material;
        }
        set => material = value;
    }

    ~ScalableBlur()
    {
        if (!material) return;

        if (Application.isPlaying)
            Object.Destroy(material);
        else
            Object.DestroyImmediate(material);
    }

    public void Configure(BlurConfig config)
    {
        this.config = (ScalableBlurConfig) config;
    }

    public void Blur(CommandBuffer          cmd,
                     RenderTargetIdentifier src,
                     Rect                   srcCropRegion,
                     RenderTexture          target)
    {
        float radius = config.Radius;
        Material.SetFloat(ShaderProperties.blurRadius, radius);
        Material.SetVector(ShaderProperties.blurTextureCropRegion, srcCropRegion.ToMinMaxVector());

        int firstDownsampleFactor = config.Iteration > 0 ? 1 : 0;
        int stepCount             = Mathf.Max(config.Iteration * 2 - 1, 1);

        int firstIRT = ShaderProperties.intermediateRT[0];
        CreateTempRenderTextureFrom(cmd, firstIRT, target, firstDownsampleFactor);

        // cmd.BlitFullscreenTriangle(src, firstIRT, Material, CROP_BLUR_PASS);
        cmd.Blit(src, firstIRT, Material, CROP_BLUR_PASS);

        for (var i = 1; i < stepCount; i++)
        {
            BlurAtDepth(cmd, i, target);
        }

        // cmd.BlitFullscreenTriangle(ShaderProperties.intermediateRT[stepCount - 1], target, Material, BLUR_PASS);
        cmd.Blit(ShaderProperties.intermediateRT[stepCount - 1], target, Material, BLUR_PASS);

        CleanupIntermediateRT(cmd, stepCount);
    }

    protected virtual void BlurAtDepth(CommandBuffer cmd, int depth, RenderTexture baseTexture)
    {
        int sizeLevel = Utility.SimplePingPong(depth, config.Iteration - 1) + 1;
        sizeLevel = Mathf.Min(sizeLevel, config.MaxDepth);
        CreateTempRenderTextureFrom(cmd, ShaderProperties.intermediateRT[depth], baseTexture, sizeLevel);

        // cmd.BlitFullscreenTriangle(
        cmd.Blit(
            ShaderProperties.intermediateRT[depth - 1],
            ShaderProperties.intermediateRT[depth],
            Material,
            0
        );
    }

    static void CreateTempRenderTextureFrom(CommandBuffer cmd,
                                            int           nameId,
                                            RenderTexture src,
                                            int           downsampleFactor)
    {
        int w = src.width >> downsampleFactor; //= width / 2^downsample
        int h = src.height >> downsampleFactor;

        cmd.GetTemporaryRT(nameId, w, h, 0, FilterMode.Bilinear, src.format);
    }

    static void CleanupIntermediateRT(CommandBuffer cmd, int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            cmd.ReleaseTemporaryRT(ShaderProperties.intermediateRT[i]);
        }
    }
}
}
