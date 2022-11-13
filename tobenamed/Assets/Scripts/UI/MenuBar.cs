using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuBar : MonoBehaviour
{
    public List<GameObject> menuItems;
    public List<GameObject> itemCells = new();
    private List<Action> interactionActions = new();
    private List<int> itemCounts = new();
    public GameObject menuBar;
    public GameObject itemCellPrefab;
    public SmoothFloat widthSF;
    public GameObject SelectedItem { get { return menuItems[(int)selectedItemIndex]; } }
    public const float itemScale = 50;
    public const float itemSeparation = 45;
    public const float menuBarHeight = 90;
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

    public void AddItem(GameObject objectToAdd) {
        if (menuItems.Contains(objectToAdd)) {
            var index = menuItems.FindIndex(x => objectToAdd.Equals(x));
            itemCells[index].GetComponent<MenuItem>().ItemCount += 1;
        }
        else {
            var newCell = Instantiate(itemCellPrefab, menuBar.transform);
            newCell.GetComponent<MenuItem>().displayItem = Instantiate(objectToAdd, newCell.transform);
            newCell.GetComponent<MenuItem>().displayItem.transform.localScale = Vector3.one * newCell.GetComponent<MenuItem>().displayItem.GetComponent<PlaceableObject>().menuPreviewSize;
            itemCells.Add(newCell);
            menuItems.Add(objectToAdd);
            UpdateInteractionListeners();
            AdjustPositions();
        }
    }

    public void RemoveItem(GameObject objectToRemove) {
        //    if ( more than one item) {
        //          decrement item count
        //    }
        //    else {
        //          Remove item
        //    }
        throw new NotImplementedException();
    }

    private void UpdateInteractionListeners() {
        for (int i = menuItems.Count - 2; i >= 0; i--) {
            itemCells[i].GetComponent<MenuItem>().RemoveInteractionListener(KeyCode.Mouse0, interactionActions[i]);
        }
        interactionActions.Clear();
        for (int i = 0; i < menuItems.Count; i++) {
            int itemSelectionIndex = i;
            interactionActions.Add(delegate {
                SelectItem(itemSelectionIndex);
            });
            itemCells[i].GetComponent<MenuItem>().AddInteractionListener(KeyCode.Mouse0, interactionActions[i]);
        }
    }

    private void AdjustPositions() {
        for (int i = 0; i < menuItems.Count; i++) {
            float instanceSpacing = (itemSeparation + itemScale);
            float firstCoord = -(itemCells.Count - 1) * instanceSpacing * 0.5f;
            itemCells[i].GetComponent<DEAnimator>().MoveTo(new(firstCoord + i*instanceSpacing,0,-itemScale));
            //itemCells[i].GetComponent<DEAnimator>().MoveTo(new(i * (itemSeparation + itemScale) + 0.5f * itemScale + itemSeparation, 0, -itemScale));
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
