using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public enum HandSide
{
    Left,
    Right
}

public class Hand : MonoBehaviour
{
    public HandSide side = HandSide.Left;
    public float speed = 2.0f;

    private Transform _transform;
    private DynamicInput _input;

    private float _moveHorizontal = 0.0f;
    private float _moveVertical = 0.0f;

    private Vector3 _originalPos;
    public float offset = 3.0f;

    private void Start()
    {
        _transform = GetComponent<Transform>();

        _originalPos = new Vector3(_transform.position.x, _transform.position.y, _transform.position.z);

        _input = new DynamicInput();
        _input.Enable();

        var horizontalMovement = side == HandSide.Left ? _input.Actions.LeftHandHorizontalMovement : _input.Actions.RightHandHorizontalMovement;
        var verticalMovement = side == HandSide.Left ? _input.Actions.LeftHandVerticalMovement : _input.Actions.RightHandVerticalMovement;

        horizontalMovement.performed += ctx => _moveHorizontal = ctx.ReadValue<float>();
        horizontalMovement.canceled += ctx => _moveHorizontal = 0.0f;
        verticalMovement.performed += ctx => _moveVertical = ctx.ReadValue<float>();
        verticalMovement.canceled += ctx => _moveVertical = 0.0f;
        
        _input.Actions.TriggerMinigame.performed += ctx =>
        {
            GameManager.Instance.canProceed = true;
            SceneManager.LoadScene("TopDown");
        };
    }

    private void FixedUpdate()
    {
        var movement = new Vector3(_moveHorizontal, _moveVertical, 0.0f);

        var newPosition = _transform.position + movement * speed * Time.deltaTime;

        if (newPosition.x >= _originalPos.x + offset || newPosition.x <= _originalPos.x - offset)
            newPosition.x = _transform.position.x;

        if (newPosition.y >= _originalPos.y + offset || newPosition.y <= _originalPos.y - offset + 2)
            newPosition.y = _transform.position.y;

        _transform.position = newPosition;
    }
}
