using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected abstract void Interact(PlayerController player);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerController player =
            other.GetComponent<PlayerController>();

        if (player == null)
            return;

        Interact(player);

        Destroy(gameObject);
    }
}