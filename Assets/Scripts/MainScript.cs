using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public List<NPC> NPCs;
    public int startingState = 0;
    public int currentState = 0;

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
        NPCs[0].givenName = "NPC 0";
        if (currentState == 0){
            NPCs[0].script.Add("Hello " + PlayerPrefs.GetString("playerName") + " welcome to the new world");
        }
        else if (currentState == 1){
            NPCs[0].script.Add("Hello " + PlayerPrefs.GetString("playerName") + " welcome to the new world");
        }
    }
}
