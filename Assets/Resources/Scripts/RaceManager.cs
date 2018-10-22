using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager instance;

    public Transform cpParent;
    public List<Transform> cpList = new List<Transform>();
    public int CPCount;

    private void Awake()
    {


        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        FillCpList();
    }

    private void FillCpList()
    {
        foreach (Transform t in cpParent)
        {
            cpList.Add(t);
        }
        CPCount = cpList.Count;
    }
}


