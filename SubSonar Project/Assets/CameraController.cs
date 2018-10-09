using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Transform player;

    public float minimumX = -30;
    public float maximumX = 30;

    public float minimumY = -30;
    public float maximumY = 30;
    float pitch = 0f;
    float yaw = 0f;
    public float horizontalOffset = 0f;
    public float verticalOffset = 0f;
    public float depthOffset = 0f;
	// Use this for initialization
	void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        horizontalOffset = transform.position.x;
        verticalOffset = transform.position.y;
        depthOffset = transform.position.z;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = new Vector3(horizontalOffset, verticalOffset, player.transform.position.z + depthOffset);
        //transform.LookAt(player.transform);
        pitch = transform.eulerAngles.x;
        yaw = transform.eulerAngles.y;
        
	}
}
