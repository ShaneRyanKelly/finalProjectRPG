using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    // global controller should pull scene data from json file on back end
    // json file should contain all scene data including game objects, character dialogues
    // global controller should parse dialogues
    private SceneData scenes;
    bool newScene = true;
    public TextAsset scenesJson;
    // Start is called before the first frame update
    void Awake(){
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        scenes = JsonUtility.FromJson<SceneData>(scenesJson.text);
        Debug.Log(scenes.sceneName);
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

    public string ReturnString(){
        return "Hello World";
    }
}
