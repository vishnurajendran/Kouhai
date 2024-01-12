using Kouhai.Debugging;
using System.Collections;
using System.Collections.Generic;
using Kouhai.Core.AssetManagement;
using UnityEngine;
using UnityEngine.UI;

public class KouhaiCharacterHandler : MonoBehaviour
{
    private const string KOUHAI_CHAR_PATH = "Images/Characters";

    [SerializeField]
    private GameObject charImgRef;

    [SerializeField]
    private Transform characterParent;

    [SerializeField]
    private SerializableDictionary<string, Image> characterImages;

    public void Start()
    {
        characterImages = new SerializableDictionary<string, Image>();
    }

    public void ShowCharacter(string name, string expression, string position)
    {
        string nameLower = name.ToLower();
        string expressionLower = expression.ToLower();
        var characterImagePath = $"{KOUHAI_CHAR_PATH}/{nameLower}/{nameLower}-{expressionLower}";
        Image image;
        if (characterImages.ContainsKey(name))
        {
            image = characterImages[name];
        }
        else
        {
            var go = Instantiate(charImgRef, characterParent);
            go.SetActive(true);
            image = go.GetComponentInChildren<Image>();
            characterImages.Add(name, image);
        }
        image.sprite = KouhaiAssetManager.LoadAsset<Sprite>(characterImagePath);
        image.transform.position = transform.FindDeepChild($"Loc_{position}").position;
        image.GetComponent<RectTransform>().sizeDelta =
            new Vector2(image.sprite.texture.width, image.sprite.texture.width);
        
        Debug.Log("Showing character");
    }

    public void HideCharacter(string name)
    {
        if(!characterImages.ContainsKey(name))
        {
            return;
        }

        var img = characterImages[name];
        Destroy(img.gameObject);
        characterImages.Remove(name);
    }

    public void ShiftCharacter(string name, string position, float time=0.25f)
    {
        var trf = transform.FindDeepChild($"Loc_{position}");
        if (trf != null)
        {
            var ch = characterImages[name].transform;
            StartCoroutine(ShiftCharacterRoutine(ch, trf, time));
        }
    }

    private static IEnumerator ShiftCharacterRoutine(Transform ch, Transform point, float time)
    {
        float timeStep = 0;
        var pos1 = ch.position;
        var pos2 = point.position;
        while(timeStep <= 1)
        {
            timeStep += Time.deltaTime / time;
            ch.position = Vector3.Lerp(pos1, pos2, timeStep);
            yield return new WaitForEndOfFrame();
        }
    }
}
