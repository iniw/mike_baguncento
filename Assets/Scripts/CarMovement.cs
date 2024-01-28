using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

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

    public bool LeaveScene()
    {
        var outside = new Vector2(_transform.position.x + _cameraWidth, _transform.position.y);
        if (_transform.position.x >= _cameraWidth)
        {
            ResetPosition();
            if (!GameManager.Instance.canProceed) return false;
            GameManager.Instance.canProceed = false;
            GameManager.Instance.day++;
            GameManager.Instance.currentTrafficLight = TrafficLightsState.Green;
            GameManager.Instance.countdown = GameManager.Instance.greenLightDuration;
            GameManager.Instance.SaveState();
            return false;
        }
        MoveTowards(outside);
        return true;
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
