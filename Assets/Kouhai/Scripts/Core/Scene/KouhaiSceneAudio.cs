using MoonSharp.Interpreter;
using RuntimeDeveloperConsole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kouhai.Core
{
    public class KouhaiSceneAudio : MonoBehaviour
    {
        private const string RES_BKG_AUDIO_PATH = "Audio/Backgrounds/";
        private const string RES_AMB_AUDIO_PATH = "Audio/Ambiance/";
        private const string RES_SFX_AUDIO_PATH = "Audio/SFX/";

        [SerializeField]
        private AudioSource background;
        [SerializeField]
        private AudioSource ambiance;
        [SerializeField]
        private AudioSource sfx;

        public void PlayBackgroundMusic(string backgroundClipName)
        {
            Debug.Log("Playing " + backgroundClipName);
            background.clip = Resources.Load<AudioClip>($"{RES_BKG_AUDIO_PATH}{backgroundClipName}");
            background.Play();
        }

        public void PlayAmbiance(string ambianceClipName)
        {
            Debug.Log("Playing " + ambianceClipName);
            ambiance.clip = Resources.Load<AudioClip>($"{RES_AMB_AUDIO_PATH}{ambianceClipName}");
            ambiance.Play();
        }

        public void PlaySFX(string sfxClipName)
        {
            Debug.Log("Playing " + sfxClipName);
            sfx.clip = Resources.Load<AudioClip>($"{RES_SFX_AUDIO_PATH}{sfxClipName}");
            sfx.Play();
        }

    }

    public static class SceneAudioConsoleCompanion{
        private static KouhaiSceneAudio GetInstance()
        {
            return GameObject.FindObjectOfType<KouhaiSceneAudio>();
        }

        [ConsoleCommand("Play Background Music", "playmusic <clipname>")]
        public static void PlayMusic(string[] args)
        {
            if (args == null || args.Length <= 0)
                return;

            GetInstance().PlayBackgroundMusic(args[0]);
        }

        [ConsoleCommand("Play Ambiance Music", "playambiance <clipname>")]
        public static void PlayAmbiance(string[] args)
        {
            if (args == null || args.Length <= 0)
                return;

            GetInstance().PlayAmbiance(args[0]);
        }

        [ConsoleCommand("Play SFX Music", "playsfx <clipname>")]
        public static void PlaySFX(string[] args)
        {
            if (args == null || args.Length <= 0)
                return;

            GetInstance().PlaySFX(args[0]);
        }
    }
}
