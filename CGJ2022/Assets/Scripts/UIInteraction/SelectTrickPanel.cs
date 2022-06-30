using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectTrickPanel : MonoBehaviour
{
    private static SelectTrickPanel instance;

    public static SelectTrickPanel Instacne
    {
        get { return instance; }
    }

    public bool isGood;
    public List<GameObject> MarkObjs;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void MarkActiveTrue()
    {
        foreach (GameObject mark in MarkObjs)
        {
            mark.SetActive(true);
        }
        SelectObjManager.Instance.canPlace = true;
    }

    public void MarkActiveFalse()
    {
        foreach (GameObject mark in MarkObjs)
        {
            mark.SetActive(false);
        }
        SelectObjManager.Instance.canPlace = false;
    }
   public void GoodTrickButtonPressed()
    {
        MarkActiveTrue();
        isGood = true;
        //设置好的积极的trick在目标上
    }
    
   public void BadTrickButtonPressed()
    {
        MarkActiveTrue();
        isGood = false;
        //设置坏的消极的trick在目标上

    }

   public void PlaceTrick()
   {
       if (isGood) PlaceGood();
       else PlaceBad();
   }
   void PlaceGood()
   {
       
       Debug.Log("放好的");
   }
   void PlaceBad()
   {
       Debug.Log("放坏的");
   }
}
