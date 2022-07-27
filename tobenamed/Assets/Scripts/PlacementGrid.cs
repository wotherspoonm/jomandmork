using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementGrid : MonoBehaviour
{
    public PlacementGrid(Vector2Int gridSize) {
        this.gridSize = gridSize;
        tileArray = new GameObject[gridSize.x, gridSize.y];
    }

    public GameObject[,] tileArray;
    public GameObject tilePrefab;
    public Vector2Int gridSize;
    public const float gridSpacing = 2f;

    // Start is called before the first frame update
    public void Initialise() {
        tileArray = new GameObject[gridSize.x, gridSize.y];
        for (int x = 0; x < gridSize.x; x++) {
            for(int y = 0; y < gridSize.y; y++) {
                Vector3 spawnPosition = new Vector3(x, y) * gridSpacing;
                Quaternion spawnRotation = new();
                Transform parent = transform;
                GameObject temp = Instantiate(tilePrefab, spawnPosition, spawnRotation, parent);
                temp.transform.localScale = new Vector3(1,1)*gridSpacing + new Vector3(0,0,0.1f);
                tileArray[x, y] = temp;
            }
        }
    }
}
