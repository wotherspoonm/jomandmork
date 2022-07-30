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
    private Vector2Int storedObjectOriginalPosition;
    private Vector3 screenPosition;
    private Vector3 worldPosition;
    private int selectedItemIndex;
    private bool itemIsSelected = false;

    void Start()
    {
        placementGrid.Initialise();
        mode = PlacementMode.NoMode;
        //Setting delegates for placement tiles
        for (int x = 0; x < placementGrid.gridSize.x; x++)
        {
            for (int y = 0; y < placementGrid.gridSize.y; y++)
            {
                int tempX = x;
                int tempY = y;
                placementGrid.tileArray[x,y].GetComponent<PlacementTile>().AddListener(KeybindManager.Instance.Place, () => OnPlaceObjectRequest(new Vector2Int(tempX, tempY)));
                placementGrid.tileArray[x,y].GetComponent<PlacementTile>().AddListener(KeybindManager.Instance.Move, () => OnMoveObjectRequest(new Vector2Int(tempX, tempY)));
                placementGrid.tileArray[x,y].GetComponent<PlacementTile>().AddListener(KeybindManager.Instance.Delete, () => OnDeleteObjectRequest(new Vector2Int(tempX, tempY)));
            }
        }
        //Setting delegates for MenuItems
        for (int i = 0; i < menuBar.menuItems.Count; i++)
        {
            int tempI = i;
            menuBar.itemCells[i].GetComponent<MenuItem>().AddListener(KeyCode.Mouse0, () => OnSelectRequest(tempI));
        }
    }

    public void OnSelectRequest(int itemIndex)
    {
        bool temp = itemIsSelected;
        StateLeave();
        itemIsSelected = temp;
        if (itemIndex == selectedItemIndex && itemIsSelected) {
            menuBar.itemCells[itemIndex].GetComponent<MenuItem>().DeselectItem();
            itemIsSelected = false;
            mode = PlacementMode.NoMode;
        }
        else {
            selectedItemIndex = itemIndex;
            itemIsSelected = true;
            menuBar.DeselectAll();
            menuBar.itemCells[itemIndex].GetComponent<MenuItem>().SelectItem();
            mode = PlacementMode.Create;
            storedObject = CloneObject(menuBar.menuItems[selectedItemIndex]);
        }
    }


    public void OnPlaceObjectRequest(Vector2Int location)
    {
        if (mode == PlacementMode.Create && !placementGrid.IsObjectPlaced(location))
        {
            placementGrid.PlaceObject(storedObject,location);
            storedObject = CloneObject(storedObject);
        }
    }

    public void OnMoveObjectRequest(Vector2Int location)
    {
        if (mode == PlacementMode.Move)
        {
            if (!placementGrid.IsObjectPlaced(location))
            {
                placementGrid.PlaceObject(storedObject,location);
                mode = PlacementMode.NoMode;
                storedObject = null;
            }
        }
        else
        {
            if (placementGrid.IsObjectPlaced(location))
            {
                StateLeave();
                storedObject = placementGrid.RemoveObject(location);
                storedObjectOriginalPosition = location;
                mode = PlacementMode.Move;
            }
        }
    }

    public void OnDeleteObjectRequest(Vector2Int location)
    {
        if (placementGrid.IsObjectPlaced(location))
        {
            GameObject objectToDestroy = placementGrid.RemoveObject(location);
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
                itemIsSelected = false;
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
        if (mode != PlacementMode.NoMode)
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
