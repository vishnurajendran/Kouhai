using System.Collections.Generic;
using Kouhai.Runtime.System;
using UnityEngine;

namespace Kouhai.Runtime.Client
{
    public class KouhaiDownloads : MonoBehaviour, IHomescreenTab
    {
        [SerializeField] private Transform entryParent;
        [SerializeField] private GameObject downloadEntryRef;

        private List<KouhaiDownloadPageItem> downloadItems;

        public void Initialise(float toolbarWidth)
        {
            downloadItems = new List<KouhaiDownloadPageItem>();
            var downloads = KouhaiDownloadManager.Downloads;
            foreach (var download in downloads)
            {
                var go = Instantiate(downloadEntryRef, entryParent);
                go.gameObject.SetActive(true);
                var pageItem = go.GetComponent<KouhaiDownloadPageItem>();
                downloadItems.Add(pageItem);
                pageItem.Initialise(download, DeleteItem);
            }
        }

        private void DeleteItem(KouhaiDownloadPageItem item)
        {
            if (downloadItems == null)
                return;

            downloadItems.Remove(item);
            Destroy(item.gameObject);
        }
        
        public void OnClose()
        {
            Destroy(this.gameObject);
        }
    }
}

