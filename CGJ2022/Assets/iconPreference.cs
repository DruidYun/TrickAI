using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class iconPreference : MonoBehaviour
{
    public Transform[] imageNormal;
    public Transform[] imageHate;


    //传入值改变图标
    public void changeIcon(int i,bool danger)
    {
        imageNormal[i].gameObject.SetActive(!danger);
        imageHate[i].gameObject.SetActive(danger);
    }

    //传入值使图标变灰
    public void greyIcon(int i)
    {
        imageNormal[i].gameObject.SetActive(true);
        imageHate[i].gameObject.SetActive(false);
        imageNormal[i].gameObject.GetComponent<Image>().color = Color.grey;
    }
}
