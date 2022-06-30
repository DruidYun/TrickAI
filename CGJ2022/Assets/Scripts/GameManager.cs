using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public struct LevelData
	{
		public string name;
		public int health;
		public int danger;
		public int change;

		public LevelData(string name,int helath,int danger,int change)
		{
			this.name = name;
			this.health = helath;
			this.danger = danger;
			this.change = change;
		}
	}
	LevelData[] SceneData = new LevelData[]
	{
		new LevelData("Level 1-0",1,0,0),
		new LevelData("Level 1-1",2,1,0),
		new LevelData("Level 1-2",2,0,1),
		new LevelData("Level 1-3",2,2,0),
		new LevelData("Level 1-4",3,0,2),
		new LevelData("Level 1-5",3,2,1),
	};

	public int CurLevel = 0;
	public bool[] DangerList = new bool[5];
	
	public int Health;
	public int Danger;
	public int Change;

	public iconPreference m_iconPreference;
	public healthUI m_healthUI;
	public ButtonUI m_ButtonUI;

	public List<Vector2Int> MakeDangerList = new List<Vector2Int>();
	public Dictionary<Vector2Int, BlockManager.BlockType> MakeGroundList = new Dictionary<Vector2Int, BlockManager.BlockType>();

	private static GameManager _instance;
	public static GameManager Instance
	{
		get { return _instance; }
	}

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
		DontDestroyOnLoad(gameObject);
		var data = SceneData[CurLevel];
		Health = data.health;
		Danger = data.danger;
		Change = data.change;
		MakeDangerList.Clear();
		MakeGroundList.Clear();
		UpdateUI();
	}

	public void Success()
	{
		Debug.Log("Success");
		CurLevel++;
		StartScene();
	}

	public void StartScene()
	{
		Debug.Log("Restart");
		var data = SceneData[CurLevel];
		ReloadScene();
		Health = data.health;
		Danger = data.danger;
		Change = data.change;
		MakeDangerList.Clear();
		MakeGroundList.Clear();
		for (int i= 0; i < 5; i++)
		{
			DangerList[i] = false;
		}
		UpdateUI();
	}

	public void ReloadScene()
	{
		Debug.Log("Next Helath");
		var data = SceneData[CurLevel];
		BlockManager.intMap = null; 
		SceneManager.LoadScene(data.name);
		UpdateUI();
	}

	public void Hurt(BlockManager.BlockType t)
	{
		DangerList[(int)t-1] = true;
		Health--;
		UpdateUI();
		if (Health <= 0)
		{
			StartReload(0);
		}
		else
		{
			StartReload(2);
		}
	}

	public void StartReload(int fun)
	{
		StartCoroutine(Reloading(fun));
	}

	public void UpdateUI()
	{
		for (int i = 0; i < 5; i++)
		{
			m_iconPreference.changeIcon(i,DangerList[i]);
		}
		m_healthUI.blood(Health);
		m_ButtonUI.setNumberDamage(Danger);
		m_ButtonUI.setNumberGround(Change);
	}

	IEnumerator Reloading(int fun)
	{
		float value = 1.2f;
		while (value >= -0.1f)
		{
			value -= 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
		if (fun == 0)
		{
			Success();
		}
		else if (fun == 1)
		{
			StartScene();
		}
		else
		{
			ReloadScene();
		}
		StopCoroutine(Reloading(fun));
	}

	public void StartPlayButton()
	{
		SelectObjManager.Instance.RunAI();
	}

	public void DamageSet()
	{
		SelectObjManager.Instance.StartMakeDanger();
	}

	public void GroundSet()
	{
		SelectObjManager.Instance.StartMakeGround();
	}
}
