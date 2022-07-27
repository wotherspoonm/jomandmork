using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableGameObject : MonoBehaviour
{
    public Dictionary<KeyCode, Action> keycodeEventDictionary = new();

    protected virtual void OnMouseOver() {
        foreach (KeyValuePair<KeyCode, Action> keyValuePair in keycodeEventDictionary) {
            KeyCode keycode = keyValuePair.Key;
            Action action = keyValuePair.Value;
            if (Input.GetKeyDown(keycode)){
                action();
            }
        }
    }

    public void AddListener(KeyCode keycode, Action action) {
        keycodeEventDictionary.Add(keycode, action);
    }
}
