using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House0 : MonoBehaviour
{
    public NPC Mom;
    private GlobalController globalController;
    // Start is called before the first frame update
    void Start()
    {
        //globalController = GameObject.Find("GlobalScript").GetComponent<GlobalController>();
        //Debug.Log(globalController.ReturnString());
        Debug.Log("House0");
        AssignDialogues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AssignDialogues(){
        Mom.givenName = "Mom";
        Mom.script.Add("Good morning " + PlayerPrefs.GetString("playerName") + "! Finally you woke up!");
        Mom.script.Add("You should go see " + PlayerPrefs.GetString("friendName") + ". Have you seen them recently? Maybe they are at home today.");
    }
}
