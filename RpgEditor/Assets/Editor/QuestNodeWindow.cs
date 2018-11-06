using System.Collections;
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


            if (allNodes[i].Quest != null)           
            {
                if (allNodes[i].complete == true)
                {
                    if (allNodes[i + 1] != null)
                    {
                     Handles.DrawLine(new Vector2(allNodes[i].rect.position.x + allNodes[i].rect.width / 5f, allNodes[i].rect.position.y + allNodes[i].rect.height / 5f),
                     new Vector2(allNodes[i + 1].rect.position.x + allNodes[i + 1].rect.width / 5f, allNodes[i + 1].rect.position.y + allNodes[i + 1].rect.height / 5f));
                    }
                    else
                    {
                        Debug.Log("me pa que ganaste");
                      
                    }

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
    private void AddFinishNode()
    {
        if (finish == true)
            return;
        else
        {

            CurrentName = "Finish Node";
            allNodes.Add(new Node(700, 0, 150, 200, CurrentName));
            allNodes[allNodes.Count - 1].FinishNode = true;
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

    private void AddStartNode()

    {
        if (start==true)
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
    private void AddNode()
    {

        CurrentName = "";
        allNodes.Add(new Node(0, 0, 150, 200, CurrentName));
        
        Repaint();
    }
    private void Drawnode(int id)
    {
       if(allNodes[id].FinishNode==true)
        {
            
            allNodes[id] = allNodes[allNodes.Count-1];
        }
        
        allNodes[id].checkQuest();
        if(allNodes[id].ConditionNode==true)
        {
            EditorGUILayout.LabelField("Condicion:", GUILayout.Width(50));
        }      
            if (allNodes[id].StartNode == true)
            GUI.backgroundColor = Color.yellow;

            if(allNodes[id].FinishNode==true)
            GUI.backgroundColor = Color.blue;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Quest:", GUILayout.Width(50));

        allNodes[id].Quest = (QuestData)EditorGUILayout.ObjectField(allNodes[id].Quest, typeof(QuestData), false);

        EditorGUILayout.EndHorizontal();

        if (allNodes[id].Quest != null)
        {
            if(allNodes[id].FinishNode==false)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("complete:", GUILayout.Width(70));
                allNodes[id].complete = EditorGUILayout.Toggle(allNodes[id].complete);
                EditorGUILayout.EndHorizontal();
            }
           
            if (allNodes[id].StartNode == false)
            {         
                space = 150;
                /*EditorGUILayout.LabelField("Know: " + allNodes[id].Quest.reqKnows, GUILayout.Width(space));
     
                EditorGUILayout.LabelField("Item: " + allNodes[id].Quest.reqItem, GUILayout.Width(space));
              
                EditorGUILayout.LabelField("Kills: " + allNodes[id].Quest.reqKills, GUILayout.Width(space));
             
                EditorGUILayout.LabelField("Explore: " + allNodes[id].Quest.reqExplore, GUILayout.Width(space));
             
                EditorGUILayout.LabelField("item: " + allNodes[id].Quest.reqItem, GUILayout.Width(space));*/
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
