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
    public int space;
    private bool finish= false;
    private bool start= false;
    public bool lining;
    public Node save;
  
    private Vector2 Graphpan;
    private Rect graphrect;
    private bool panninscreen;
    private Vector2 OriginalMousePosition;
    private Vector2 prevPan;
    private Vector2 P1;
    private Vector2 P2;



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

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Load", GUILayout.Width(50));

        //(QuestNodeWindow)EditorGUILayout.ObjectField(load,typeof(QuestNodeWindow) , false); if()

        /*

            if (GUILayout.Button("SAVE"))
        {
            ScriptableObjectsCreator.CreateNodeWindow();
        }*/

        EditorGUILayout.EndHorizontal();


        graphrect.x = Graphpan.x;
        graphrect.y = Graphpan.y;
        EditorGUI.DrawRect(new Rect(0, toolbarHeight, position.width, position.height - toolbarHeight), Color.black);

        GUI.BeginGroup(graphrect);
        BeginWindows();

        var col = GUI.backgroundColor;


        for (int i = 0; i < allNodes.Count; i++)
        {


            if (allNodes[i].complete == false && allNodes[i] != _SelectedNode)
                GUI.backgroundColor = Color.red;
            else if (allNodes[i].complete == true && allNodes[i] != _SelectedNode)
                GUI.backgroundColor = Color.green;
            if (allNodes[i] == _SelectedNode)
                GUI.backgroundColor = Color.grey;

            allNodes[i].rect = GUI.Window(i, allNodes[i].rect, Drawnode, allNodes[i].NodeName);
            GUI.backgroundColor = col;

            


             if (allNodes[i].Quest != null)           
             {
                if (allNodes[i].selected == true)
                {
                    if (allNodes[i].FinishNode == false)
                    {

                        Handles.DrawLine(P1, allNodes[i].rect.position);
                        
                        
                    }

                }


                if (allNodes[i].complete == true)
                 {

                     Handles.DrawLine(new Vector2(allNodes[i].rect.position.x + allNodes[i].rect.width / 5f, allNodes[i].rect.position.y + allNodes[i].rect.height / 5f),
                     new Vector2(allNodes[i + 1].rect.position.x + allNodes[i + 1].rect.width / 5f, allNodes[i + 1].rect.position.y + allNodes[i + 1].rect.height / 5f));
                 }

               

                 }

             }


            EndWindows();
            GUI.EndGroup();
            AddStartNode();
            AddFinishNode();
            Repaint();

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
        if (_SelectedNode != null)
        {
            var nod = new Vector2(_SelectedNode.rect.x + _SelectedNode.rect.width, _SelectedNode.rect.y + _SelectedNode.rect.height);
            if (currentE.button == 2 && currentE.type == EventType.MouseDown && graphrect.Contains(nod))
            {
                
                lining = true;

              

            }
          
            if (lining == true)
            {
                P1 = _SelectedNode.rect.position;
                
              
                
            }
            if ( lining == true && currentE.button == 2 && currentE.type == EventType.MouseDown )
            {
                Debug.Log("aca funciona");
                P2 = _SelectedNode.rect.position;
                _SelectedNode.selected = true;
               
            }


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
        GenericMenu.AddItem(new GUIContent("Add Condition Node"), false, AddConditionNode);
        GenericMenu.ShowAsContext();
       

    }
    private void AddStartNode()

    {
        if (start == true)
            return;
        else
        {
            CurrentName = "START NODE";
            allNodes.Add(new Node(0, 0, 150, 200, CurrentName));
            allNodes[0].StartNode = true;

            start = true;
            Repaint();
        }

    }
    private void AddFinishNode()
    {
        if (finish == true)
            return;
        else
        {

            CurrentName = "Finish Node";
            allNodes.Add(new Node(700, 0, 150, 200, CurrentName));
            allNodes[1].FinishNode = true;
            finish = true;
            Repaint();
        }
    }
    private void AddConditionNode()
    {
        CurrentName = "Condition Node";
        allNodes.Add(new Node(0, 0, 150, 200, CurrentName));
        allNodes[allNodes.Count-1].ConditionNode = true;
        
        Repaint();
    }

  
    private void AddNode()
    {

        CurrentName = "";
        allNodes.Add(new Node(0, 0, 150, 200, CurrentName));
        
        Repaint();
    }
    private void Drawnode(int id)
    {
      
       
      
        allNodes[id].checkQuest();
        
        if(allNodes[id].ConditionNode==true)
        {
            space = 150;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Condicion:", GUILayout.Width(50));
            allNodes[id].Param = (ParamsData)EditorGUILayout.ObjectField(allNodes[id].Param, typeof(ParamsData), false);

            EditorGUILayout.EndHorizontal();
            if(allNodes[id].Param!=null)
            {
                EditorGUILayout.LabelField("Know: " + allNodes[id].Param.reqKnows, GUILayout.Width(space));

                EditorGUILayout.LabelField("Item: " + allNodes[id].Param.reqItem, GUILayout.Width(space));

                EditorGUILayout.LabelField("Kills: " + allNodes[id].Param.reqKills, GUILayout.Width(space));

                EditorGUILayout.LabelField("Explore: " + allNodes[id].Param.reqExplore, GUILayout.Width(space));

                EditorGUILayout.LabelField("item: " + allNodes[id].Param.reqItem, GUILayout.Width(space));
            }
           

        }      
            if (allNodes[id].StartNode == true)
            GUI.backgroundColor = Color.yellow;

            if(allNodes[id].FinishNode==true)
            GUI.backgroundColor = Color.yellow;

            if(allNodes[id].ConditionNode==false
            && allNodes[id].FinishNode==false)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Quest:", GUILayout.Width(50));

            allNodes[id].Quest = (QuestData)EditorGUILayout.ObjectField(allNodes[id].Quest, typeof(QuestData), false);

            EditorGUILayout.EndHorizontal();
        }
       

        if (allNodes[id].Quest != null)
        {
            if(allNodes[id].FinishNode==false)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("complete:", GUILayout.Width(70));
                allNodes[id].complete = EditorGUILayout.Toggle(allNodes[id].complete);
                EditorGUILayout.EndHorizontal();
            } 
               
        }
        if(allNodes[id].StartNode==false && allNodes[id].FinishNode==false)
        {
            if (GUILayout.Button("Delete"))
            {

                allNodes.RemoveAt(id);


            }
        }
       

        if (!panninscreen)
        {         
            GUI.DragWindow();
        }
    }

   
}
