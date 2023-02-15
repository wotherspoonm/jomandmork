using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;
using System.Runtime.CompilerServices;

public class SliderInspectorField : InspectorFieldBase<float> {
    private float _maxValue;
    public float MaxValue => _maxValue;
    private float _minValue;
    public float MinValue => _minValue;
    private float _discreteInterval;
    public float DiscreteInterval => _discreteInterval;
    public SliderInspectorField(Func<float> getValue, Action<float> setValue, string name, float maxValue, float minValue = 0, float discreteInterval = 0) : base(getValue, setValue, name) {
        this._maxValue = maxValue;
        this._minValue = minValue;
        this._discreteInterval = discreteInterval;
    }
}

