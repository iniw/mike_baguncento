using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 5.0f;

    private Transform _transform;
    private DynamicInput _input;
    private float _moveHorizontal = 0.0f;
    private float _moveVertical = 0.0f;
    private float _boundWidth = 0.0f;
    private float _boundHeight = 0.0f;

    private float _offsetW = 0.0f;
    private float _offsetH = 0.0f;
    

    private void Start()
    {
        _transform = GetComponent<Transform>();
        var spriteRenderer = GetComponent<SpriteRenderer>();

        var spriteSize = spriteRenderer.sprite.bounds.size;

        _input = new DynamicInput();
        _input.Enable();

        _input.Actions.LeftHandHorizontalMovement.performed += ctx => _moveHorizontal = ctx.ReadValue<float>();
        _input.Actions.LeftHandHorizontalMovement.canceled += ctx => _moveHorizontal = 0.0f;

        _input.Actions.LeftHandVerticalMovement.performed += ctx => _moveVertical = ctx.ReadValue<float>();
        _input.Actions.LeftHandVerticalMovement.canceled += ctx => _moveVertical = 0.0f;

        var mainCamera = Camera.main;
        var cameraSize = mainCamera.orthographicSize;
        _boundWidth = (cameraSize * mainCamera.aspect) - spriteSize.x / 2;
        _boundHeight = cameraSize - spriteSize.y / 2;

        // Convert pixel offsets to world units
        _offsetW = 384f / (Screen.height / (2 * cameraSize));
        _offsetH = 80f / (Screen.height / (2 * cameraSize));
    }

    private void Update()
    {
        var movement = new Vector3(_moveHorizontal, _moveVertical, 0.0f);

        var newPosition = _transform.position + movement * (speed * Time.deltaTime);

        if (newPosition.x >= _boundWidth - _offsetW)
        {
            newPosition.x = _boundWidth - _offsetW;
        }
        else if (newPosition.x <= -_boundWidth)
        {
            newPosition.x = -_boundWidth;
        }

        if (newPosition.y >= _boundHeight - _offsetH)
        {
            newPosition.y = _boundHeight - _offsetH;
        }
        else if (newPosition.y <= -_boundHeight)
        {
            newPosition.y = -_boundHeight;
        }
        
        var hits = Physics2D.OverlapCircleAll(newPosition, 0.5f);
        var dead = false;
        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Cars")) continue;
            if (GameManager.Instance.currentTrafficLight == TrafficLightsState.Red) continue;
            dead = true;
            SceneManager.LoadScene("GameOver");
        }

        if (dead) return;
        _transform.position = newPosition;
    }
}