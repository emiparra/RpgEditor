using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Quest))]
public class QuestEditor : Editor {

    private Quest _quest;

    private void OnEnable()
    {
        _quest = (Quest)target;
        target.name = _quest.questTitle;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Titulo de la mision");
        _quest.questTitle = EditorGUILayout.TextField(_quest.questTitle);
        EditorGUILayout.LabelField("Descripcion de la mision");
        _quest.questDescription = EditorGUILayout.TextField(_quest.questDescription,
            GUILayout.Height(100),
            GUILayout.Width(200),
            GUILayout.ExpandWidth(false));
        EditorGUILayout.LabelField("Recompensas:");
        _quest.experienceGained = EditorGUILayout.IntField("Experiencia",_quest.experienceGained);
        _quest.creditsGained = EditorGUILayout.IntField("Creditos   ", _quest.creditsGained);
        GUILayout.Space(10);

        var saveButton = GUILayout.Button("Guardar");
        if (saveButton) SavePrefab();
        
    }

    void SavePrefab()
    {
        string localPath = "Assets/" + target.name + ".prefab";
        var prefab = PrefabUtility.CreatePrefab(localPath, _quest.gameObject);
        PrefabUtility.ReplacePrefab(_quest.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);


        /*if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)))
        {
         
            if (EditorUtility.DisplayDialog("Are you sure?",
                    "The prefab already exists. Do you want to overwrite it?",
                    "Yes",
                    "No"))
            {
                var prefab = PrefabUtility.CreatePrefab(localPath, _quest.gameObject);
                PrefabUtility.ReplacePrefab(_quest.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
            }
        }*/
    }

    
}
