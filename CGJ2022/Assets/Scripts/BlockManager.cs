using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
	public const float Size = 5.2f;
	public const float Gap = 0.1f;
	public const float SumSize = Size + Gap;

	public List<GameObject> safeBlocks;
	public List<GameObject> dangerBlocks;
	public GameObject wallBlock;
	public static Dictionary<Vector2Int, GameObject> map = new Dictionary<Vector2Int, GameObject>();
	public static Vector2Int LeftDown = new Vector2Int(9999, 9999);
	public static Vector2Int RightUp = new Vector2Int(-9999, -9999);
	public static int[,] intMap;
	public static Vector2Int stPos;
	public static Vector2Int edPos;
	public static float posY;

	public enum BlockType
	{
		Wall = -1,
		Null = 0,
		qiqiu = 1,
		yongchi = 2,
		feichuan = 3,
		puke = 4,
		fengshan = 5,
		zhongdian = 6,
	}

	public GameObject safe;
	public GameObject danger;
	public GameObject cube;
	public GameObject dangerUI;
	public bool isEmpty;
	public bool isDanger;
	public bool isStart;
	public bool isEnd;
	public BlockType type;
	public BlockType readyBuildType;
	public Vector2Int pos;


	public void DestroyChildren(GameObject obj)
	{
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			Destroy(obj.transform.GetChild(i).gameObject);
		}
	}

	public void ChangeType(BlockType t)
	{
		Debug.Log(t);
		if (t == BlockType.zhongdian)
		{
			Instantiate(safeBlocks[(int)t - 1], safe.transform);
			return;
		}
		if (t == BlockType.Wall || type == BlockType.Wall)
		{
			Debug.Log("dont change wall");
			return;
		}
		type = t;
		DestroyChildren(safe);
		DestroyChildren(danger);
		if (t != BlockType.Null)
		{
			Instantiate(safeBlocks[(int)t - 1],safe.transform);
			Instantiate(dangerBlocks[(int)t - 1], danger.transform);
		}
		danger.SetActive(isDanger);
		safe.SetActive(!isDanger);
		intMap[pos.x - LeftDown.x, pos.y - LeftDown.y] = (int)type;
	}


	public void SetDanger(bool d)
	{
		isDanger = d;
		danger.SetActive(isDanger);
		safe.SetActive(!isDanger);
	}

	public static Vector2 GetWorldPos(Vector2Int pos)
	{
		Debug.Log(pos +"--" + (pos + LeftDown + Vector2.one * 0.5f) * SumSize);
		return ((pos + LeftDown + Vector2.one * 0.5f) * SumSize);
	}

	public static Vector3 GetWorldVec3(Vector2Int pos)
	{
		return new Vector3(GetWorldPos(pos).x, posY, GetWorldPos(pos).y);
	}

	static List<BlockManager> roundGround = new List<BlockManager>();
	public List<BlockManager> GetRoundGround()
	{
		roundGround.Clear();
		if (type == BlockType.Null || type == BlockType.Wall || isEnd || isStart)
		{
			return roundGround;
		}
		Vector2Int[] dir = new Vector2Int[] { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
		foreach (var d in dir)
		{
			if (map.ContainsKey(d + pos))
			{
				var b = map[d + pos].GetComponent<BlockManager>();
				if (b.type == BlockType.Null && !b.isEnd && !b.isStart)
				{
					roundGround.Add(b);
				}
			}
		}
		return roundGround;
	}

	public static void ClearRound()
	{
		foreach (var item in roundGround)
		{
			item.cube.SetActive(false);
			item.readyBuildType = BlockType.Null;
		}
		roundGround.Clear();
	}

	public void Awake()
	{
		pos = new Vector2Int(Mathf.RoundToInt((transform.position.x - SumSize / 2) / SumSize), Mathf.RoundToInt((transform.position.z - SumSize / 2) / SumSize));
		map[pos] = gameObject;
		LeftDown.x = Mathf.Min(pos.x, LeftDown.x);
		LeftDown.y = Mathf.Min(pos.y, LeftDown.y);
		RightUp.x = Mathf.Max(pos.x, RightUp.x);
		RightUp.y = Mathf.Max(pos.y, RightUp.y);
		posY = transform.position.y;
	}

	public void Start()
	{
		if (GameManager.Instance.MakeGroundList.ContainsKey(pos))
		{
			type = GameManager.Instance.MakeGroundList[pos];
		}

		if (GameManager.Instance.MakeDangerList.Contains(pos))
		{
			isDanger = true;
		}

		dangerUI.SetActive(isDanger);
		cube.SetActive(false);
		ChangeType(type);
		if (intMap == null)
		{
			intMap = new int[RightUp.x - LeftDown.x + 1 , RightUp.y - LeftDown.y + 1];
		}
		if (isStart) stPos = pos - LeftDown;
		if (isEnd) edPos = pos - LeftDown;
		Debug.Log(pos - LeftDown);
		intMap[pos.x - LeftDown.x, pos.y - LeftDown.y] = (int)type;
		//ChangeType(BlockType.Type2);
	}


}
