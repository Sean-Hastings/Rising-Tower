using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;
    public Vector3 offset;
    public float zoomSpeed;

    public PlayerInput input_controller;

	// Use this for initialization
	void Start () {
        input_controller.RegisterHandler(PlayerInput.Inputs.MouseWheel, UpdateCameraZoom);
    }
	
    protected void UpdateCameraZoom()
    {
        /**
        if (input_controller.MouseWheel != 0)
        {
            Vector3 offsetOffset = transform.forward * input_controller.MouseWheel * zoomSpeed;

            if (offset.y + offsetOffset.y <= closest_distance)
            {
                offsetOffset *= (closest_distance - offset.y) / offsetOffset.y;
            }
            else if (offset.y + offsetOffset.y >= farthest_distance)
            {
                offsetOffset *= (farthest_distance - offset.y) / offsetOffset.y;
            }

            offset += offsetOffset;
            offset.x = 0;
        }
        **/
        transform.position = target.position + offset;
        transform.LookAt(target);
	}
}
