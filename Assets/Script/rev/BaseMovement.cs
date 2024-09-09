using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    public float speed = 1f;  // Kecepatan karakter
    public float jumpForce = 5f;  // Kekuatan lompatan
    public Transform groundCheck;  // Objek untuk mengecek apakah karakter menyentuh tanah
    public float groundCheckRadius = 0.2f;  // Radius pengecekan tanah
    public LayerMask groundLayer;  // Layer yang dianggap tanah

    Animator animator;
    Rigidbody rb;

    public bool isGrounded(){
        bool grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        return grounded;
    }

    void Start(){
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update(){
        
    }

    
    public void MoveCharacter(Vector3 customInput, float speedInput, float animationInput){
        if (customInput.magnitude > 1){ 
            customInput.Normalize();
        }
        
        if (customInput != Vector3.zero){
            animator.SetFloat("Blend", animationInput);  
            
            Quaternion targetRotation = Quaternion.LookRotation(customInput);
            targetRotation.x = 0;  
            targetRotation.z = 0;  
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); 
        }else{
            animator.SetFloat("Blend", 0f); 
        }
        
        if(customInput.y != 0 && isGrounded()){
            Jump(speedInput);
        }

        float resultanSpeed = speed * speedInput;
        rb.MovePosition(rb.position + customInput * resultanSpeed * Time.fixedDeltaTime);
    }

    public void Jump(float inputForce){
        animator.SetBool("isJumping", true);
        float resultanForce = jumpForce * inputForce;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  
    }

    public void climbing(float climbingSpeed){
        Vector3 climbingDirection = new Vector3(0, climbingSpeed, 0);  // Gerakan ke atas
        rb.velocity = climbingDirection;
    }

    public void PushPullObject(Vector3 customInput, float speedInput){
        Debug.Log("movement : " + speedInput);
        if (customInput.magnitude > 1){ 
            customInput.Normalize();
        }

        // if (customInput != Vector3.zero){
        //     animator.SetFloat("Blend", 0.5f);  // Animasi berjalan
        // }else{
        //     animator.SetFloat("Blend", 0f);  // Menghentikan animasi jika tidak ada gerakan
        // }

        float resultanSpeed = speed * speedInput;
        transform.position += customInput * resultanSpeed * Time.deltaTime;
    }
}
