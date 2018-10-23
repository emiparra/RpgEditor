using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Quest))]
public class QuestEditor : Editor {

    private Quest _quest;
    private GUIStyle _titlesLabelField;
    private GUIStyle _wrap;
    private Vector2 _scrollDesc;

    private void OnEnable()
    {
        _quest = (Quest)target;
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
        SaveQuest(); 
    }


    void QuestTitleAndDescription()
    {
        EditorGUILayout.LabelField("Titulo de la misión", _titlesLabelField);
        GUILayout.Space(10);
        _quest.questTitle = EditorGUILayout.TextField(_quest.questTitle);
        target.name = _quest.questTitle;
        EditorGUILayout.LabelField("Descripción:", _titlesLabelField);
        GUILayout.Space(10);
       // _scrollDesc = EditorGUILayout.BeginScrollView(_scrollDesc,GUILayout.Width(200),GUILayout.Height(100));
        _quest.questDescription = EditorGUILayout.TextField(_quest.questDescription,
            _wrap,
            GUILayout.Height(100),
            GUILayout.Width(200),
            GUILayout.ExpandWidth(false));
        //EditorGUILayout.EndScrollView();
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
        _quest.reqLvl = EditorGUILayout.IntField(_quest.reqLvl);
    }

    #region Guardar Mision


    void SaveQuest()
    {
        var saveButton = GUILayout.Button("Guardar");
        if (saveButton) SavePrefab();
    }

    void SavePrefab()
    {
        string localPath = "Assets/Quests/Quest Register/" + target.name + ".prefab";

        if (AssetDatabase.LoadAssetAtPath(localPath, typeof(Quest)))
        {
            if (EditorUtility.DisplayDialog("Estas seguro?",
                                "Esta mision ya existe en el registro, ¿queres sobreescribirla?",
                                "Si, al toque vieja",
                                "No, era mentira"))
            {
                CreatePrefab(localPath);
            }
        }
        else
        {
            CreatePrefab(localPath);
        }   

    }
    void CreatePrefab(string path)
    {
        var prefab = PrefabUtility.CreatePrefab(path, _quest.gameObject);
        PrefabUtility.ReplacePrefab(_quest.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
    }
    #endregion


}
