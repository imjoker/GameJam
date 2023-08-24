using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbableWall : MonoBehaviour
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

        // Stops the player from being affected by gravity while on ladder
        other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        Global.currState = Global.ePlayerState.CLIMB;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;

        float y = Input.GetAxis("Vertical");
        other.transform.Translate(new Vector3(0.0f, y, 0.0f));
        other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;

        // Stars the player from being affected by gravity while on ladder
        other.gameObject.GetComponent<Rigidbody2D>().gravityScale = Global.defGravityScale;
        Global.currState = Global.ePlayerState.WALK;
    }
}
