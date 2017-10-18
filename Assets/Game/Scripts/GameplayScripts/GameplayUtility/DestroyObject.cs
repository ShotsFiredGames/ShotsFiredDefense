using System.Collections;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float destroyAfter;

    void Start()
    {
        Destroy(gameObject, destroyAfter);
    }
}