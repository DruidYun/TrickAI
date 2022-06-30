using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{
    //当前第几关
    public int levelIndex;
    //三种技能的数量
    public int hurt;
    public int love;
    public int groundExtend;
    //AI血量
    public int health;
    public Transform theAI;//子物件，移动的主角

    public Transform[] colorList;//场景颜色展示需要挂图标
    public int[] blackList;//对每个场景的偏好
    public Transform healthTxt;//血量展示

    void Start()
    {
        
    }

    //按下按钮，角色可以开始行走
    public void buttonStart()
    {
        goBack();
        theAI.GetComponent<AIController>().setMove();
    }

    public void hurtHelth()
    {
        playerSuccess();
        //hurt--;
        //if(hurt<=0)
        //{
        //    playerSuccess();
        //}
        //else
        //{
        //    if(hurt==0 && love==0 && groundExtend == 0)
        //    {
        //        //直接调用onClick()
        //        buttonStart();
        //    }
        //}
        //goBack();
    }

    void goBack()//一次掉血后返回初始位置
    {
        //人物回到初始位置
        Vector2 pos = theAI.GetComponent<AIController>().originalPos;
        theAI.position = new Vector3(pos.x, theAI.position.y, pos.y);

        //获得一次blacklist并显示
        blackList = theAI.GetComponent<PathManagement>().blacklist;
        for (int i = 0; i < blackList.Length; i++)
        {
            //colorList[blackList[i]].GetComponent<Renderer>().material.color = Color.red;
        }
        //healthTxt.GetComponent<Text>().text = health.ToString();

    }
    public void playerSuccess()
    {
        Debug.Log("Success");
        int currentLevel = PlayerPrefs.GetInt("currentLevel");
        if (currentLevel < levelIndex + 1)
        {
            currentLevel++;
            PlayerPrefs.SetInt("currentLevel", currentLevel);
        }
        //Time.timeScale = 0;
        var ai = FindObjectOfType<AIController>();
        ai.canWalk = false;
        ai.GetComponent<Dissolve>().StartDissolve();
    }

    public void playerFail()
    {
        var ai = FindObjectOfType<AIController>();
        ai.canWalk = false;
        GameManager.Instance.StartReload(1);
    }



}
