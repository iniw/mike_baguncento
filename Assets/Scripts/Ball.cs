using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var rigidBody = GetComponent<Rigidbody2D>();
        // disable gravity on creation to not fall straight down
        rigidBody.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
