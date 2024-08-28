using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveMode {
    run, crouch, walk
}

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(MouseLook))]
public class Movement : MonoBehaviour
{
    //public PlayerData playerData;
    [SerializeField] private Transform subCamera;
    public PlayerData playerData;
    private CharacterController cc;
    public Vector3 crouchPositionCam, normalPositionCam;
    private float speed;
    //public Animator animator;
    float deltaX, deltaZ, char_back, abovechar, char_front, char_left, char_right, bottomchar;
    RaycastHit hit;
    Vector3 movement;
    float heightcc, centercc;
    Vector3 PositionCam;
    bool isCrouch, isRun, Crouching;
    public MoveMode moveMode;

    private Animator animator;
    
    void Start (){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponentInChildren<Animator>();
        cc = GetComponent<CharacterController>();
    }
    /**
    private void AnimationEvent (float m_deltaX, float m_deltaZ) {
        animator.SetBool("isCrouch", isCrouch || CheckAbove());
        animator.SetFloat("magnitude", cc.velocity.magnitude);
        animator.SetFloat("InputAxisZ", m_deltaZ);
        animator.SetFloat("InputAxisX", m_deltaX);
    }**/
    private void FixedUpdate () {
        UpdatePositionEdgeChar();
        deltaX = Input.GetAxis("Horizontal") * speed;
        deltaZ = Input.GetAxis("Vertical") * speed;
        AnimationFPS();
            
        /**if(controllerMode == ControllerMode.Windows) {
            
            
        } else if(controllerMode == ControllerMode.Android) {
            deltaX = gameManager.scriptedObject.JoystickLeft.InputDir.x * playerData.speed;
            deltaZ = gameManager.scriptedObject.JoystickLeft.InputDir.y * playerData.speed;
        }**/
        movement = new Vector3 (deltaX, 0, deltaZ);
		movement = Vector3.ClampMagnitude (movement, speed);
        movement.y = -playerData.gravity;
		movement *= Time.deltaTime;
		movement = transform.TransformDirection (movement);
        cc.Move (movement);

        
    }

    private void AnimationFPS () {
        float charMagnitude = Mathf.Clamp(Mathf.Abs(cc.velocity.magnitude), 0, 1);
        animator.SetFloat("moveX", deltaX * charMagnitude);
        animator.SetFloat("moveZ", deltaZ * charMagnitude);
        if(isCrouch)
            Crouching = true;
        else if(!isCrouch) {
            if(CheckAbove()) {
                Crouching = true;
            } else {
                Crouching = false;
            }
        }
            
        animator.SetBool("Crouch", Crouching);
        //animator.SetFloat("magnitude", charMagnitude);
    }

    private void Update () {
        //velocityMagnitude = cc.velocity.magnitude;
        //if(controllerMode == ControllerMode.Windows) {
            isRun = Input.GetKey(KeyCode.LeftShift);
        //}
        if(isRun && deltaZ > (1) && !CheckAbove()) {
            //clampAnimVal = Mathf.Lerp(clampAnimVal, playerData.runSpeed, 10 * Time.deltaTime);
            isCrouch = false;
            moveMode = MoveMode.run;
        } else if((isCrouch || CheckAbove()) && cc.isGrounded) {
            moveMode = MoveMode.crouch;
            //clampAnimVal = Mathf.Lerp(clampAnimVal, crouchSpeed, 10 * Time.deltaTime);
        } else {
            moveMode = MoveMode.walk;
        } 
        

        //groundCheck();
        //AnimationEvent(deltaX, deltaZ);
        MovementMode();
        CrouchingEvent();
        //PeekEvent();
    }

    /**private void PeekEvent () {
        bool checkRightTop = Physics.Raycast(checkSideright.position, checkSideright.right, out hit,  1f);
        bool checkLeftTop = Physics.Raycast(checkSideleft.position,  -checkSideleft.right, out hit,  1f);
        canPeekLeft = !checkLeftTop;
        canPeekRight = !checkRightTop;
        if(Input.GetKey(KeyCode.Q) && canPeekLeft && !isRun) {
            if(subCamera.transform.localPosition.x != -0.65f && subCamera.transform.localRotation.z != 3.5f) {
                PeekChanging(-0.65f, 3.5f);
                playerData.peek = true;
            }
        } else if(Input.GetKey(KeyCode.E) && canPeekRight && !isRun){
            if(subCamera.transform.localPosition.x != 0.65f && subCamera.transform.localRotation.z != -3.5f) {
                PeekChanging(0.65f, -3.5f);
                playerData.peek = true;
            }
        } else {
            if(subCamera.transform.localPosition.x != 0 && subCamera.transform.localRotation.z != 0) {
                PeekChanging(0, 0);
                playerData.peek = false;
            }
        }
    }

    private void PeekChanging (float peekPos, float peekRot) {
        subCamera.transform.localPosition = Vector3.Lerp(subCamera.transform.localPosition, new Vector3(peekPos, subCamera.transform.localPosition.y, subCamera.transform.localPosition.z), 8 * Time.deltaTime);
        subCamera.transform.localRotation = Quaternion.Lerp(subCamera.transform.localRotation, Quaternion.Euler(subCamera.transform.localRotation.x, subCamera.transform.localRotation.y, peekRot), 8 * Time.deltaTime);
    }**/
    
    private void CrouchingEvent () {
        //previous_y_cc = cc.transform.position.y - cc.height / 2 - cc.skinWidth;
        //if(controllerMode == ControllerMode.Windows) {
            isCrouch = Input.GetKey(KeyCode.C) && isRun == false;
        //}
        if((isCrouch && !CheckAbove())){
            if(PositionCam != crouchPositionCam)
                PositionCam = crouchPositionCam;
            if(heightcc != playerData.crouchHeight)
                heightcc = playerData.crouchHeight;
        } else if(CheckAbove()) {
            if(PositionCam != crouchPositionCam)
                PositionCam = crouchPositionCam;
            if(heightcc != playerData.crouchHeight)
                heightcc = playerData.crouchHeight;
        } else {
            if(PositionCam != normalPositionCam)
                PositionCam = normalPositionCam;
            if(heightcc != playerData.normalHeight)
                heightcc = playerData.normalHeight;
        }
        if(subCamera.localPosition != PositionCam) 
            subCamera.localPosition = Vector3.Lerp(subCamera.localPosition, PositionCam, playerData.crouchWeightSpeed * Time.deltaTime);
        if(cc.height != heightcc)
            cc.height = Mathf.Lerp(cc.height, heightcc, playerData.crouchWeightSpeed * Time.deltaTime);
        if(cc.center.y != centercc) {
            cc.center = Vector3.Lerp(cc.center, new Vector3(cc.center.x, heightcc * 0.5f, cc.center.z), playerData.crouchWeightSpeed * Time.deltaTime);
        }
    }
    private void MovementMode (){
        switch(moveMode) {
            case MoveMode.walk : 
                if(speed != playerData.walkSpeed)
                    speed = Mathf.Lerp(speed, playerData.walkSpeed, 7f * Time.deltaTime);
            break;
            case MoveMode.run : 
                if(speed != playerData.runSpeed)
                    speed = Mathf.Lerp(speed, playerData.runSpeed, 5f * Time.deltaTime);
            break;
            case MoveMode.crouch : 
                //heightCC = crouchHeight;
                //changeHeightCrouch(heightCC, -0.6f);
                if(speed != playerData.crouchSpeed)
                    speed = Mathf.Lerp(speed, playerData.crouchSpeed, 10f * Time.deltaTime);
            break;
        }
    }

    private bool CheckAbove () {
        bool checkFrontTop = Physics.Raycast(new Vector3(cc.transform.position.x, abovechar, char_front), Vector3.up, out hit, 1f, playerData.crouchLayer);
        bool checkBottomTop = Physics.Raycast(new Vector3(cc.transform.position.x, abovechar, char_back), Vector3.up, out hit, 1f, playerData.crouchLayer);
        bool checkRightTop = Physics.Raycast(new Vector3(char_right, abovechar, cc.transform.position.z), Vector3.up, out hit,  1f, playerData.crouchLayer);
        bool checkLeftTop = Physics.Raycast(new Vector3(char_left, abovechar, cc.transform.position.z), Vector3.up, out hit,  1f, playerData.crouchLayer);
        if(checkFrontTop || checkBottomTop || checkRightTop || checkLeftTop) {
            return true;
        } else {
            return false;
        }
        
    }
    /**private void groundCheck () {
        if(cc.isGrounded) {
            Collider[] checkRay = Physics.OverlapSphere(new Vector3(cc.transform.position.x, bottomchar, cc.transform.position.z), 0.2f, playerData.WhatIsGround); 
            for (int i = 0; i < checkRay.Length; i++)
            {
                if(checkRay[i]) {
                   // Debug.Log("Check is Aktif");
                    //GroundLayer gl = checkRay[i].transform.GetComponent<GroundLayer>();
                    if(gl != null)
                        Debug.Log("sound");
                        //footstep.WhatIsGround = gl.whatGround.ToString();
                    } else {
                } 
            }
        }
    }**/

    private void UpdatePositionEdgeChar () {
        abovechar = cc.bounds.center.y + cc.bounds.extents.y;
        char_back = cc.bounds.center.z - cc.bounds.extents.z;
        char_front = cc.bounds.center.z + cc.bounds.extents.z;
        char_left = cc.bounds.center.x - cc.bounds.extents.x;
        char_right = cc.bounds.center.x + cc.bounds.extents.x;
        bottomchar = cc.bounds.center.y - cc.bounds.extents.y;
    }

    void OnDrawGizmos () {
        Gizmos.color = Color.red;
        if(cc != null)
            Gizmos.DrawWireSphere(new Vector3(cc.transform.position.x, bottomchar , cc.transform.position.z), 0.2f);
        //Gizmos.DrawRay(checkSideleft.position, -checkSideleft.right * 1f);
        //Gizmos.DrawRay(checkSideright.position, checkSideright.right * 1f);
    }

    public void EventButton (string act) {
        if(act == "Run") {
            isRun = !isRun;
        } else if(act == "Crouch") {
            isCrouch = !isCrouch;
        }
    }
  
}
