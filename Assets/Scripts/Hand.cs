using UnityEngine;

public enum HandSide
{
    Left,
    Right
}

public class Hand : MonoBehaviour
{
    public HandSide side = HandSide.Left;
    public float speed = 9.0f;
    public float maxSpeed = 10.0f;
    public float accelerationRate = 2.0f;
    public float decelerationRate = 2.0f;
    public float acceleration = MinSpeed;

    private Transform _transform;
    private const float MinSpeed = 5.0f;
    private DynamicInput _input;

    private float _moveHorizontal = 0.0f;
    private float _moveVertical = 0.0f;

    private Camera _camera;
    private float _screenWidth;
    private float _screenHeight;
    
    private void Awake()
    {
        _camera = Camera.main;
        _screenHeight = Screen.height;
        _screenWidth = Screen.width;
    }

    private void Start()
    {
        _transform = GetComponent<Transform>();

        _input = new DynamicInput();
        _input.Enable();

        var horizontalMovement = side == HandSide.Left ? _input.Actions.LeftHandHorizontalMovement : _input.Actions.RightHandHorizontalMovement;
        var verticalMovement = side == HandSide.Left ? _input.Actions.LeftHandVerticalMovement : _input.Actions.RightHandVerticalMovement;

        horizontalMovement.performed += ctx => _moveHorizontal = ctx.ReadValue<float>();
        horizontalMovement.canceled += ctx => _moveHorizontal = 0.0f;
        verticalMovement.performed += ctx => _moveVertical = ctx.ReadValue<float>();
        verticalMovement.canceled += ctx => _moveVertical = 0.0f;
    }

    private void FixedUpdate()
    {
        var movement = new Vector3(_moveHorizontal, _moveVertical, 0.0f);

        if (movement != Vector3.zero)
        {
            acceleration += accelerationRate * Time.deltaTime;
            acceleration = Mathf.Min(acceleration, maxSpeed);
        }
        else
        {
            acceleration -= decelerationRate * Time.deltaTime;
            acceleration = Mathf.Max(acceleration, MinSpeed);
        }

        var newPosition = _transform.position + movement * speed * acceleration * Time.deltaTime;
        
        Vector3 minBounds, maxBounds;
        if (side == HandSide.Left)
        {
            minBounds = _camera.ScreenToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
            maxBounds = _camera.ScreenToWorldPoint(new Vector3(_screenWidth / 2, _screenHeight / 2, _camera.farClipPlane));
        }
        else
        {
            minBounds = _camera.ScreenToWorldPoint(new Vector3(_screenWidth / 2, 0, _camera.nearClipPlane));
            maxBounds = _camera.ScreenToWorldPoint(new Vector3(_screenWidth, _screenHeight / 2, _camera.farClipPlane));
        }

        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x - 2);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        _transform.position = newPosition;
    }
}
