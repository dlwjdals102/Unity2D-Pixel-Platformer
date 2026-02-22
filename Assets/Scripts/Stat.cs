using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float value;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>(); 


    public float GetValue()
    {
        return GetFinalValue();
    }

    public void SetValue(float value)
    {
        this.value = value;
    }

    public void AddModifier(float value, string source)
    {
        StatModifier modToAdd = new StatModifier(value, source);
        modifiers.Add(modToAdd);
    }

    public void RemoveModifier(string source)
    {
        modifiers.RemoveAll(modifiers => modifiers.source == source);
    }

    private float GetFinalValue()
    {
        float finalValue = value;

        foreach (var modifier in modifiers)
        {
            finalValue += modifier.value;
        }

        return finalValue;
    }
}

[Serializable]
public class StatModifier
{
    public float value;
    public string source;

    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}
