using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class InspectorUI : MonoBehaviour
{
    public List<InspectorFieldBase> inspectorFields = new();
    private Camera _uiCamera;
    [SerializeField] private GameObject _sliderPrefab;
    [SerializeField] private Transform _content;
    private const string _uiCameraTag = "UICamera";

    private void Start() {
        var y = GameObject.FindGameObjectWithTag(_uiCameraTag);
        _uiCamera = y.GetComponent<Camera>();
    }
    // Start is called before the first frame update
    public void Initialize()
    {
        foreach (InspectorFieldBase inspectorField in inspectorFields) {
            if (inspectorField is SliderInspectorField sliderInspectorField) {
                var go = Instantiate(_sliderPrefab,_content);
                var slider = go.GetComponentInChildren<Slider>();
                if(sliderInspectorField.DiscreteInterval == 0) {
                    slider.wholeNumbers = false;
                    slider.minValue = sliderInspectorField.MinValue;
                    slider.maxValue = sliderInspectorField.MaxValue;
                    slider.value = sliderInspectorField.GetValue.Invoke();
                    slider.onValueChanged.AddListener((value) => {
                        sliderInspectorField.SetValue(value);
                    });
                }
                else {
                    slider.wholeNumbers = true;
                    slider.minValue = sliderInspectorField.MinValue / sliderInspectorField.DiscreteInterval;
                    slider.maxValue = sliderInspectorField.MaxValue / sliderInspectorField.DiscreteInterval;
                    slider.value = sliderInspectorField.GetValue.Invoke() / sliderInspectorField.DiscreteInterval;
                    slider.onValueChanged.AddListener(value => {
                        sliderInspectorField.SetValue(value * sliderInspectorField.DiscreteInterval);
                    });
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePosition = Input.mousePosition;
            if (!RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), mousePosition, _uiCamera)) {
                Destroy(gameObject);
            }
        }
    }
}
