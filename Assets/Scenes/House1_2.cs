using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House1_2 : MonoBehaviour
{
    public NPC FriendMom;
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
        FriendMom.givenName = "Mom";
        FriendMom.script.Add("Hi " + PlayerPrefs.GetString("playerName") + "! "  + PlayerPrefs.GetString("friendName") +  " Has gone off to your special place again.");
    }
}
