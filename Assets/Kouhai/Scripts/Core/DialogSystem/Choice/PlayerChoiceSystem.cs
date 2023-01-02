using Kouhai.Debugging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kouhai.Core
{
    public class PlayerChoiceSystem : MonoBehaviour
    {
        [SerializeField]
        private DialogUI dialogSystem;

        [SerializeField]
        private CanvasGroup cg;

        [SerializeField]
        private Transform choiceParent;

        [SerializeField]
        private GameObject choiceItemRef;

        private List<PlayerChoiceItem> choices;
        public int PlayerChoice { get; private set; }

        public void SetChoices(List<string> choices)
        {
            KouhaiGlobals.IsWaitingForPlayerInput = true;
            PlayerChoiceReset();
            LoadChoices(choices);
        }

        public void SetPlayerChoice(int id)
        {
            PlayerChoice = id;
            StartCoroutine(Fade(false, KouhaiGlobals.FadeDuration, onComplete: () =>
            {
                KouhaiGlobals.IsWaitingForPlayerInput = false;
                ClearChoices();
            }));
        }

        private void PlayerChoiceReset()
        {
            PlayerChoice = 0;
        }

        private void ClearChoices()
        {
            if (choices == null)
                return;

            foreach(var choice in choices)
            {
                if(choice != null)
                    Destroy(choice.gameObject);
            }

            choices.Clear();
        }

        private void LoadChoices(List<string> choicesList)
        {
            StartCoroutine(LoadChoicesRoutine(choicesList));
        }

        private IEnumerator LoadChoicesRoutine(List<string> choicesList)
        {
            yield return new WaitWhile(()=> dialogSystem.IsDialogOngoing);

            if (choices == null)
                choices = new List<PlayerChoiceItem>();

            ClearChoices();
            foreach (var choice in choicesList)
            {
                var go = Instantiate(choiceItemRef, choiceParent);
                go.SetActive(true);
                PlayerChoiceItem item = go.GetComponent<PlayerChoiceItem>();
                item.Init(this, choice, choicesList.IndexOf(choice) + 1);
                choices.Add(item);
            }

            LayoutRebuilder.MarkLayoutForRebuild(choiceParent.GetComponent<RectTransform>());
            StartCoroutine(Fade(true, KouhaiGlobals.FadeDuration));
        }

        private IEnumerator Fade(bool fadeIn, float dur, System.Action onComplete=null)
        {
            float timeStep = 0;
            float currAlpha = cg.alpha;
            float newAlpha = fadeIn ? 1f : 0f;
            yield return new WaitForEndOfFrame();

            if (!fadeIn)
                cg.blocksRaycasts = false;

            while (timeStep <= 1)
            {
                timeStep += Time.deltaTime / dur;
                cg.alpha = Mathf.Lerp(currAlpha, newAlpha, timeStep);
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForEndOfFrame(); 
            
            if (fadeIn)
                cg.blocksRaycasts = true;
            onComplete?.Invoke();
        }
    }
}
