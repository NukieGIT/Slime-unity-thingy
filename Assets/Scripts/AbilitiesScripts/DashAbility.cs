using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Dash")]
public class DashAbility : Ability
{
    [SerializeField] private float dashVelocity;
    private float originalGravity;

    public override void Activate(GameObject parent)
    {
        PlayerMovement movement = parent.GetComponent<PlayerMovement>();
        Rigidbody2D rigidbody2D = parent.GetComponent<Rigidbody2D>();
        TrailRenderer trailRenderer = parent.GetComponent<TrailRenderer>();
        Camera camera = Camera.main;

        Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseWorldPoint2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        originalGravity = rigidbody2D.gravityScale;
        rigidbody2D.gravityScale = 0f;

        trailRenderer.emitting = true;

        movement.IsDashing = true;
        rigidbody2D.velocity = (mouseWorldPoint2D - rigidbody2D.position).normalized * dashVelocity;
        
    }
    public override void Finshed(GameObject parent)
    {
        PlayerMovement movement = parent.GetComponent<PlayerMovement>();
        Rigidbody2D rigidbody2D = parent.GetComponent<Rigidbody2D>();
        TrailRenderer trailRenderer = parent.GetComponent<TrailRenderer>();

        trailRenderer.emitting = false;

        movement.IsDashing = false;
        rigidbody2D.velocity = Vector2.zero;
        movement.MoveAccel = 0;
        rigidbody2D.gravityScale = originalGravity;
    }
}
