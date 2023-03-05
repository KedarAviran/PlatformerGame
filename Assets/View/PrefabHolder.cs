using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    [SerializeField]
    List<GameObject> maps = new List<GameObject>();
    [SerializeField]
    List<GameObject> figures = new List<GameObject>();
    [SerializeField]
    List<GameObject> skills = new List<GameObject>();
    [SerializeField]
    GameObject damagePopup;
    [SerializeField]
    AudioClip jump, hit;
    public static PrefabHolder instance;
    public void Awake()
    {
        instance = this;
    }
    public AudioClip getJumpSound()
    {
        return jump;
    }
    public AudioClip getHitSound()
    {
        return hit;
    }
    public GameObject getDamagePopup()
    {
        return damagePopup;
    }
    public GameObject getMapByID(int mapID)
    {
        return maps[mapID];
    }
    public GameObject getFigureByType(int figureType)
    {
        foreach (GameObject obj in figures)
            if (obj.GetComponent<FigureInfo>().figureType == figureType)
                return obj;
        return null;
    }
    public GameObject getSkillByID(int skillID)
    {
        foreach (GameObject skill in skills)
            if (skill.GetComponent<SkillScript>().skillID == skillID)
                return skill;
        return null;
    }
}
