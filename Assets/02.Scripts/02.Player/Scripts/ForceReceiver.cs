using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    private float vertiaclVelocity;

    public Vector3 Movement => Vector3.up * vertiaclVelocity;
          
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded)
        {
            vertiaclVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            vertiaclVelocity += Physics.gravity.y * Time.deltaTime;
        }
    }

    public void Jump(float jumpForce)
    {
        vertiaclVelocity += jumpForce;
    }
}
