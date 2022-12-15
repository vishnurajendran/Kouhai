using Kouhai.Scripting.Interpretter;
using MoonSharp.Interpreter;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kouhai.Scripting.Proxies {

    public class AudioClipData
    {
        private string ClipName;
        private bool Loop;
        private int Volume;

        public AudioClipData() { }
        public AudioClipData(string name, bool loop, int volume)
        {
            this.ClipName = name;
            this.Loop = loop;
            this.Volume = volume;
        }

        public Table ToTable(Script owner)
        {
            var table = new Table(owner);
            table["ClipName"] = ClipName;
            table["Loop"] = Loop;
            table["Volume"] = Volume;
            return table;
        }

        public static AudioClipData FromTable(Table table)
        {
            return new AudioClipData()
            {
                ClipName = table.Get("ClipName").String,
                Loop = table.Get("Loop").Boolean,
                Volume = (int)table.Get("Volume").Number
            };
        }
    }

    [MoonSharpUserData]
    public class KouhaiSceneProxy : KouhaiRuntimeProxy
    {
        private readonly Core.KouhaiSceneVisuals sceneVisuals;
        private readonly Core.KouhaiSceneAudio sceneAudio;

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

    }
}
