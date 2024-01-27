using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class AngleSelector : MonoBehaviour
{
    private Transform _transform;
    private float _yRotation = 0f;

    private const float MAX_ANGLE = 220f;

    private const float FORCE_MAGNITUDE = 10f;

    private DynamicInput _input;

    public Hand hand;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();

        InvokeRepeating(nameof(UpdateAngle), 0, 0.01f);

        _input = new DynamicInput();
        _input.Enable();

        var throwBallAction = hand.side == HandSide.Left ? _input.Actions.LeftHandThrowBall : _input.Actions.RightHandThrowBall;
        throwBallAction.performed += _ =>
        {
            var rigidBody = GameObject.Find("Ball").GetComponent<Rigidbody2D>();

            var vector = (hand.side == HandSide.Left ? transform.right : -transform.right) * FORCE_MAGNITUDE;

            Debug.Log($"Vector = {vector} | R={transform.right} | U={transform.up}");

            rigidBody.gravityScale = 1;
            rigidBody.AddForce(vector, ForceMode2D.Impulse);
        };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            var ball = GameObject.Find("Ball");
            ball.transform.position = transform.position;

            var rigidBody = ball.GetComponent<Rigidbody2D>();
            rigidBody.gravityScale = 0;
            rigidBody.velocity = Vector2.zero;
        }
    }

    void LateUpdate()
    {
        transform.position = hand.transform.position;
    }

    void UpdateAngle()
    {
        float triangleWave = Mathf.Abs((_yRotation++ % MAX_ANGLE) - MAX_ANGLE / 2) - 10f;
        _transform.eulerAngles = new Vector3(0, 0, hand.side == HandSide.Left ? triangleWave : -triangleWave);
    }
}
