using UnityEngine;

public class ParallaxLoop : MonoBehaviour
{
    [SerializeField] private float width = 20f;
    [SerializeField] private float speedFactor = 0.2f;

    private void Update()
    {
        if (GameManager.Instance == null ||
            GameManager.Instance.IsGameOver)
            return;

        transform.Translate(
            Vector3.left *
            GameManager.Instance.WorldSpeed *
            speedFactor *
            Time.deltaTime
        );

        if (transform.position.x <= -width)
        {
            transform.position +=
                Vector3.right * width * 2f;
        }
    }
}