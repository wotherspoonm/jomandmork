using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuBar : MonoBehaviour
{
    public Dictionary<PlaceableObjectData, int> menubarItemCountPairs = new();
    private PlaceableObjectData _selectedObject; // Can be null. Null indicates no selected item.
    public PlaceableObjectData SelectedObject => _selectedObject;
    public bool ItemIsSelected => _selectedObject != null;

    // Event Handlers
    public EventHandler<ItemQuantityChangedEventArgs> ItemQuantityChangedEventHandler;
    public EventHandler<MenubarSelectionEventArgs2> MenubarSelectionEventHandler;
    public EventHandler<MenubarSelectionEventArgs2> MenubarDeselectionEventHandler;

    public void AddItem(PlaceableObjectData placeableObjectData, int amount = 1) {
        if (menubarItemCountPairs.ContainsKey(placeableObjectData)) {
            menubarItemCountPairs[placeableObjectData] += amount;
        }
        else {
            menubarItemCountPairs.Add(placeableObjectData, amount);
        }
        // Send the event
        ItemQuantityChangedEventHandler?.Invoke(this, new(placeableObjectData, amount));
    }
    public void RemoveItem(PlaceableObjectData placeableObjectData, int amount = 1) {
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
    public void SelectItem(PlaceableObjectData placeableObjectData) {
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
    public PlaceableObjectData placeableObjectData;
    public int amount;

    public ItemQuantityChangedEventArgs(PlaceableObjectData placeableObjectData, int amount) {
        this.placeableObjectData = placeableObjectData;
        this.amount = amount;
    }
}
public class MenubarSelectionEventArgs2 : EventArgs {
    public MenubarSelectionEventArgs2(PlaceableObjectData objectToSelect) {
        this.objectToSelect = objectToSelect;
    }
    public PlaceableObjectData objectToSelect;
}
