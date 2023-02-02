using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CustomDebug {
    public class MenubarListItem : MonoBehaviour {
        public PlaceableObjectData placeableObjectData;
        public MenuBar menuBar;
        public TextMeshProUGUI textMeshPro;

        public void Start() {
            textMeshPro.text = placeableObjectData.name;
        }
        public void AddItem() {
            menuBar.AddItem(placeableObjectData.prefab);
        }

        public void RemoveItem() {
            menuBar.RemoveItem(placeableObjectData.prefab);
        }
    }
}
