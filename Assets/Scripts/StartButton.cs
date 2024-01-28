using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private DynamicInput _input;
    // Start is called before the first frame update
    void Start()
    {
        _input = new DynamicInput();
        _input.Enable();

        GetComponent<Button>().onClick.AddListener(() =>
        {
            _input.Disable();
            SceneManager.LoadScene("TopDown");
        });

        _input.Actions.StartGame.performed += ctx =>
        {
            _input.Disable();
            SceneManager.LoadScene("TopDown");
        };

    }
}
