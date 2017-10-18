using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float destroyAfter;

    float speed;
    Vector3 direction;
    Rigidbody rb;
    float lerpSpeed;
    
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, destroyAfter);
    }
	
	void Update ()
    {
        lerpSpeed += speed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, direction, lerpSpeed);
    }

    public void SetVariables(float _speed, Vector3 _direction)
    {
        speed = _speed;
        direction = _direction;
    }
}
