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
    public NPCController nPCController;

    // Start is called before the first frame update
    void Awake(){
        int numControllers = FindObjectsOfType(typeof(GlobalController)).Length;
        if (numControllers != 1){
            Destroy(this.gameObject);
        }
        else {
            DontDestroyOnLoad(this.gameObject);
        }
    }
    void Start()
    {
        scenes = JsonUtility.FromJson<SceneList>(scenesJson.text);
        dialogues = JsonUtility.FromJson<DialogueList>(dialoguesJson.text);
        Debug.Log(dialogues.scenes);
        currentScene = SceneManager.GetActiveScene();
        currentState = scenes.scenes[currentScene.buildIndex].sceneState;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("start global controller");
        nPCController.InstantiateScene(scenes.scenes[currentScene.buildIndex], dialogues.scenes[currentScene.buildIndex]);
        nPCController.InstantiateNPCs();
        nPCController.AssignDialogues();
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
        nPCController.InstantiateScene(scenes.scenes[currentScene.buildIndex], dialogues.scenes[currentScene.buildIndex]);
        nPCController.InstantiateNPCs();
        nPCController.AssignDialogues();
    }

    /*public static void AssignDialogues(){
        Debug.Log(currentScene.buildIndex);
        List<NPCData> sceneNPCs = scenes.scenes[currentScene.buildIndex].NPCs;
        for (int i = 0; i < sceneNPCs.Count; i++)
        {
            NPCData currentNPCData = sceneNPCs[i];
            Dialogue currentDialogue = dialogues.scenes[currentScene.buildIndex].NPCs[currentNPCData.index];
            //find the npc and assign dialogues.
            if (currentNPCData.state >= currentDialogue.states.Count){
                continue;
            }
            NPC currentNPC = GameObject.Find(sceneNPCs[i].givenName).GetComponent<NPC>();
            //Debug.Log(dialogues.states[0].lines[0]);
            int queryState = GetNPCState(currentNPC.NPCIndex);
            
            Debug.Log("query state: " + currentState + " " + queryState);
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
        List<NPCData> sceneNPCs = scenes.scenes[currentScene.buildIndex].NPCs;
        int queryState = nPC.NPCState;
        Dialogue currentDialogue = dialogues.scenes[currentScene.buildIndex].NPCs[nPC.NPCIndex];
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

    public static int GetNPCState(int NPCIndex){
        Dialogue currentDialogue = dialogues.scenes[currentScene.buildIndex].NPCs[NPCIndex];
        if (currentState >= currentDialogue.states.Count){
            return currentDialogue.states.Count - 1;
        }
        return currentState;
    }

    private void InstantiateNPCs(){
        NPCList.Clear();
        List<NPCData> sceneNPCs = scenes.scenes[currentScene.buildIndex].NPCs;
        for (int i = 0; i < sceneNPCs.Count; i++){
            Debug.Log("in loop " + i);
            NPCData newNPCData = sceneNPCs[i];
            Debug.Log(dialogues.scenes[0]);
            Dialogue newDialogue = dialogues.scenes[currentScene.buildIndex].NPCs[newNPCData.index];
            Vector3 location = new Vector3(newNPCData.location[0], newNPCData.location[1], newNPCData.location[2]);
            Debug.Log(newNPCData.state);
            if (newNPCData.state <= newDialogue.states.Count){
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

    //Not sure if this should happen here or in the NPC script, think about it!
    /*public static void MoveNPC(NPC nPC, List<MoveDirs> moveVectors){
        Debug.Log("Moving to" + moveVectors[0].moveVector[0]);
        for (int i = 0; i < moveVectors.Count; i++){
            Vector3 translateVector = new Vector3(moveVectors[i].moveVector[0], moveVectors[i].moveVector[1], moveVectors[i].moveVector[2]);
            nPC.transform.Translate(translateVector * Time.deltaTime * 10.0f);
        }
    }*/

    public string ReturnString(){
        return "Hello World";
    }
}
