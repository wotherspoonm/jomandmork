using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChangeKeybindPopup : MonoBehaviour
{
    public TextMeshProUGUI popupText;

    public void Show(string text) {
        popupText.text = text;
        gameObject.SetActive(true);
    }
    public void Hide() {
        popupText.text = "If you are seeing this, something has gone wrong";
        gameObject.SetActive(false);
    }
}
