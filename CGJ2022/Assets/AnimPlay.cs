using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPlay : MonoBehaviour
{
    private static AnimPlay _instance;
    public static AnimPlay Instace
    {
        get { return _instance; }

    }

    private void Awake()
    {
        if (_instance==null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    public void PlayAnim(GameObject go)
    {
        Debug.Log(go.GetComponentInChildren<Animator>());
		foreach (var item in go.GetComponentsInChildren<Animator>())
        {
            item.SetBool("isActive", true);
        }
    }
    // Update is called once per frame

}
