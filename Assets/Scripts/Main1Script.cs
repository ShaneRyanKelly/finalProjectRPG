using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main1Script : MonoBehaviour
{
    public NPC Friend;
    public NPC NPC0;
    private GlobalController globalController;
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
        Friend.script.Add("" + PlayerPrefs.GetString("playerName") + "! You remember how to get there right?");
        Friend.script.Add("Of course, you do! Just follow me to the East, out of town.");
        NPC0.givenName = "NPC0";
        NPC0.script.Add("Shouldn't you be headed somewhere, to the East.");
    }
}