using UnityEngine;

public class AngleSelector : MonoBehaviour
{
    private float _yRotation = 0f;

    private const float MAX_ANGLE = 220f;
    private const float TIME_TO_FINISH_ROTATION = 2f;

    public Hand hand;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        transform.position = hand.transform.position;
    }

    void UpdateAngle()
    {
        float triangleWave = Mathf.Abs((_yRotation++ % MAX_ANGLE) - MAX_ANGLE / 2) - 10f;
        transform.eulerAngles = new Vector3(0, 0, hand.side == HandSide.Left ? triangleWave : -triangleWave);
    }

    void OnEnable()
    {
        InvokeRepeating(nameof(UpdateAngle), 0, TIME_TO_FINISH_ROTATION / MAX_ANGLE);
    }

    void OnDisable()
    {
        CancelInvoke(nameof(UpdateAngle));
    }
}
