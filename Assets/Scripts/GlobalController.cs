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
            Debug.Log(dialogues.dialogues[0].states[0].lines[0]);
            //This is a total unmitigated disaster, fix it oh my god please.
            /*for (int j = 0; j < dialogues.dialogues[currentNPC.NPCIndex].dialogues[currentState].lines.Count; j++){
                Debug.Log(dialogues.dialogues[currentNPC.NPCIndex].dialogues[currentState]);
                currentNPC.script.Add(dialogues.dialogues[currentNPC.NPCIndex].dialogues[currentState].lines[j]);
            }*/
        }
    }

    public string ReturnString(){
        return "Hello World";
    }
}
