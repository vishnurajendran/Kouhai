using System;
using System.Collections;
using System.Collections.Generic;
using Kouhai.Runtime.System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Kouhai.Runtime.Client
{
    public class KouhaiHomescreen : MonoBehaviour
    {
        public enum HomeScreenTab
        {
            Library,
            Store,
            Downloads,
            Messages,
            Settings
        }

        [SerializeField] private HomeScreenTab currentTabType;
        [SerializeField] private RectTransform toolbarRectTransform;
        [SerializeField] private Transform tabParent;
        [SerializeField] private SerializableDictionary<Button, HomeScreenTab> tabButtonDict;

        private IHomescreenTab currentTabRef;
        
        private void Start()
        {
            SetActiveTab(HomeScreenTab.Library);
            foreach (var kv in tabButtonDict)
            {
                kv.Key.onClick.AddListener(() =>
                {
                    SetActiveTab(kv.Value);
                });
            }
        }

        private void SetActiveTab(HomeScreenTab toTab)
        {
            Debug.Log($"Switching to Tab {toTab}");
            if(currentTabRef != null)
                currentTabRef.OnClose();

            currentTabRef = SpawnTabPage(toTab);
            currentTabRef?.Initialise(toolbarRectTransform.sizeDelta.x);
        }

        private IHomescreenTab SpawnTabPage(HomeScreenTab targetTab)
        {
            var resPath = KouhaiResourcesPath.GetHomeTabPagePath(targetTab);
            if (string.IsNullOrEmpty(resPath))
                return null;
            
            var resPrefab = Resources.Load<GameObject>(resPath);
            var go = Instantiate(resPrefab, tabParent);
            var rectTrf = go.GetComponent<RectTransform>();
            rectTrf.offsetMax = rectTrf.offsetMin = Vector2.zero;
            return go.GetComponent<IHomescreenTab>();
        }
    }
}
