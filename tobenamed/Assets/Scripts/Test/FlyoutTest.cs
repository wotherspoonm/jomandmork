using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyoutTest : InteractableGameObject
{
    public GameObject propertyWindow;
    // Start is called before the first frame update
    void Start()
    {
        AddInteractionListener(KeyCode.Mouse1, ShowPropertyWindow);
    }

    protected virtual void ShowPropertyWindow(object sender, InteractableGameObjectEventArgs e) {

    }
}
