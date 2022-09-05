using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityInputType {
    press,
    hold,
    toggle
}

public enum Abilities
{
    Dash,
    SpeedBoost
}

public abstract class Ability : ScriptableObject
{
    public Abilities AbilityName;
    public float cooldownTime;
    public float activeTime;
    public float maxTimeBeforeReset;
    public AbilityInputType InputType;

    public abstract void Activate(GameObject parent);
    public abstract void Finshed(GameObject parent);
}
