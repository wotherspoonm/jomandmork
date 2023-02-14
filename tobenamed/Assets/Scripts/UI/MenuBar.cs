using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuBar : MonoBehaviour
{
    public Dictionary<PlaceableObjectSO, int> menubarItemCountPairs = new();
    private PlaceableObjectSO _selectedObject; // Can be null. Null indicates no selected item.
    public PlaceableObjectSO SelectedObject => _selectedObject;
    public bool ItemIsSelected => _selectedObject != null;

    // Event Handlers
    public EventHandler<ItemQuantityChangedEventArgs> ItemQuantityChangedEventHandler;
    public EventHandler<MenubarSelectionEventArgs2> MenubarSelectionEventHandler;
    public EventHandler<MenubarSelectionEventArgs2> MenubarDeselectionEventHandler;

    public void AddItem(PlaceableObjectSO placeableObjectData, int amount = 1) {
        if (menubarItemCountPairs.ContainsKey(placeableObjectData)) {
            menubarItemCountPairs[placeableObjectData] += amount;
        }
        else {
            menubarItemCountPairs.Add(placeableObjectData, amount);
        }
        // Send the event
        ItemQuantityChangedEventHandler?.Invoke(this, new(placeableObjectData, amount));
    }
    public void RemoveItem(PlaceableObjectSO placeableObjectData, int amount = 1) {
        if(!menubarItemCountPairs.ContainsKey(placeableObjectData)) {
            throw new Exception("Tried to remove item that does not exist");
        }
        int itemCount = menubarItemCountPairs[placeableObjectData];
        if (itemCount < amount) {
            throw new Exception("Tried to remove more items than exist");
        }
        if (itemCount > amount) {
            menubarItemCountPairs[placeableObjectData] -= amount;
        }
        else {
            DeselectItem();
            menubarItemCountPairs.Remove(placeableObjectData);
        }
        // Send the event
        ItemQuantityChangedEventHandler?.Invoke(this, new(placeableObjectData, -amount));
    }
    public void SelectItem(PlaceableObjectSO placeableObjectData) {
        if (ItemIsSelected) DeselectItem();
        _selectedObject = placeableObjectData;
        MenubarSelectionEventHandler?.Invoke(this, new(placeableObjectData));
    }
    public void DeselectItem() {
        if (_selectedObject == null) return;
        var objectToDeselect = _selectedObject;
        _selectedObject = null;
        MenubarDeselectionEventHandler?.Invoke(this, new(objectToDeselect));
    }
}

public class ItemQuantityChangedEventArgs : EventArgs {
    public PlaceableObjectSO placeableObjectData;
    public int amount;

    public ItemQuantityChangedEventArgs(PlaceableObjectSO placeableObjectData, int amount) {
        this.placeableObjectData = placeableObjectData;
        this.amount = amount;
    }
}
public class MenubarSelectionEventArgs2 : EventArgs {
    public MenubarSelectionEventArgs2(PlaceableObjectSO objectToSelect) {
        this.objectToSelect = objectToSelect;
    }
    public PlaceableObjectSO objectToSelect;
}
