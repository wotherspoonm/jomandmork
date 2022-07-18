using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBarManager : MonoBehaviour
{
    public List<GameObject> menuItems;
    public GameObject menuBar;
    public GameObject ItemCellPrefab;
    public const float itemScale = 50;
    public const float itemSeparation = 15;
    public const float menuBarHeight = 90;
    // Start is called before the first frame update
    void Start()
    {
        menuBar.GetComponent<RectTransform>().sizeDelta = new Vector2((itemSeparation + itemScale) * (menuItems.Count) + itemSeparation, menuBarHeight);
        for(int i = 0; i < menuItems.Count; i++) {
            var newCell = Instantiate(ItemCellPrefab, menuBar.transform);
            newCell.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * (itemSeparation + itemScale) + 0.5f * itemScale + itemSeparation, 0, 0);
            _ = Instantiate(menuItems[i], newCell.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
