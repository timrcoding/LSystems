using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBehaviour : MonoBehaviour
{
    public float speed = 1;
    public float currentAngle;
    public GameObject vehicle;
    public Vector3 stackPosition;
    public float stackRotation;
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;
        
        
        
    }

    public void rotateObject(float ang)
    {
        transform.Rotate(new Vector3(0, 0, ang), Space.World);
        currentAngle = ang;
    }

    public void createNewVehicle()
    {
        Instantiate(gameObject, transform.position, transform.rotation);
    }

    public void setStack()
    {
        stackPosition = transform.position;
        stackRotation = currentAngle;
    }

    public void retrieveStack()
    {
        Instantiate(vehicle, stackPosition, Quaternion.Euler(0, 0, stackRotation));
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        
    }

    
}
