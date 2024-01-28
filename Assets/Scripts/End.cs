using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    private DynamicInput _input;

    // Start is called before the first frame update
    void Start()
    {
        _input = new DynamicInput();
        _input.Enable();

        if (GameManager.Instance != null)
            GameManager.Instance.Destroy();

        _input.Actions.StartGame.performed += ctx =>
        {
            _input.Disable();
            SceneManager.LoadScene("Initial");
        };
    }
}
