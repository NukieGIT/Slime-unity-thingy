using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Speed Boost")]
public class SpeedAbility : Ability
{
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float accelerationMultiplier;
    private float originalSpeed;
    private float originalAcceleration;

    public override void Activate(GameObject parent)
    {
        PlayerMovement movement = parent.GetComponent<PlayerMovement>();
        originalSpeed = movement.MaxSpeed;
        originalAcceleration = movement.MoveAcceleration;
        movement.MaxSpeed *= speedMultiplier;
        movement.MoveAcceleration *= accelerationMultiplier;

    }
    public override void Finshed(GameObject parent)
    {
        PlayerMovement movement = parent.GetComponent<PlayerMovement>();
        movement.MaxSpeed = originalSpeed;
        movement.MoveAcceleration = originalAcceleration;
    }
}
