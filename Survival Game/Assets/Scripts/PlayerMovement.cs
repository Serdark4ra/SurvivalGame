using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
     public CharacterController controller;
 
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;
 
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Animator animator;

 
    Vector3 velocity;
 
    bool isGrounded;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }
 
    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Blend", speed);
        //checking if we hit the ground to reset our falling velocity, otherwise we will fall faster the next time
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
 
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            animator.SetBool("isJumping", false);
        }
 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
 
        //right is the red Axis, foward is the blue axis
        Vector3 move = transform.right * x + transform.forward * z;
 
        
 
        //check if the player is on the ground so he can jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //the equation for jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("isJumping", true);
        }

        if (move != Vector3.zero)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                Debug.Log("Running");
                speed = 15f;
                
            }
            else
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", true);
                
                Debug.Log("Walking");
                speed = 6f;
            }

            controller.Move(move * speed * Time.deltaTime);
            
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
            Debug.Log("Not Running");
        }
 
        velocity.y += gravity * Time.deltaTime;
 
        controller.Move(velocity * Time.deltaTime);
    }
}
