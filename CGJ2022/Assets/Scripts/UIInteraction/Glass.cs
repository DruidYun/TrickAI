using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour,IPlaceable
{
    // Start is called before the first frame update
    public GameObject mark;
    public GameObject ChoosePanelObj;
    void Start()
    {
        if(mark!=null)AddMarkToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMarkToList()
    {
        SelectTrickPanel.Instacne.MarkObjs.Add(mark);
    }
}
