using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace  Kouhai.Scripts.Runtime.Client
{
   public class KouhaiHomeIcon : MonoBehaviour
{
   [SerializeField] private KouhaiAppLocalisation.LocalisationTextType textType;
   [SerializeField] private float fadeTime = 0.15f;
   [SerializeField] private float fadeInDelay = 2;

   private Coroutine routine;
   
   public Action OnClick;
   
   private void Start()
   {
      SetupMenuButtons();
   }
   private void SetupMenuButtons()
   {
      GetComponentInChildren<TMP_Text>().text = KouhaiAppLocalisation.Current.GetLocalisedText(textType);
      GetComponentInChildren<Button>().onClick.AddListener(()=>OnClick?.Invoke());
      var evntTrigger = GetComponentInChildren<EventTrigger>();

      var pointerEnterTrigger = new EventTrigger.Entry();
      pointerEnterTrigger.eventID = EventTriggerType.PointerEnter;
      pointerEnterTrigger.callback.AddListener((data) =>
      {
         OnPointerEnter();
      });
      
      var pointerExitTrigger = new EventTrigger.Entry();
      pointerExitTrigger.eventID = EventTriggerType.PointerExit;
      pointerExitTrigger.callback.AddListener((data) =>
      {
         OnPointerExit();
      });

      evntTrigger.triggers.Add(pointerEnterTrigger);
      evntTrigger.triggers.Add(pointerExitTrigger);
   }

   private void OnPointerEnter()
   {
      Fade(true);
   }
   
   private void OnPointerExit()
   {
      Fade(false);
   }

   private void Fade(bool fadeIN)
   {
      if(routine != null)
         StopCoroutine(routine);
      routine = StartCoroutine(FadeRoutine(fadeIN));
   }
   

   IEnumerator FadeRoutine(bool fadeIN)
   {
      if (fadeIN)
      {
         yield return new WaitForSeconds(fadeInDelay);
      }

      var cg = GetComponentInChildren<CanvasGroup>();
      float from = cg.alpha;
      float to = fadeIN ? 1 : 0;
      float timeStep = 0;
      while (timeStep <= 1)
      {
         timeStep += Time.deltaTime / fadeTime;
         cg.alpha = Mathf.Lerp(from, to, timeStep);
         yield return new WaitForEndOfFrame();
      }
      yield return new WaitForEndOfFrame();
   }
}
}

