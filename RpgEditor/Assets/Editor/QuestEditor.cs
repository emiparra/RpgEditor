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
        
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Titulo de la mision");
        _quest.questTitle = EditorGUILayout.TextField(_quest.questTitle);
        target.name = _quest.questTitle;
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

    #region Guardar Mision

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
