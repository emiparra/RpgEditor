using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;


[CustomEditor(typeof(ParamsData))]
public class ParamsEditor : Editor {

    private ParamsData _params;
    private GUIStyle _titlesLabelField;

    private void OnEnable()
    {
        _params = (ParamsData)target;
        _titlesLabelField = new GUIStyle();
        _titlesLabelField.fontSize = 20;
        _titlesLabelField.fontStyle = FontStyle.BoldAndItalic;
    }

    public override void OnInspectorGUI()
    {
        GUI.DrawTexture(new Rect(0, 43, Screen.width, Screen.height), (Texture2D)Resources.Load("parchment_paper_detail"), ScaleMode.StretchToFill, true, 10f);
        Requirements();
    }

    void Requirements()
    {
        Separator("divider1", 25f);
        EditorGUILayout.LabelField("Requisitos:", _titlesLabelField);
        GUILayout.Space(10);
        _params.reqLvl = EditorGUILayout.IntField("Nivel Necesario", _params.reqLvl);
        _params.reqItem = EditorGUILayout.TextField("Objeto Necesario", _params.reqItem);
        _params.reqKnows = EditorGUILayout.TextField("Conocer a", _params.reqKnows);
        _params.reqExplore = EditorGUILayout.TextField("Haber explorado", _params.reqExplore);
        _params.reqKills = EditorGUILayout.TextField("Haber matado a", _params.reqKills);
        Separator("divider1", 25f);
    }

    void Separator(string img, float hgt)
    {
        GUILayout.Space(10);
        GUI.DrawTexture(GUILayoutUtility.GetRect(Screen.width, hgt), (Texture2D)Resources.Load(img), ScaleMode.StretchToFill, true, 10f);
        GUILayout.Space(10);
    }



}
