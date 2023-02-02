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
    public MenuBar menuBar;
    [CanBeNull] private GameObject storedObject;
    private int storedObjectOriginalPosition;
    private Vector3 screenPosition;
    private Vector3 worldPosition;

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
        }
        // Adding Menubar delegates
        menuBar.MenubarSelectionEventHandler += OnMenubarSelection;
        menuBar.MenubarDeselectionEventHandler += OnMenubarDeselection;
    }

    public void OnMenubarSelection(object sender, MenubarSelectionEventArgs e)
    {
        if (mode != PlacementMode.Create) StateLeave();
        if (storedObject != null) Destroy(storedObject);
        mode = PlacementMode.Create;
        storedObject = CloneObject(e.gameObjectToSelect);
    }

    public void OnMenubarDeselection(object sender, MenubarSelectionEventArgs e)
    {
        StateLeave();
        storedObject = null;
        mode = PlacementMode.NoMode;

    }

    public void OnPlaceObjectRequest(int index)
    {
        if (mode == PlacementMode.Create && !placementGrid.IsObjectPlaced(index))
        {
            placementGrid.PlaceObject(storedObject,index);
            storedObject = CloneObject(storedObject);
            menuBar.RemoveItem(storedObject);
        }
    }

    public void OnMoveObjectRequest(int index)
    {
        if (mode == PlacementMode.Move)
        {
            if (!placementGrid.IsObjectPlaced(index))
            {
                placementGrid.PlaceObject(storedObject, index);
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
                storedObjectOriginalPosition = index;
                mode = PlacementMode.Move;
            }
        }
    }

    public void OnDeleteObjectRequest(int index)
    {
        if (placementGrid.IsObjectPlaced(index))
        {
            GameObject objectToDestroy = placementGrid.RemoveObject(index);
            menuBar.AddItem(objectToDestroy);
            Destroy(objectToDestroy);
        }
    }

    public void StateLeave()
    {
        switch (mode)
        {
            case PlacementMode.Create:
                menuBar.DeselectAll();
                Destroy(storedObject);
                break;
            case PlacementMode.Move:
                placementGrid.PlaceObject(storedObject,storedObjectOriginalPosition);
                storedObject = null;
                break;
        }
    }
    void Update()
    {
        //Code to make object follow position of mouse
        if (mode != PlacementMode.NoMode && storedObject != null)
        {
            screenPosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hitData))
            {
                worldPosition = hitData.point;
            }
            worldPosition.z = storedObject.transform.position.z;
            storedObject.transform.position = worldPosition;
        }
    }

    public GameObject CloneObject(GameObject objectToClone)
    {
        Vector3 spawnPosition = placementGrid.transform.position;
        spawnPosition += new Vector3(0, 0, PlacementTile.distanceFromGrid);
        Quaternion spawnRotation = new();
        GameObject result = Instantiate(objectToClone, spawnPosition, spawnRotation);
        return result;
    }
}
