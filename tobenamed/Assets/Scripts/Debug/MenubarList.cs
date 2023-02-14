using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomDebug {
    public class MenubarList : MonoBehaviour {
        [SerializeField]
        private List<PlaceableObjectSO> placeableObjectData;
        [SerializeField]
        private GameObject listItemPrefab;
        [SerializeField]
        private GameObject contentContainer;
        [SerializeField]
        private MenuBar menuBar2;

        private void Awake() {
            foreach (PlaceableObjectSO data in placeableObjectData) {
                GameObject obj = Instantiate(listItemPrefab, contentContainer.transform);
                obj.GetComponent<MenubarListItem>().placeableObjectData = data;
                obj.GetComponent<MenubarListItem>().menubar2 = menuBar2;
            }
        }
    }
}

