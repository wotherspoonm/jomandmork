using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationButton : MonoBehaviour
{
    public MainMenuPage destination;
    MainMenuPage parentPage;
    SmoothVector parentPagePos;
    SmoothVector destinationPagePos;

    private void Start() {
        parentPage = transform.parent.GetComponent<MainMenuPage>();
        gameObject.GetComponent<Button>().onClick.AddListener(delegate {
            parentPage.TransitionOut();
            destination.TransitionIn();
        });
    }
}
