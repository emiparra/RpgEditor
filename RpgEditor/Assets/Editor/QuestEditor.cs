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
       // SaveQuest();
        
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
        _quest.reqLvl = EditorGUILayout.IntField("Nivel Necesario", _quest.reqLvl);
        _quest.reqItem = (GameObject)EditorGUILayout.ObjectField("Objeto Necesario",_quest.reqItem,typeof(GameObject),true);
        _quest.reqKnows = EditorGUILayout.TextField("Conocer a",_quest.reqKnows);
    }

   /*region Guardar Mision


    /*void SaveQuest()
    {
        var saveButton = GUILayout.Button("Guardar");
        if (saveButton) SavePrefab();
    }*/

   /* void SavePrefab()
    {
      
        var localPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Quests/Quest Register/" + typeof(QuestEditor).ToString() + ".asset");
        if (AssetDatabase.LoadAssetAtPath(localPath, typeof(Quest)))
        {
            if (EditorUtility.DisplayDialog("Estas seguro?",
                                "Esta mision ya existe en el registro, ¿queres sobreescribirla?",
                                "Si, al toque vieja",
                                "No, era mentira"))
            {
                ScriptableObjectUtility.CreateAsset<QuestEditor>();
            }
        }
        else
        {
            ScriptableObjectUtility.CreateAsset<QuestEditor>();
        }   

    }*/

    /*void CreatePrefab(string path)
    {
        _quest.json = JsonUtility.ToJson(_quest);
        Debug.Log(_quest.json);
        var prefab = PrefabUtility.CreatePrefab(path, _quest.gameObject);
        PrefabUtility.ReplacePrefab(_quest.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
        Clear();
    }*/

    /*void Clear()
    {
        _quest.questTitle = "Hola buenas tardes";
        _quest.questDescription = "";
        _quest.reqLvl = 0;
        _quest.reqItem = null;
        _quest.reqKnows = "";
        _quest.experienceGained = 0;
        _quest.creditsGained = 0;
    }
    #endregion*/


}
