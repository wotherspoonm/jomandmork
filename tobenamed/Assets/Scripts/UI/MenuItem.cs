using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool isHovered;
    public GameObject displayItem;
    Quaternion defaultRotation;
    private void Start() {
        isHovered = false;
        defaultRotation = displayItem.transform.rotation;
    }

    private void FixedUpdate() {
        Vector3 spinSpeed = new(0, 0, 50f);
        if (isHovered)
        displayItem.transform.rotation = Quaternion.Euler(displayItem.transform.rotation.eulerAngles + spinSpeed * Time.fixedDeltaTime);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        isHovered = false;
        displayItem.transform.rotation = defaultRotation;
    }
}
