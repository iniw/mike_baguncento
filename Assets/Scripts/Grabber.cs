using UnityEngine;

public class Grabber : MonoBehaviour
{

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
            _ballInArea.transform.position = hand.transform.position;
    }

    private void GrabBall()
    {
        Vector2 pos = _ballInArea.transform.position;
        Vector2 ourPos = transform.position;

        var delta = pos - ourPos;
        var score = Mathf.RoundToInt(delta.magnitude * GameManager.Instance.ballsThrowMultiplier) * GameManager.Instance.playableBalls;

        GameManager.Instance.score += score;

        _ballInArea.transform.position = hand.transform.position;
        _ballInArea.constraints = RigidbodyConstraints2D.FreezeAll;

        var ballIdx = int.Parse(_ballInArea.gameObject.name[^1].ToString());
        gameObject.GetComponentInParent<SpriteRenderer>().sprite = GameManager.Instance.handSprites[ballIdx];
        _ballInArea.GetComponent<SpriteRenderer>().enabled = false;

        angleSelector.gameObject.SetActive(true);
    }


    private void ThrowBall()
    {
        if (!_ballInArea)
        {
            _state = State.Idle;
            return;
        }

        if (GameManager.Instance.spawnedBalls < GameManager.Instance.balls.Length)
            GameManager.Instance.SpawnBall();

        _ballInArea.constraints = RigidbodyConstraints2D.None;

        _ballInArea.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponentInParent<SpriteRenderer>().sprite = GameManager.Instance.handSprites[^1];

        var vector = (hand.side == HandSide.Left ? angleSelector.transform.right : -angleSelector.transform.right) * FORCE_MAGNITUDE;
        _ballInArea.AddForce(vector, ForceMode2D.Impulse);
        angleSelector.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (_state == State.Idle && collider.gameObject.CompareTag("Ball"))
        {
            _state = State.BallInArea;
            _ballInArea = collider.GetComponent<Rigidbody2D>();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (_state == State.BallInArea && _ballInArea.gameObject == collider.gameObject)
        {
            _state = State.Idle;
            _ballInArea = null;
        }
    }
}
