using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 5.0f;

    public DynamicInput input;
    private float _moveHorizontal = 0.0f;
    private float _moveVertical = 0.0f;
    private float _boundWidth = 0.0f;
    private float _boundHeight = 0.0f;

    private float _offsetW = 0.0f;
    private float _offsetH = 0.0f;

    public static Player Instance;
    private static readonly int Dir = Animator.StringToHash("dir");

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        var spriteRenderer = GetComponent<SpriteRenderer>();

        var spriteSize = spriteRenderer.sprite.bounds.size;

        input = new DynamicInput();

        input.Actions.LeftHandHorizontalMovement.performed += ctx => _moveHorizontal = ctx.ReadValue<float>();
        input.Actions.LeftHandHorizontalMovement.canceled += ctx => _moveHorizontal = 0.0f;

        input.Actions.LeftHandVerticalMovement.performed += ctx => _moveVertical = ctx.ReadValue<float>();
        input.Actions.LeftHandVerticalMovement.canceled += ctx => _moveVertical = 0.0f;

        var mainCamera = Camera.main;
        var cameraSize = mainCamera.orthographicSize;
        _boundWidth = (cameraSize * mainCamera.aspect) - spriteSize.x / 2;
        _boundHeight = cameraSize - spriteSize.y / 2;

        // Convert pixel offsets to world units
        _offsetW = 384f / (Screen.height / (2 * cameraSize));
        _offsetH = 80f / (Screen.height / (2 * cameraSize));

        LoadState(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        SceneManager.sceneLoaded += LoadState;
    }

    private void LoadState(Scene scene, LoadSceneMode mode)
    {
        if (Instance == null || this == null)
            return;

        if (scene.name == "TopDown")
        {
            input.Enable();
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            input.Disable();
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void Update()
    {
        var movement = new Vector3(_moveHorizontal, _moveVertical, 0.0f);

        var newPosition = transform.position + movement * (speed * Time.deltaTime);

        if (newPosition.x >= _boundWidth - _offsetW)
            newPosition.x = _boundWidth - _offsetW;
        else if (newPosition.x <= -_boundWidth)
            newPosition.x = -_boundWidth;

        if (newPosition.y >= _boundHeight - _offsetH)
            newPosition.y = _boundHeight - _offsetH;
        else if (newPosition.y <= -_boundHeight)
            newPosition.y = -_boundHeight;

        var animController = GetComponent<Animator>();
        if (newPosition.x > transform.position.x)
            animController.SetInteger(Dir, 3);
        else if (newPosition.x < transform.position.x)
            animController.SetInteger(Dir, 2);
        else if (newPosition.y > transform.position.y)
            animController.SetInteger(Dir, 1);
        else if (newPosition.y < transform.position.y)
            animController.SetInteger(Dir, 4);
        else
            animController.SetInteger(Dir, 0);

        transform.position = newPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Instance == null || this == null)
            return;

        if (!other.CompareTag("Cars"))
            return;

        Debug.Log("Collided");
        if (GameManager.Instance.currentTrafficLight != TrafficLightsState.Red)
        {
            GameManager.Instance.Destroy();
            SceneManager.LoadScene("Death");
        }
        else
        {
            if (_moveHorizontal != 0)
                _moveHorizontal = 0;

            if (_moveVertical != 0)
                _moveVertical = 0;
        }
    }
}
