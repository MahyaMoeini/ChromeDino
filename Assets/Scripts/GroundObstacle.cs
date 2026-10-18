using UnityEngine;

public class GroundObstacle : Interactable
{
    protected override void Interact(PlayerController player)
    {
        player.TakeDamage();
    }
}