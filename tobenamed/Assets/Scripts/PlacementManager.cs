using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class PlacementManager
{
    public PlacementMode mode;
    [CanBeNull] private GameObject storedObject;
    private Vector3 storedObjectOriginalPosition;
    private Vector3 screenPosition;
    private Vector3 worldPosition;
    private static readonly PlacementManager instance = new PlacementManager();

    static PlacementManager()
    {
    }

    private PlacementManager()
    {
        mode = PlacementMode.NoMode;
    }

    public static PlacementManager Instance
    {
        get { return instance; }
    }

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
}
