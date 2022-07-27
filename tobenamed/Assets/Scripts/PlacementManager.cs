using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;

public class PlacementManager : MonoBehaviour
{
    public PlacementMode mode;
    public PlacementGrid placementGrid;
    [CanBeNull] private GameObject storedObject;
    private Vector3 storedObjectOriginalPosition;
    private Vector3 screenPosition;
    private Vector3 worldPosition;

    void Start()
    {
        placementGrid.Initialise();
        mode = PlacementMode.NoMode;
        for (int x = 0; x < placementGrid.gridSize.x; x++)
        {
            for (int y = 0; y < placementGrid.gridSize.y; y++)
            {
                placementGrid.tileArray[x,y].GetComponent<PlacementTile>().AddListener(KeybindManager.Instance.Place, () => OnPlaceObjectRequest(new Vector2Int(x,y)));
                placementGrid.tileArray[x, y].GetComponent<PlacementTile>().AddListener(KeybindManager.Instance.Move, () => OnMoveObjectRequest(new Vector2Int(x, y)));
                placementGrid.tileArray[x, y].GetComponent<PlacementTile>().AddListener(KeybindManager.Instance.Delete, () => OnDeleteObjectRequest(new Vector2Int(x, y)));
            }
        }
    }


    public void OnPlaceObjectRequest(Vector2Int location)
    {
        Debug.Log("Place bitch");
        //Code to place object
    }

    public void OnMoveObjectRequest(Vector2Int location)
    {
        Debug.Log("Move bitch");
        //Code to move object
    }

    public void OnDeleteObjectRequest(Vector2Int location)
    {
        Debug.Log("Delete bitch");
        //Code to delete object
    }
    /*
    public void MoveObject()
    {
        if (mode == PlacementMode.Move && storedObject != null)
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

    public void StateLeave()
    {
        switch (mode)
        {
            case PlacementMode.Create:
                MenuBarManager.Instance.DeselectAll();
                storedObject = null;
                break;
            case PlacementMode.Move:
                storedObject.transform.position = storedObjectOriginalPosition;
                storedObject.transform.parent.transform.gameObject.GetComponent<PlacementTile>().objectPlaced = true;
                break;
            case PlacementMode.NoMode:
                break;
        }
    }

    public void TransitionToCreate(GameObject objectToStore)
    {
        StateLeave();
        storedObject = objectToStore;
        mode = PlacementMode.Create;
    }

    public void TransitionToMove(GameObject objectToStore)
    {
        StateLeave();
        storedObject = objectToStore;
        storedObjectOriginalPosition = objectToStore.transform.position;
        mode = PlacementMode.Move;
    }

    public void TransitionToNoMode()
    {
        StateLeave();
        mode = PlacementMode.NoMode;
    }

    [CanBeNull]
    public GameObject GetStoredObject() {
        return storedObject;
    }
    */
}
