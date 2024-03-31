using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VerticalMenuSetter : MonoBehaviour
{
    [Header("上下に表示する画像(任意)")]
    [SerializeField] Sprite horizontalRuleTexture = null;
    [SerializeField] Vector2 horizontalRuleScale = new Vector2(1, 1);

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
        if (horizontalRuleTexture != null) GenerateHrImage();

        foreach (var element in elements)
        {
            GenerateTextElement(element);
        }

        // add horizontal rule image
        if (horizontalRuleTexture != null) GenerateHrImage();
    }

    void GenerateHrImage(){
        GameObject _hr = new GameObject("hr");
        RectTransform _trans =  _hr.AddComponent(typeof(RectTransform)) as RectTransform;
        Image _hrImg = _hr.AddComponent(typeof(Image)) as Image;

        // set image
        _hrImg.sprite = horizontalRuleTexture;

        // set size
        Vector2 spriteSize = horizontalRuleTexture.rect.size;
        _trans.sizeDelta = spriteSize;
        _trans.localScale = horizontalRuleScale;
        
        // set parent to this
        _trans.SetParent(transform);
    }

    void GenerateTextElement(ListElement element)
    {
        GameObject _textElement = Instantiate(textElementPrefab, transform);

        // add text component
        Text _text = _textElement.GetComponent<Text>();
        if (_text != null)
        {
            _text.text = element.Label;
        }

        // add button component for call event
        // TODO: controller support
        Button _button = _textElement.GetComponent<Button>();
        if (_button != null)
        {
            _button.onClick.AddListener(() => element.Function.Invoke());
        }
    }
}
