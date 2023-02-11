using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlaceableObject : MonoBehaviour 
{
    public PlaceableObjectData data;


    // Inspecor Related
    [SerializeField] private GameObject _informationPanelPrefab;
    [SerializeField] private Vector3 _popupOffset;
    [SerializeField] private RectTransform _uiCanvasRect;
    [SerializeField] protected List<InspectorFieldBase> _inspectorFields = new();
    private const string _uiCanvasTag = "UICanvas";
    private const string _uiCameraTag = "UICamera";
    private Camera _uiCamera;

    protected virtual void Start() {
        var x = GameObject.FindGameObjectWithTag(_uiCanvasTag);
        var y = GameObject.FindGameObjectWithTag(_uiCameraTag);
        _uiCamera = y.GetComponent<Camera>();
        _uiCanvasRect = x.GetComponent<RectTransform>();
    }
    public virtual void Select() {
        gameObject.GetComponent<Renderer>().material = data.selectedMaterial;
    }
    public virtual void Deselect() {
        gameObject.GetComponent<Renderer>().material = data.material;
    }
    public virtual void Rotate() {

    }
    private void OnMouseUp() {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(transform.position + _popupOffset);
        // Transform worldspace coordinates to UI Space
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_uiCanvasRect, screenPoint, _uiCamera, out Vector2 canvasPos);
        ShowObjectInspector(canvasPos);
    }
    private void ShowObjectInspector(Vector2 localPosition) {
        GameObject temp = Instantiate(_informationPanelPrefab, _uiCanvasRect);
        temp.transform.localPosition = localPosition;
        InspectorUI inspectorUI = temp.GetComponent<InspectorUI>();
        inspectorUI.inspectorFields = _inspectorFields;
        inspectorUI.Initialize();
    }
}
