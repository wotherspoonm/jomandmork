using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuBar : MonoBehaviour
{
    public PrefabManager prefabManager;
    public List<MenuItem> menuItems = new();
    public GameObject menuBar;
    public GameObject itemCellPrefab;
    public SmoothFloat widthSF;
    public const float itemScale = 50;
    public const float itemSeparation = 45;
    public const float menuBarHeight = 90;
    public MenuItem selectedItem;
    private int? selectedItemIndex = null;
    public bool ItemIsSelected { get { return selectedItemIndex != null; } }
    public EventHandler<MenubarSelectionEventArgs> MenubarSelectionEventHandler;
    public EventHandler<MenubarSelectionEventArgs> MenubarDeselectionEventHandler;

    // Start is called before the first frame update
    void Start()
    {
        widthSF = new SmoothFloat(0, 1, 1, 1);
        // Set menu bar size  and position based on the number of items
        var menuRectTransform = menuBar.GetComponent<RectTransform>();
        menuRectTransform.sizeDelta = new Vector2((itemSeparation + itemScale) * (menuItems.Count) + itemSeparation, menuBarHeight);
        menuRectTransform.anchoredPosition3D = new Vector3(menuRectTransform.anchoredPosition3D.x, menuRectTransform.anchoredPosition3D.y, itemScale);
    }

    public void AddItem(GameObject objectToAdd) {
        // Find existing item of the same type on the menubar
        MenuItem existingItem = menuItems.Find(x => x.displayItemPO.data.name == objectToAdd.GetComponent<PlaceableObject>().data.name);
        if(existingItem == null) {
            MenuItem newMenuItem = Instantiate(itemCellPrefab, menuBar.transform).GetComponent<MenuItem>();
            newMenuItem.displayItem = Instantiate(prefabManager.GetPrefabFromData(objectToAdd.GetComponent<PlaceableObject>().data), newMenuItem.transform);
            newMenuItem.displayItem.transform.localScale = Vector3.one * newMenuItem.GetComponent<MenuItem>().displayItem.GetComponent<PlaceableObject>().data.menuPreviewSize;
            menuItems.Add(newMenuItem);
            newMenuItem.AddInteractionListener(KeyCode.Mouse0, (sender, e) => {
                if(sender is MenuItem menuItem)
                SelectItem(menuItem);
            });
            AdjustPositions();
        }
        else {
            existingItem.ItemCount++;
        }
    }

    public void RemoveItem(GameObject objectToRemove) {
        // Find existing item of the same type on the menubar
        MenuItem existingItem = menuItems.Find(x => x.displayItemPO.data.name == objectToRemove.GetComponent<PlaceableObject>().data.name);
        if(existingItem == null) {
            Debug.LogWarning("Tried to remove item that does not exist");
        }
        else {
            int index = menuItems.FindIndex(x => x.displayItemPO.data.name == objectToRemove.GetComponent<PlaceableObject>().data.name);
            var menuItem = menuItems[index];
            var itemCellGo = menuItems[index];
            if (menuItem.ItemCount == 1) {
                DeselectAll();
                menuItems.RemoveAt(index);
                AdjustPositions();
                Destroy(itemCellGo.gameObject);
            }
            else {
                menuItem.ItemCount --;
            }
        }
    }

    private void AdjustPositions() {
        for (int i = 0; i < menuItems.Count; i++) {
            float instanceSpacing = (itemSeparation + itemScale);
            float firstCoord = -(menuItems.Count - 1) * instanceSpacing * 0.5f;
            menuItems[i].GetComponent<DEAnimator>().MoveTo(new(firstCoord + i*instanceSpacing,0,-itemScale));
            //itemCells[i].GetComponent<DEAnimator>().MoveTo(new(i * (itemSeparation + itemScale) + 0.5f * itemScale + itemSeparation, 0, -itemScale));
        }
    }


    public void SelectItem(MenuItem itemToSelect) {
        if(selectedItem != null) {
            selectedItem.DeselectItem();
            // If we are trying to select an object that is already selected, instead deselect it
            if(selectedItem == itemToSelect) {
                selectedItem = null;
                OnMenubarDeselection();
                return;
            }
        }
        itemToSelect.SelectItem();
        OnMenubarSelection(new MenubarSelectionEventArgs(prefabManager.GetPrefabFromData(itemToSelect.displayItem.GetComponent<PlaceableObject>().data)));
        selectedItem = itemToSelect;
    }

    public void DeselectAll()
    {
        for (int i = 0; i < menuItems.Count; i++) if (menuItems[i].GetComponent<MenuItem>().IsSelected)
        {
            menuItems[i].GetComponent<MenuItem>().DeselectItem();
            OnMenubarDeselection();
        }
    }

    protected virtual void OnMenubarSelection(MenubarSelectionEventArgs eventArgs)
    {
        MenubarSelectionEventHandler?.Invoke(this, eventArgs);
    }

    protected virtual void OnMenubarDeselection()
    {
        selectedItemIndex = null;
        MenubarDeselectionEventHandler?.Invoke(this, null);
    }
}

public class MenubarSelectionEventArgs : EventArgs
{
    public MenubarSelectionEventArgs(GameObject gameObjectToSelect)
    {
        this.gameObjectToSelect = gameObjectToSelect;
    }
    public GameObject gameObjectToSelect;
}
