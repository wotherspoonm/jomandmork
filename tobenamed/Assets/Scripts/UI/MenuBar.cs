using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuBar : MonoBehaviour
{
    public List<GameObject> menuItems;
    public List<GameObject> itemCells = new();
    public GameObject menuBar;
    public GameObject itemCellPrefab;
    public const float itemScale = 50;
    public const float itemSeparation = 45;
    public const float menuBarHeight = 90;

    // Start is called before the first frame update
    void Start()
    {
        // Set menu bar size  and position based on the number of items
        var menuRectTransform = menuBar.GetComponent<RectTransform>();
        menuRectTransform.sizeDelta = new Vector2((itemSeparation + itemScale) * (menuItems.Count) + itemSeparation, menuBarHeight);
        menuRectTransform.anchoredPosition3D = new Vector3(menuRectTransform.anchoredPosition3D.x, menuRectTransform.anchoredPosition3D.y, itemScale);
        // Spawn items for display 
        for (int i = 0; i < menuItems.Count; i++) {
            var newCell = Instantiate(itemCellPrefab, menuBar.transform);
            newCell.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(i * (itemSeparation + itemScale) + 0.5f * itemScale + itemSeparation, 0, -itemScale);
            newCell.GetComponent<MenuItem>().displayItem = Instantiate(menuItems[i], newCell.transform);
            newCell.GetComponent<MenuItem>().displayItem.transform.localScale = Vector3.one * newCell.GetComponent<MenuItem>().displayItem.GetComponent<PlaceableObject>().menuPreviewSize;
            itemCells.Add(newCell);
        }
    }

    public void DeselectAll()
    {
        for (int i = 0; i < itemCells.Count; i++)
        {
            itemCells[i].GetComponent<MenuItem>().DeselectItem();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
