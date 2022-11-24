using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kouhai.Core.Input;

public class InputTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        KouhaiInputSystem.Instance.OnUserSelects += () => Debug.Log("SELECT");
        KouhaiInputSystem.Instance.OnUserCancels += () => Debug.Log("CANCEL");
        KouhaiInputSystem.Instance.OnUserNaviagtesUp += () => Debug.Log("UP");
        KouhaiInputSystem.Instance.OnUserNaviagtesDown += () => Debug.Log("DOWN");
    }
}

