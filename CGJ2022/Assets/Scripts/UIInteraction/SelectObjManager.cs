using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SelectObjManager : MonoBehaviour
{
	public enum SelectState
	{
		Null = 0,
		MakeDanger = 1,
		SelectGround = 2,
		MakeGround = 3
	}

	private static SelectObjManager _instance;
	public static SelectObjManager Instance
	{
		get { return _instance; }
	}

	//物体z轴距摄像机的长度
	public LayerMask groundLayerMask;
	private bool isChooseSuccess = false;
	public bool canPlace= false;
	public BlockManager CurrentObj;
	public GameObject AIPrefab;
	
	public SelectState selectState = SelectState.Null;

	public int dangerTime;
	public int outerTime;

	public bool run = false;

	void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Update()
	{
		//if(canPlace)MoveCurrentPlaceObj();
		//if (canPlace&&isChooseSuccess && Input.GetMouseButton(0))
		//{
		//	//CurrentObj.GetComponent<Glass>().ChoosePanelObj.SetActive(true);
		//	canPlace = false;
		//	SelectTrickPanel.Instacne.MarkActiveFalse();
		//}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			RunAI();
			return;
		}

		var last = CurrentObj;
		var block = TryGetObj();

		switch (selectState)
		{
			case SelectState.Null:
				BlockManager.ClearRound();
				break;
			case SelectState.MakeDanger:
				BlockManager.ClearRound();
				if (last != block && last != null)
				{
					last.dangerUI.SetActive(last.isDanger);
				}
				if (block != null && block.type != BlockManager.BlockType.Null && block.type != BlockManager.BlockType.Wall
					&& block.type != BlockManager.BlockType.zhongdian && !block.isStart && !block.isEnd && !block.isDanger)
				{
					block.dangerUI.SetActive(true);
					if (Input.GetMouseButtonDown(0) && GameManager.Instance.Danger > 0)
					{
						block.SetDanger(true);
						block.GetComponent<Animator>().Play("Build");
						GameManager.Instance.MakeDangerList.Add(block.pos);
						GameManager.Instance.Danger--;
						GameManager.Instance.UpdateUI();
						selectState = SelectState.Null;
					}
				}
				break;
			case SelectState.SelectGround:
				if (last != block && last != null)
				{
					foreach (var item in last.GetRoundGround())
					{
						item.cube.SetActive(false);
						item.readyBuildType = BlockManager.BlockType.Null;
					}
				}
				if (block != null)
				{
					foreach (var item in block.GetRoundGround())
					{
						item.cube.SetActive(true);
						if (Input.GetMouseButtonDown(0))
						{
							selectState = SelectState.MakeGround;
							item.readyBuildType = block.type;
						}
					}
				}
				break;
			case SelectState.MakeGround:
				if (last != block && last != null)
				{
					if (last != null && last.readyBuildType != BlockManager.BlockType.Null)
					{
						last.cube.GetComponent<MeshRenderer>().materials[0].color = new Color(0,1,0,0.3f);
					}
				}
				if (block != null && block.readyBuildType != BlockManager.BlockType.Null)
				{
					Debug.Log(block.readyBuildType);
					block.cube.GetComponent<MeshRenderer>().materials[0].color = new Color(0, 1, 0, 0.6f);
					if (Input.GetMouseButtonDown(0) && GameManager.Instance.Change > 0)
					{
						Debug.Log("change");
						block.ChangeType(block.readyBuildType);
						block.GetComponent<Animator>().Play("Build");
						block.readyBuildType = BlockManager.BlockType.Null;
						BlockManager.ClearRound();
						GameManager.Instance.MakeGroundList[block.pos] = block.type;
						GameManager.Instance.Change--;
						GameManager.Instance.UpdateUI();
						selectState = SelectState.Null;
					}
				}
				break;
			default:
				break;
		}

	}

	public void RunAI()
	{
		selectState = SelectState.Null;
		if (run) return;
		run = true;
		Instantiate(AIPrefab, BlockManager.GetWorldVec3(BlockManager.stPos), Quaternion.identity);
		var pm = FindObjectOfType<PathManagement>();
		pm.conveyLevel(BlockManager.stPos,BlockManager.edPos, PathManagement.Dir.Left);
		for (int i = 0; i < GameManager.Instance.DangerList.Length; i++)
		{
			if (GameManager.Instance.DangerList[i] == true)
			{
				pm.blacklist[pm.listp++] = i + 1;
			}
		}
		//foreach (var item in BlockManager.map)
		//{
		//	BlockManager.intMap[item.Key.x - BlockManager.LeftDown.x, item.Key.y - BlockManager.LeftDown.y] = (int)item.Value.GetComponent<BlockManager>().type;
		//}
		var path = FindObjectOfType<PathManagement>().Pathfinder(BlockManager.intMap, new Vector2Int[1]);
		FindObjectOfType<AIController>().updatePath(path);
		for (int i = 0; i < path.Length; i++)
		{
			Debug.Log(path[i]);
		}
	}

	public void StartMakeDanger()
	{
		if (run) return;
		selectState = SelectState.MakeDanger;

	}

	public void StartMakeGround()
	{
		if (run) return;
		selectState = SelectState.SelectGround;

	}

	BlockManager TryGetObj()
	{
		Vector3 point;
		Vector3 ScreenPosition;
		ScreenPosition = Input.mousePosition;
		Ray ray = Camera.main.ScreenPointToRay(ScreenPosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000, groundLayerMask))
		{
			var cur = hitInfo.collider.gameObject.GetComponent<BlockManager>();
			isChooseSuccess = true;
			CurrentObj = cur;
			return CurrentObj;
		}
		else
		{
			isChooseSuccess = false;
			return null;
		}
	}

	 void MoveCurrentPlaceObj()
	{

	}

	 
}