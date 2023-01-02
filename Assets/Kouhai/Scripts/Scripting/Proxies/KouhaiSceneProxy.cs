using Kouhai.Scripting.Interpretter;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Kouhai.Scripting.Proxies {
    
    [MoonSharpUserData]
    public class KouhaiSceneProxy : KouhaiRuntimeProxy
    {
        private readonly Core.KouhaiSceneVisuals sceneVisuals;
        private readonly Core.KouhaiSceneAudio sceneAudio;
        private readonly Core.KouhaiSceneEffects sceneEffects;

        [MoonSharpHidden]
        public override string Symbol => "Scene";

        [MoonSharpHidden]
        public override KouhaiRuntimeProxy GetProxyInstance()
        {
            return new KouhaiSceneProxy();
        }

        public KouhaiSceneProxy()
        {
            sceneVisuals = GameObject.FindObjectOfType<Core.KouhaiSceneVisuals>();
            sceneAudio = GameObject.FindObjectOfType<Core.KouhaiSceneAudio>();
            sceneEffects = GameObject.FindObjectOfType<Core.KouhaiSceneEffects>();
        }
        
        public void SetBackgroundImage(string bgImageName)
        {
            sceneVisuals.ChangeBackground(bgImageName);
        }
        public void PlayBackgroundMusic(string backgroundClip)
        {
            sceneAudio.PlayBackgroundMusic(backgroundClip);
        }
        public void PlayAmbiance(string ambianceClip)
        {
            sceneAudio.PlayAmbiance(ambianceClip);
        }
        public void PlaySFX(string sfxClipName)
        {
            sceneAudio.PlaySFX(sfxClipName);
        }
        public void ShakeScreen(float intensity, float duration)
        {
            sceneEffects.ShakeScreen(intensity, duration);   
        }
        public void FadeToBlack(float duration)
        {
            sceneEffects.FadeToBlack(duration);
        }
        public void FadeFromBlack(float duration)
        {
            sceneEffects.FadeFromBlack(duration);
        }
        public void FlashScreen(Table color, float duration)
        {
            Debug.Log($"Color {(float)color.Get("r").Number} {(float)color.Get("g").Number} {(float)color.Get("b").Number}");
            var col = new Color((float)color.Get("r").Number, (float)color.Get("g").Number,
                (float)color.Get("b").Number, 1);
            sceneEffects.FlashScreen(duration, col);
        }
        public void SetChromaticAberration(float intensity, float duration)
        {
            sceneEffects.SetChromaticAberration(intensity, duration);
        }
        public void SetBloom(float intensity, float duration)
        {
            sceneEffects.SetBloom(intensity, duration);
        }
        public void SetGrain(float intensity, float duration)
        {
            sceneEffects.SetGrain(intensity, duration);
        }
        public void SetVignetteIntensity(float intensity, float duration)
        {
            sceneEffects.SetVignetteIntensity(intensity, duration);
        }
        public void SetVignetteColor(Table color, float duration)
        {
            var col = new Color((float)color.Get("r").Number, (float)color.Get("g").Number,
                (float)color.Get("b").Number, 1);
            sceneEffects.SetVignetteColor(col, duration);
        }
        public void SetSaturation(float saturation, float duration)
        {
            sceneEffects.SetSaturation(saturation, duration);
        }
    }
}
