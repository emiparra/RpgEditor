using UnityEngine;
using UnityEditor;

public class ScriptableObjectsCreator
{
    [MenuItem("RPG/Crear/Quest")]
    public static void CreateQuest()
    {
        ScriptableObjectUtility.CreateAsset<QuestData>();
    }
    [MenuItem("RPG/Crear/Parametro")]
    public static void CreateParam()
    {
        ScriptableObjectUtility.CreateAsset<ParamsData>();
    }
}