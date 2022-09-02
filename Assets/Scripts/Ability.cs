using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityInputType {
    press,
    hold,
    toggle
}

public abstract class Ability : ScriptableObject
{
    public new string name;
    public float cooldownTime;
    public float activeTime;
    [Tooltip("Only applies to Hold InputType")]
    public float maxTimeBeforeReset;
    public AbilityInputType InputType;

    public abstract void Activate(GameObject parent);
    public abstract void Finshed(GameObject parent);
}
