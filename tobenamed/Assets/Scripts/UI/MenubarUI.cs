using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class MenubarUI : MonoBehaviour
{
    [SerializeField] private Menubar _menubar;
    public Dictionary<PlaceableObjectData, MenuItem> menuItemPODataMap = new();
    [SerializeField] private GameObject _menuItemPrefab;
    public SmoothFloat widthSF;
    public const float itemScale = 50;
    public const float itemSeparation = 45;
    public const float menuBarHeight = 90;
    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to events
        _menubar.ItemQuantityChangedEventHandler += OnMenubarItemQuantityChanged;
        _menubar.MenubarSelectionEventHandler += OnMenubarSelection;
        _menubar.MenubarDeselectionEventHandler += OnMenubarDeselection;

        // Old code that needs refactoring
        widthSF = new SmoothFloat(0, 1, 1, 1);
        var menuRectTransform = GetComponent<RectTransform>();
        menuRectTransform.sizeDelta = new Vector2((itemSeparation + itemScale) * (menuItemPODataMap.Count) + itemSeparation, menuBarHeight);
        menuRectTransform.anchoredPosition3D = new Vector3(menuRectTransform.anchoredPosition3D.x, menuRectTransform.anchoredPosition3D.y, itemScale);
    }
    public void AddItem(PlaceableObjectData placeableObjectData, int amount = 1) {
        if(menuItemPODataMap.ContainsKey(placeableObjectData)) {
            menuItemPODataMap[placeableObjectData].ItemCount += amount;
        }
        else {
            MenuItem newMenuItem = Instantiate(_menuItemPrefab, transform).GetComponent<MenuItem>();
            newMenuItem.displayGameObject = Instantiate(placeableObjectData.prefab, newMenuItem.transform);
            newMenuItem.displayGameObject.transform.localScale = Vector3.one * placeableObjectData.menuPreviewSize;
            menuItemPODataMap.Add(placeableObjectData, newMenuItem);
            newMenuItem.AddInteractionListener(KeyCode.Mouse0, (sender, e) => {
                if (sender is MenuItem menuItem)
                    _menubar.SelectItem(placeableObjectData);
            });
            AdjustPositions();
        }
    }

    public void RemoveItem(PlaceableObjectData placeableObjectData, int amount = 1) {
        if (!menuItemPODataMap.ContainsKey(placeableObjectData)) {
            throw new Exception("Tried to remove item that does not exist");
        }
        MenuItem menuItem = menuItemPODataMap[placeableObjectData];
        int itemCount = menuItem.ItemCount;
        if (itemCount < amount) {
            throw new Exception("Tried to remove more items than exist");
        }
        if (itemCount > amount) {
            menuItemPODataMap[placeableObjectData].ItemCount -= amount;
        }
        else {
            menuItemPODataMap.Remove(placeableObjectData);
            Destroy(menuItem.gameObject);
            AdjustPositions();
        }
    }

    private void AdjustPositions() {
        int i = 0;
        foreach (var placeableObjectData in menuItemPODataMap.Values) {
            float instanceSpacing = (itemSeparation + itemScale);
            float firstCoord = -(menuItemPODataMap.Count - 1) * instanceSpacing * 0.5f;
            placeableObjectData.GetComponent<DEAnimator>().MoveTo(new(firstCoord + i * instanceSpacing, 0, -itemScale));
            
            i++;
        }
        var menuRectTransform = GetComponent<RectTransform>();
        menuRectTransform.sizeDelta = new Vector2((itemSeparation + itemScale) * (menuItemPODataMap.Count) + itemSeparation, menuBarHeight);
        menuRectTransform.anchoredPosition3D = new Vector3(menuRectTransform.anchoredPosition3D.x, menuRectTransform.anchoredPosition3D.y, itemScale);
    }

    public void SelectItem(PlaceableObjectData placeableObjectData) {
        menuItemPODataMap[placeableObjectData].ShowAsSelected();
    }
    public void DeselectItem(PlaceableObjectData placeableObjectData) {
        menuItemPODataMap[placeableObjectData].ShowAsDeselected();
    }
    protected virtual void OnMenubarDeselection(object sender, MenubarSelectionEventArgs2 e) {
        DeselectItem(e.objectToSelect);
    }
    protected virtual void OnMenubarSelection(object sender, MenubarSelectionEventArgs2 e) {
        SelectItem(e.objectToSelect);
    }
    protected virtual void OnMenubarItemQuantityChanged(object sender, ItemQuantityChangedEventArgs e) {
        if(e.amount > 0) { // Is an item addition request
            AddItem(e.placeableObjectData, e.amount);
        }
        else { // Was a item subtraction request. Must be negative because RemoveItem takes the positive number of items to remove. The amount is negative. 
            RemoveItem(e.placeableObjectData, -e.amount);
        }
    }
}
