using System;
using UnityEngine;
using UnityEngine.UI;

namespace LeTai.TrueShadow
{
[AddComponentMenu("")]
[ExecuteAlways]
public partial class ShadowRenderer : MonoBehaviour, ILayoutIgnorer, IMaterialModifier, IMeshModifier
{
    public bool ignoreLayout => true;

    internal CanvasRenderer CanvasRenderer { get; private set; }

    TrueShadow    shadow;
    RectTransform rt;
    RawImage      graphic;
    Texture       shadowTexture;

    public static void Initialize(TrueShadow shadow, ref ShadowRenderer renderer)
    {
        if (renderer && renderer.shadow == shadow)
        {
            renderer.gameObject.SetActive(true);
            return;
        }

        var obj = new GameObject($"{shadow.gameObject.name}'s Shadow") {
#if LETAI_TRUESHADOW_DEBUG
            hideFlags = DebugSettings.Instance.showObjects
                            ? HideFlags.DontSave
                            : HideFlags.HideAndDontSave
#else
            hideFlags = HideFlags.HideAndDontSave
#endif
        };

#if LETAI_TRUESHADOW_DEBUG && UNITY_EDITOR
        UnityEditor.SceneVisibilityManager.instance.DisablePicking(obj, true);
#endif

        shadow.SetHierachyDirty();

        var rt = obj.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.zero;

        var graphic = obj.AddComponent<RawImage>();
        graphic.raycastTarget = false;
        graphic.color         = shadow.Color;

        renderer         = obj.AddComponent<ShadowRenderer>();
        renderer.shadow  = shadow;
        renderer.rt      = rt;
        renderer.graphic = graphic;

        // renderer.RecreateGraphic(shadow.Baked ? GraphicType.Image : GraphicType.RawImage);

        renderer.UpdateMaterial();

        renderer.CanvasRenderer = obj.GetComponent<CanvasRenderer>();
        renderer.CanvasRenderer.SetColor(shadow.IgnoreCasterColor ? Color.white : shadow.CanvasRenderer.GetColor());
        renderer.CanvasRenderer.SetAlpha(shadow.CanvasRenderer.GetAlpha());

        renderer.ReLayout();
    }

    public void UpdateMaterial()
    {
        if (shadow.Graphic is MaskableGraphic mg)
            graphic.maskable = mg.maskable;

        var mat = shadow.BlendMode.GetMaterial();
        graphic.material = mat ? mat : shadow.GetShadowRenderingNormalMaterial();
    }

    internal void ReLayout()
    {
        if (!isActiveAndEnabled)
            return;

        var casterRt = shadow.RectTransform;
        if (!casterRt)
        {
            CanvasRenderer.SetAlpha(0);
            return;
        }

        if (!shadowTexture)
        {
            CanvasRenderer.SetAlpha(0);
            return;
        }

        var nudgeSize = !(shadow.Graphic is Text);
        var container   = shadow.ShadowContainer;
        var canvasScale = container?.Snapshot?.canvasScale ?? graphic.canvas.scaleFactor;

        var casterMeshBounds = shadow.SpriteMesh.bounds;

        var misalignRatio = container == null
                                ? Vector2.one
                                : (Vector2)casterMeshBounds.size * canvasScale / (Vector2)container.ImprintSize;

        var shadowTexSize = new Vector2(shadowTexture.width, shadowTexture.height);
        shadowTexSize *= misalignRatio;
        shadowTexSize /= canvasScale;
        if (nudgeSize)
        {
            if (shadow.Inset)
                shadowTexSize += Vector2.one;
            else
                shadowTexSize -= Vector2.one;
        }

        rt.sizeDelta = shadowTexSize;


        float paddingLS = container?.Padding ?? Mathf.CeilToInt(shadow.Size * canvasScale);
        paddingLS /= canvasScale;
        if (nudgeSize)
        {
            if (shadow.Inset)
                paddingLS += .5f;
            else
                paddingLS -= .5f;
        }

        // pivot should be relative to the un-blurred part of the texture, not the whole mesh
        var casterPivotLS = -(Vector2)casterMeshBounds.min;
        rt.pivot = (casterPivotLS + paddingLS * misalignRatio) / shadowTexSize;


        var canvasRelativeOffset = container?.Snapshot?.canvasRelativeOffset / canvasScale ?? shadow.Offset;
        var offset = shadow.ShadowAsSibling
                         ? shadow.Offset.WithZ(0)
                         : canvasRelativeOffset.WithZ(0);
        rt.localPosition = shadow.ShadowAsSibling
                               ? casterRt.localPosition + offset
                               : offset;

        rt.localRotation = shadow.ShadowAsSibling ? casterRt.localRotation : Quaternion.identity;
        rt.localScale    = shadow.ShadowAsSibling ? casterRt.localScale : Vector3.one;


        var color = shadow.Color;
        if (shadow.UseCasterAlpha)
            color.a *= shadow.Graphic.color.a;
        graphic.color = color;

        CanvasRenderer.SetColor(shadow.IgnoreCasterColor ? Color.white : shadow.CanvasRenderer.GetColor());
        CanvasRenderer.SetAlpha(shadow.CanvasRenderer.GetAlpha());

        graphic.Rebuild(CanvasUpdate.PreRender);
    }

    public void SetTexture(Texture texture)
    {
        shadowTexture = texture;
        CanvasRenderer.SetTexture(texture);
        graphic.texture = texture;
    }

    public void SetMaterialDirty()
    {
        graphic.SetMaterialDirty();
    }

    public void ModifyMesh(VertexHelper vertexHelper)
    {
        if (!shadow)
            return;

        shadow.ModifyShadowRendererMesh(vertexHelper);
    }

    public void ModifyMesh(Mesh mesh)
    {
        Debug.Assert(true, "This should only be called on old unsupported Unity version");
    }

    protected virtual void LateUpdate()
    {
        // Destroy events are not consistently called for some reason, have to poll
        if (!shadow)
            Dispose();
    }

    bool willBeDestroyed;

    protected virtual void OnDestroy()
    {
        willBeDestroyed = true;
    }

    public void Dispose()
    {
        if (willBeDestroyed) return;

        if (shadow && shadow.ShadowAsSibling)
        {
            // Destroy does not happen immediately. Want out of hierarchy.
            gameObject.SetActive(false);
            transform.SetParent(null);
        }

#if UNITY_EDITOR
        // This look redundant but is necessary!
        if (!Application.isPlaying && !UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            DestroyImmediate(gameObject);
        else if (Application.isPlaying)
            Destroy(gameObject);
#else
            Destroy(gameObject);
#endif
    }
}
}
