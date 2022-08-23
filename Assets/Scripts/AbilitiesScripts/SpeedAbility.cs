using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Speed Boost")]
public class SpeedAbility : Ability
{
    [SerializeField] private float speedMultiplier;
    private float originalSpeed;

    public override void Activate(GameObject parent)
    {
        PlayerMovement movement = parent.GetComponent<PlayerMovement>();
        originalSpeed = movement.MaxSpeed;
        movement.MaxSpeed *= speedMultiplier;
        
    }
    public override void Finshed(GameObject parent)
    {
        PlayerMovement movement = parent.GetComponent<PlayerMovement>();
        movement.MaxSpeed = originalSpeed;
    }
}
