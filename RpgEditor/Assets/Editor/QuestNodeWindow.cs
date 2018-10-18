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
    private float toolbarHeight = 100;
    private Node _SelectedNode;
    string Nname;


	[MenuItem("RPG/Quest Nodes")]
	
    public static void OpenWindow()
    {
        var Nodewindow = GetWindow<QuestNodeWindow>();
        Nodewindow.allNodes = new List<Node>();
        Nodewindow.style = new GUIStyle();
        Nodewindow.style.fontSize = 15;
        Nodewindow.style.alignment = TextAnchor.MiddleLeft;
        Nodewindow.style.fontStyle = FontStyle.Normal;

     
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Node Name");
        Nname= GUILayout.TextField(Nname);
        
        if (GUILayout.Button("Create Node", GUILayout.Width(toolbarHeight), GUILayout.Height(30)))
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
        BeginWindows();
     
     
        for (int i = 0; i < allNodes.Count; i++)
        {
            if (allNodes[i] == _SelectedNode)
                GUI.backgroundColor = Color.black;
            allNodes[i].rect = GUI.Window(i, allNodes[i].rect, Drawnode, allNodes[i].NodeName);
         
        }
        EndWindows();
        
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

           
            GUI.DragWindow();

      
    }
}
