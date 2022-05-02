using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;
    public float sprintSpeed = 20f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public DynamicChangeManager dcm;

    Vector3 velocity;
    bool isGrounded;

    public GameObject deathParticles;

    private bool disableFeature = true;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (move == Vector3.zero && !disableFeature)
        {
            dcm.emFreeze();
        }else if (Input.GetKey(KeyCode.LeftShift) && move != Vector3.zero)
        {
            controller.Move(move * sprintSpeed * Time.deltaTime);
            dcm.emSprint();
        }
        else
        {
            controller.Move(move * speed * Time.deltaTime);
            dcm.emResetSpeed();
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity*Time.deltaTime);

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
        }
    }
}
