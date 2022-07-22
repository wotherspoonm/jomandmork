using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBarManager : MonoBehaviour
{
    public List<GameObject> menuItems;
    List<GameObject> itemCells = new();
    public GameObject menuBar;
    public GameObject itemCellPrefab;
    public const float itemScale = 50;
    public const float itemSeparation = 45;
    public const float menuBarHeight = 90;
    private int selectedItemIndex;
    private bool itemIsSelected = false;
    public GameObject SelectedItem { get { return menuItems[selectedItemIndex]; } }
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
        SetButtonClickDelegates();
    }
    /// <summary>
    /// Needs to be called whenever the menuItems list is changed in any ways. 
    /// </summary>
    void SetButtonClickDelegates() {
        for (int i = 0; i < itemCells.Count; i++) {
            int temp = i;
            itemCells[i].GetComponent<Button>().onClick.AddListener(delegate { SelectItem(temp); });
        }
    }
    /// <summary>
    /// Selects the item at the given index, and deselects all others 
    /// </summary>
    /// <param name="index"></param>
    void SelectItem(int index) {
        if (index == selectedItemIndex && itemIsSelected) {
            itemCells[index].GetComponent<MenuItem>().DeselectItem();
            itemIsSelected = false;
            PlacementManager.Instance.SetSelectedObject(SelectedItem, PlacementMode.NoMode);
        }
        else {
            selectedItemIndex = index;
            itemIsSelected = true;
            itemCells[index].GetComponent<MenuItem>().SelectItem();
            for (int i = 0; i < itemCells.Count; i++) {
                if (i == index) continue;
                itemCells[i].GetComponent<MenuItem>().DeselectItem();
            }
            PlacementManager.Instance.SetSelectedObject(SelectedItem,PlacementMode.Creating);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
