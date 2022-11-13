using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    [SerializeField]
    List<GameObject> figures = new List<GameObject>();
    public static PrefabHolder instance;
    public void Start()
    {
        instance = this;
    }
    public GameObject getFigureByType(int figureType)
    {
        foreach (GameObject obj in figures)
            if (obj.GetComponent<FigureInfo>().figureType == figureType)
                return obj;
        return null;
    }
}
