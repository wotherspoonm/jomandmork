using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class PlacementManager : MonoBehaviour
{
    private PlacementMode mode;
    private GameObject selectedObject;
    private GameObject movingObject;
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
        switch (mode)
        {
            case PlacementMode.Creating:
                selectedObject = objectToStore;
                break;
            case PlacementMode.Moving:
                movingObject = objectToStore;
                break;
        }
        this.mode = mode;
    }

    [CanBeNull]
    public GameObject GetStoredObject(PlacementMode desiredMode)
    {
        //Debug.Log("Current mode: " + this.mode);
        //Debug.Log("Desired mode: " + desiredMode);
        switch (this.mode)
        {
            case PlacementMode.Moving:
                if (desiredMode == PlacementMode.Creating)
                {
                    Debug.Log("In if loop");
                    this.mode = desiredMode;
                    return selectedObject;
                }
                this.mode = PlacementMode.NoMode;
                return movingObject;
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
}
