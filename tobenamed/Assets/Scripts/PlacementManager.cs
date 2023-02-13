using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

public class PlacementManager : MonoBehaviour
{
    public PlacementMode mode;
    public PlacementGrid placementGrid;
    public MenuBar menubar;
    [CanBeNull] private PlaceableObject storedObject;
    private int _storedObjectOriginalPosition;
    private Vector3 _screenPosition;
    private Vector3 _worldPosition;

    void Start()
    {
        //placementGrid.Initialise();
        mode = PlacementMode.NoMode;
        // Setting delegates for placement tiles
        for (int j = 0; j < placementGrid.tileList.Count; j++)
        {
            int i = j;
            placementGrid.tileList[i].GetComponent<PlacementTile>().AddInteractionListener(KeybindManager.Instance.Place, (sender, e) => OnPlaceObjectRequest(i));
            placementGrid.tileList[i].GetComponent<PlacementTile>().AddInteractionListener(KeybindManager.Instance.Move, (sender, e) => OnMoveObjectRequest(i));
            placementGrid.tileList[i].GetComponent<PlacementTile>().AddInteractionListener(KeybindManager.Instance.Delete, (sender, e) => OnDeleteObjectRequest(i));
            placementGrid.tileList[i].GetComponent<PlacementTile>().OnTileMouseEnter += (sender, e) => OnTileMouseEnter(i);
        }
        // Adding Menubar delegates
        menubar.MenubarSelectionEventHandler += OnMenubarSelection;
        menubar.MenubarDeselectionEventHandler += OnMenubarDeselection;
    }

    public void OnTileMouseEnter(int index) {
        if ((mode == PlacementMode.Create || mode == PlacementMode.Move) && !placementGrid.IsObjectPlaced(index)) {
            placementGrid.SpawnGhostObject(storedObject, index);
        }
    }

    public void OnMenubarSelection(object sender, MenubarSelectionEventArgs2 e) {
        if (mode != PlacementMode.Create) StateLeave();
        mode = PlacementMode.Create;
        storedObject = e.objectToSelect.prefab.GetComponent<PlaceableObject>();
    }

    public void OnMenubarDeselection(object sender, MenubarSelectionEventArgs2 e) {
        StateLeave();
        storedObject = null;
        mode = PlacementMode.NoMode;
    }

    public void OnPlaceObjectRequest(int index)
    {
        if (mode == PlacementMode.Create && !placementGrid.IsObjectPlaced(index))
        {
            placementGrid.PlaceObject(storedObject,index);
            menubar.RemoveItem(storedObject.GetComponent<PlaceableObject>().data);
        }
    }

    public void OnMoveObjectRequest(int index)
    {
        if (mode == PlacementMode.Move)
        {
            if (!placementGrid.IsObjectPlaced(index))
            {
                placementGrid.PlaceObject(storedObject, index);
                Destroy(storedObject.gameObject);
                mode = PlacementMode.NoMode;
                storedObject = null;
            }
        }
        else
        {
            if (placementGrid.IsObjectPlaced(index))
            {
                StateLeave();
                storedObject = placementGrid.RemoveObject(index);
                _storedObjectOriginalPosition = index;
                storedObject.gameObject.SetActive(false);
                placementGrid.SpawnGhostObject(storedObject, index);
                mode = PlacementMode.Move;
            }
        }
    }

    public void OnDeleteObjectRequest(int index)
    {
        if (placementGrid.IsObjectPlaced(index))
        {
            PlaceableObject objectToDestroy = placementGrid.RemoveObject(index);
            menubar.AddItem(objectToDestroy.data);
            Destroy(objectToDestroy.gameObject);
        }
    }

    public void StateLeave()
    {
        switch (mode)
        {
            case PlacementMode.Create:
                menubar.DeselectItem();
                break;
            case PlacementMode.Move:
                placementGrid.PlaceObject(storedObject,_storedObjectOriginalPosition);
                Destroy(storedObject.gameObject);
                storedObject = null;
                break;
        }
    }
}
