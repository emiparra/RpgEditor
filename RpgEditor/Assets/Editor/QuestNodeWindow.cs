using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestNodeWindow : EditorWindow {


    private List<Nodes> nodes;
    private GUIStyle nodeStyle;
	[MenuItem("RPG/Quest Nodes")]
	private static void OpenWindow()
    {
        QuestNodeWindow window = GetWindow<QuestNodeWindow>();
        window.titleContent = new GUIContent("Quest Nodes");
    }

    private void OnEnable()
    {
        nodeStyle = new GUIStyle();
        nodeStyle.border = new RectOffset(10, 10, 10, 10);
    }
    private void OnGUI()
    {
        DrawNodes();
        Events(Event.current);
        

        if (GUI.changed)
            Repaint();
    }
    private void DrawNodes()
    {
        if(nodes != null)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].draw();
            }
        }
    }
    private void Events(Event e)
    {
       switch(e.type)
        {
            case EventType.MouseDown:
                if(e.button == 1)
                {
                    processContextMenu(e.mousePosition);

                }
                break;
        }


    }

    private void ProcessNodeEvent(Event e)
    {
        if(nodes != null)
        {
            for (int i = nodes.Count -1; i >=0; i--)
            {
                bool GuiChanged = nodes[i].Events(e);
                if(GuiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
    }
    private void processContextMenu(Vector2 Mouseposition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add Node"), false, () => OnClickAddNode(Mouseposition));
        genericMenu.ShowAsContext();
    }

    private void OnClickAddNode(Vector2 Mouseposition)
    {
        if(nodes == null)
        {
            nodes = new List<Nodes>();
        }
        nodes.Add(new Nodes(Mouseposition, 200, 50, nodeStyle));
    }

}
