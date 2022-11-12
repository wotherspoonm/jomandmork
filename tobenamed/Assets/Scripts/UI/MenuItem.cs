using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuItem : InteractableGameObject, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    bool isHovered;
    bool isSelected;
    public bool IsSelected { get { return isSelected; } }
    public GameObject displayItem;
    private DEAnimator animator;
    private PlaceableObject displayItemPO;
    private Vector3 defaultRotation;
    private float spinspeed = 50;


    private void Start() {
        isHovered = false;
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
        isHovered = true;
        animator.SetSpin(spinspeed);
    }

    public void OnPointerExit(PointerEventData eventData) {
        isHovered = false;
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
