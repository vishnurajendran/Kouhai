using System;
using System.Collections;
using System.Collections.Generic;
using Kouhai.Runtime.Client;
using Kouhai.Runtime.System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class KouhaiDownloadPageItem : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text title;
    [FormerlySerializedAs("progrss")] [SerializeField] private Image progress;
    [SerializeField] private TMPro.TMP_Text progressText;
    [SerializeField] private Button cancelBttn;
    [SerializeField] private Button retryButton;

    private KouhaiDownloadEntry downloadEntry;
    private Action<KouhaiDownloadPageItem> removeAction;
    public void Initialise(KouhaiDownloadEntry downloadEntry, Action<KouhaiDownloadPageItem> removeAction)
    {
        this.removeAction = removeAction;
        this.downloadEntry = downloadEntry;
        this.title.text = this.downloadEntry.DownloadTitle;
        progress.fillAmount = downloadEntry.CurrentProgress;
        progressText.text = $"{(int)(progress.fillAmount * 100f)}%";
        
        this.cancelBttn.onClick.AddListener(DownloadCancelled);
        retryButton.onClick.AddListener(RetryDownload);
        downloadEntry.OnDownloadSizeFetched += DownloadSizeFecthed;
        downloadEntry.OnDownloadProgress += UpdateProgress;
        downloadEntry.OnDownloadFinalising += FinaliseDownload;
        downloadEntry.OnDownloadCompleted += DownloadCompleted;
        downloadEntry.OnDownloadCancelled += DownloadCancelled;
    }

    private void UpdateProgress(float total, float current)
    {
        progress.fillAmount = (float)current / total;
        progressText.text = $"{(int)(progress.fillAmount * 100f)}%";
    }

    private void FinaliseDownload()
    {
        Debug.Log("Download finalising");
        cancelBttn.gameObject.SetActive(false);
    }

    private void DownloadCancelled()
    {
        Debug.Log("Download cancelled");
        downloadEntry.CancelDownload();
        RequestDelete();
    }
    
    private void RetryDownload()
    {
        retryButton.gameObject.SetActive(false);
        title.text = $"{downloadEntry.DownloadTitle} (Calculating...)";
        UpdateProgress(1, 0f);
        downloadEntry.StartOrContinueDownload();
    }

    private void DownloadSizeFecthed(long size)
    {
        title.text = $"{downloadEntry.DownloadTitle} ({StringifySize(size)})";
        UpdateProgress(1, 0f);
    }

    private string StringifySize(long size)
    {
        var gb = (float)size / Mathf.Pow(1024, 3);
        var mb = (float)size / Mathf.Pow(1024, 2);
        var kb = (float)size / 1024f;

        if (gb > 1) return $"{gb:F2} GB";
        if (mb > 1) return $"{mb:F2} MB";
        if (kb > 1) return $"{kb:F2} KB";

        return $"{size} Bytes";
    }
    
    private void DownloadCompleted(bool success)
    {
        Debug.Log("Download completed");
        //TODO: Do the unpacking here
        
        RequestDelete();
    }

    private void RequestDelete()
    {
        removeAction?.Invoke(this);
    }
    
    private void OnDestroy()
    {
        downloadEntry.OnDownloadProgress -= UpdateProgress;
        downloadEntry.OnDownloadFinalising -= FinaliseDownload;
        downloadEntry.OnDownloadCompleted -= DownloadCompleted;
        downloadEntry.OnDownloadCancelled -= DownloadCancelled;
    }
}
