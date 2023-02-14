using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CustomDebug {
    public class MenubarListItem : MonoBehaviour {
        public PlaceableObjectSO placeableObjectData;
        public MenuBar menubar2;
        public TextMeshProUGUI textMeshPro;

        public void Start() {
            textMeshPro.text = placeableObjectData.name;
        }
        public void AddItem() {
            menubar2.AddItem(placeableObjectData);
        }

        public void RemoveItem() {
            menubar2.AddItem(placeableObjectData);
        }
    }
}
