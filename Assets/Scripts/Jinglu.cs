using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jinglu : MonoBehaviour
{
    public string nameText;
    public string[] sentences;
    // Start is called before the first frame update
    void Start()
    {
        sentences[0] = "Hello " + PlayerPrefs.GetString("playerName") + " welcome to the new world.";
        sentences[1] = "I now have two lines of dialogue!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
