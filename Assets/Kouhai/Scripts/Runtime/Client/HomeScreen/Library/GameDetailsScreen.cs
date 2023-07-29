using System;
using System.Collections;
using Kouhai.Constants;
using Kouhai.Publishing;
using Kouhai.Runtime.System;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kouhai.Runtime.Client
{
    public class GameDetailsScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descText;
        [SerializeField] private TMP_Text developerText;
        [SerializeField] private Image icon;
        [SerializeField] private Button playButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private Button closeButton;

        public void Initialise(string gameDirectory, Action<string> OnPlayClicked,Action<string> OnDeleteClicked, Action OnCloseClicked)
        {
            var pubPath = $"{gameDirectory}/{KouhaiConstants.PUBLISH_FILENAME}";
            var pubData = KouhaiPublishingData.GetFromFile(pubPath);
            titleText.text = pubData.ProjectName;
            descText.text = pubData.ProjectDescription;
            developerText.text = pubData.Developer;
            
            playButton.onClick.AddListener(() =>
            {
                OnPlayClicked?.Invoke(gameDirectory);
            });
            deleteButton.onClick.AddListener(() =>
            {
                OnDeleteClicked?.Invoke(gameDirectory);
            });
            closeButton.onClick.AddListener(() =>
            {
                OnCloseClicked?.Invoke();
            });

            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            yield return new WaitForSeconds(0.25f);
            var cg = GetComponentInChildren<CanvasGroup>();
            float timeStep = 0;
            while (timeStep <= 1)
            {
                timeStep += Time.deltaTime / 0.15f;
                cg.alpha = Mathf.Lerp(0, 1, timeStep);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}