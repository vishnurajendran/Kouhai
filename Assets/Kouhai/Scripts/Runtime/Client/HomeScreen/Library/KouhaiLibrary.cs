using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Kouhai.Constants;
using Kouhai.Core.AssetManagement;
using Kouhai.Publishing;
using Kouhai.Runtime.System;
using Kouhai.Scripts.Runtime.System.Notification;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Kouhai.Runtime.Client
{
    public class KouhaiLibrary : MonoBehaviour, IHomescreenTab
    {
        [SerializeField] private Transform gameIconParent;
        [FormerlySerializedAs("libraryDetailsParent")] 
        [SerializeField] private Transform detailsGOParent;
        
        [SerializeField] private RectTransform searchFieldTransform;

        [SerializeField] private Button searchButton;

        private GameObject detailsPrefab;
        private GameObject detailsParentPrefab;

        private Transform gameDetailsParent;
        
        private Coroutine searchToggleRoutine;
        private bool searchToggle;
        private float toolbarWidth = 100;

        public void Initialise(float toolbarWidth)
        {
            this.toolbarWidth = toolbarWidth;
            var gameDirectoryInLibrary = GetGamesInLibrary();
            LoadLibrary(gameDirectoryInLibrary);
            searchButton.onClick.AddListener(ToggleSearch);
        }
        
        private string[] GetGamesInLibrary()
        {
            if (!Directory.Exists(KouhaiUserDataPaths.GamesRootDirectory))
                Directory.CreateDirectory(KouhaiUserDataPaths.GamesRootDirectory);
            
            return Directory.GetDirectories(KouhaiUserDataPaths.GamesRootDirectory);
        }

        private void LoadLibrary(string[] allGameDirs)
        {
            if (allGameDirs == null)
                return;
            
            foreach (var gameDir in allGameDirs)
            {
                CreateEntry(gameDir);
            }
        }

        private void CreateEntry(string gameDirPath)
        {
            var publishInfoPath = $"{gameDirPath}/{KouhaiConstants.PUBLISH_FILENAME}";
            var publishInfo = KouhaiPublishingData.GetFromFile(publishInfoPath);
            if (publishInfoPath == null)
                return;
            
            var gameIcon = Resources.Load<GameObject>(KouhaiResourcesPath.LibraryItem);
            var instance = Instantiate(gameIcon, gameIconParent);
            var libItme = instance.GetComponent<KouhaiLibraryItem>();
            libItme.Initialise(publishInfo, gameDirPath, OnItemSelected, detailsGOParent);
        }

        private void OnItemSelected(string selectedGameDirectory, Vector3 iconLocation)
        {
            Debug.Log($"Selected Game {selectedGameDirectory}");
            OpenDetails(selectedGameDirectory,iconLocation);
        }

        private void OpenDetails(string gameDir, Vector3 startPosition)
        {
            if (detailsParentPrefab == null)
                detailsParentPrefab = Resources.Load<GameObject>(KouhaiResourcesPath.GameDetailsParent);

            var go = Instantiate(detailsParentPrefab, detailsGOParent);
            go.transform.position = startPosition;
            gameDetailsParent = go.transform;
            StartCoroutine(ExpandDetailsParent(go.GetComponent<RectTransform>(), () =>
            {
                SpawnDetails(gameDir);
            }));
        }

        private void SpawnDetails(string gameDirectory)
        {
            if (detailsPrefab == null)
                detailsPrefab = Resources.Load<GameObject>(KouhaiResourcesPath.GameDetails);

            var go = Instantiate(detailsPrefab, gameDetailsParent);
            var gameDetails = go.GetComponent<GameDetailsScreen>();
            gameDetails.Initialise(gameDirectory, PlayGame, DeleteGameDirectory, CloseDetails);
        }

        private void PlayGame(string path)
        {
            KouhaiCrossSceneData.Instance.SetSelectedGameDirectory(path);
            SceneManager.LoadScene(KouhaiConstants.PLAYER_SCENE);
        }

        private void DeleteGameDirectory(string path)
        {
            var title = KouhaiAppLocalisation.Current.GetLocalisedText(KouhaiAppLocalisation.LocalisationTextType
                .DeleteGameTitle);
            var desc = KouhaiAppLocalisation.Current.GetLocalisedText(KouhaiAppLocalisation.LocalisationTextType
                .DeleteGameDesc);
            var yes = KouhaiAppLocalisation.Current.GetLocalisedText(KouhaiAppLocalisation.LocalisationTextType.Yes);
            var no = KouhaiAppLocalisation.Current.GetLocalisedText(KouhaiAppLocalisation.LocalisationTextType.No);
            KouhaiNotificationManager.ShowDialogYesNo(KouhaiDialogPopup.DialogType.INFO,title, desc,yes, no, () =>
            {
                DeleteGame(path);
            }, null);
        }

        private void DeleteGame(string path)
        {
            Debug.Log($"Deleting Game! {path}");
        }
        
        private void CloseDetails()
        {
            if(gameDetailsParent != null)
                Destroy(gameDetailsParent.gameObject);
        }
        
        IEnumerator ExpandDetailsParent(RectTransform rectTransform, Action onComplete=null)
        {
            Debug.Log(Screen.width);
            var targetSize = new Vector2(Screen.width - toolbarWidth, Screen.height);
            yield return new WaitForEndOfFrame();
            var currSize = rectTransform.sizeDelta;
            var currPos = rectTransform.anchoredPosition;
            float timeStep = 0;
            while (timeStep <= 1)
            {
                timeStep += Time.deltaTime / 0.15f;
                rectTransform.sizeDelta = Vector2.Lerp(currSize, targetSize, timeStep);
                rectTransform.anchoredPosition = Vector2.Lerp(currPos, Vector2.zero, timeStep);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMax = rectTransform.offsetMin = Vector2.zero;
            onComplete?.Invoke();
        }
        
        private void ToggleSearch()
        {
            searchToggle = !searchToggle;
            
            if(searchToggleRoutine != null)
                StopCoroutine(searchToggleRoutine);

            searchToggleRoutine = StartCoroutine(SearchToggleRoutine(searchToggle));
        }

        private IEnumerator SearchToggleRoutine(bool toggle)
        {
            float timeStep = 0;
            var cg = searchFieldTransform.GetComponentInChildren<CanvasGroup>();
            Vector2 currSize = searchFieldTransform.sizeDelta;
            float currAlpha = cg.alpha;
            float newAlpha = toggle ? 1 : 0;
            float newX = toggle ? 500 : 0;
            Vector2 newSize = new Vector2(newX, currSize.y);
            while (timeStep <= 1)
            {
                timeStep += Time.deltaTime / 0.15f;
                cg.alpha = Mathf.Lerp(currAlpha, newAlpha, timeStep);
                searchFieldTransform.sizeDelta = Vector2.Lerp(currSize, newSize, timeStep);
                yield return new WaitForEndOfFrame();
            }
        }

        public void OnClose()
        {
            Destroy(this.gameObject);
        }
    }

}
