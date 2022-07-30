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
    public GameObject displayItem;
    PlaceableObject displayItemPO;
    public SmoothVector rotation;
    Quaternion defaultRotation;
    public Vector3 dispRotation;
    public Vector3 dispTRotation;

    private void Start() {
        isHovered = false;
        defaultRotation = displayItem.transform.rotation;
        rotation = new(defaultRotation.eulerAngles, 1, 1f, 0);
        // Setup button and add  click listener
        displayItemPO = displayItem.GetComponent<PlaceableObject>();
    }

    public void SelectItem() {
        isSelected = true;
        displayItemPO.Select();
    }
    public void DeselectItem() {
        isSelected = false;
        if(!isSelected) rotation.TargetValue = defaultRotation.eulerAngles + rotation.Value - new Vector3((rotation.Value.x - defaultRotation.eulerAngles.x + 180) % 360 - 180 +defaultRotation.eulerAngles.x, (rotation.Value.y - defaultRotation.eulerAngles.y + 180) % 360 - 180 + defaultRotation.eulerAngles.y, (rotation.Value.z + defaultRotation.eulerAngles.z - 180) % 360 - 180 + defaultRotation.eulerAngles.z);
        displayItemPO.Deselect();
    }

    private void FixedUpdate() {
        Vector3 spinSpeed = new(0, 0, 125f);
        if (isHovered || isSelected) {
            rotation.TargetValue += spinSpeed * Time.fixedDeltaTime;
        }
        rotation.TimeStep(Time.fixedDeltaTime);
        displayItem.transform.rotation = Quaternion.Euler(rotation.Value);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        isHovered = false;
        if (!isSelected) rotation.TargetValue = defaultRotation.eulerAngles + rotation.Value - new Vector3((rotation.Value.x - defaultRotation.eulerAngles.x + 180) % 360 - 180 + defaultRotation.eulerAngles.x, (rotation.Value.y - defaultRotation.eulerAngles.y + 180) % 360 - 180 + defaultRotation.eulerAngles.y, (rotation.Value.z + defaultRotation.eulerAngles.z - 180) % 360 - 180 + defaultRotation.eulerAngles.z);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        base.Interact();
    }
}
