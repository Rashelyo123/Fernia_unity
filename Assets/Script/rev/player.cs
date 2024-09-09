using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerState{
    normal_,
    running_,
    grab_,
    pick_up_
};

public class player : MonoBehaviour
{
    public const float walkingAnimation = 0.5f;
    public const float runningAnimation = 1f;
    public float runningAcceleration = 2f;
    public float walkingAcceleration = 1f;
    public float deacceleration = 0.8f;
    public float climbForce = 5f;
    public LayerMask wallLayer, movableLayer;
    public BaseMovement baseMovement;
    PlayerState playerState;

    

    public bool IsNearWall(){
        RaycastHit hit;
        float rayDistance = 1f;
        Vector3 rayDirection = transform.forward;

        if (Physics.Raycast(transform.position, rayDirection, out hit, rayDistance, wallLayer)){
            return true;
        }
        return false;
    }
    GameObject target;

    public GameObject HitMovableObject(){
        GameObject temp = null;
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f, movableLayer)){
            // Jika mendeteksi objek yang bisa dipindahkan
            temp = hit.collider.gameObject;
            if(target == null)  target = temp;
            temp.transform.parent = transform;
        }else{
            temp = null;
        }
        return temp;
    }

    void Start (){
        baseMovement = GetComponent<BaseMovement>();
        playerState = PlayerState.normal_;
    }
    void Update(){
        Debug.Log("playerstate : " + playerState);
        stateManager();
        switch(playerState){
            case PlayerState.normal_: 
                move(walkingAcceleration, walkingAnimation); 
                break;
            case PlayerState.running_: 
                move(runningAcceleration, runningAnimation); 
                if(Input.GetKeyUp(KeyCode.LeftShift)){
                    playerState = PlayerState.normal_;
                }
                break;
            case PlayerState.grab_: 
                AttachedWithObject();
                if(Input.GetKeyUp(KeyCode.X)){
                    playerState = PlayerState.normal_;
                    target.transform.parent = null;
                }
                break;
        }
    }

    

    public void move(float acceleration, float animation){
        Vector3 customInput = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow)){
            customInput.z = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow)){
            customInput.z = -1;
        }
        if (Input.GetKey(KeyCode.LeftArrow)){
            customInput.x = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow)){
            customInput.x = 1;
        }

        if(Input.GetKey(KeyCode.Space) && IsNearWall()){
            baseMovement.climbing(climbForce);
        }else if(Input.GetKeyDown(KeyCode.Space)){
            customInput.y = 1;
        }
        baseMovement.MoveCharacter(customInput, acceleration, animation);
    }

    public void stateManager(){
        if(Input.GetKey(KeyCode.LeftShift)){
            playerState = PlayerState.running_;
        }else if(Input.GetKey(KeyCode.X) && HitMovableObject()!=null){
            playerState = PlayerState.grab_;
        }else{
            playerState = PlayerState.normal_;
        }
    }

    public void PositionInit(){

    }
    Vector3? initPose = null;
    
    public void AttachedWithObject(){
        if (initPose == null){
            initPose = target.transform.position;
        }
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        float movementSpeed = targetRb.mass * deacceleration;
        
        Vector3 customInput = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow)){
            customInput.z = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow)){
            customInput.z = -1;
        }
        if (Input.GetKey(KeyCode.LeftArrow)){
            customInput.x = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow)){
            customInput.x = 1;
        }

        if (initPose.HasValue){
            target.transform.position = initPose.Value; 
        }
       baseMovement.PushPullObject(customInput, movementSpeed);
    }
}
