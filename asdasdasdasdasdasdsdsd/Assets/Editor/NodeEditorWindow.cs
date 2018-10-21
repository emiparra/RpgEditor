using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class NodeEditorWindow : EditorWindow
{
    private List<BaseNode> allNodes;
    private GUIStyle myStyle;
    private string currentName;
    private float toolbarHeight = 100;

    private BaseNode _selectedNode;

    //para el paneo
    private bool _panningScreen;
    private Vector2 graphPan;
    private Vector2 _originalMousePosition;
    private Vector2 prevPan;
    private Rect graphRect;

    public GUIStyle wrapTextFieldStyle;

    [MenuItem("CustomTools/MyNodeEditor")]
    public static void OpenWindow()
    {
        var mySelf = GetWindow<NodeEditorWindow>();
        mySelf.allNodes = new List<BaseNode>();
        mySelf.myStyle = new GUIStyle();
        mySelf.myStyle.fontSize = 20;
        mySelf.myStyle.alignment = TextAnchor.MiddleCenter;
        mySelf.myStyle.fontStyle = FontStyle.BoldAndItalic;

        mySelf.graphPan = new Vector2(0, mySelf.toolbarHeight);
        mySelf.graphRect = new Rect(0, mySelf.toolbarHeight, 1000000, 1000000);

        //creo un style para asignar a los textos de manera que usen wordwrap
        //le paso el style por defecto como parametro para mantener el mismo "look"
        mySelf.wrapTextFieldStyle = new GUIStyle(EditorStyles.textField);
        mySelf.wrapTextFieldStyle.wordWrap = true;

      
    }

    private void OnGUI()
    {
        /*chequeamos los eventos de mouse. 
        pedir el Event.current desde adentro de la misma funcion puede traer problemas 
        (es el evento que esta ocurriendo "ahora")
        por lo que para estar seguros que nos funciona bien siempre tomamos el de OnGUI y se lo mandamos como parámetro
         */
        CheckMouseInput(Event.current);

        EditorGUILayout.BeginVertical(GUILayout.Height(100));
        EditorGUILayout.LabelField("THIS IS MY NODE EDITOR", myStyle, GUILayout.Height(50));
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        currentName = EditorGUILayout.TextField("Nombre: ", currentName);
        EditorGUILayout.Space();
        if (GUILayout.Button("Create Node", GUILayout.Width(toolbarHeight), GUILayout.Height(30)))
            AddNode();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        //Le creo un cuadrado de color para el fondo, para que se vea mas interesante.
        graphRect.x = graphPan.x;
        graphRect.y = graphPan.y;
        EditorGUI.DrawRect(new Rect(0, toolbarHeight, position.width, position.height - toolbarHeight), Color.gray);

        //utilizamos el BeginGroup para que tome todo lo que dibujamos adentro como un grupo único, lo que nos va a permitir panear el grafo.
        GUI.BeginGroup(graphRect);
        /*
         BeginWindows();
         //Habilita la opcion de que aca adentro se puedan crear "ventanas intermedias" o "ventanas dentro de ventanas".
         por lo tanto, aca adentro vamos a dibujar los nodos.
         EndWindows();
         */
        BeginWindows();
        var oriCol = GUI.backgroundColor;
        for (int i = 0; i < allNodes.Count; i++)
        {
            foreach(var c in allNodes[i].connected)
                Handles.DrawLine(new Vector2(allNodes[i].myRect.position.x + allNodes[i].myRect.width/2f, allNodes[i].myRect.position.y + allNodes[i].myRect.height/2f), new Vector2(c.myRect.position.x + c.myRect.width / 2f, c.myRect.position.y + c.myRect.height / 2f));
        }

        for (int i = 0; i < allNodes.Count; i++)
        {
            if(allNodes[i] == _selectedNode)
                GUI.backgroundColor = Color.green;

            allNodes[i].myRect = GUI.Window(i, allNodes[i].myRect, DrawNode, allNodes[i].nodeName);
            GUI.backgroundColor = oriCol;
        }
        EndWindows();
        GUI.EndGroup();
    }

    private void CheckMouseInput(Event currentE)
    {
        if (!graphRect.Contains(currentE.mousePosition) || !(focusedWindow == this || mouseOverWindow == this))
            return;

        //panning
        if (currentE.button == 2 && currentE.type == EventType.MouseDown)
        {
            _panningScreen = true;
            prevPan = new Vector2(graphPan.x, graphPan.y);
            _originalMousePosition = currentE.mousePosition;
        }
        else if (currentE.button == 2 && currentE.type == EventType.MouseUp)
            _panningScreen = false;

        if (_panningScreen)
        {
            var newX = prevPan.x + currentE.mousePosition.x - _originalMousePosition.x;
            graphPan.x = newX > 0 ? 0 : newX;

            var newY = prevPan.y + currentE.mousePosition.y - _originalMousePosition.y;
            graphPan.y = newY > toolbarHeight ? toolbarHeight : newY;

            Repaint();
        }

        //context menu
        if (currentE.button == 1 && currentE.type == EventType.MouseDown)
            ContextMenuOpen();

        //node selection
        BaseNode overNode = null;
        for (int i = 0; i < allNodes.Count; i++)
        {
            allNodes[i].CheckMouse(Event.current, graphPan);
            if (allNodes[i].OverNode)
                overNode = allNodes[i];
        }

        var prevSel = _selectedNode;
        if (currentE.button == 0 && currentE.type == EventType.MouseDown)
        {
            if (overNode != null)
                _selectedNode = overNode;
            else
                _selectedNode = null;

            if (prevSel != _selectedNode)
                Repaint();
        }
    }

    #region CONTEXT MENU
    private void ContextMenuOpen()
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Mi primer item!"), false, PrimerItem);
        menu.AddDisabledItem(new GUIContent("un item desactivado :("));
        menu.AddItem(new GUIContent("LLENO DE OPCIONES/Como un supermercado"), false, PrimerItem);
        menu.AddDisabledItem(new GUIContent("LLENO DE OPCIONES/Como cuando tu amigo se deja el face abierto"));
        menu.ShowAsContext();
    }

    private void PrimerItem()
    {
        Debug.Log("no hago nada");
    }
    #endregion

    private void AddNode()
    {
        allNodes.Add(new BaseNode(0, 0, 200, 150, currentName));
        currentName = "";
        Repaint();
    }

    private void DrawNode(int id)
    {
        //le dibujamos lo que queramos al nodo...
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Dialogo", GUILayout.Width(100));

        allNodes[id].dialogo = EditorGUILayout.TextField(allNodes[id].dialogo, wrapTextFieldStyle, GUILayout.Height(50));
        EditorGUILayout.EndHorizontal();
        allNodes[id].duration = EditorGUILayout.FloatField("Duration", allNodes[id].duration);
        var n = EditorGUILayout.TextField("Nodo:", "");
        if(n != "" && n != " ")
        {
            for (int i = 0; i < allNodes.Count; i++)
            {
                if (allNodes[i].nodeName == n)
                    allNodes[id].connected.Add(allNodes[i]);
            }
            Repaint();
        }

        if (!_panningScreen)
        {
            //esto habilita el arrastre del nodo.
            //pasandole como parámetro un Rect podemos setear que la zona "agarrable" a una específica.
            GUI.DragWindow();

            if (!allNodes[id].OverNode) return;

            //clampeamos los valores para asegurarnos que no se puede arrastrar el nodo por fuera del "área" que nosotros podemos panear
            if (allNodes[id].myRect.x < 0)
                allNodes[id].myRect.x = 0;

            if (allNodes[id].myRect.y < toolbarHeight - graphPan.y)
                allNodes[id].myRect.y = toolbarHeight - graphPan.y;
        }
    }
}
