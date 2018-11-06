﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Node 
 {
    public Rect rect;
    public string NodeName;
    public  QuestData Quest;
    public ParamsData Param;
    private bool _overnode;
    public bool complete;
    public List<Node> Connected;
    public bool StartNode;
    public bool FinishNode;
    public bool ConditionNode;
    public string Knows;
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
       

        if ( complete == true)
        {
            Connected.Add(this);
        }
        else if (complete == false)
        {
            if(Connected.Contains(this))
            {

                Connected.Remove(this);
            }
        }

     


    }


    
   
}
