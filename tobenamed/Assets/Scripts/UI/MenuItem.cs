using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MenuItem : InteractableGameObject, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private bool isSelected;
    public bool IsSelected { get { return isSelected; } }
    public GameObject displayGameObject;
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
        displayItemPO = displayGameObject.GetComponent<PlaceableObject>();
        animator = displayGameObject.AddComponent<DEAnimator>();
        defaultRotation = displayGameObject.transform.rotation.eulerAngles;
    }

    public void ShowAsSelected() {
        isSelected = true;
        displayItemPO.Select();
        animator.SetSpin(spinspeed);
    }
    public void ShowAsDeselected() {
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
