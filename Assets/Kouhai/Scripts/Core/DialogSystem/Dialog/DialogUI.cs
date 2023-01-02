using Kouhai.Core.Input;
using Kouhai.Debugging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kouhai.Core
{
    public class DialogUI : MonoBehaviour
    {
        [field: SerializeField]
        public bool IsDialogOngoing { get; private set; }

        [SerializeField] private TMPro.TMP_Text dialogText;
        [SerializeField] private CanvasGroup dialogCanvas;

        private bool dialogWindow = false;
        private Coroutine fadeRoutine;
        private Coroutine textRoutine;

        private bool CanInteract { get; set; } = false;
        private Queue<string> textQueue;

        private void Start()
        {
            RegisterInputs();
        }

        private void RegisterInputs()
        {
            KouhaiInputSystem.Instance.OnUserSelects += () => {
                if (!CanInteract)
                    return;

                KouhaiGlobals.IsWaitingForPlayerInput = false;
                ContinueToNext();
            };
        }

        public void SayDialog(string dialogText)
        {
            if(textQueue == null)
                textQueue = new Queue<string>();

            textQueue.Enqueue(dialogText);
            ShowDialog(true, StartDialog);
            KouhaiGlobals.IsWaitingForPlayerInput = true;
        }

        private void ShowDialog(bool show, System.Action onComplete = null)
        {
            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);

            if (show)
            {
                IsDialogOngoing = true;
                dialogWindow = true;
            }

            fadeRoutine = StartCoroutine(DisplayDialog(show, KouhaiGlobals.FadeDuration, () => { 
                if(!show)
                    IsDialogOngoing=false;
                onComplete?.Invoke();   
            }));
        }

        private void StartDialog()
        {
            var text = textQueue.Dequeue();
            if (textRoutine != null)
                StopCoroutine(textRoutine);
            
            if(textQueue.Count <= 0)
                CanInteract = false;

            textRoutine = StartCoroutine(StartTextAnimationRoutine(text, KouhaiGlobals.TextAnimationDuration, ()=> { CanInteract = true; }));
        }

        private void ContinueToNext()
        {
            StartCoroutine(LateContinue());
        }

        private IEnumerator LateContinue()
        {
            yield return KouhaiGlobals.DelayBetweenStatementsExecution;
            if (!CanInteract)
                yield break;

            if (textQueue.Count <= 0)
            {
                CanInteract = false;
                ShowDialog(false, () => {
                    dialogText.text = string.Empty;
                    dialogWindow = false;
                });
            }
            else
            {
                StartDialog();
            }
        }

        private IEnumerator StartTextAnimationRoutine(string text, float interval, System.Action onComplete = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                onComplete?.Invoke();
                yield break;
            }

            dialogText.text = "";
            string[] splits = text.Split(" ");
            for (int i = 0; i < splits.Length; i++)
            {
                dialogText.text += splits[i] + (i < splits.Length - 1 ? " " : "");
                yield return new WaitForSeconds(interval);
            }
            yield return new WaitForEndOfFrame();
            onComplete?.Invoke();
        }

        private IEnumerator DisplayDialog(bool fadeIn, float dur, System.Action onComplete = null)
        {
            float timeStep = 0;
            float start = dialogCanvas.alpha;
            float end = fadeIn ? 1f : 0f;

            if(start == end)
            {
                onComplete?.Invoke();
                yield break;
            }

            while (timeStep <= 1)
            {
                timeStep += Time.deltaTime / dur;
                dialogCanvas.alpha = Mathf.Lerp(start, end, timeStep);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
            onComplete?.Invoke();
        }
    }
}
