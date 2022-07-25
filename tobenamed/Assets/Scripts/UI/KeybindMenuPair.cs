using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeybindMenuPair : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI buttonText;
    public Button button;
    public Keybind keybind;
    private bool setKeybind;
    private int[] keycodes;

    public void Start() {
        keycodes = (int[]) System.Enum.GetValues(typeof(KeyCode));
        button.onClick.AddListener(delegate {
            setKeybind = true;
        });
    }

    public void Update() {
        if(setKeybind) {
            for(int i = 1; i < keycodes.Length; i++) {
                if (Input.GetKey((KeyCode)keycodes[i])) {
                    Debug.Log((KeyCode)keycodes[i]);
                    KeybindManager.Instance.SetKeybind(keybind, (KeyCode)keycodes[i]);
                    buttonText.text = keybind.keyCode.ToString();
                    setKeybind = false;
                }
            }
        }
    }
}
