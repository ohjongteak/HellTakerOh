using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class VirtualJoyStick : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransfrom;


    [SerializeField, Range(10, 150)]
    private float leverRange;
    void Start()
    {
        rectTransfrom = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        var inputPos = eventData.position - rectTransfrom.anchoredPosition;
        lever.anchoredPosition = inputPos;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        var inputPos = eventData.position - rectTransfrom.anchoredPosition;
        lever.anchoredPosition = inputPos;
        Debug.Log("드래그 중");
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
    }

    // Start is called before the first frame update
   
}
