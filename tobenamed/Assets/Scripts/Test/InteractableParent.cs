using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableParent : MonoBehaviour
{
    public InteractableGameObject child;
    // Start is called before the first frame update
    void Start()
    {
        child.AddInteractionListener(KeyCode.Mouse0, hello);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void hello()
    {
        Debug.Log("Hello");
    }
}
