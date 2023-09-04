using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class NPC : MonoBehaviour
{
    public string givenName;
    public int NPCIndex;
    public int NPCState;
    public int NPCDialogueIndex;
    public Dialogue dialogueList;
    public List<string> script = new List<string>();
    public Vector3 location;
    public int moveSpeed = 1;
    public bool isMoving = false;
    
    public List<MoveDirs> moveVectors;
    public Vector3 moveDestination;
    public Vector3 translateVector;

    NPCController npcController;

    public void createNPC(NPC newNPC, NPCData newNPCData, Dialogue newDialogue){
        Debug.Log("creating NPC");
        this.givenName = newNPCData.givenName;
        this.NPCIndex = newNPCData.index;
        this.NPCDialogueIndex =  newNPCData.dialogueIndex;
        this.NPCState = newNPCData.state;
        this.location = new Vector3(newNPCData.location[0], newNPCData.location[1], newNPCData.location[2]);
        this.dialogueList = newDialogue;
        Debug.Log("NPC Created");
    }

    // Start is called before the first frame update
    void Start()
    {
        npcController = FindObjectOfType<NPCController>();
    }

    void OnTriggerEnter(Collider myCollider){
        Debug.Log(myCollider.name + " can talk to " + this.givenName);
        npcController.inRange = true;
        npcController.localNPC = this;
    }

    void OnTriggerExit(Collider myCollider){
        Debug.Log(myCollider.name + " is too far from " + this.givenName);
        npcController.inRange = false;
        npcController.localNPC = null;
        if (npcController.canvasActive){
            Destroy(npcController.currentCanvas);
            npcController.canvasActive = false;
        }
    }
}
