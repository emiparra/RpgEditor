using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Node 
 {
    public Rect rect;
    public string NodeName;
    public static GameObject Quest;
    private bool _overnode;
    public bool complete;
    public List<Node> Connected;
    public Quest Q;
    
    public Node(float x, float y, float width, float height, string name)
    {
        rect = new Rect(x, y, width, height);
        Connected = new List<Node>();
        NodeName = name;
    }
   
    public void CheckMouse(Event cE, Vector2 pan)
    {
        if (rect.Contains(cE.mousePosition - pan))
            _overnode = true;
        else
            _overnode = false;
    }
    public bool OverNode
    {
        get { return _overnode; }
    }
    public void checkQuest()
    {
       Q = Quest.GetComponent<Quest>();
        Q.previousQuest = complete;
        if ( complete == true)
        {
            Debug.Log("ADENTRO!");
            Connected.Add(this);
        }
        else if (complete == false)
        {
            if(Connected.Contains(this))
            {
                Debug.Log("AFUERA!");
                Connected.Remove(this);
            }
        }
    }
    
   
}
