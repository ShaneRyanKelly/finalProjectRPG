using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    // global controller should pull scene data from json file on back end
    // json file should contain all scene data including game objects, character dialogues
    // global controller should parse dialogues
    public static SceneList scenes;
    public static DialogueList dialogues;
    public TextAsset scenesJson;
    public TextAsset dialoguesJson;
    public static Scene currentScene;
    public static int currentState;
    public List<NPC> NPCList;
    public NPC NPCPrefab;
    public NPC newNPC;
    // Start is called before the first frame update
    void Awake(){
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        scenes = JsonUtility.FromJson<SceneList>(scenesJson.text);
        dialogues = JsonUtility.FromJson<DialogueList>(dialoguesJson.text);
        currentScene = SceneManager.GetActiveScene();
        currentState = scenes.scenes[currentScene.buildIndex].sceneState;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("start global controller");
        InstantiateNPCs();
        AssignDialogues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = SceneManager.GetActiveScene();
        currentState = scenes.scenes[currentScene.buildIndex].sceneState;
        Debug.Log("scene load: " + currentScene.buildIndex);
        InstantiateNPCs();
        AssignDialogues();
    }

    public static void AssignDialogues(){
        Debug.Log(currentScene.buildIndex);
        for (int i = 0; i < scenes.scenes[currentScene.buildIndex].NPCs.Count; i++)
        {
            //find the npc and assign dialogues.
            NPC currentNPC = GameObject.Find(scenes.scenes[currentScene.buildIndex].NPCs[i].givenName).GetComponent<NPC>();
            //Debug.Log(dialogues.dialogues[0].states[0].lines[0]);
            int queryState = GetNPCState(currentNPC.NPCDialogueIndex);
            
            Debug.Log("query state: " + currentState + " " + queryState);
            currentNPC.script.Clear();
            for (int j = 0; j < dialogues.dialogues[currentNPC.NPCDialogueIndex].states[queryState].lines.Count; j++){
                string currentLine = dialogues.dialogues[currentNPC.NPCDialogueIndex].states[queryState].lines[j];
                //Debug.Log(currentLine);
                string parsedLine = ParseDialogue(currentLine);
                //Debug.Log(parsedLine);
                currentNPC.script.Add(parsedLine);
            }
        }
    }

    public static void CheckEvent(NPC nPC){
        // Events now handled by global controller find a way to make npcs move and modify scene gameobjects from here.
        int queryState = GetNPCState(nPC.NPCIndex);
        if (dialogues.dialogues[nPC.NPCDialogueIndex].states[queryState].hasEvent){
            scenes.scenes[GlobalController.currentScene.buildIndex].sceneState++;
            currentState++;
            AssignDialogues();
        }
        //Not sure if this should happen here or in the NPC script, think about it!
        if (dialogues.dialogues[nPC.NPCIndex].states[queryState].hasMove){
            List<MoveDirs> moveVectors = dialogues.dialogues[nPC.NPCDialogueIndex].states[queryState].moveTo;
            nPC.TriggerMove(moveVectors);
            //Debug.Log("NPC Moves: " + moveVector[0] + ", " + moveVector[1] + ", " + moveVector[2]);
        }
    }

    public static int GetNPCState(int NPCIndex){
        Debug.Log(dialogues.dialogues[NPCIndex].states.Count);
        if (currentState >= dialogues.dialogues[NPCIndex].states.Count){
            return dialogues.dialogues[NPCIndex].states.Count - 1;
        }
        return currentState;
    }

    private void InstantiateNPCs(){
        NPCList.Clear();
        for (int i = 0; i < scenes.scenes[currentScene.buildIndex].NPCs.Count; i++){
            Debug.Log("in loop " + i);
            NPCData newNPCData = scenes.scenes[currentScene.buildIndex].NPCs[i];
            Dialogue newDialogue = dialogues.dialogues[newNPCData.index];
            Vector3 location = new Vector3(newNPCData.location[0], newNPCData.location[1], newNPCData.location[2]);
            newNPC = Instantiate(NPCPrefab, location, Quaternion.identity);
            newNPC.name = newNPCData.givenName;
            newNPC.createNPC(newNPC, newNPCData, newDialogue);
            Debug.Log("new npc " + newNPC.location);
            NPCList.Add(newNPC);
        }
        Debug.Log("instantiated NPCS");
    }

    //Not sure if this should happen here or in the NPC script, think about it!
    /*public static void MoveNPC(NPC nPC, List<MoveDirs> moveVectors){
        Debug.Log("Moving to" + moveVectors[0].moveVector[0]);
        for (int i = 0; i < moveVectors.Count; i++){
            Vector3 translateVector = new Vector3(moveVectors[i].moveVector[0], moveVectors[i].moveVector[1], moveVectors[i].moveVector[2]);
            nPC.transform.Translate(translateVector * Time.deltaTime * 10.0f);
        }
    }*/

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

    public string ReturnString(){
        return "Hello World";
    }
}
