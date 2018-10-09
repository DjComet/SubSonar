using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubController : MonoBehaviour {

    
    public float propellerSpeed = 5f;
    
    public float maxPitchAngle = 50f;
    public float zSpeed = 5f;
    public float lateralSpeed = 2f;
    public float sensitivity = 0.5f;
    public float PitchMaxSpeed = 4f;
    public float PitchSpeed = 3f;
    public float bodyPitchSpeed = 1f;
    public float finsPitchAccel = 5f;
    public float bodyPitchAccel = 2f;
    public float t = 0.5f;
    public float t2 = 0.5f;
    private Quaternion pitchedDown;
    private Quaternion pitchedUp;

    public Vector3 initialTouchPos;
    public Vector3 currentTouchPos;
    public Vector3 dragDirection;
    public Vector3 initialSubPos;
    public Vector3 targetSubPos;

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
            Debug.Log("i: " + i);
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

        move();
        
	}
    
    void move()
    {        
        if(hasEnergy)
        {
            touchDrag();
            Vector3 totalSubPos;
            totalSubPos.x = targetSubPos.x;
            totalSubPos.y = targetSubPos.y;
            totalSubPos.z = transform.position.z + zSpeed * Time.deltaTime;
            transform.position = totalSubPos;
        }
    }

    void rotate()
    {
        pitchedDown = Quaternion.AngleAxis(maxPitchAngle, transform.right);
        pitchedUp = Quaternion.AngleAxis(-maxPitchAngle, transform.right);

        //PitchSpeed = accelerate(PitchMaxSpeed, finsPitchAccel, PitchSpeed);
        //bodyPitchSpeed = accelerate(PitchMaxSpeed, bodyPitchAccel, bodyPitchSpeed);
        if (Input.GetButton("Fire1"))
        {
            t += PitchSpeed * Time.deltaTime;
            t2 += bodyPitchSpeed * Time.deltaTime;
        }
        else if(Input.GetButton("Fire2"))
        {
            t -= PitchSpeed * Time.deltaTime;
            t2 -= bodyPitchSpeed * Time.deltaTime;
        }
        else
        {
            if(t<0.5f || t2<0.5f)
            {
                t +=  PitchSpeed * Time.deltaTime;
                t2 += bodyPitchSpeed * Time.deltaTime;
                t = Mathf.Clamp(t, 0, 0.5f);
                t2 = Mathf.Clamp(t2, 0, 0.5f);
            }
            else if(t>0.5f || t2>0.5f)
            {
                t -= PitchSpeed * Time.deltaTime;
                t2 -= bodyPitchSpeed * Time.deltaTime;
                t = Mathf.Clamp(t, 0.5f, 1);
                t2 = Mathf.Clamp(t2, 0.5f, 1);
            }
        }
        t = Mathf.Clamp01(t);
        t2 = Mathf.Clamp01(t2);

        for (int i = fins.Count-1; i>=0; i--)
        {
            fins[i].rotation = Quaternion.Slerp(pitchedDown, pitchedUp, t);
        }
        transform.rotation = Quaternion.Slerp(pitchedDown, pitchedUp, t2);
    }


    void touchDrag()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            initialTouchPos = Input.mousePosition;
            Debug.Log("First ScreenMousePosition: " + initialTouchPos);
            initialTouchPos = GetWorldPositionOnPlane(initialTouchPos, transform.position.z);//The mouse projection plane has to be created on the subs z position
            Debug.Log("First WorldMousePosition: " + initialTouchPos);
            initialSubPos = transform.position;
        }
        else if(Input.GetButton("Fire1"))
        {
            currentTouchPos = Input.mousePosition;
            Debug.Log("Current ScreenMousePosition: " + currentTouchPos);
            currentTouchPos = GetWorldPositionOnPlane(currentTouchPos, transform.position.z);
            Debug.Log("Current WorldMousePosition: " + currentTouchPos + "Distance Cam-sub : " + Vector3.Distance(Camera.main.transform.position, transform.position));
            dragDirection = currentTouchPos - initialTouchPos;
            Debug.Log("DragDirection = " + dragDirection);

            targetSubPos = initialSubPos + dragDirection * sensitivity; //Thanks to the sensitivity we can set how much distance the sub actually travels
                                                                        //in relation to the distance traveled by the mouse/touch input
        }
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


