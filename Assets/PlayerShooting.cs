using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public float speed = 1000;
    public float fireFrequency;

    bool isFiring;
    bool readyToFire;

    Camera myCamera;
    RaycastHit hit;

    void Start ()
    {
        readyToFire = true;
        myCamera = Camera.main;
	}
	
	void Update ()
    {
        isFiring = PlayerInput.isFiring;

        if(isFiring && readyToFire)
        {
            readyToFire = false;
            StartCoroutine(FireFrequency());
        }
    }

    IEnumerator FireFrequency()
    {
        yield return new WaitForSeconds(fireFrequency);
        readyToFire = true;
    }
}
