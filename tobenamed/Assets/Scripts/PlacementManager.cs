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
    public Menubar menubar;
    [CanBeNull] private GameObject storedObject;
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
        }
        // Adding Menubar delegates
        menubar.MenubarSelectionEventHandler += OnMenubarSelection;
        menubar.MenubarDeselectionEventHandler += OnMenubarDeselection;
    }

    public void OnMenubarSelection(object sender, MenubarSelectionEventArgs2 e) {
        if (mode != PlacementMode.Create) StateLeave();
        if (storedObject != null) Destroy(storedObject);
        mode = PlacementMode.Create;
        storedObject = CloneObject(e.objectToSelect.prefab);
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
            storedObject = CloneObject(storedObject);
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
                mode = PlacementMode.Move;
            }
        }
    }

    public void OnDeleteObjectRequest(int index)
    {
        if (placementGrid.IsObjectPlaced(index))
        {
            GameObject objectToDestroy = placementGrid.RemoveObject(index);
            menubar.AddItem(objectToDestroy.GetComponent<PlaceableObject>().data);
            Destroy(objectToDestroy);
        }
    }

    public void StateLeave()
    {
        switch (mode)
        {
            case PlacementMode.Create:
                menubar.DeselectItem();
                Destroy(storedObject);
                break;
            case PlacementMode.Move:
                placementGrid.PlaceObject(storedObject,_storedObjectOriginalPosition);
                storedObject = null;
                break;
        }
    }
    void Update()
    {
        //Code to make object follow position of mouse
        if (mode != PlacementMode.NoMode && storedObject != null)
        {
            _screenPosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(_screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hitData))
            {
                _worldPosition = hitData.point;
            }
            _worldPosition.z = storedObject.transform.position.z;
            storedObject.transform.position = _worldPosition;
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
