using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

public class PlacementManager : MonoBehaviour
{
    private PlacementMode _mode;
    [SerializeField] private PlacementGrid _placementGrid;
    [SerializeField] private MenuBar _menubar;

    [CanBeNull] private PlaceableObject _storedObject;
    private int _storedObjectOriginalPosition;
    private Vector3 _screenPosition;
    private Vector3 _worldPosition;

    void Start()
    {
        //placementGrid.Initialise();
        _mode = PlacementMode.NoMode;
        // Setting delegates for placement tiles
        for (int j = 0; j < _placementGrid.tileList.Count; j++)
        {
            int i = j;
            _placementGrid.tileList[i].GetComponent<PlacementTile>().AddInteractionListener(KeybindManager.Instance.Place, (sender, e) => OnPlaceObjectRequest(i));
            _placementGrid.tileList[i].GetComponent<PlacementTile>().AddInteractionListener(KeybindManager.Instance.Move, (sender, e) => OnMoveObjectRequest(i));
            _placementGrid.tileList[i].GetComponent<PlacementTile>().AddInteractionListener(KeybindManager.Instance.Delete, (sender, e) => OnDeleteObjectRequest(i));
            _placementGrid.tileList[i].GetComponent<PlacementTile>().OnTileMouseEnter += (sender, e) => OnTileMouseEnter(i);
        }
        // Adding Menubar delegates
        _menubar.MenubarSelectionEventHandler += OnMenubarSelection;
        _menubar.MenubarDeselectionEventHandler += OnMenubarDeselection;
    }

    private void OnTileMouseEnter(int index) {
        if (!_placementGrid.IsObjectPlaced(index)) {
            if (_mode == PlacementMode.Create) {
                _placementGrid.SpawnGhostObject(_storedObject.data, index);
            }
            else if (_mode == PlacementMode.Move) {
                _placementGrid.SpawnGhostObject(_storedObject, index);
            }
        }
    }

    private void OnMenubarSelection(object sender, MenubarSelectionEventArgs2 e) {
        if (_mode != PlacementMode.Create) StateLeave();
        _mode = PlacementMode.Create;
        _storedObject = e.objectToSelect.prefab.GetComponent<PlaceableObject>();
    }

    private void OnMenubarDeselection(object sender, MenubarSelectionEventArgs2 e) {
        StateLeave();
        _storedObject = null;
        _mode = PlacementMode.NoMode;
    }

    private void OnPlaceObjectRequest(int index)
    {
        if (_mode == PlacementMode.Create && !_placementGrid.IsObjectPlaced(index))
        {
            _placementGrid.PlaceObject(_storedObject.data, index);
            _menubar.RemoveItem(_storedObject.GetComponent<PlaceableObject>().data);
        }
    }

    private void OnMoveObjectRequest(int index)
    {
        if (_mode == PlacementMode.Move)
        {
            if (!_placementGrid.IsObjectPlaced(index))
            {
                _placementGrid.PlaceObject(_storedObject, index);
                Destroy(_storedObject.gameObject);
                _mode = PlacementMode.NoMode;
                _storedObject = null;
            }
        }
        else
        {
            if (_placementGrid.IsObjectPlaced(index))
            {
                StateLeave();
                _storedObject = _placementGrid.RemoveObject(index);
                _storedObjectOriginalPosition = index;
                _storedObject.gameObject.SetActive(false);
                _placementGrid.SpawnGhostObject(_storedObject, index);
                _mode = PlacementMode.Move;
            }
        }
    }

    private void OnDeleteObjectRequest(int index)
    {
        if (_placementGrid.IsObjectPlaced(index))
        {
            PlaceableObject objectToDestroy = _placementGrid.RemoveObject(index);
            _menubar.AddItem(objectToDestroy.data);
            Destroy(objectToDestroy.gameObject);
        }
    }

    private void StateLeave()
    {
        switch (_mode)
        {
            case PlacementMode.Create:
                _menubar.DeselectItem();
                break;
            case PlacementMode.Move:
                _placementGrid.PlaceObject(_storedObject, _storedObjectOriginalPosition);
                Destroy(_storedObject.gameObject);
                _storedObject = null;
                break;
        }
    }
}
