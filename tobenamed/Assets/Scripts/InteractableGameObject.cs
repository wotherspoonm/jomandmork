using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableGameObject : MonoBehaviour
{
    public Dictionary<KeyCode, Action> keycodeEventDictionary = new();

    protected virtual void Interact() {
        foreach (KeyValuePair<KeyCode, Action> keyValuePair in keycodeEventDictionary) {
            KeyCode keycode = keyValuePair.Key;
            Action action = keyValuePair.Value;
            if (Input.GetKeyDown(keycode)){
                action();
            }
        }
    }

    public void AddInteractionListener(KeyCode keycode, Action action) {
        if (!keycodeEventDictionary.ContainsKey(keycode))
        {
            keycodeEventDictionary.Add(keycode, action);
        }
        else
        {
            keycodeEventDictionary[keycode] += action;
        }
    }
}
