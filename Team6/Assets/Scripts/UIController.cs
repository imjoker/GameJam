using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Text ControlsLocked;
    [SerializeField]
    Text ControlsUnlocked;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayUnlockedControls ()
    {
        ControlsLocked.gameObject.SetActive (false);
        ControlsUnlocked.gameObject.SetActive (true);

    }
}
