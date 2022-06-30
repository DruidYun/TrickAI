using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//关卡选择页
public class levelController : MonoBehaviour
{
    int currentLevel;
    public int maxLevel;
    Button[] levelButtons;
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel");
        for(int i = 0; i < maxLevel; i++)
        {
            levelButtons[i].interactable = true;
            if(i<currentLevel)
            {
                levelButtons[i].interactable = false;
            }
        }
    }
}
