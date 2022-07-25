using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private SmoothVector arrowDistance;
    public GameObject lArrow;
    private Vector3 lDefaultPosition;
    public GameObject rArrow;
    private Vector3 rDefaultPosition;
    // Start is called before the first frame update
    void Start()
    {
        arrowDistance = new(new Vector3(), 1, 1, 0);
        lDefaultPosition = lArrow.GetComponent<RectTransform>().localPosition;
        rDefaultPosition = rArrow.GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        arrowDistance.TimeStep(Time.fixedDeltaTime);
        lArrow.GetComponent<RectTransform>().localPosition = lDefaultPosition - arrowDistance.Value;
        rArrow.GetComponent<RectTransform>().localPosition = rDefaultPosition + arrowDistance.Value;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        arrowDistance.TargetValue = new Vector3(20, 0, 0);
    }

    public void OnPointerExit(PointerEventData eventData) {
        arrowDistance.TargetValue = new Vector3(0, 0, 0);
    }
}
