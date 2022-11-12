using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyWindow : MonoBehaviour
{
    public List<PropertyWindowComponent> propertyFields = new List<PropertyWindowComponent>();
    private float padding;
    private Vector2 size = new();
    private Vector2 PaddedSize { get { return size + 2 * padding * Vector2.one; } }
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < propertyFields.Count; i++)
        {
            var field = propertyFields[i];
            size.y += field.Size.y;
            // Make sure that the property window is at least as wide as the widest component
            if (field.Size.x > size.x) size.x = field.Size.x;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
