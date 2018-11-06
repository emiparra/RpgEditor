using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

[CustomEditor(typeof(QuestData))]
public class QuestEditor : Editor {

    private QuestData _quest;
    private GUIStyle _titlesLabelField;
    private GUIStyle _wrap;
    private GUIStyle _newFont;
    private Vector2 _scrollDesc;
  

    private void OnEnable()
    {
        _quest = (QuestData)target;
        _titlesLabelField = new GUIStyle();
        _titlesLabelField.fontSize = 20;
        _titlesLabelField.fontStyle = FontStyle.BoldAndItalic;
        _newFont = new GUIStyle();
        _newFont.fontStyle = FontStyle.Italic;
        _newFont.fontSize = 12;
        _wrap = new GUIStyle(EditorStyles.textArea);
        _wrap.wordWrap = true;
        _wrap.fontStyle = FontStyle.Italic;


           
        

    }

    public override void OnInspectorGUI()
    {
        FontAndBackground();
        QuestTitleAndDescription();
        Bounties();
        GUILayout.Space(300);
        _quest.fantasieFont = (Font)EditorGUILayout.ObjectField("Fuente", _quest.fantasieFont, typeof(Font), true);

    }


    void FontAndBackground()
    {
        GUI.DrawTexture(new Rect(0, 43, Screen.width, Screen.height),
            (Texture2D)Resources.Load("parchment_paper_detail"),
            ScaleMode.StretchToFill, true, 10f);
       


    }


    void QuestTitleAndDescription()
    {
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Titulo de la misión", _titlesLabelField);
        GUILayout.Space(10);
        _quest.questTitle = EditorGUILayout.TextField(_quest.questTitle,_newFont);
        Separator("divider1",25f);
        target.name = _quest.questTitle;
        EditorGUILayout.LabelField("Descripción:", _titlesLabelField);
        GUILayout.Space(10);
        _quest.questDescription = EditorGUILayout.TextArea(_quest.questDescription,
            _wrap,
            GUILayout.Height(100),
            GUILayout.Width(200),
            GUILayout.ExpandWidth(false),
            GUILayout.ExpandHeight(true));
        Separator("divider1",25f);
 

    }



    void Bounties()
    {
        EditorGUILayout.LabelField("Recompensas:", _titlesLabelField);
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Experiencia", _newFont);
        _quest.experienceGained = EditorGUILayout.IntField(_quest.experienceGained,_newFont);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Creditos", _newFont);
        _quest.creditsGained = EditorGUILayout.IntField(_quest.creditsGained,_newFont);
        GUILayout.EndHorizontal();
        Separator("divider2",70f);

    }


    void Separator(string img,float hgt)
    {
        GUILayout.Space(10);
        GUI.DrawTexture(GUILayoutUtility.GetRect(Screen.width, hgt),
            (Texture2D)Resources.Load(img),
            ScaleMode.StretchToFill,
            true,
            10f);
        GUILayout.Space(10);
    }





}
