using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class PlacementManager
{
    private PlacementMode mode;
    [CanBeNull] private GameObject selectedObject;
    [CanBeNull] private GameObject movingObject;
    private PlacementTile parentOfMovingObject;
    private static readonly PlacementManager instance = new PlacementManager();

    static PlacementManager()
    {
    }

    private PlacementManager()
    {
    }

    public static PlacementManager Instance
    {
        get { return instance; }
    }

    public void SetSelectedObject(GameObject objectToStore, PlacementMode mode)
    {
        this.selectedObject = objectToStore;
        this.mode = mode;
    }

    public void SetMovingObject(GameObject objectToStore, PlacementMode mode, PlacementTile parentOfMovingObject)
    {
        this.movingObject = objectToStore;
        this.mode = mode;
        this.parentOfMovingObject = parentOfMovingObject;
    }

    [CanBeNull]
    public GameObject GetStoredObjectAndUpdate(PlacementMode desiredMode)
    {
        switch (this.mode)
        {
            case PlacementMode.Moving:
                if (desiredMode == PlacementMode.Creating)
                {
                    this.mode = PlacementMode.Creating;
                    return selectedObject;
                }
                else if (desiredMode == PlacementMode.Moving)
                {
                    if (selectedObject != null)
                    {
                        this.mode = PlacementMode.Creating; // return to placement mode if there is an object selected
                    }
                    else
                    {
                        this.mode = PlacementMode.NoMode;
                    }

                    parentOfMovingObject.objectPlaced = false;
                    return movingObject;
                }
                return null;
            case PlacementMode.Creating:
                if (desiredMode == PlacementMode.Moving)
                {
                    return null;
                }
                return selectedObject;
            case PlacementMode.NoMode:
                return null;
            default:
                throw new SwitchExpressionException();
        }
    }

    [CanBeNull]
    public GameObject GetStoredObject()
    {
        switch (mode)
        {
            case PlacementMode.Creating:
                return selectedObject;
            case PlacementMode.Moving:
                return movingObject;
            default:
                return null;
        }
    }
}
