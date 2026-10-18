using UnityEngine;

public class GroundLoop : MonoBehaviour
{
    [SerializeField] private float groundWidth = 20f;

    private void Update()
    {
        if (GameManager.Instance == null ||
            GameManager.Instance.IsGameOver)
            return;

        transform.Translate(
            Vector3.left *
            GameManager.Instance.WorldSpeed *
            Time.deltaTime
        );

        if (transform.position.x <= -groundWidth)
        {
            transform.position +=
                Vector3.right * groundWidth * 2f;
        }
    }
}