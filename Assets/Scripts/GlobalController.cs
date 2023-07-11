using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    public List<GameObject> scenes;
    int House0State = 0;
    bool newScene = true;
    // Start is called before the first frame update
    void Awake(){
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (newScene){
            //sceneControllers.Add(GameObject.Find("SceneController").GetComponent<MainScript>());
            //Debug.Log(sceneControllers[0].NPCs[0].script[0]);
            
            newScene = false;
        }
    }

    public string ReturnString(){
        return "Hello World";
    }
}
