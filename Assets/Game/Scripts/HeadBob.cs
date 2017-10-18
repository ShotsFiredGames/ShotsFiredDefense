using System.Collections;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public Vector3 offset;
    public float headbobSpeed;
    public float headbobAmountX;
    public float headbobAmountY;
    [HideInInspector]
    public float headbobStepCounter;
    Vector3 parentLastPosition;

    void Start()
    {
        offset = transform.localPosition;
        parentLastPosition = transform.root.position;
    }

    void Update()
    {
        if (PlayerMovement.inAir) return;

        headbobStepCounter += Vector3.Distance(parentLastPosition, transform.root.position) * headbobSpeed;
        transform.localPosition = offset + new Vector3(Mathf.Sin(headbobStepCounter) * headbobAmountX,
            (Mathf.Cos(headbobStepCounter * 2) * headbobAmountY * -1), 0);
        parentLastPosition = transform.root.position;
    }
}