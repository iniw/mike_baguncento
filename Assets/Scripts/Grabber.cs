using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Grabber : MonoBehaviour
{
    static private int _spawnedBalls = 0;
    static private Rigidbody2D[] _balls;

    private Rigidbody2D _ballInArea;
    private DynamicInput _input;

    public Hand hand;

    public AngleSelector angleSelector;

    private const float FORCE_MAGNITUDE = 30f;

    enum State
    {
        Idle,
        BallInArea,
        BallGrabbed
    }

    private State _state = State.Idle;

    // Start is called before the first frame update
    void Start()
    {
        if (_spawnedBalls == 0)
        {
            _balls = GameObject.FindGameObjectsWithTag("Ball").Select(ball => ball.GetComponent<Rigidbody2D>()).ToArray();
            foreach (Rigidbody2D ball in _balls)
            {
                ball.gameObject.SetActive(false);
            }

            _balls[0].gameObject.SetActive(true);
            _spawnedBalls++;
        }

        _input = new DynamicInput();
        _input.Enable();

        var grabAction = hand.side == HandSide.Left ? _input.Actions.LeftHandAction : _input.Actions.RightHandAction;
        grabAction.performed += _ =>
        {
            switch (_state)
            {
                case State.Idle:
                    break;
                case State.BallInArea:
                    GrabBall();
                    _state = State.BallGrabbed;
                    break;
                case State.BallGrabbed:
                    ThrowBall();
                    _state = State.Idle;
                    _ballInArea = null;
                    break;
            }
        };
    }

    void LateUpdate()
    {
        if (_state == State.BallGrabbed)
        {
            _ballInArea.transform.position = hand.transform.position;
        }
    }

    private void GrabBall()
    {
        _ballInArea.transform.position = hand.transform.position;
        _ballInArea.constraints = RigidbodyConstraints2D.FreezeAll;
        angleSelector.gameObject.SetActive(true);
    }


    private void ThrowBall()
    {
        if (_spawnedBalls < _balls.Length)
        {
            _balls[_spawnedBalls].gameObject.SetActive(true);
            _spawnedBalls++;
        }
        _ballInArea.constraints = RigidbodyConstraints2D.None;

        var vector = (hand.side == HandSide.Left ? angleSelector.transform.right : -angleSelector.transform.right) * FORCE_MAGNITUDE;
        _ballInArea.AddForce(vector, ForceMode2D.Impulse);
        angleSelector.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (_state != State.Idle || !collider.gameObject.CompareTag("Ball"))
            return;

        Debug.Log("Ball entered");

        _state = State.BallInArea;
        _ballInArea = collider.GetComponent<Rigidbody2D>();
    }
}
