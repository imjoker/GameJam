using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.Types;
public class MonkeyAI : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip audioClip;

    [SerializeField]
    private TextMeshPro text;

    private bool isPlayerNearby = false;

    PlayerMovement player;

    private Animator MonkeyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        MonkeyAnimator = GetComponent<Animator>();
        source.clip = audioClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Interact") && isPlayerNearby)
        {
            player.TransformPlayerIntoMonkeyHuman();
            source.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;

        player = other.GetComponent<PlayerMovement>();

        text.enabled = true;
        isPlayerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;

        source.Stop();
        text.enabled = false;
        isPlayerNearby = false;
    }
}
