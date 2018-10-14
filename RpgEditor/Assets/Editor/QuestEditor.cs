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
        EditorGUILayout.LabelField("UN LABEL");
        _quest.questDescription = EditorGUILayout.TextField(_quest.questDescription);
    }
}
