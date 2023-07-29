using System;
using System.Collections;
using Kouhai.Publishing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kouhai.Runtime.Client
{
    public class KouhaiLibraryItem : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text title;
        [SerializeField] private Image glow;

        private CanvasGroup glowCg;
        private Coroutine glowUpdate;
        private Coroutine glowReset;
        
        public void Initialise(KouhaiPublishingData publishingData,string gameDirPath, Action<string, Vector3> OnClick, Transform detailsParent)
        {
            title.text = publishingData.ProjectName;
            glowCg = glow.GetComponent<CanvasGroup>();
            
            GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                OnClick?.Invoke(gameDirPath, transform.position);
            });

            var eventTrigger = GetComponent<EventTrigger>();
            var pointerEntry = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerEnter
            };
            pointerEntry.callback.AddListener(OnPointerEntry);
            
            var pointerExit = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerExit
            };
            pointerExit.callback.AddListener(OnPointerExit);
            
            eventTrigger.triggers.Add(pointerEntry);
            eventTrigger.triggers.Add(pointerExit);
        }

        private void OnPointerEntry(BaseEventData data)
        {
            if(glowReset != null)
                StopCoroutine(glowReset);
            
            if(glowUpdate != null)
                StopCoroutine(glowUpdate);

            glowUpdate = StartCoroutine(PointerUpdate());
        }
        
        private void OnPointerExit(BaseEventData data)
        {
            if(glowUpdate != null)
                StopCoroutine(glowUpdate);
            
            if(glowReset != null)
                StopCoroutine(glowReset);

            glowReset = StartCoroutine(GlowReset());
        }
        
        IEnumerator PointerUpdate()
        {
            float timeStep = 0;
            var currAlpha = glowCg.alpha;
            while (true)
            {
                if (timeStep <= 1)
                {
                    timeStep += Time.deltaTime / 0.15f;
                    glowCg.alpha = Mathf.Lerp(currAlpha, 1f, timeStep);
                }
                
                var mouseInput = Input.mousePosition;
                glow.transform.position = Vector3.Lerp(glow.transform.position, mouseInput, 0.05f);
                yield return new WaitForEndOfFrame();
            }
        }
        
        IEnumerator GlowReset()
        {
            float timeStep = 0;
            var currAlpha = glowCg.alpha;
            while (timeStep <= 1)
            {
                timeStep += Time.deltaTime / 0.15f;
                glowCg.alpha = Mathf.Lerp(currAlpha, 0f, timeStep);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}