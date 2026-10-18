using UnityEngine;

public class FlyingObstacle : Interactable
{
    protected override void Interact(PlayerController player)
    {
        player.TakeDamage();
    }
}
