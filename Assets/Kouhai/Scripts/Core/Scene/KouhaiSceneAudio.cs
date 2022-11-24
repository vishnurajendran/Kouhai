using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kouhai.Core
{
    public class KouhaiSceneAudio : MonoBehaviour
    {
        private const string RES_BKG_AUDIO_PATH = "Audio/Backgrounds/";

        [SerializeField]
        private AudioSource background;

        public void PlayBackgroundMusic(string backgroundClipName, bool loop, int volume)
        {
            background.clip = Resources.Load<AudioClip>($"{RES_BKG_AUDIO_PATH}{backgroundClipName}");
            background.loop = loop;
            background.volume = (float)volume / 100;
            background.Play();
        }

        public (string, bool, int) GetCurrentMusic()
        {
            return new ( 
                background.clip != null ? background.clip.name : "", 
                background.loop,
                (int)background.volume * 100 
            );
        }
    }
}
