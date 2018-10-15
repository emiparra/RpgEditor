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


    private bool _panningScreen;
    private Vector2 graphpan;
    private Vector2 _originalMousePosition;
    private Vector2 prevPan;
    private Rect graphRect;

    public GUIStyle Wrapstyle;
	[MenuItem("RPG/Quest Nodes")]
	
    public static void OpenWindow()
    {
        var Nodewindow = GetWindow<QuestNodeWindow>();
        Nodewindow.allNodes = new List<Node>();
        Nodewindow.style = new GUIStyle();
        Nodewindow.style.fontSize = 15;
        Nodewindow.style.alignment = TextAnchor.MiddleLeft;
        Nodewindow.style.fontStyle = FontStyle.Normal;

        Nodewindow.graphpan = new Vector2(0, Nodewindow.toolbarHeight);
        Nodewindow.graphRect = new Rect(0, Nodewindow.toolbarHeight, 10000, 10000);

        Nodewindow.Wrapstyle = new GUIStyle(EditorStyles.textField);
        Nodewindow.Wrapstyle.wordWrap = true;
    }

    private void OnGUI()
    {
        CheckMouse(Event.current);
        if (GUILayout.Button("Create Node", GUILayout.Width(toolbarHeight), GUILayout.Height(30)))
            AddNode();
    }
    private void CheckMouse(Event E)
    {
        if (!graphRect.Contains(E.mousePosition) || !(focusedWindow == this) || mouseOverWindow == this)
            return;
        if (E.button == 2 && E.type == EventType.MouseDown)
        {
            _panningScreen = true;
            prevPan = new Vector2(graphpan.x, graphpan.y);
            _originalMousePosition = E.mousePosition;
        }
        else if (E.button == 2 && E.type == EventType.MouseUp)
            _panningScreen = false;

        if(_panningScreen)
        {
            var newX = prevPan.x + E.mousePosition.x - _originalMousePosition.x;
            graphpan.x = newX > 0 ? 0 : newX;

            var newY = prevPan.y + E.mousePosition.y - _originalMousePosition.y;
            graphpan.y = newY > toolbarHeight ? toolbarHeight : newY;
            Repaint();
        }
        Node overNode = null;
        for (int i = 0; i < allNodes.Count; i++)
        {
            allNodes[i].CheckMouse(Event.current, graphpan);
            if (allNodes[i].OverNode)
                overNode = allNodes[i];
        }
        var prevSel = _SelectedNode;
        if(E.button == 0 && E.type == EventType.MouseDown)
        {
            if (overNode != null)
                _SelectedNode = overNode;
            else
                _SelectedNode = null;
            if (prevSel != _SelectedNode)
                Repaint();

        }
        
    }
    private void AddNode()
    {
        allNodes.Add(new Node(0, 0, 200, 150, CurrentName));
        CurrentName = "hola";
        Repaint();
    }
    private void Drawnode(int id)
    {
        //  EditorGUILayout.ObjectField("OB",Node.Quest);
        EditorGUILayout.LabelField("Quest", GUILayout.Width(50));
        
        if(!_panningScreen)
        {
            GUI.DragWindow();

            if (allNodes[id].rect.x < 0)
                allNodes[id].rect.x = 0;
            if (allNodes[id].rect.y < toolbarHeight - graphpan.y)
                allNodes[id].rect.y = toolbarHeight - graphpan.y;
        }
    }
}
