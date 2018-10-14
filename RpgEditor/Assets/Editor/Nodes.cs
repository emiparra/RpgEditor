using System;
using UnityEditor;
using UnityEngine;

public class Nodes {

    public Rect rect;
    public string title;
    public GUIStyle style;
    public bool Dragged;

    public Nodes(Vector2 position, float width, float height, GUIStyle nodestyle)
    {
        rect = new Rect(position.x, position.y, width, height);
        style = nodestyle;
    }
    public void Drag (Vector2 delta)
    {
        rect.position += delta;
    }
    public void draw()
    {
        GUI.Box(rect, title, style);
    }
    public bool Events(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if(e.button== 0)
                {
                    if(rect.Contains(e.mousePosition))
                    {
                        Dragged = true;
                        GUI.changed = true;
                    }
                    else
                    {
                        GUI.changed = true;
                    }
                   
                }
                break;
            case EventType.MouseUp:
                Dragged = false;
                break;
            case EventType.MouseDrag:
                if(e.button ==0 && Dragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }
        return false;
    }

}
