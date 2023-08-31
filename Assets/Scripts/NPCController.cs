using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public List<NPC> NPCList;
    public NPC NPCPrefab;
    public NPC newNPC;
    public static SceneData sceneData;
    public static DialogueData dialogueData;
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

    public static void CheckEvent(NPC nPC){
        // Events now handled by global controller find a way to make npcs move and modify scene gameobjects from here.
        List<NPCData> sceneNPCs = sceneData.NPCs;
        int queryState = nPC.NPCState;
        Dialogue currentDialogue = dialogueData.NPCs[nPC.NPCIndex];
        if (currentDialogue.states[queryState].hasEvent){
            nPC.NPCState++;
            sceneNPCs[nPC.NPCIndex].state++;
            AssignDialogues();
        }
        if (currentDialogue.states[queryState].hasMove){
            nPC.NPCState++;
            sceneNPCs[nPC.NPCIndex].state++;
            List<MoveDirs> moveVectors = currentDialogue.states[queryState].moveTo;
            nPC.TriggerMove(moveVectors);
            //Debug.Log("NPC Moves: " + moveVector[0] + ", " + moveVector[1] + ", " + moveVector[2]);
        }
        //Not sure if this should happen here or in the NPC script, think about it!
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
}
