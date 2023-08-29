using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class NPC : MonoBehaviour
{
    NPC thisNPC;
    bool inRange = false;
    public string givenName;
    public int NPCIndex;
    public int NPCState;
    public int dialogueState;
    public int NPCDialogueIndex;
    public Dialogue dialogueList;
    public List<string> script = new List<string>();
    public List<Event> events = new List<Event>();
    public Vector3 location;
    
    public GameObject canvas;
    GameObject currentCanvas;
    bool canvasActive = false;
    private int dialogueIndex = 0;
    
    int moveSpeed = 1;
    bool eventTriggered = false;
    bool isMoving = false;
    
    List<MoveDirs> moveToVectors;
    Vector3 moveDestination;
    Vector3 translateVector;

    int eventIndex = 0;
    int moveIndex = 0;
    int modifyIndex = 0;

    public void createNPC(NPC newNPC, NPCData newNPCData, Dialogue newDialogue){
        Debug.Log("creating NPC");
        this.givenName = newNPCData.givenName;
        this.NPCIndex = newNPCData.index;
        this.NPCDialogueIndex =  newNPCData.dialogueIndex;
        this.NPCState = newNPCData.state;
        this.location = new Vector3(newNPCData.location[0], newNPCData.location[1], newNPCData.location[2]);
        this.dialogueList = newDialogue;
        //gameObject = newNPC;
        Debug.Log("NPC Created");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && !eventTriggered && !canvasActive && Input.GetKeyDown("space")){
            currentCanvas = Instantiate(canvas, new Vector3(0, 0, 0), Quaternion.identity);
            DisplayNextDialogue();
            canvasActive = true;
        }
        else if (inRange && !eventTriggered && canvasActive && dialogueIndex < script.Count && Input.GetKeyDown("space")){
            DisplayNextDialogue();
        }
        else if (canvasActive && !eventTriggered && Input.GetKeyDown("space")){
            Destroy(currentCanvas);
            canvasActive = false;
            GlobalController.CheckEvent(this);
            dialogueIndex = 0;
        }
        /*else if (eventTriggered && modifyIndex < events[eventIndex].fromObjects.Count && events[eventIndex].hasObject){
            modifyObjects();
        }
        else if (eventTriggered && eventIndex < events.Count && moveIndex < events[eventIndex].moveTo.Count){
            transform.Translate(events[eventIndex].moveDirs[moveIndex] * Time.deltaTime * moveSpeed);
            Debug.Log(Round(transform.position, 0));
            if (Round(transform.position, 0) == events[eventIndex].moveTo[moveIndex]){
                moveIndex++;
            }
        }
        else if (eventTriggered && moveIndex >= events[eventIndex].moveTo.Count){
            eventTriggered = false;
            Destroy(this.gameObject);
        }*/
        if (isMoving){
            if (moveIndex < moveToVectors.Count){
                transform.Translate(translateVector * Time.deltaTime * moveSpeed);
                if (Round(transform.position, 0) == Round(moveDestination, 0)){
                    moveIndex++;
                    if (moveIndex >= moveToVectors.Count){
                        isMoving = false;
                        Destroy(this.gameObject);
                    }
                    else {
                        translateVector = new Vector3(moveToVectors[moveIndex].moveVector[0], moveToVectors[moveIndex].moveVector[1], moveToVectors[moveIndex].moveVector[2]);
                        moveDestination = new Vector3(moveToVectors[moveIndex].destinations[0], moveToVectors[moveIndex].destinations[1], moveToVectors[moveIndex].destinations[2]);
                    }
                    
                }
            }
            
        }
    }

    void modifyObjects(){
        for (int i = 0; i < events[eventIndex].fromObjects.Count; i++){
            for (int j = 0; j < events[eventIndex].toObjects.Count; j++){
                Debug.Log(events[i].fromObjects[i]);
                GameObject.Find(events[i].fromObjects[j]).name = events[eventIndex].toObjects[j];
                modifyIndex++;
            }
        }
    }

    void checkEvent(){
        
    }

    Vector3 Round(Vector3 vector3, int decimalPlaces)
	{
		float multiplier = 1;
		for (int i = 0; i < decimalPlaces; i++)
		{
			multiplier *= 10f;
		}
		return new Vector3(
			Mathf.Round(vector3.x * multiplier) / multiplier,
			Mathf.Round(vector3.y * multiplier) / multiplier,
			Mathf.Round(vector3.z * multiplier) / multiplier);
	}

    void DisplayNextDialogue(){
        GameObject nameTextObject = GameObject.Find("Name");
        TextMeshProUGUI nameText = nameTextObject.GetComponent<TextMeshProUGUI>();
        nameText.text = givenName;
        GameObject dialogueTextObject = GameObject.Find("Text");
        TextMeshProUGUI dialogueText = dialogueTextObject.GetComponent<TextMeshProUGUI>();
        dialogueText.text = script[dialogueIndex];
        dialogueIndex++;
    }

    public void TriggerMove(List<MoveDirs> moveVectors){
        isMoving = true;
        moveToVectors = moveVectors;
        translateVector = new Vector3(moveToVectors[0].moveVector[0], moveVectors[0].moveVector[1], moveVectors[0].moveVector[2]);
        moveDestination = new Vector3(moveToVectors[0].destinations[0], moveToVectors[0].destinations[1], moveToVectors[0].destinations[2]);
        
    }

    void OnTriggerEnter(Collider myCollider){
        Debug.Log(myCollider.name + " can talk to " + this.givenName);
        inRange = true;
    }

    void OnTriggerExit(Collider myCollider){
        Debug.Log(myCollider.name + " is too far from " + this.givenName);
        inRange = false;
        if (canvasActive){
            Destroy(currentCanvas);
            canvasActive = false;
        }
    }
}
