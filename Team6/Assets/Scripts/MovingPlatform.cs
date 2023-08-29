using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField]
    private float     speed = 5.0f;
    [SerializeField]
    private Transform startPos;
    [SerializeField]
    private Transform endPos;

    private Transform currDestination;

    void FixedUpdate()
    {
        var dist = speed * Time.deltaTime;

        if (gameObject.transform.position == startPos.position)
            currDestination = endPos;
        else if (transform.position == endPos.position)
            currDestination = startPos;

        gameObject.transform.position = Vector3.MoveTowards (transform.position, currDestination.position, dist);
    }
}
