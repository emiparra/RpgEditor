﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class QuestNodeWindow : EditorWindow
{
    private List<Node> allNodes;
    private GUIStyle style;
    private string CurrentName;
    private float toolbarHeight = 20;
    private Node _SelectedNode;
    string Nname;
    private int startNodes = 0;
    
   

    private Vector2 Graphpan;
    private Rect graphrect;
    private bool panninscreen;
    private Vector2 OriginalMousePosition;
    private Vector2 prevPan;


	[MenuItem("RPG/Quest Nodes")]
	
    public static void OpenWindow()
    {
        var Nodewindow = GetWindow<QuestNodeWindow>();
        Nodewindow.allNodes = new List<Node>();
        Nodewindow.style = new GUIStyle();
        Nodewindow.style.fontSize = 15;
        Nodewindow.style.alignment = TextAnchor.MiddleLeft;
        Nodewindow.style.fontStyle = FontStyle.Normal;

        Nodewindow.Graphpan = new Vector2(0, Nodewindow.toolbarHeight);
        Nodewindow.graphrect = new Rect(0, Nodewindow.toolbarHeight, 100000, 100000);
        
     
    }

    private void OnGUI()
    {
        CheckMouse(Event.current);
        
      
      

        graphrect.x = Graphpan.x;
        graphrect.y = Graphpan.y;
        EditorGUI.DrawRect(new Rect(0, toolbarHeight, position.width, position.height - toolbarHeight), Color.black);

        GUI.BeginGroup(graphrect);
        BeginWindows();

        var col = GUI.backgroundColor;
        

        for (int i = 0; i < allNodes.Count; i++)
        {
           

            if(allNodes[i].complete == false && allNodes[i] != _SelectedNode)
                GUI.backgroundColor = Color.red;
           else if (allNodes[i].complete == true && allNodes[i] != _SelectedNode)
                GUI.backgroundColor = Color.green;
            if (allNodes[i] == _SelectedNode)
                GUI.backgroundColor = Color.grey;

            allNodes[i].rect = GUI.Window(i, allNodes[i].rect, Drawnode, allNodes[i].NodeName);
            GUI.backgroundColor = col;
        }
        //
        EndWindows();
        GUI.EndGroup();
        
    }
    
    

    private void CheckMouse(Event currentE)
    {
        if (!graphrect.Contains(currentE.mousePosition) || !(focusedWindow == this || mouseOverWindow == this))
            return;

        if (currentE.button == 2 && currentE.type == EventType.MouseDown)
        {
            panninscreen = true;
            prevPan = new Vector2(Graphpan.x, Graphpan.y);
            OriginalMousePosition = currentE.mousePosition;

        }
        else if (currentE.button == 2 && currentE.type == EventType.MouseUp)
            panninscreen = false;
        if(panninscreen)
        {
            var newX = prevPan.x + currentE.mousePosition.x - OriginalMousePosition.x;
            Graphpan.x = newX > 0 ? 0 : newX;

            var newY = prevPan.y + currentE.mousePosition.y - OriginalMousePosition.y;
            Graphpan.y = newY > toolbarHeight ? toolbarHeight : newY;

            Repaint();

        }

        if (currentE.button == 1 && currentE.type == EventType.MouseDown && graphrect.Contains(currentE.mousePosition))
            ContextMenu();

        Node overnode = null;
        for (int i = 0; i < allNodes.Count; i++)
        {

          
           
                allNodes[i].CheckMouse(Event.current, Graphpan);
                if (allNodes[i].OverNode)
                    overnode = allNodes[i];
          

          
        }
        var prev = _SelectedNode;
        if (currentE.button == 0 && currentE.type == EventType.MouseDown)
        {
            if (overnode != null)
                _SelectedNode = overnode;
            else
                _SelectedNode = null;
            if (prev != _SelectedNode)
                Repaint();
                    
                    
                    }
    }
 
    private void ContextMenu()
    {
        GenericMenu GenericMenu = new GenericMenu();
        GenericMenu.AddItem(new GUIContent("Add Node"), false, AddNode);
        GenericMenu.AddItem(new GUIContent("Add Start Node"), false, AddStartNode);
        GenericMenu.AddItem(new GUIContent("Add Condition Node"), false, AddConditionNode);
        GenericMenu.ShowAsContext();
       

    }
    private void AddConditionNode()
    {
        CurrentName = "Condition Node";
        allNodes.Add(new Node(0, 0, 150, 150, CurrentName));
        allNodes[allNodes.Count-1].ConditionNode = true;
        Repaint();
    }

    private void AddStartNode()

    {
        if (startNodes != 0)
            return;
        else
        {
            CurrentName = "START NODE";
            allNodes.Add(new Node(0, 0, 150, 150, CurrentName));
            allNodes[0].StartNode = true;
            startNodes++;
            Repaint();
        }

    }
    private void AddNode()
    {
        if (startNodes == 0)
            return;

        CurrentName = "";
        allNodes.Add(new Node(0, 0, 150, 150, CurrentName));
        
        Repaint();
    }
    private void Drawnode(int id)
    {
        allNodes[id].checkQuest();
        if(allNodes[id].ConditionNode==true)
        {
            Debug.Log("SOOYCONDICION!");
            EditorGUILayout.LabelField("Condicion:", GUILayout.Width(50));
        }


        if (allNodes[id].StartNode == true)
            GUI.backgroundColor = Color.yellow;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Quest:", GUILayout.Width(50));

        allNodes[id].Quest = (QuestData)EditorGUILayout.ObjectField(allNodes[id].Quest, typeof(QuestData), false);



        EditorGUILayout.EndHorizontal();


        
        if (allNodes[id].Quest != null)
        {
           
          

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("complete:", GUILayout.Width(70));
            allNodes[id].complete = EditorGUILayout.Toggle (allNodes[id].complete);

            EditorGUILayout.EndHorizontal();
            
            if(allNodes[id].complete == true)
            {

                Debug.Log("estrue man");
            }
            else
            {

                Debug.Log("noooo");
            }
            
           
             
        }

        if (allNodes[id].StartNode == false)
        {
            if (allNodes[id].Quest!=null)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("knows:", GUILayout.Width(70));
                EditorGUILayout.LabelField(allNodes[id].Knows);
                EditorGUILayout.EndHorizontal();
            }
               
        }
        // EditorGUILayout.LabelField("level:" + allNodes[id].Q.data.reqLvl);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    
      
        if (GUILayout.Button("Delete"))
        {
            if(allNodes[id].StartNode==true)
            {
                startNodes--;
            }
            allNodes.RemoveAt(id);
          //  allNodes = allNodes.GetRange(0, allNodes.Count - 2);

        }

        if (!panninscreen)
        {
          
            GUI.DragWindow();
            if (!allNodes[id].OverNode) return;
            if (allNodes[id].rect.x < 0)
                allNodes[id].rect.x = 0;
            if (allNodes[id].rect.y < toolbarHeight - Graphpan.y)
                allNodes[id].rect.y = toolbarHeight - Graphpan.y;
        }
    }
}
