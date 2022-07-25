using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class KeybindManager: MonoBehaviour
{
    private static KeybindManager instance;
    public readonly Keybind place = new("placeKeybind", KeyCode.Mouse0);
    public KeyCode Place { get { return place.keyCode; } }
    public readonly Keybind delete = new("deleteKeybind", KeyCode.Mouse1);
    public KeyCode Delete { get { return delete.keyCode; } }
    public readonly Keybind move = new("moveKeybind", KeyCode.Mouse2);
    public KeyCode Move { get { return move.keyCode; } }

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        LoadKeybinds();
    }

    public static KeybindManager Instance{
        get { return instance; }
    }

    /// <summary>
    /// Loads the keybinds from PlayerPrefs.
    /// </summary>
    public void LoadKeybinds() {
        LoadKeybind(place);
        LoadKeybind(delete);
    }

    void LoadKeybind(Keybind keybind) {
        keybind.keyCode = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(keybind.name, keybind.defaultKeyCode.ToString()));
    }
    /// <summary>
    /// Changes the keybind to that which is specified.
    /// </summary>
    /// <param name="keybind"></param>
    /// <param name="newKeyCode"></param>
    public void SetKeybind(Keybind keybind, KeyCode newKeyCode) {
        Debug.Log($"Changed keycode of {keybind.name} from {keybind.keyCode} to {newKeyCode}");
        keybind.keyCode = newKeyCode;
        PlayerPrefs.SetString(keybind.name, newKeyCode.ToString());
    }
}
/// <summary>
/// Stores the name under which the keybind is stored as well its value and default value.
/// </summary>
public class Keybind {
    public Keybind(string name, KeyCode defaultKeyCode) {
        this.name = name;
        this.keyCode = defaultKeyCode;
        this.defaultKeyCode = defaultKeyCode;
    }
    public string name;
    public KeyCode keyCode;
    public KeyCode defaultKeyCode;
}
