using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class trigger : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider myCollider){
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(this.name);
        //Debug.Log(lastPosition);
        SceneManager.LoadScene(this.name);
        /*if (scene.name == "SampleScene")
        {
            SceneManager.LoadScene("SampleScene1-2");
        }
        else if (scene.name == "SampleScene1-2")
        {
            SceneManager.LoadScene("SampleScene1-3");
        }
        else if (scene.name == "SampleScene1-3")
        {
            SceneManager.LoadScene("SampleScene1-4");
        }
        else if (scene.name == "SampleScene1-4")
        {
            SceneManager.LoadScene("SampleScene");
        }*/
    }
}
