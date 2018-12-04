using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Connection
{
    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    public Action<Connection> OnClickRemoveConnection;

    public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> OnClickRemoveConnection)
    {
        this.inPoint = inPoint;
        this.outPoint = outPoint;
        this.OnClickRemoveConnection = OnClickRemoveConnection;
    }

    public void Draw()
    {
        Handles.DrawBezier(inPoint.node.rect.center,outPoint.node.rect.center,inPoint.node.rect.center + Vector2.left * 50f,outPoint.node.rect.center - Vector2.left * 50f,
            Color.white,
            null,
            2f
        );
        if(outPoint.node.next.Contains(inPoint.node) == false)
        {
            outPoint.node.next.Add(inPoint.node);
        }
        if(inPoint.node.previous.Contains(outPoint.node) == false)
        {
            inPoint.node.previous.Add(outPoint.node);
        }
        
        
        
        if (Handles.Button((inPoint.node.rect.center + outPoint.node.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
        {
            if (OnClickRemoveConnection != null)
            {
                OnClickRemoveConnection(this);
            }
        }
    }
}
