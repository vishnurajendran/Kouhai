using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class KouhaiSceneEffects : MonoBehaviour
{
    [SerializeField] private Transform baseSceneParent;
    [SerializeField] private CanvasGroup blackOverlay;
    [SerializeField] private CanvasGroup whiteOverlay;
    [SerializeField] private PostProcessVolume ppVolume;

    [SerializeField] private bool test;
    private IEnumerator Start()
    {
        if (test)
        {
            yield return new WaitForSeconds(5);
            SetVignetteIntensity(0.5f, 2);
            SetVignetteColor(Color.red, 2);
            yield return new WaitForSeconds(2);
            SetVignetteColor(Color.black, 1);
            SetVignetteIntensity(0.15f, 1);
            yield return new WaitForSeconds(2);
            SetSaturation(-1, 1);
            yield return new WaitForSeconds(2);
            SetSaturation(0, 1);
            yield return new WaitForSeconds(2);
            FlashScreen(1, Color.white);
            yield return new WaitForSeconds(2);
            FadeToBlack(2);
        }
    }

    /// <summary>
    /// Shake screen
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="strength"></param>
    public void ShakeScreen(float duration, float strength)
    {
        baseSceneParent.DOShakePosition(duration, strength);
    }

    /// <summary>
    /// Fade to black
    /// </summary>
    /// <param name="duration"></param>
    public void FadeToBlack(float duration)
    {
        blackOverlay.DOFade(1, duration);
    }

    /// <summary>
    /// Fade from black
    /// </summary>
    /// <param name="duration"></param>
    public void FadeFromBlack(float duration)
    {
        blackOverlay.DOFade(0, duration);
    }

    /// <summary>
    /// Flashes screen with a specific color
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="color"></param>
    public void FlashScreen(float duration, Color color)
    {
        whiteOverlay.GetComponent<Image>().color = color;
        whiteOverlay.DOFade(1,duration/2).OnComplete(() =>
        {
            whiteOverlay.DOFade(0, duration / 2);
        });
    }
    
    /// <summary>
    /// Sets chromatic aberration intensity
    /// </summary>
    /// <param name="intensity"></param>
    /// <param name="duration"></param>
    public void SetChromaticAberation(float intensity, float duration)
    {
        intensity = Mathf.Clamp01(intensity);
        var caIntensity = ppVolume.profile.GetSetting<ChromaticAberration>().intensity;
        var currValue = caIntensity.value;
        DOTween.To(() => currValue, x => currValue = x, intensity, duration)
            .OnUpdate(() =>
            {
                caIntensity.value = currValue;
            });
    }
    
    /// <summary>
    /// Sets bloom intensity
    /// </summary>
    /// <param name="intensity"></param>
    /// <param name="duration"></param>
    public void SetBloom(float intensity, float duration)
    {
        intensity = Mathf.Clamp01(intensity);
        var blmIntensity = ppVolume.profile.GetSetting<Bloom>().intensity;
        var currValue = blmIntensity.value;
        DOTween.To(() => currValue, x => currValue = x, intensity, duration)
            .OnUpdate(() =>
            {
                blmIntensity.value = currValue;
            });
    }
    
    /// <summary>
    /// Set film grain intensity
    /// </summary>
    /// <param name="intensity"></param>
    /// <param name="duration"></param>
    public void SetGrain(float intensity, float duration)
    {
        intensity = Mathf.Clamp01(intensity);
        var grnIntensity = ppVolume.profile.GetSetting<Grain>().intensity;
        var currValue = grnIntensity.value;
        DOTween.To(() => currValue, x => currValue = x, intensity, duration)
            .OnUpdate(() =>
            {
                grnIntensity.value = currValue;
            });
    }
    
    /// <summary>
    /// Set vignette intensity
    /// </summary>
    /// <param name="intensity"></param>
    /// <param name="duration"></param>
    public void SetVignetteIntensity(float intensity, float duration)
    {
        intensity = Mathf.Clamp01(intensity);
        var vigIntensity = ppVolume.profile.GetSetting<Vignette>().intensity;
        var currValue = vigIntensity.value;
        DOTween.To(() => currValue, x => currValue = x, intensity, duration)
            .OnUpdate(() =>
            {
                vigIntensity.value = currValue;
            });
    }
    
    /// <summary>
    /// Set vignette color
    /// </summary>
    /// <param name="color"></param>
    /// <param name="duration"></param>
    public void SetVignetteColor(Color color, float duration)
    {
        var caIntensity = ppVolume.profile.GetSetting<Vignette>().color;
        var currValue = caIntensity.value;
        DOTween.To(() => currValue, x => currValue = x, color, duration)
            .OnUpdate(() =>
            {
                caIntensity.value = currValue;
            });
    }

    /// <summary>
    /// Sets saturation of scene
    /// </summary>
    /// <param name="saturation"></param>
    /// <param name="duration"></param>
    public void SetSaturation(float saturation, float duration)
    {
        saturation = Mathf.Clamp((int)(saturation * 100),-100,100);
        var satIntensity = ppVolume.profile.GetSetting<ColorGrading>().saturation;
        var currValue = satIntensity.value;
        DOTween.To(() => currValue, x => currValue = x, saturation, duration)
            .OnUpdate(() =>
            {
                satIntensity.value = currValue;
            });
    }
    
}
