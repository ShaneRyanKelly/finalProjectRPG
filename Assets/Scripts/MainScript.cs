using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public NPC mainNPC;
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
        mainNPC.givenName = "NPC 0";
        mainNPC.script.Add("Hello " + PlayerPrefs.GetString("playerName") + " welcome to the new world");
    }
}
