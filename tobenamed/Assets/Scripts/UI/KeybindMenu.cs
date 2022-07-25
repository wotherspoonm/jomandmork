using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindMenu : MonoBehaviour
{
    private List<Keybind> keybinds = new();
    private List<string> keybindNames = new();
    public GameObject keybindPairPrefab;
    public List<KeybindMenuPair> keybindMenuPairs;
    // Start is called before the first frame update
    void Start()
    {
        keybinds.Add(KeybindManager.Instance.place);
        keybindNames.Add("Place");
        keybinds.Add(KeybindManager.Instance.delete);
        keybindNames.Add("Delete");
        keybinds.Add(KeybindManager.Instance.move);
        keybindNames.Add("Move");

        for (int i = 0; i < keybinds.Count; i++) {
            var temp = Instantiate(keybindPairPrefab, gameObject.transform);

            temp.GetComponent<KeybindMenuPair>().text.text = keybindNames[i];
            temp.GetComponent<KeybindMenuPair>().buttonText.text = keybinds[i].keyCode.ToString();
            temp.GetComponent<KeybindMenuPair>().keybind = keybinds[i];
            temp.GetComponent<RectTransform>().anchoredPosition = temp.GetComponent<RectTransform>().anchoredPosition + i * new Vector2(0, -40);
            keybindMenuPairs.Add(temp.GetComponent<KeybindMenuPair>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
