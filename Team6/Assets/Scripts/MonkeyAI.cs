using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.Types;

public class MonkeyAI : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip[] audioClips;

    [SerializeField]
    private TextMeshPro text;

    private bool isPlayerNearby = false;

    private int currAudioNdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Interact") && isPlayerNearby)
        {

            source.clip = audioClips[currAudioNdx];

            source.Play();
            currAudioNdx = ((currAudioNdx + 1) % 3);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;

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
        currAudioNdx = 0;
    }
}
