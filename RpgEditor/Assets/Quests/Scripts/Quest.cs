using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest : MonoBehaviour
{
    public string questTitle;
    public string questDescription;
    public int experienceGained;
    public int creditsGained;
    public bool showQuest;
    public int reqLvl;
    public GameObject reqItem;
    public string reqKnows;

    public string json;
	



}
