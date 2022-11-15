using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MenuItem : InteractableGameObject, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    bool isSelected;
    public bool IsSelected { get { return isSelected; } }
    public GameObject displayItem;
    private DEAnimator animator;
    public PlaceableObject displayItemPO;
    [SerializeField]
    private TextMeshProUGUI itemCountText;
    private int itemCount = 1;
    public int ItemCount { get { return itemCount; } set { itemCount = value; itemCountText.text = itemCount.ToString(); } }
    private Vector3 defaultRotation;
    private float spinspeed = 50;


    private void Start() {
        // Setup button and add  click listener
        displayItemPO = displayItem.GetComponent<PlaceableObject>();
        animator = displayItem.AddComponent<DEAnimator>();
        defaultRotation = displayItem.transform.rotation.eulerAngles;
    }

    public void SelectItem() {
        isSelected = true;
        displayItemPO.Select();
        animator.SetSpin(spinspeed);
    }
    public void DeselectItem() {
        isSelected = false;
        displayItemPO.Deselect();
        animator.SetSpin(0);
        animator.SetAngle(defaultRotation);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        animator.SetSpin(spinspeed);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!isSelected) {
            animator.SetSpin(0);
            animator.SetAngle(defaultRotation);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        base.Interact();
    }
}
