using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthUI : MonoBehaviour
{
    public int health;
    public Image[] healthImage;


    //调用函数改变血量
    public void blood(int currentHealth)
    {
        for(int i=0;i<3;i++)
        {
            healthImage[i].gameObject.SetActive(false);
        }
        for(int j=0;j<currentHealth;j++)
        {
            healthImage[j].gameObject.SetActive(true);
        }
    }
}
