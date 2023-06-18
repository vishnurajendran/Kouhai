using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kouhai.Runtime.System
{
    public class KouhaiLoadingScreen : MonoBehaviour
    {
        [SerializeField]
        private Image progress;

        [SerializeField] private TMPro.TMP_Text infoText;
        
        private Coroutine fadeRoutine;
        private Coroutine waitRoutine;
        
        private static KouhaiLoadingScreen instance;
        public static KouhaiLoadingScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<KouhaiLoadingScreen>();
                    if (instance == null)
                    {
                        var go = Instantiate(Resources.Load<GameObject>(KouhaiResourcesPath.LoadingScreenPath));
                        DontDestroyOnLoad(go);
                        instance = go.GetComponent<KouhaiLoadingScreen>();
                    }
                }
                return instance;
            }
        }

        public void Show(string msg,Action onComplete=null)
        {
            SetText(msg);
            this.gameObject.SetActive(true);
            StopWaitingAniamtion();
            Fade(true, () =>
            {
                onComplete?.Invoke();
            });
        }

        public void SetText(string msg)
        {
            this.infoText.text = msg;
        }
        
        public void Hide(Action onComplete=null)
        {
            StopWaitingAniamtion();
            Fade(false, () =>
            {
                this.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        }
        
        private void Fade(bool fadeIn, Action onComplete)
        {
            if(fadeRoutine != null)
                StopCoroutine(fadeRoutine);

            fadeRoutine = StartCoroutine(FadeRoutine(fadeIn, onComplete));
        }
        
        IEnumerator FadeRoutine(bool fadeIn, Action onComplete)
        {
            var cg = GetComponent<CanvasGroup>();
            float timeStep = 0;
            float currAlpha = cg.alpha;
            float newAlpha = fadeIn ? 1 : 0;
            
            if (currAlpha == newAlpha)
            {
                onComplete?.Invoke();
                yield break;
            }
            
            while (timeStep <= 1)
            {
                timeStep += Time.deltaTime / 1f;
                cg.alpha = Mathf.Lerp(currAlpha, newAlpha, timeStep);
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForEndOfFrame();
            onComplete?.Invoke();
        }

        public void ShowWaitingAniamtion()
        {
            StopWaitingAniamtion();
            waitRoutine = StartCoroutine(WaitingAnimation());
        }
        
        public void StopWaitingAniamtion()
        {
            progress.fillOrigin = (progress.fillOrigin + 1) % 2;
            progress.fillAmount = 0;

            if (waitRoutine != null)
                StopCoroutine(waitRoutine);
            waitRoutine = null;
        }
        
        IEnumerator WaitingAnimation()
        {
            float timeStep = 0;
            progress.fillOrigin = 0;
            var min = 0;
            var max = 1;
            while (true)
            {
                timeStep = 0;
                progress.fillOrigin = (progress.fillOrigin + 1) % 2;
                min = (min + 1) % 2;
                max = (max + 1) % 2;
                
                while (timeStep <= 1)
                {
                    timeStep += Time.deltaTime / 1f;
                    progress.fillAmount = Mathf.Lerp(min, max, timeStep);
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }

}
