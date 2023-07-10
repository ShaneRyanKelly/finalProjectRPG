using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneController
{
    public string sceneName;
    public NPC[] npcArray;

    [TextArea(3, 10)]
    public string[] sentences;
}