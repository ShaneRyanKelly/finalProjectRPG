using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Event
{
    public List<string> fromObjects;
    public List<string> toObjects;
    public bool hasObject;
    public List<Vector3> moveDirs;
    public List<Vector3> moveTo;
    public bool hasMove;

}
