using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameTrigger : MonoBehaviour
{
    private DynamicInput _input;

    private void Start()
    {
        _input = new DynamicInput();
        _input.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _input.Actions.TriggerMinigame.performed += ctx =>
        {
            SceneManager.LoadScene("Minigame");
        };
    }
}
