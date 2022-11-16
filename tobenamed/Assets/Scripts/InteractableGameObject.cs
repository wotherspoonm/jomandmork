using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableGameObject : MonoBehaviour
{
    public Dictionary<KeyCode, EventHandler<InteractableGameObjectEventArgs>> keycodeEventDictionary = new();

    protected virtual void Interact() {
        foreach (KeyValuePair<KeyCode, EventHandler<InteractableGameObjectEventArgs>> keyValuePair in keycodeEventDictionary) {
            KeyCode keycode = keyValuePair.Key;
            EventHandler<InteractableGameObjectEventArgs> eventHandler = keyValuePair.Value;
            if (Input.GetKeyDown(keycode)){
                eventHandler?.Invoke(this, new());
            }
        }
    }

    public void AddInteractionListener(KeyCode keycode, EventHandler<InteractableGameObjectEventArgs> action) {
        if (!keycodeEventDictionary.ContainsKey(keycode))
        {
            keycodeEventDictionary.Add(keycode, action);
        }
        else
        {
            keycodeEventDictionary[keycode] += action;
        }
    }

    public void RemoveInteractionListener(KeyCode keycode, EventHandler<InteractableGameObjectEventArgs> action) {
        if (keycodeEventDictionary.ContainsKey(keycode))
            keycodeEventDictionary[keycode] -= action;
    }
}
public class InteractableGameObjectEventArgs : EventArgs {

}