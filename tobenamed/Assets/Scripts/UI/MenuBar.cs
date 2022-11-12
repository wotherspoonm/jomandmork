using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuBar : MonoBehaviour
{
    public List<GameObject> menuItems;
    public List<GameObject> itemCells = new();
    public GameObject menuBar;
    public GameObject itemCellPrefab;
    public GameObject SelectedItem { get { return menuItems[(int)selectedItemIndex]; } }
    public const float itemScale = 50;
    public const float itemSeparation = 45;
    public const float menuBarHeight = 90;
    private int? selectedItemIndex = null;
    public bool ItemIsSelected { get { return selectedItemIndex != null; } }
    public EventHandler<MenubarSelectionEventArgs> MenubarSelectionEventHandler;
    public EventHandler MenubarDeselectionEventHandler;

    // Start is called before the first frame update
    void Start()
    {
        // Set menu bar size  and position based on the number of items
        var menuRectTransform = menuBar.GetComponent<RectTransform>();
        menuRectTransform.sizeDelta = new Vector2((itemSeparation + itemScale) * (menuItems.Count) + itemSeparation, menuBarHeight);
        menuRectTransform.anchoredPosition3D = new Vector3(menuRectTransform.anchoredPosition3D.x, menuRectTransform.anchoredPosition3D.y, itemScale);
        // Spawn items for display 
        for (int i = 0; i < menuItems.Count; i++) {
            var newCell = Instantiate(itemCellPrefab, menuBar.transform);
            newCell.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(i * (itemSeparation + itemScale) + 0.5f * itemScale + itemSeparation, 0, -itemScale);
            newCell.GetComponent<MenuItem>().displayItem = Instantiate(menuItems[i], newCell.transform);
            newCell.GetComponent<MenuItem>().displayItem.transform.localScale = Vector3.one * newCell.GetComponent<MenuItem>().displayItem.GetComponent<PlaceableObject>().menuPreviewSize;
            itemCells.Add(newCell);
            int itemSelectionIndex = i;
            newCell.GetComponent<MenuItem>().AddInteractionListener(KeyCode.Mouse0, delegate
            {
                SelectItem(itemSelectionIndex);
            });
        }
    }

    public void SelectItem(int itemIndex)
    {
        if(selectedItemIndex != null)
        {
            itemCells[(int)selectedItemIndex].GetComponent<MenuItem>().DeselectItem();
            if (selectedItemIndex == itemIndex)
            {
                // Send MenubarDeselection event
                OnMenubarDeselection();
                return;
            }
        }
        var itemToSelect = menuItems[itemIndex];
        selectedItemIndex = itemIndex;
        itemCells[itemIndex].GetComponent<MenuItem>().SelectItem();
        // Send MenubarSelection event 
        OnMenubarSelection(new MenubarSelectionEventArgs(itemToSelect));
    }

    public void DeselectAll()
    {
        for (int i = 0; i < itemCells.Count; i++) if (itemCells[i].GetComponent<MenuItem>().IsSelected)
        {
            OnMenubarDeselection();
            itemCells[i].GetComponent<MenuItem>().DeselectItem();
        }
    }

    protected virtual void OnMenubarSelection(MenubarSelectionEventArgs eventArgs)
    {
        MenubarSelectionEventHandler?.Invoke(this, eventArgs);
    }

    protected virtual void OnMenubarDeselection()
    {
        MenubarDeselectionEventHandler?.Invoke(this, EventArgs.Empty);
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
