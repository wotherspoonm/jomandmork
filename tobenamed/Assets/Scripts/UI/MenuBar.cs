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
    public List<MenuItem> itemCells = new();
    private List<Action> interactionActions = new();
    public GameObject menuBar;
    public GameObject itemCellPrefab;
    public SmoothFloat widthSF;
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
        menuRectTransform.sizeDelta = new Vector2((itemSeparation + itemScale) * (itemCells.Count) + itemSeparation, menuBarHeight);
        menuRectTransform.anchoredPosition3D = new Vector3(menuRectTransform.anchoredPosition3D.x, menuRectTransform.anchoredPosition3D.y, itemScale);
    }

    public void AddItem(GameObject objectToAdd) {
        // This line should be simplified
        if (itemCells.Exists(x => x.displayItemPO.data.name == objectToAdd.GetComponent<PlaceableObject>().data.name)) {
            int index = itemCells.FindIndex(x => x.displayItemPO.data.name == objectToAdd.GetComponent<PlaceableObject>().data.name);
            itemCells[index].ItemCount += 1;
        }
        else {
            var newMenuItem = Instantiate(itemCellPrefab, menuBar.transform).GetComponent<MenuItem>();
            newMenuItem.displayItem = Instantiate(prefabManager.GetPrefabFromData(objectToAdd.GetComponent<PlaceableObject>().data), newMenuItem.transform);
            newMenuItem.displayItem.transform.localScale = Vector3.one * newMenuItem.GetComponent<MenuItem>().displayItem.GetComponent<PlaceableObject>().data.menuPreviewSize;
            itemCells.Add(newMenuItem);
            UpdateInteractionListeners(true);
            AdjustPositions();
        }
    }

    public void RemoveItem(GameObject objectToRemove) {
        if (itemCells.Exists(x => x.displayItemPO.data.name == objectToRemove.GetComponent<PlaceableObject>().data.name)) {
            int index = itemCells.FindIndex(x => x.displayItemPO.data.name == objectToRemove.GetComponent<PlaceableObject>().data.name);
            var menuItem = itemCells[index].GetComponent<MenuItem>();
            var itemCellGo = itemCells[index];
            if (menuItem.ItemCount == 1) {
                interactionActions.RemoveAt(index);
                itemCells.RemoveAt(index);
                UpdateInteractionListeners(false);
                AdjustPositions();
                DeselectAll();
                Destroy(itemCellGo.gameObject);
            }
            else {
                menuItem.ItemCount -= 1;
            }
        }
        else {
            Debug.LogWarning("Tried to remove item that does not exist");
        }
    }

    private void UpdateInteractionListeners(bool isAdding) {
        for (int i = interactionActions.Count - 1; i >= 0; i--) {
            itemCells[i].GetComponent<MenuItem>().RemoveInteractionListener(KeyCode.Mouse0, interactionActions[i]);
        }
        interactionActions.Clear();
        for (int i = 0; i < itemCells.Count; i++) {
            int itemSelectionIndex = i;
            interactionActions.Add(delegate {
                SelectItem(itemSelectionIndex);
            });
            itemCells[i].GetComponent<MenuItem>().AddInteractionListener(KeyCode.Mouse0, interactionActions[i]);
        }
    }

    private void AdjustPositions() {
        for (int i = 0; i < itemCells.Count; i++) {
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
        var itemToSelect = itemCells[itemIndex].displayItem;
        selectedItemIndex = itemIndex;
        itemCells[itemIndex].GetComponent<MenuItem>().SelectItem();
        // Send MenubarSelection event 
        OnMenubarSelection(new MenubarSelectionEventArgs(prefabManager.GetPrefabFromData(itemToSelect.GetComponent<PlaceableObject>().data)));
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
