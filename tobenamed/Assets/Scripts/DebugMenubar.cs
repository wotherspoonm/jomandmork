using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenubar : MonoBehaviour
{
    public PlaceableObjectData placeableObjectData;
    public MenuBar menuBar;

    public void AddItem() {
        menuBar.AddItem(placeableObjectData.prefab);
    }

    public void RemoveItem() {
        menuBar.RemoveItem(placeableObjectData.prefab);
    }
}
