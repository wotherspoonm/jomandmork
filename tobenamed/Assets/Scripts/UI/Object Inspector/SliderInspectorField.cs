using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;

public class SliderInspectorField : InspectorFieldBase<float> {
    private float _maxValue;
    public float MaxValue => _maxValue;
    private float _minValue;
    public float MinValue => _minValue;
    private bool _isDiscrete;
    public bool IsDiscrete => _isDiscrete;
    public SliderInspectorField(Func<float> getValue, Action<float> setValue, string name, float maxValue, float minValue = 0, bool isDiscrete = false) : base(getValue, setValue, name) {
        this._maxValue = maxValue;
        this._minValue = minValue;
        this._isDiscrete = isDiscrete;
    }
}

