using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    MeshRenderer[] mrs;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void StartDissolve()
    {
        StartCoroutine(Dissolveing());
    }

    // Update is called once per frame
    IEnumerator Dissolveing()
    {
        
        float value = 1;
        while (value >= -0.1f)
        {
            yield return new WaitForSeconds(0.01f);
             mrs = GetComponentsInChildren<MeshRenderer>();
             foreach (MeshRenderer mr in mrs)
             {
                 mr.material.SetFloat("_BurnAmount",1-value);
             }

             value -= 0.01f;
        }
        StopCoroutine(Dissolveing());
    }
}
