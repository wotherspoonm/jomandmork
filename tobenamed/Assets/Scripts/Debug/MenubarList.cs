using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomDebug {
    public class MenubarList : MonoBehaviour {
        [SerializeField]
        private List<PlaceableObjectData> placeableObjectData;
        [SerializeField]
        private GameObject listItemPrefab;
        [SerializeField]
        private GameObject contentContainer;
        [SerializeField]
        private MenuBar menuBar;

        private void Awake() {
            foreach (PlaceableObjectData data in placeableObjectData) {
                GameObject obj = Instantiate(listItemPrefab, contentContainer.transform);
                obj.GetComponent<MenubarListItem>().placeableObjectData = data;
                obj.GetComponent<MenubarListItem>().menuBar = menuBar;
            }
        }
    }
}

