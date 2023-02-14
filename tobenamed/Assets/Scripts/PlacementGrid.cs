using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;
using UnityEditor.Animations;

public class PlacementGrid : MonoBehaviour
{
    public List<GameObject> tileList;
    public GameObject tilePrefab;
    public const float gridSpacing = 1f;


    public bool IsObjectPlaced(int index)
    {
        return tileList[index].GetComponent<PlacementTile>().IsObjectPlaced();
    }

    public void PlaceObject(PlaceableObject objectToPlace, int index)
    {
        tileList[index].GetComponent<PlacementTile>().PlaceObject(objectToPlace);
    }

    public void PlaceObject(PlaceableObjectSO objectToPlaceSO, int index) {
        tileList[index].GetComponent<PlacementTile>().PlaceObject(objectToPlaceSO);
    }

    public void SpawnGhostObject(PlaceableObject objectToSpawn, int index) {
        tileList[index].GetComponent<PlacementTile>().SpawnGhostObject(objectToSpawn);
    }

    public void SpawnGhostObject(PlaceableObjectSO objectToSpawnSO, int index) {
        tileList[index].GetComponent<PlacementTile>().SpawnGhostObject(objectToSpawnSO);
    }

    public PlaceableObject RemoveObject(int index)
    {
        return tileList[index].GetComponent<PlacementTile>().RemoveObject();
    }
}
