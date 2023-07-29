using System.Collections;
using System.Collections.Generic;
using Kouhai.Runtime.Client;
using UnityEngine;

public class KouhaiDownloads : MonoBehaviour, IHomescreenTab
{
    public void Initialise(float toolbarWidth)
    {
       
    }

    public void OnClose()
    {
       Destroy(this.gameObject);
    }
}
