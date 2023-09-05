using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lines
{
    public List<string> lines;
    public bool hasEvent;
    public bool hasMove;
    public List<MoveDirs> moveTo;
    public bool hasChoices;
    public List<int> ToStates;
    public bool final;
}