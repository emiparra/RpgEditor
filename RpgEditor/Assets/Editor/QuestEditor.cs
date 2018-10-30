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
    private Vector2 _scrollDesc;
  

    private void OnEnable()
    {
        _quest = (QuestData)target;
        _titlesLabelField = new GUIStyle();
        _titlesLabelField.fontSize = 20;
        _wrap = new GUIStyle(EditorStyles.textField);
        _wrap.wordWrap = true;


           
        

    }

    public override void OnInspectorGUI()
    {
        QuestTitleAndDescription();
        Requirements();
        Bounties();
        
    }


    void QuestTitleAndDescription()
    {
        EditorGUILayout.LabelField("Titulo de la misión", _titlesLabelField);
        GUILayout.Space(10);
        _quest.questTitle = EditorGUILayout.TextField(_quest.questTitle);
        target.name = _quest.questTitle;
        EditorGUILayout.LabelField("Descripción:", _titlesLabelField);
        GUILayout.Space(10);
        EditorGUILayout.BeginVertical(GUILayout.Height(100));
       _scrollDesc = EditorGUILayout.BeginScrollView(_scrollDesc,GUILayout.Height(100));
        _quest.questDescription = EditorGUILayout.TextField(_quest.questDescription,
            _wrap,
            GUILayout.Height(100),
            GUILayout.Width(200),
            GUILayout.ExpandWidth(false));
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }



    void Bounties()
    {
        EditorGUILayout.LabelField("Recompensas:", _titlesLabelField);
        GUILayout.Space(10);
        _quest.experienceGained = EditorGUILayout.IntField("Experiencia", _quest.experienceGained);
        _quest.creditsGained = EditorGUILayout.IntField("Creditos   ", _quest.creditsGained);
        GUILayout.Space(10);
    }



    void Requirements()
    {
        EditorGUILayout.LabelField("Requisitos:", _titlesLabelField);
        GUILayout.Space(10);
        _quest.reqLvl = EditorGUILayout.IntField("Nivel Necesario", _quest.reqLvl);
        _quest.reqItem = EditorGUILayout.TextField("Objeto Necesario", _quest.reqItem);
        _quest.reqKnows = EditorGUILayout.TextField("Conocer a", _quest.reqKnows);
        _quest.reqExplore = EditorGUILayout.TextField("Haber explorado", _quest.reqExplore);
        _quest.reqKills = EditorGUILayout.TextField("Haber matado a", _quest.reqKills);
    }

 
       /*if (AssetDatabase.LoadAssetAtPath(localPath, typeof(Quest)))
        {
            if (EditorUtility.DisplayDialog("Estas seguro?",
                                "Esta mision ya existe en el registro, ¿queres sobreescribirla?",
                                "Si, al toque vieja",
                                "No, era mentira"))
       */

    


}
