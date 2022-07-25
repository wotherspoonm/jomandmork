using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationButton : MonoBehaviour
{
    public GameObject destination;
    GameObject parentPage;

    private void Start() {
        parentPage = transform.parent.gameObject;
        gameObject.GetComponent<Button>().onClick.AddListener(delegate {
            parentPage.SetActive(false);
            destination.SetActive(true);
        });
    }
}
