using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Transform player;

    public float horizontalOffset = 0f;
    public float verticalOffset = 0f;
    public float depthOffset = 0f;
	// Use this for initialization
	void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        horizontalOffset = transform.position.z;
        verticalOffset = transform.position.y;
        depthOffset = transform.position.x;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = player.position + new Vector3(depthOffset, verticalOffset, horizontalOffset);
	}
}
