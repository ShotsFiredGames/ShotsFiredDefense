using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public float speed = 1000;
    public float fireFrequency;
    public GameObject bulletHole;

    bool isFiring;
    bool readyToFire;

    Recoil recoil;
    Camera myCamera;
    RaycastHit hit;

    void Start ()
    {
        readyToFire = true;
        myCamera = Camera.main;
        recoil = GetComponentInChildren<Recoil>();
	}
	
	void Update ()
    {
        isFiring = PlayerInput.isFiring;

        if(isFiring && readyToFire)
        {
            readyToFire = false;
            recoil.Fire();

            if(Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, 1000))
            {
                Vector3 position = hit.point + (hit.normal * .1f);
                Quaternion rotation = Quaternion.LookRotation(hit.normal);
                if(bulletHole != null)
                Instantiate(bulletHole, position, rotation);
            }

            StartCoroutine(FireFrequency());
        }
    }

    IEnumerator FireFrequency()
    {
        yield return new WaitForSeconds(fireFrequency);
        readyToFire = true;
    }
}
