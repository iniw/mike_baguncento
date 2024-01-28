using UnityEngine;

public class GoldBag : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.money += GameManager.Instance.score;
            GameManager.Instance.score = 0;

            gameObject.SetActive(false);
        }
    }
}
