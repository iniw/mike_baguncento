using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Killing ball");
            GameManager.Instance.playableBalls--;
            collider.gameObject.SetActive(false);

            if (GameManager.Instance.spawnedBalls < GameManager.Instance.balls.Length)
            {
                GameManager.Instance.SpawnBall();
            }
            else
            {
                if (GameManager.Instance.playableBalls == 0)
                {
                    GameManager.Instance.canProceed = true;
                    SceneManager.LoadScene("TopDown");
                }
            }
        }
    }
}
