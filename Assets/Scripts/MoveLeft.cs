using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private float destroyX = -12f;

    private void Update()
    {
        if (GameManager.Instance == null ||
            GameManager.Instance.IsGameOver)
            return;

        transform.Translate(
            Vector3.left *
            GameManager.Instance.WorldSpeed *
            speedMultiplier *
            Time.deltaTime
        );

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}