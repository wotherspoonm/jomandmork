using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    private static MenuBarManager instance;
    public static MenuBarManager Instance { get { return instance; } }

    private void Awake()
    {
        // Singleton setup
        if (instance == null) {
            instance = this;
        }
    }
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
            Debug.Log("We the same");
            itemCells[index].GetComponent<MenuItem>().DeselectItem();
            itemIsSelected = false;
            PlacementManager.Instance.TransitionToNoMode();
        }
        else {
            Debug.Log("We not the same");
            selectedItemIndex = index;
            PlacementManager.Instance.TransitionToCreate(SelectedItem);
            itemIsSelected = true;
            itemCells[index].GetComponent<MenuItem>().SelectItem();
            for (int i = 0; i < itemCells.Count; i++) {
                if (i == index) continue;
                itemCells[i].GetComponent<MenuItem>().DeselectItem();
            }
        }
    }

    public void DeselectAll() {
        if (itemIsSelected) {
            itemCells[selectedItemIndex].GetComponent<MenuItem>().DeselectItem();
            itemIsSelected = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(selectedItemIndex);
    }
}
