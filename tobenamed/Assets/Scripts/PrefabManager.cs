using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public List<GameObject> prefabs;

    public GameObject GetPrefabFromData(PlaceableObjectData data) {
        return prefabs.Find(x => x.GetComponent<PlaceableObject>().data.name == data.name);
    }
}
