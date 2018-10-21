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
    private float toolbarHeight = 50;
    private Node _SelectedNode;
    string Nname;


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
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Node Name");
        Nname= GUILayout.TextField(Nname);
        
        if (GUILayout.Button("Create Node", GUILayout.Width(toolbarHeight+50), GUILayout.Height(30)))
        {
            if(Nname == "")
            {
                Debug.Log("error, no hay nombre");
            }
            else
            { 
            AddNode();
            Nname = GUILayout.TextField("");
            }
        }
        
        EditorGUILayout.EndHorizontal();

        graphrect.x = Graphpan.x;
        graphrect.y = Graphpan.y;
        EditorGUI.DrawRect(new Rect(0, toolbarHeight, position.width, position.height - toolbarHeight), Color.black);

        GUI.BeginGroup(graphrect);
        BeginWindows();
        var col = GUI.backgroundColor;
        for (int i = 0; i < allNodes.Count; i++)
        {
            if (allNodes[i] == _SelectedNode)
                GUI.backgroundColor = Color.red;
            allNodes[i].rect = GUI.Window(i, allNodes[i].rect, Drawnode, allNodes[i].NodeName);
            GUI.backgroundColor = col;
        }
        EndWindows();
        GUI.EndGroup();
        
    }

    

    private void CheckMouse(Event currentE)
    {
        if (!graphrect.Contains(currentE.mousePosition) || !(focusedWindow == this) || mouseOverWindow == this)
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

        Node overnode = null;
        for (int i = 0; i < allNodes.Count; i++)
        {
            allNodes[i].CheckMouse(Event.current, Graphpan);
            if (allNodes[i].OverNode)
                overnode = allNodes[i];
        }
        var prevsel = _SelectedNode;
        if(currentE.button==0 && currentE.type== EventType.MouseDown)
        {
            if (overnode != null)
                _SelectedNode = overnode;
            else
                _SelectedNode = null;
            if (prevsel != _SelectedNode)
                Repaint();
        }
        if (currentE.button == 1 && currentE.type == EventType.MouseDown)
            ContextMenu();

    }
    private void ContextMenu()
    {
        GenericMenu GenericMenu = new GenericMenu();
        GenericMenu.AddItem(new GUIContent("Add Node"), false, AddNode);
        GenericMenu.ShowAsContext();

    }
  
    private void AddNode()
    {
        CurrentName = Nname;
        allNodes.Add(new Node(0, 0, 150, 150, CurrentName));
        
        Repaint();
    }
    private void Drawnode(int id)
    {
       
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Quest:", GUILayout.Width(50));
        Node.Quest = (GameObject)EditorGUILayout.ObjectField(Node.Quest,typeof(GameObject),false);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Delete", GUILayout.Width(toolbarHeight), GUILayout.Height(30)))
        {
            
            allNodes.RemoveAt(id);
           
           
        }
        if(!panninscreen)
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
