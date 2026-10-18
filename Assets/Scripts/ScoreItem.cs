using UnityEngine;

public class ScoreItem : Interactable
{
    [SerializeField] private int scoreValue = 50;

    protected override void Interact(PlayerController player)
    {
        player.AddScore(scoreValue);
    }
}