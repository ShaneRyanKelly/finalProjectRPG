using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCController : MonoBehaviour
{
    public List<NPC> NPCList;
    public NPC NPCPrefab;
    public NPC newNPC;
    private NPC movingNPC;
    public static SceneData sceneData;
    public static DialogueData dialogueData;
    public DialogueContainer canvas;
    public DialogueContainer currentCanvas;
    public bool canvasActive = false;
    public NPC localNPC;
    public bool inRange = false;
    int dialogueIndex = 0;
    int moveIndex = 0;
    bool awaitInput = false;
    
    // this stays a bool
    enum range { inRange, outRange };
    enum state { inDialogue };
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
        Debug.Log(DialogueController.id);
    }

    // Update is called once per frame
    void Update()
    {
        // This could be refactored to use state machine
        // Seperate DialogueController? Necessary? How to handle interactions with other objects in game?
        if (awaitInput){
            Debug.Log("Awaiting input");

            // this is why i think we need DialogueController
            if (false){
                DisplayChoices();
            }
            if (Input.GetKey(KeyCode.Alpha0)){
                AssignState(0);
                awaitInput = false;
            }
            else if (Input.GetKey(KeyCode.Alpha1)){
                AssignState(1);
                awaitInput = false;
            }
        }
        else if (!awaitInput) {
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
                CheckEvent();
            }
            else if (movingNPC != null && movingNPC.isMoving){
                if (moveIndex < movingNPC.moveVectors.Count){
                    movingNPC.transform.Translate(movingNPC.translateVector * Time.deltaTime * movingNPC.moveSpeed);
                    if (Round(movingNPC.transform.position, 0) == Round(movingNPC.moveDestination, 0)){
                        moveIndex++;
                        if (moveIndex >= movingNPC.moveVectors.Count){
                            if (dialogueData.NPCs[movingNPC.NPCIndex].states.Count <= movingNPC.NPCState){
                                Debug.Log("destroy?");
                                Destroy(movingNPC.gameObject);
                            }
                            moveIndex = 0;
                            movingNPC.isMoving = false;
                            AssignDialogues();
                        }
                        else {
                            movingNPC.translateVector = new Vector3(movingNPC.moveVectors[moveIndex].moveVector[0], movingNPC.moveVectors[moveIndex].moveVector[1], movingNPC.moveVectors[moveIndex].moveVector[2]);
                            movingNPC.moveDestination = new Vector3(movingNPC.moveVectors[moveIndex].destinations[0], movingNPC.moveVectors[moveIndex].destinations[1], movingNPC.moveVectors[moveIndex].destinations[2]);
                        }
                        
                    }
                }
                
            }
        }
    }
    public void AssignDialogues(){
        List<NPCData> sceneNPCs = sceneData.NPCs;
        dialogueIndex = 0;
        for (int i = 0; i < sceneNPCs.Count; i++)
        {
            NPCData currentNPCData = sceneNPCs[i];
            Dialogue currentDialogue = dialogueData.NPCs[currentNPCData.index];
            NPC currentNPC;
            Debug.Log(sceneNPCs[i].givenName);
            if (currentNPCData.state < currentDialogue.states.Count){
                currentNPC = GameObject.Find(sceneNPCs[i].givenName).GetComponent<NPC>();
            }
            else{
                continue;
            }
            //find the npc and assign dialogues.
            Debug.Log(currentNPC.givenName + " " + currentNPC.NPCState);
            if (currentNPC.NPCState >= currentDialogue.states.Count){
                Debug.Log("continue");
                continue;
            }
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

    private void AssignState(int state){
        List<NPCData> sceneNPCs = sceneData.NPCs;
        Dialogue currentDialogue = dialogueData.NPCs[localNPC.NPCIndex];
        int queryState =  localNPC.NPCState;
        localNPC.NPCState = currentDialogue.states[queryState].ToStates[state];
        sceneNPCs[localNPC.NPCIndex].state = currentDialogue.states[queryState].ToStates[state];
        AssignDialogues();
        DisplayNextDialogue();
    }

    public void CheckEvent(){
        // Events now handled by global controller find a way to make npcs move and modify scene gameobjects from here.
        List<NPCData> sceneNPCs = sceneData.NPCs;
        int queryState =  localNPC.NPCState;
        Dialogue currentDialogue = dialogueData.NPCs[localNPC.NPCIndex];
        int ToState = currentDialogue.states[queryState].ToStates[0];
        Debug.Log(currentDialogue.states[queryState].hasChoices);
        if (currentDialogue.states[queryState].hasEvent){
            localNPC.NPCState = ToState;
            sceneNPCs[localNPC.NPCIndex].state = ToState;
            AssignDialogues();
        }
        if (currentDialogue.states[queryState].hasMove){
            Destroy(currentCanvas.gameObject);
            canvasActive = false;
            Debug.Log("To state " + ToState);
            localNPC.NPCState = ToState;
            sceneNPCs[localNPC.NPCIndex].state = ToState;
            localNPC.moveVectors = currentDialogue.states[queryState].moveTo;
            TriggerMove();
            //Debug.Log("NPC Moves: " + moveVector[0] + ", " + moveVector[1] + ", " + moveVector[2]);
        }
        if (currentDialogue.states[queryState].hasChoices){
            // PresentChoices();
            awaitInput = true;
        }
        if (currentDialogue.states[queryState].final){
            Destroy(currentCanvas.gameObject);
            canvasActive = false;
            localNPC.NPCState = ToState;
            sceneNPCs[localNPC.NPCIndex].state = ToState;
            AssignDialogues();
        }
        else {
            DisplayNextDialogue();
        }
    }

    void DisplayChoices(){
        // code to display choices with
    }

    void DisplayNextDialogue(){
        GameObject nameTextObject = GameObject.Find("Name");
        TextMeshProUGUI nameText = nameTextObject.GetComponent<TextMeshProUGUI>();
        nameText.text = localNPC.givenName;
        GameObject dialogueTextObject = GameObject.Find("Text");
        TextMeshProUGUI dialogueText = dialogueTextObject.GetComponent<TextMeshProUGUI>();
        if (dialogueIndex < localNPC.script.Count){
            dialogueText.text = localNPC.script[dialogueIndex];
            dialogueIndex++;
        }
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

    private void PresentChoices(){

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
        movingNPC = localNPC;
        // find a way to save state and location if player leaves scene during move
        sceneData.NPCs[movingNPC.NPCIndex].location = movingNPC.moveVectors[movingNPC.moveVectors.Count - 1].destinations;
        Debug.Log(sceneData.NPCs[movingNPC.NPCIndex].location);
        localNPC.isMoving = true;
        localNPC.translateVector = new Vector3(localNPC.moveVectors[0].moveVector[0], localNPC.moveVectors[0].moveVector[1], localNPC.moveVectors[0].moveVector[2]);
        localNPC.moveDestination = new Vector3(localNPC.moveVectors[0].destinations[0], localNPC.moveVectors[0].destinations[1], localNPC.moveVectors[0].destinations[2]);
        
    }
}
