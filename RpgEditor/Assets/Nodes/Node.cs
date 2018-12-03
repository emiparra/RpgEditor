using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Node:EditorWindow
{ 
    public Rect rect;
    public bool connected=false;
    public string  Ntitle;
    public bool isDragged;
    public bool isSelected;
    public string NodeName;
    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    public bool Start = false;
    public bool Finish = false;
    public bool Condition = false;
    public ParamsData Param;
    public QuestData Quest;
    public int space;
    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;
    public int level;
    public string know;
    public string killed;
    public string item;
    public string explore;
    public bool currentQuest;
    public List<Node> next = new List<Node>();
    public List<Node> previous = new List<Node>();

    public Action<Node> OnRemoveNode;
    public UnityEngine.Object nn;
   

    public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
    {
       
        rect = new Rect(position.x, position.y, 220,220);
        style = nodeStyle;
        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);
        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
    }

    public void Draw()
    {

       
        inPoint.Draw();
        outPoint.Draw();
    

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Start:", GUILayout.Width(70));
        Start = EditorGUILayout.Toggle(Start);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Finish:", GUILayout.Width(70));
        Finish = EditorGUILayout.Toggle(Finish);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Condition:", GUILayout.Width(70));
        Condition = EditorGUILayout.Toggle(Condition);
        EditorGUILayout.EndHorizontal();
       
        if (Finish==false && Condition == false)
        {
           
           Quest = (QuestData)EditorGUILayout.ObjectField(Quest, typeof(QuestData), true);
            Param = (ParamsData)EditorGUILayout.ObjectField(Param, typeof(ParamsData), true);
            currentQuest = EditorGUILayout.Toggle("Current Quest", currentQuest);
        }
        if (Start == true)
        {
          
            Finish = false;
            Condition = false;
           
        }
        if (Condition == true)
        {
         
            Start = false;
            Finish = false;
            EditorGUILayout.LabelField("Condicion:", GUILayout.Width(70));
            level = EditorGUILayout.IntField("Level", level);
            killed = EditorGUILayout.TextField("Killed", killed);
            item = EditorGUILayout.TextField("Item", item);
            explore = EditorGUILayout.TextField("Explored", explore);
            know = EditorGUILayout.TextField("Know", know);
            if (GUILayout.Button("CheckParams")) CheckParams();

        }
        if (Finish == true)
        {
           
            Start = false;
            Condition = false;
        }

       
    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (rect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    }
                    else
                    {
                        GUI.changed = true;
                        isSelected = false;
                        style = defaultNodeStyle;
                    }
                }
                if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
                {
                    ProcessContextMenu();
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }
        return false;
    }
    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    private void OnClickRemoveNode()
    {
        if (OnRemoveNode != null)
        {
            OnRemoveNode(this);
        }
    }


   
    public void CheckParams()
    {
        foreach (var item in previous)
        {
            Debug.Log(item.Param);
        }
    }



}
