using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindMenu : MonoBehaviour
{
    private List<KeyCode> keybinds = new();
    private List<string> keybindNames = new();
    public GameObject keybindPairPrefab;
    // Start is called before the first frame update
    void Start()
    {
        keybinds.Add(KeybindManager.Instance.Place);
        keybindNames.Add("Place");
        keybinds.Add(KeybindManager.Instance.Delete);
        keybindNames.Add("Delete");
        keybinds.Add(KeybindManager.Instance.Move);
        keybindNames.Add("Move");

        for (int i = 0; i < keybinds.Count; i++) {
            var temp = Instantiate(keybindPairPrefab, gameObject.transform);

            temp.GetComponent<KeybindMenuPair>().text.text = keybindNames[i];
            temp.GetComponent<KeybindMenuPair>().buttonText.text = keybinds[i].ToString();
            temp.GetComponent<RectTransform>().anchoredPosition = temp.GetComponent<RectTransform>().anchoredPosition + i * new Vector2(0, -40);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
