using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;

        Destroy(other.gameObject);
        // Respawn
    }

}
