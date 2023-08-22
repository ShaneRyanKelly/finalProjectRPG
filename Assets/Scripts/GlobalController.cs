using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    // global controller should pull scene data from json file on back end
    // json file should contain all scene data including game objects, character dialogues
    // global controller should parse dialogues
    public SceneList scenes;
    public DialogueList dialogues;
    bool newScene = true;
    public TextAsset scenesJson;
    public TextAsset dialoguesJson;
    private Scene currentScene;
    private int currentState;
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
        AssignDialogues();
    }

    // Update is called once per frame
    void Update()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // cool this works.
        //Debug.Log("OnSceneLoaded: " + scene.name);
        //Debug.Log(mode);

    }

    public void AssignDialogues(){
        for (int i = 0; i < scenes.scenes[0].NPCs.Count; i++)
        {
            //find the npc and assign dialogues.
            NPC currentNPC = GameObject.Find(scenes.scenes[0].NPCs[i].name).GetComponent<NPC>();
            //Debug.Log(dialogues.dialogues[0].states[0].lines[0]);
            for (int j = 0; j < dialogues.dialogues[currentNPC.NPCIndex].states[currentState].lines.Count; j++){
                string currentLine = dialogues.dialogues[currentNPC.NPCIndex].states[currentState].lines[j];
                Debug.Log(currentLine);
                string parsedLine = parseDialogue(currentLine);
                Debug.Log(parsedLine);
                currentNPC.script.Add(parsedLine);
            }
        }
    }

    private string parseDialogue(string rawString){
        // find embedded var and insert value
        bool parsingVar = false;
        string currentVar = "";
        int varStart = 0;
        int varEnd = 0;
        char[] stringArray = rawString.ToCharArray();
        Debug.Log(rawString);
        for (int i = 0; i < rawString.Length; i++){
            if (stringArray[i] == '{'){
                varStart = i;
                parsingVar = true;
                Debug.Log("parsing variable name");
            }
            else if (stringArray[i] == '}'){
                varEnd = i+1;
                Debug.Log(rawString.Substring(varStart, varEnd-varStart));
                rawString = rawString.Replace(rawString.Substring(varStart, varEnd-varStart), PlayerPrefs.GetString(currentVar));
                currentVar = "";
                varStart = 0;
                varEnd = 0;
                Debug.Log("var end");
                parsingVar = false;
            }
            else if (parsingVar){
                currentVar += stringArray[i];
            }
        }
        Debug.Log(rawString);
        return rawString;
    }

    public string ReturnString(){
        return "Hello World";
    }
}
