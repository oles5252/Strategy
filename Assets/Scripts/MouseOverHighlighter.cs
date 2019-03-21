using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseOverHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Color color = Color.yellow;
    public Text text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = color; //Or however you do your color
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = Color.black; //Or however you do your color
    }
}
