using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class InspectorFieldBase {
    protected readonly string _name;
    public InspectorFieldBase(string name) {
        this._name = name;
    }
}
public abstract class InspectorFieldBase<T> : InspectorFieldBase
{
    public InspectorFieldBase(Func<T> getValue, Action<T> setValue, string name): base(name) {
        GetValue += getValue;
        SetValue += setValue;
    }

    public Func<T> GetValue;
    public Action<T> SetValue;
    public string Name => _name;

}
