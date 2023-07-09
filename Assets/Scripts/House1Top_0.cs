using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House1Top_0 : MonoBehaviour
{
    public NPC Friend;
    // Start is called before the first frame update
    void Start()
    {
        AssignDialogues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AssignDialogues(){
        Friend.givenName = PlayerPrefs.GetString("friendName");
        Friend.script.Add("Hi " + PlayerPrefs.GetString("playerName") + "! Let's go to our special place.");
    }
}
