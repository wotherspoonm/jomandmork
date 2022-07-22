using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class PlacementManager : MonoBehaviour
{
    private PlacementMode mode;
    private GameObject storedObject;
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

    public void SetStoredObject(GameObject objectToStore, PlacementMode mode)
    {
        switch (this.mode)
        {
            case PlacementMode.Moving:
                return;
            case PlacementMode.Creating:
            case PlacementMode.NoMode:
                this.mode = mode;
                this.storedObject = objectToStore;
                break;
            default:
                throw new SwitchExpressionException();
        }
    }

    [CanBeNull]
    public GameObject GetStoredObject()
    {
        switch (this.mode)
        {
            case PlacementMode.Moving:
                this.mode = PlacementMode.NoMode;
                return storedObject;
            case PlacementMode.Creating:
                return storedObject;
            case PlacementMode.NoMode:
                return null;
            default:
                throw new SwitchExpressionException();
        }
    }
}
