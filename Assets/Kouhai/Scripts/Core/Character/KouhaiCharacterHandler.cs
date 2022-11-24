using Kouhai.Debugging;
using System.Collections;
using System.Collections.Generic;
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
        image.sprite = Resources.Load<Sprite>(characterImagePath);
        image.transform.position = transform.FindDeepChild($"Loc_{position}").position;
    }
}
