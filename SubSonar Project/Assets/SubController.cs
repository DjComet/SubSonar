using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubController : MonoBehaviour {
    float dt;
    
    public float propellerSpeed = 5f;
    
    public float maxPitchAngle = 50f;
    public float maxZSpeed = 5f;
    public float maxXYSpeed = 2f;
    public float subAccel = 10f;
    public Vector3 subSpeed;
    public Vector2 direction;
    public float sensitivity = 0.75f;
    
    public float t = 0.5f;
    
    private Quaternion pitchedDown;
    private Quaternion pitchedUp;

    public Vector3 initialTouchPos;
    public Vector3 currentTouchPos;
    public Vector3 dragDirection;
    public Vector3 initialSubPos;
    public Vector3 targetSubPos;
    Vector3 totalSubPos = Vector3.zero;
    public List<Transform> propellers;
    public List<Transform> fins;

    public bool hasEnergy = true;
    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        Debug.Log(transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "Propeller")
            {
                propellers.Add(transform.GetChild(i).transform);
            }
            if(transform.GetChild(i).tag == "Fin")
            {
                fins.Add(transform.GetChild(i).transform);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        dt = Time.deltaTime;
        move();
        
	}
    
    void move()
    {        
        if(hasEnergy)
        {
            touchDrag();
            
            /*
            direction.x = Mathf.Sign(targetSubPos.x - transform.position.x);
            if (targetSubPos.x - transform.position.x == 0) direction.x = 0;
            direction.y = Mathf.Sign(targetSubPos.y - transform.position.y);
            if (targetSubPos.y - transform.position.y == 0) direction.y = 0;

            float xTargetSpeed = maxXYSpeed * direction.x;
            float yTargetSpeed = maxXYSpeed * direction.y;
            float xOffsetSpeed = xTargetSpeed - subSpeed.x;
            float yOffsetSpeed = yTargetSpeed - subSpeed.y;
            xOffsetSpeed = Mathf.Clamp(xOffsetSpeed, -subAccel * dt, subAccel * dt);
            yOffsetSpeed = Mathf.Clamp(yOffsetSpeed, -subAccel * dt, subAccel * dt);
            subSpeed.x += xOffsetSpeed;
            subSpeed.y += yOffsetSpeed;*/


            totalSubPos.x = Mathf.MoveTowards(transform.position.x, targetSubPos.x, 0.5f);
            totalSubPos.y = Mathf.MoveTowards(transform.position.y, targetSubPos.y, 0.5f);

            totalSubPos.x = targetSubPos.x;
            totalSubPos.y = targetSubPos.y;
            totalSubPos.z = transform.position.z + maxZSpeed * dt;
            transform.position = totalSubPos;
        }
    }

    


    void rotate()
    {
    }

    void touchDrag()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            initialTouchPos = Input.mousePosition;
            Debug.Log("First ScreenMousePosition: " + initialTouchPos);
            initialTouchPos = GetWorldPositionOnPlane(initialTouchPos, transform.position.z);//The mouse projection plane has to be created on the subs z position
            Debug.Log("First WorldMousePosition: " + initialTouchPos);
            initialSubPos = transform.position;
        }
        else if (Input.GetButton("Fire1"))
        {
            currentTouchPos = Input.mousePosition;
            Debug.Log("Current ScreenMousePosition: " + currentTouchPos);
            currentTouchPos = GetWorldPositionOnPlane(currentTouchPos, transform.position.z);
            Debug.Log("Current WorldMousePosition: " + currentTouchPos + "Distance Cam-sub : " + Vector3.Distance(Camera.main.transform.position, transform.position));
            dragDirection = currentTouchPos - initialTouchPos;
            Debug.Log("DragDirection = " + dragDirection);

            targetSubPos = initialSubPos + dragDirection * sensitivity; //Thanks to the sensitivity we can set how much distance the sub actually travels
                                                                        //in relation to the distance traveled by the mouse/touch input
            t = Vector3.Distance(transform.position, targetSubPos) / Vector3.Distance(initialSubPos, targetSubPos);
        }
        else t = 0;

    }

    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
}


