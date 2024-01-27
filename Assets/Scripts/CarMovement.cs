using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private Transform _transform;
    private Vector2 _originalPos;
    
    public float speed = 2.0f;
    public Vector2 finalPosition = Vector2.zero;
    
    private float _speedMultiplier = 1.0f;
    private float _cameraWidth = 0.0f;
    
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _originalPos = _transform.position;
        _cameraWidth = Camera.main.aspect * Camera.main.orthographicSize * 2;
    }
    
    public void MoveToFinalPosition()
    {
        MoveTowards(finalPosition);
    }

    public void LeaveScene()
    {
        var outside = new Vector2(_transform.position.x + _cameraWidth, _transform.position.y);
        if (_transform.position.x >= _cameraWidth) ResetPosition();
        else MoveTowards(outside);
    }
    
    private void MoveTowards(Vector2 target)
    {
        var position = _transform.position;
        var newPosition = Vector2.MoveTowards(position, target, speed * Time.deltaTime * _speedMultiplier);
        _transform.position = newPosition;
    }
    
    private void ResetPosition()
    {
        _transform.position = _originalPos;
    }
    
    public void SetspeedMultiplier(float multiplier)
    {
        _speedMultiplier = multiplier;
    }
}
