using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenubar : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject spherePrefab;
    public MenuBar menuBar;

    public void AddCube() {
        menuBar.AddItem(cubePrefab);
    }

    public void AddSphere() {
        menuBar.AddItem(spherePrefab);
    }

    public void RemoveCube() {
        menuBar.RemoveItem(cubePrefab);
    }

    public void RemoveSphere() {
        menuBar.RemoveItem(spherePrefab);
    }
}
