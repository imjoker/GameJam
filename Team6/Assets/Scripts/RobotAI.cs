using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAI : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;
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

        gameObject.transform.position = Vector3.MoveTowards(transform.position, currDestination.position, dist);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;

        other.gameObject.SetActive(false);

        other.gameObject.GetComponent<PlayerMovement>().Invoke("ReSpawnAtSavedCheckPoint", 2);
    }
}
