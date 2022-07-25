using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindMenu : MonoBehaviour
{
    private List<Keybind> keybinds = new();
    private List<string> keybindNames = new();
    public GameObject keybindPairPrefab;
    public List<KeybindMenuPair> keybindMenuPairs;
    public ChangeKeybindPopup changeKeyPopup;
    public GameObject keybindParent;
    private int[] keycodes;
    private bool setKeybind = false;
    private int keybindToSet = 0;
    // Start is called before the first frame update
    void Start()
    {
        // Load all known keybinds
        keycodes = (int[])System.Enum.GetValues(typeof(KeyCode));
        // Add keybinds to menu list
        keybinds.Add(KeybindManager.Instance.place);
        keybindNames.Add("Place");
        keybinds.Add(KeybindManager.Instance.delete);
        keybindNames.Add("Delete");
        keybinds.Add(KeybindManager.Instance.move);
        keybindNames.Add("Move");

        for (int i = 0; i < keybinds.Count; i++) {
            var temp = Instantiate(keybindPairPrefab, keybindParent.transform);

            temp.GetComponent<KeybindMenuPair>().text.text = keybindNames[i];
            temp.GetComponent<KeybindMenuPair>().buttonText.text = keybinds[i].keyCode.ToString();
            temp.GetComponent<RectTransform>().anchoredPosition = temp.GetComponent<RectTransform>().anchoredPosition + i * new Vector2(0, -40);
            keybindMenuPairs.Add(temp.GetComponent<KeybindMenuPair>());
            int t = i;
            keybindMenuPairs[i].button.onClick.AddListener(delegate {
                UpdateKeybind(t);
            });
        }
    }

    void UpdateKeybind(int i) {
        setKeybind = true;
        keybindToSet = i;
        changeKeyPopup.Show($"Press any key to bind for {keybindNames[keybindToSet]} or escape to cancel");
    }
    // Update is called once per frame
    void Update()
    {
        if (setKeybind) {
            for (int i = 1; i < keycodes.Length; i++) {
                if (Input.GetKey(KeyCode.Escape)){
                    changeKeyPopup.Hide();
                    break;
                }
                if (Input.GetKey((KeyCode)keycodes[i])) {
                    KeybindManager.Instance.SetKeybind(keybinds[keybindToSet], (KeyCode)keycodes[i]);
                    keybindMenuPairs[keybindToSet].buttonText.text = keybinds[keybindToSet].keyCode.ToString();
                    setKeybind = false;
                    changeKeyPopup.Hide();
                }
            }
        }
    }
}
