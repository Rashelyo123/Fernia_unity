using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public enum RotationAxis {
		MouseX = 1,
		MouseY = 2
	}

	public RotationAxis axes = RotationAxis.MouseX;

	public float minimumVert = -45.0f;
	public float maximumVert = 45.0f;

	private float _rotationX = 0;

	private float rotationY;

	float x;

    public PlayerData playerData;

	//[SerializeField] private Movement movement;

	void FixedUpdate () {
			if(axes == RotationAxis.MouseX) {
				//if(gameManager.dataManager.controllerMode == ControllerMode.Windows) {
					x = Input.GetAxis("Mouse X") * playerData.sensitivyCamera;
				//} else if(controllerMode == ControllerMode.Android) {
					//x = gameManager.scriptedObject.JoystickRight.InputDir.x * playerData.sensitivyCamera;
				//}
				transform.Rotate(0, x, 0);
			} else if(axes == RotationAxis.MouseY) {
				//if(controllerMode == ControllerMode.Windows) {
					_rotationX -= Input.GetAxis("Mouse Y") * playerData.sensitivyCamera;
				//} else if(gameManager.dataManager.controllerMode == ControllerMode.Android) {
					//_rotationX -= gameManager.scriptedObject.JoystickRight.InputDir.y * gameManager.dataManager.playerData.sensitivyCamera;
				//}
				_rotationX = Mathf.Clamp (_rotationX, minimumVert, maximumVert);
				rotationY = transform.localEulerAngles.y;
				transform.localEulerAngles =new Vector3(_rotationX, rotationY, 0);
			}
	}
}
