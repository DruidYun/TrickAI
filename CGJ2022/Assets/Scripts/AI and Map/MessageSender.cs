using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSender : MonoBehaviour
{
    int[,] map;
    float[,] res1;
    private void Start()
    {
        map = new int[8, 11] {
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
        {-1,-1,-1,-1, 0, 0, 0, 0, 0,-1,-1 },
        {-1,-1,-1,-1, 0,-1,-1,-1, 0,-1,-1 },
        {-1,-1,-1,-1, 0,-1, 1,-1, 0,-1,-1 },
        {-1, 0, 0, 0, 0, 0, 1, 0, 2, 0,-1 },
        {-1, 0, 0, 0, 0, 0, 1, 0, 0, 0,-1 },
        {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0,-1 },
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 }};
        GetComponent<PathManagement>().conveyLevel(new Vector2Int(5, 2), new Vector2Int(5, 8), PathManagement.Dir.Right);
        Vector2Int[] res, haz;
        haz = new Vector2Int[4];
        haz[0] = new Vector2Int(5, 6);
        haz[1] = new Vector2Int(4, 6);
        haz[2] = new Vector2Int(3, 6);
        haz[3] = new Vector2Int(4, 8);
        //res = GetComponent<PathManagement>().Pathfinder(map, haz);
        //res = GetComponent<PathManagement>().Pathfinder(map, haz);

        //for(int i=0;i<res.Length;i++)
        //{
        //    Debug.Log(res[i].x);
        //    Debug.Log(res[i].y);
        //    Debug.Log("/////");
        //}

        //GetComponent<AIController>().updatePath(res);
    }
}
