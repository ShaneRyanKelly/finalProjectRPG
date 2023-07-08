using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class trigger : MonoBehaviour
{
    public string exitName;
    public Vector3 exitCoord;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider myCollider){
        SetPreferences();
        SceneManager.LoadScene(this.name);
    }

    void SetPreferences(){
        Scene scene = SceneManager.GetActiveScene();
        PlayerPrefs.SetString("exitName", this.name);
        PlayerPrefs.SetFloat("x", exitCoord.x);
        PlayerPrefs.SetFloat("y", exitCoord.y);
        PlayerPrefs.SetFloat("z", exitCoord.z);
    }
}
