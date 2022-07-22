using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool isHovered;
    public GameObject displayItem;
    public SmoothVector rotation;
    Quaternion defaultRotation;
    public Vector3 dispRotation;
    public Vector3 dispTRotation;
    private void Start() {
        isHovered = false;
        defaultRotation = displayItem.transform.rotation;
        rotation = new(defaultRotation.eulerAngles, 1, 1f, 0);
    }

    private void FixedUpdate() {
        Vector3 spinSpeed = new(0, 0, 125f);
        if (isHovered) {
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
        rotation.TargetValue = defaultRotation.eulerAngles + rotation.Value - new Vector3((rotation.Value.x + 180) % 360 - 180, (rotation.Value.y + 180) % 360 - 180, (rotation.Value.z + 180) % 360 - 180);
    }
}
