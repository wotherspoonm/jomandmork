using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PropertyWindowComponent : MonoBehaviour
{
    protected Vector2 size;
    public Vector2 Size { get { return size; } }
}
