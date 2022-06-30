using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManagement : MonoBehaviour
{
    public enum Dir
	{
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
	}

    int[,] pathvalue, map;
    public int[] blacklist;
    public int listp;
    Vector2Int[] stack, result;
    int stackp, minlen, direction;
    Vector2Int spawn, target;
    bool[,] vis;
    void Awake()
    {
        pathvalue = new int[20, 20];
        stack = new Vector2Int[114514];
        vis = new bool[20, 20];
        for (int i = 0; i < 20; i++)
            for (int j = 0; j < 20; j++) pathvalue[i, j] = 0;
        blacklist = new int[10];
    }

    void depthFirst(Vector2Int current, int len, int dir)
    {
        if (len > minlen)
		{
            return;
		}
        if (current == target)
        {
            if (len < minlen) //仅当寻到新的最短路时
            {
                minlen = len;
                result = new Vector2Int[stackp + 1];
                for(int i = 0; i < stackp; i++)
                {
                    result[i] = stack[i];
                }
                result[stackp] = target; //将栈中坐标全面迁移至结果数组内
            }
            return;
        }
        int[] value = new int[4];
        int[] order = new int[4];
        for (int i = 0; i < 4; i++) order[i] = i;
        value[0] = pathvalue[current.x - 1, current.y];
        value[1] = pathvalue[current.x + 1, current.y];
        value[2] = pathvalue[current.x, current.y - 1];
        value[3] = pathvalue[current.x, current.y + 1];
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 3 - i; j++)
                if (value[j] < value[j + 1] || value[j] == value[j + 1] && order[j + 1] == dir)
                {
                    int val = value[j];
                    int ord = order[j];
                    order[j] = order[j + 1];
                    value[j] = value[j + 1];
                    order[j + 1] = ord;
                    value[j + 1] = val;
                }
        //确定方向顺序
        for(int i = 0; i < 4; i++)
        {
            int nextx, nexty;
            switch (order[i])
            {
                case 0: nextx = current.x - 1; nexty = current.y; break;
                case 1: nextx = current.x + 1; nexty = current.y; break;
                case 2: nextx = current.x; nexty = current.y - 1; break;
                case 3: nextx = current.x; nexty = current.y + 1; break;
                default: nextx = nexty = 0; break;
            }
            if (value[i] >= 0 && !vis[nextx, nexty])
            {
                if (order[i] != dir)
                {
                    stack[stackp] = current;
                    stackp++;
                }
                vis[nextx, nexty] = true;
                depthFirst(new Vector2Int(nextx, nexty), len + 1, order[i]);
                vis[nextx, nexty] = false;
                if (order[i] != dir) stackp--;
            }
        }
    }

    public Vector2Int[] Pathfinder(int[,] map, Vector2Int[] hazard) //上下左右为0123
    {
        bool[,] spike = new bool[20, 20];
        for(int i = 0; i < hazard.Length && hazard[i].magnitude != 0; i++)
        {
            spike[(int)hazard[i].x, (int)hazard[i].y] = true;
        }
        for (int i = 0; i < 20; i++)
            for (int j = 0; j < 20; j++)
                vis[i, j] = false;
        stackp = 1;
        stack[0] = spawn;
        result = new Vector2Int[1];
        result[0] = spawn;
        minlen = 1919810;
        for (int i = 0; i < 20; i++)
            for (int j = 0; j < 20; j++) pathvalue[i, j] = 0;
        for (int i = 0; i < map.GetLength(0); i++)
            for (int j = 0; j < map.GetLength(1); j++)
                if (map[i, j] < 0) pathvalue[i, j] = -114514; //这些物块有什么走的必要吗？
        for (int i = 0; i < map.GetLength(0); i++)
            for (int j = 0; j < map.GetLength(1); j++)
                for (int k = 0; k < listp; k++)
                    if (map[i, j] == blacklist[k]) pathvalue[i, j] -= 1;
        depthFirst(spawn, 0, direction);
        if (result.Length > 1)
        {
            for (int k = 1; k < result.Length; k++)
            {
                if (result[k].x == result[k - 1].x)
                {
                    int xloc = (int)result[k].x;
                    int yloc = (int)result[k - 1].y;
                    for(int p = 0; Mathf.Abs(p) < Mathf.Abs(result[k].y - result[k-1].y); p += (int)Mathf.Sign(result[k].y - result[k - 1].y))
                    {
                        if (spike[xloc, yloc + p])
                        {
                            blacklist[listp] = map[xloc, yloc + p];
                            listp++;
                            return result;
                        }
                    }
                }
                else
                {
                    int yloc = (int)result[k].y;
                    int xloc = (int)result[k - 1].x;
                    for (int p = 0; Mathf.Abs(p) < Mathf.Abs(result[k].x - result[k - 1].x); p += (int)Mathf.Sign(result[k].x - result[k - 1].x))
                    {
                        if (spike[xloc + p, yloc])
                        {
                            blacklist[listp] = map[xloc + p, yloc];
                            listp++;
                            return result;
                        }
                    }
                }
            }
        } //检查是否踩刺，更新权值
        return result;
    }

    public void conveyLevel(Vector2Int initiallocation, Vector2Int targetlocation, Dir initialdirection)
    {
        spawn = initiallocation;
        target = targetlocation;
        direction = (int)initialdirection;
        for (int i = 0; i < 20; i++)
            for (int j = 0; j < 20; j++) pathvalue[i, j] = 0;
        listp = 0;
    }
    
}
