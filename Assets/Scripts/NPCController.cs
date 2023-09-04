using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCController : MonoBehaviour
{
    public List<NPC> NPCList;
    public NPC NPCPrefab;
    public NPC newNPC;
    public static SceneData sceneData;
    public static DialogueData dialogueData;
    public GameObject canvas;
    public GameObject currentCanvas;
    public bool canvasActive = false;
    public NPC localNPC;
    public bool inRange = false;
    int dialogueIndex = 0;
    int moveIndex = 0;
    
    void Awake(){
        int numControllers = FindObjectsOfType(typeof(NPCController)).Length;
        if (numControllers != 1){
            Destroy(this.gameObject);
        }
        else {
            DontDestroyOnLoad(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && !localNPC.isMoving && !canvasActive && Input.GetKeyDown("space")){
            Debug.Log("canvas");
            currentCanvas = Instantiate(canvas, new Vector3(0, 0, 0), Quaternion.identity);
            canvasActive = true;
            DisplayNextDialogue();
        }
        else if (inRange && !localNPC.isMoving && canvasActive && dialogueIndex < localNPC.script.Count && Input.GetKeyDown("space")){
            DisplayNextDialogue();
        }
        else if (canvasActive && !localNPC.isMoving && Input.GetKeyDown("space")){
            Destroy(currentCanvas);
            canvasActive = false;
            CheckEvent();
            dialogueIndex = 0;
        }
        if (localNPC != null && localNPC.isMoving){
            if (moveIndex < localNPC.moveVectors.Count){
                localNPC.transform.Translate(localNPC.translateVector * Time.deltaTime * localNPC.moveSpeed);
                if (Round(localNPC.transform.position, 0) == Round(localNPC.moveDestination, 0)){
                    moveIndex++;
                    if (moveIndex >= localNPC.moveVectors.Count){
                        Destroy(localNPC.gameObject);
                    }
                    else {
                        localNPC.translateVector = new Vector3(localNPC.moveVectors[moveIndex].moveVector[0], localNPC.moveVectors[moveIndex].moveVector[1], localNPC.moveVectors[moveIndex].moveVector[2]);
                        localNPC.moveDestination = new Vector3(localNPC.moveVectors[moveIndex].destinations[0], localNPC.moveVectors[moveIndex].destinations[1], localNPC.moveVectors[moveIndex].destinations[2]);
                    }
                    
                }
            }
            
        }
    }
    public static void AssignDialogues(){
        List<NPCData> sceneNPCs = sceneData.NPCs;
        for (int i = 0; i < sceneNPCs.Count; i++)
        {
            NPCData currentNPCData = sceneNPCs[i];
            Dialogue currentDialogue = dialogueData.NPCs[currentNPCData.index];
            //find the npc and assign dialogues.
            if (currentNPCData.state >= currentDialogue.states.Count){
                continue;
            }
            NPC currentNPC = GameObject.Find(sceneNPCs[i].givenName).GetComponent<NPC>();
            //Debug.Log(dialogues.states[0].lines[0]);
            currentNPC.script.Clear();
            for (int j = 0; j < currentDialogue.states[currentNPC.NPCState].lines.Count; j++){
                string currentLine = currentDialogue.states[currentNPC.NPCState].lines[j];
                //Debug.Log(currentLine);
                string parsedLine = ParseDialogue(currentLine);
                //Debug.Log(parsedLine);
                currentNPC.script.Add(parsedLine);
            }
        }
    }

    public void CheckEvent(){
        // Events now handled by global controller find a way to make npcs move and modify scene gameobjects from here.
        List<NPCData> sceneNPCs = sceneData.NPCs;
        int queryState =  localNPC.NPCState;
        Dialogue currentDialogue = dialogueData.NPCs[localNPC.NPCIndex];
        if (currentDialogue.states[queryState].hasEvent){
            localNPC.NPCState++;
            sceneNPCs[localNPC.NPCIndex].state++;
            AssignDialogues();
        }
        if (currentDialogue.states[queryState].hasMove){
            localNPC.NPCState++;
            sceneNPCs[localNPC.NPCIndex].state++;
            localNPC.moveVectors = currentDialogue.states[queryState].moveTo;
            TriggerMove();
            //Debug.Log("NPC Moves: " + moveVector[0] + ", " + moveVector[1] + ", " + moveVector[2]);
        }
        //Not sure if this should happen here or in the NPC script, think about it!
    }

    void DisplayNextDialogue(){
        GameObject nameTextObject = GameObject.Find("Name");
        TextMeshProUGUI nameText = nameTextObject.GetComponent<TextMeshProUGUI>();
        nameText.text = localNPC.givenName;
        GameObject dialogueTextObject = GameObject.Find("Text");
        TextMeshProUGUI dialogueText = dialogueTextObject.GetComponent<TextMeshProUGUI>();
        dialogueText.text = localNPC.script[dialogueIndex];
        dialogueIndex++;
    }

    public void InstantiateNPCs(){
        NPCList.Clear();
        List<NPCData> sceneNPCs = sceneData.NPCs;
        for (int i = 0; i < sceneNPCs.Count; i++){
            Debug.Log("in loop " + i);
            NPCData newNPCData = sceneNPCs[i];
            Debug.Log(GlobalController.dialogues.scenes[0]);
            Dialogue newDialogue = dialogueData.NPCs[newNPCData.index];
            Vector3 location = new Vector3(newNPCData.location[0], newNPCData.location[1], newNPCData.location[2]);
            Debug.Log(newNPCData.state);
            if (newNPCData.state < newDialogue.states.Count){
                newNPC = Instantiate(NPCPrefab, location, Quaternion.identity);
                newNPC.name = newNPCData.givenName;
                newNPC.createNPC(newNPC, newNPCData, newDialogue);
                Debug.Log("new npc " + newNPC.location);
                NPCList.Add(newNPC);
            }
            else {
                
            }
        }
        Debug.Log("instantiated NPCS");
    }

    public void InstantiateScene(SceneData newSceneData, DialogueData newDialogueData){
        sceneData = newSceneData;
        dialogueData = newDialogueData;
    }
    private static string ParseDialogue(string rawString){
        // find embedded var and insert value
        bool parsingVar = false;
        string currentVar = "";
        int varStart = 0;
        int varEnd = 0;
        char[] stringArray = rawString.ToCharArray();
        //Debug.Log(rawString);
        for (int i = 0; i < rawString.Length; i++){
            if (stringArray[i] == '{'){
                varStart = i;
                parsingVar = true;
                Debug.Log("parsing variable name");
            }
            else if (stringArray[i] == '}'){
                varEnd = i+1;
                //Debug.Log(rawString.Substring(varStart, varEnd-varStart));
                rawString = rawString.Replace(rawString.Substring(varStart, varEnd-varStart), PlayerPrefs.GetString(currentVar));
                currentVar = "";
                varStart = 0;
                varEnd = 0;
                //Debug.Log("var end");
                parsingVar = false;
            }
            else if (parsingVar){
                currentVar += stringArray[i];
            }
        }
        //Debug.Log(rawString);
        return rawString;
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

    public void TriggerMove(){
        localNPC.isMoving = true;
        localNPC.translateVector = new Vector3(localNPC.moveVectors[0].moveVector[0], localNPC.moveVectors[0].moveVector[1], localNPC.moveVectors[0].moveVector[2]);
        localNPC.moveDestination = new Vector3(localNPC.moveVectors[0].destinations[0], localNPC.moveVectors[0].destinations[1], localNPC.moveVectors[0].destinations[2]);
        
    }
}
