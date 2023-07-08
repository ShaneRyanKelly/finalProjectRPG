using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class NPC : MonoBehaviour
{
    bool inRange = false;
    public string givenName;
    public List<string> script = new List<string>();
    public GameObject canvas;
    GameObject currentCanvas;
    bool canvasActive = false;
    int dialogueIndex = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && !canvasActive && Input.GetKeyDown("space")){
            currentCanvas = Instantiate(canvas, new Vector3(0, 0, 0), Quaternion.identity);
            DisplayNextDialogue();
            canvasActive = true;
        }
        else if (inRange && canvasActive && dialogueIndex < script.Count && Input.GetKeyDown("space")){
            DisplayNextDialogue();
        }
        else if (canvasActive && Input.GetKeyDown("space")){
            Destroy(currentCanvas);
            canvasActive = false;
            dialogueIndex = 0;
        }
    }

    void DisplayNextDialogue(){
        GameObject nameTextObject = GameObject.Find("Name");
        TextMeshProUGUI nameText = nameTextObject.GetComponent<TextMeshProUGUI>();
        nameText.text = givenName;
        GameObject dialogueTextObject = GameObject.Find("Text");
        TextMeshProUGUI dialogueText = dialogueTextObject.GetComponent<TextMeshProUGUI>();
        dialogueText.text = script[dialogueIndex];
        dialogueIndex++;
    }

    void OnTriggerEnter(Collider myCollider){
        Debug.Log(myCollider.name + " can talk to " + this.givenName);
        inRange = true;
    }

    void OnTriggerExit(Collider myCollider){
        Debug.Log(myCollider.name + " is too far from " + this.givenName);
        inRange = false;
        if (canvasActive){
            Destroy(currentCanvas);
            canvasActive = false;
        }
    }
}
