using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New All Data", menuName = "Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float walkSpeed, runSpeed, crouchSpeed, crouchHeight, normalHeight, gravity, crouchWeightSpeed;
    public LayerMask crouchLayer, WhatIsGround;
    public float sensitivyCamera;
}
