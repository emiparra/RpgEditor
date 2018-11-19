using UnityEngine;
using UnityEditor;

public class ScriptableObjectsCreator
{
    [MenuItem("RPG/Crear/Quest")]
    public static void CreateQuest()
    {
        var path = "Assets/Quests/Quest Register/";
        ScriptableObjectUtility.CreateAsset<QuestData>(path);
    }
    [MenuItem("RPG/Crear/Parametro")]
    public static void CreateParam()
    {
        var path = "Assets/Quests/Params Register/";
        ScriptableObjectUtility.CreateAsset<ParamsData>(path);
    }
   
    public static void CreateNodeWindow()
    {
        var path = "Assets/saveWindow/";
        ScriptableObjectUtility.CreateAsset<Node>(path);
    }
}