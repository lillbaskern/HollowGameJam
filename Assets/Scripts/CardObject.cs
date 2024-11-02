using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using static UnityEditor.PlayerSettings;
using System.Collections.Generic;

public class CardObject : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform Hand;
    public Card CardData;

    Transform CardSlot;

    public TextMeshProUGUI SuitText;
    public TextMeshProUGUI RankText;

    public Transform CardContainer;

    Vector2 targetPos;


    private Canvas canvas;
    private Image imageComponent;
    private Vector3 offset;



    //events
    [Header("Events")]
    [HideInInspector] public UnityEvent<CardObject> PointerEnterEvent;
    [HideInInspector] public UnityEvent<CardObject> PointerExitEvent;
    [HideInInspector] public UnityEvent<CardObject, bool> PointerUpEvent;
    [HideInInspector] public UnityEvent<CardObject> PointerDownEvent;
    [HideInInspector] public UnityEvent<CardObject> BeginDragEvent;
    [HideInInspector] public UnityEvent<CardObject> EndDragEvent;
    [HideInInspector] public UnityEvent<CardObject, bool> SelectEvent;

    [Header("States")]
    public bool isHovering;
    public bool isDragging = false;


    [HideInInspector] public bool wasDragged;

    [Header("Selection")]
    public bool selected;
    public float selectionOffset = 50;
    private float pointerDownTime;
    private float pointerUpTime;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        imageComponent = GetComponent<Image>();

        CardContainer = GameObject.Find("CardContainer").transform;
        Hand = GameObject.Find("Hand").transform;
    }

    public void Update()
    {
        if (isDragging)
        {
            this.transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * 10);


            for (int i = 0; i < HandVisualizer.Instance.transform.childCount; i++) 
            {
                if(this.transform.position.x > HandVisualizer.Instance.transform.GetChild(i).position.x)
                {
                    CardSlot.SetSiblingIndex(i);
                }
                if(this.transform.position.x < HandVisualizer.Instance.transform.GetChild(0).position.x)
                    CardSlot.SetSiblingIndex(0);
            }


        }
        if (!isDragging)
        {
            this.transform.position = Vector2.Lerp(transform.position, CardSlot.position + offset, Time.deltaTime * 12);
        }
    }

    public void Deselect()
    {
    }

    public void AddData(Card card, Transform slot)
    {
        CardData = card;
        CardSlot = slot;

        if (CardData == null) return;

        SuitText.text = CardData.Suit.ToString();
        RankText.text = (int)CardData.Rank < 10? ((int)CardData.Rank + 1).ToString(): CardData.Rank.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDragEvent.Invoke(this);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
        canvas.GetComponent<GraphicRaycaster>().enabled = false;
        imageComponent.raycastTarget = false;

        if(!selected)
            this.transform.SetParent(CardContainer.transform);

        wasDragged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        targetPos = eventData.position;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        EndDragEvent.Invoke(this);
        isDragging = false;
        canvas.GetComponent<GraphicRaycaster>().enabled = true;
        imageComponent.raycastTarget = true;

        StartCoroutine(FrameWait());

        IEnumerator FrameWait()
        {
            yield return new WaitForEndOfFrame();
            wasDragged = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        PointerDownEvent.Invoke(this);
        pointerDownTime = Time.time;


    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging)
            selected = !selected;

        if (selected && !HandVisualizer.Instance.SelectedCards.Contains(this))
            HandVisualizer.Instance.SelectedCards.Add(this);
        if (!selected)
            HandVisualizer.Instance.SelectedCards.Remove(this);

        UpdateOffset();
    }

    public void UpdateOffset()
    {
        offset = selected ? new(0, selectionOffset) : Vector3.zero;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent.Invoke(this);
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitEvent.Invoke(this);
        isHovering = false;
    }

    public int SiblingAmount()
    {
        return transform.parent.CompareTag("Slot") ? transform.parent.parent.childCount - 1 : 0;
    }

    public int ParentIndex()
    {
        return transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
    }

    private void OnDestroy()
    {
        Destroy(CardSlot.gameObject);
    }
}
