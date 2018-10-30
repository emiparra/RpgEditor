using UnityEngine;
using UnityEditor;

public class ScriptableObjectsCreator
{
    [MenuItem("RPG/Crear Quest")]
    public static void CreateCharacterConfig()
    {
        ScriptableObjectUtility.CreateAsset<QuestData>();
    }
}