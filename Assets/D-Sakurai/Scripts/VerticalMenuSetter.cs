using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VerticalMenuSetter : MonoBehaviour
{
    [Header("上下に表示する画像(任意)")]
    [SerializeField] Sprite HorizontalRuleTexture = null;
    [SerializeField] Vector2 HorizontalRuleScale = new Vector2(1, 1);

    [System.Serializable]
    public struct ListElement
    {
        public string Label;
        public UnityEvent Function;
    }

    [Header("リストに表示する内容")]
    [SerializeField] private ListElement[] elements;
    [Header("テキストの表示に使用するPrefab")]
    [SerializeField] private GameObject textElementPrefab;


    void Start()
    {
        Destroy(transform.GetChild(0).gameObject);

        // add horizontal rule image
        if (HorizontalRuleTexture != null) GenerateHrImage();

        foreach (var element in elements)
        {
            GenerateTextElement(element);
        }

        // add horizontal rule image
        if (HorizontalRuleTexture != null) GenerateHrImage();
    }

    void GenerateHrImage(){
        GameObject hr = new GameObject("hr");
        RectTransform trans =  hr.AddComponent(typeof(RectTransform)) as RectTransform;
        Image hrImg = hr.AddComponent(typeof(Image)) as Image;

        hrImg.sprite = HorizontalRuleTexture;

        Vector2 spriteSize = HorizontalRuleTexture.rect.size;
        trans.sizeDelta = spriteSize;
        trans.localScale = HorizontalRuleScale;
        
        trans.SetParent(transform);
    }

    void GenerateTextElement(ListElement element)
    {
        GameObject textElement = Instantiate(textElementPrefab, transform);
        Text text = textElement.GetComponent<Text>();
        if (text != null)
        {
            text.text = element.Label;
        }

        Button button = textElement.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => element.Function.Invoke());
        }
    }
}
