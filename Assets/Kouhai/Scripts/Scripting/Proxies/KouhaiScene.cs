using Kouhai.Scripting.Interpretter;
using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kouhai.Scripting.Proxies {

    public class AudioClipData
    {
        public string ClipName;
        public bool Loop;
        public int Volume;

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
    public class KouhaiScene : KouhaiRuntimeProxy
    {
        private Core.KouhaiSceneVisuals sceneVisuals;
        private Core.KouhaiSceneAudio sceneAudio;

        public string Background
        {
            get
            {
                return sceneVisuals.GetCurrent();
            }
            set
            {
                sceneVisuals.ChangeBackground(value);
            }
        }

        public Table Music
        {
            get {
                var val = sceneAudio.GetCurrentMusic();
                return new AudioClipData(val.Item1, val.Item2, val.Item3).ToTable(ownerScript.luaScript); 
            }
            set {
                var clipData = AudioClipData.FromTable(value);
                sceneAudio.PlayBackgroundMusic(clipData.ClipName, clipData.Loop, clipData.Volume); 
            }
        }

        public void PlayBackgroundMusic(string backgroundClip, bool loop, int volume)
        {
            sceneAudio.PlayBackgroundMusic(backgroundClip, loop, volume);
        }

        [SerializeField]
        public override string Symbol => "Scene";
        [MoonSharpHidden]
        public override KouhaiRuntimeProxy GetProxyInstance()
        {
            return new KouhaiScene();
        }

        public KouhaiScene()
        {
            sceneVisuals = GameObject.FindObjectOfType<Core.KouhaiSceneVisuals>();
            sceneAudio = GameObject.FindObjectOfType<Core.KouhaiSceneAudio>();
        }
    }
}
