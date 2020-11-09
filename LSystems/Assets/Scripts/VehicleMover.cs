using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMover : MonoBehaviour
{
    public float spawnSpeed = 1f;
    public GameObject vehicle;
    void Start()
    {
        StartCoroutine(stepThroughSentence());
    }

    IEnumerator stepThroughSentence()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        for (int i = 0; i < SentenceConstructor.instance.resultants.Count; i++)
        {
            string sentence = SentenceConstructor.instance.resultants[i];
            Instantiate(vehicle, vehicle.transform.position, Quaternion.identity);

            Debug.Log(sentence);
            foreach (char c in sentence)
            {
                if (c.ToString() == "F")
                {
                    
                }
                else if (c.ToString() == "+")
                {
                    Debug.Log("Turn Right");
                    getAllVehicles(-25.7f);
                }
                else if (c.ToString() == "-")
                {
                    Debug.Log("Turn Left");
                    getAllVehicles(25.7f);
                }
                else if (c.ToString() == "[")
                {
                    Debug.Log("SetStack");
                    setVehicleStack();
                }
                else if (c.ToString() == "]")
                {
                    Debug.Log("RetrieveStack");
                    retrieveVehicleStack();
                }
                yield return new WaitForSeconds(spawnSpeed);
            }
            spawnSpeed *= 0.5f;
            Debug.Log(spawnSpeed);
            stopAllVehicles();
        }
       
    }

    void getAllVehicles(float ang)
    {
        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");
        foreach (GameObject g in vehicles)
        {
            g.GetComponent<VehicleBehaviour>().rotateObject(ang);
        }
    }

    void setVehicleStack()
    {
        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");
        foreach (GameObject g in vehicles)
        {
            g.GetComponent<VehicleBehaviour>().setStack();
        }
    }

    void retrieveVehicleStack()
    {
        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");
        foreach (GameObject g in vehicles)
        {
            g.GetComponent<VehicleBehaviour>().retrieveStack();
            
        }
    }

    void createNewVehicles()
    {
        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");
        foreach (GameObject g in vehicles)
        {
            g.GetComponent<VehicleBehaviour>().createNewVehicle();
        }
    }

    void stopAllVehicles()
    {
        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");
        foreach (GameObject g in vehicles)
        {
            g.GetComponent<VehicleBehaviour>().speed = 0;
        }
        Debug.Log("Stopped");
    }
}
