using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiaoBianTouMin : MonoBehaviour
{

    private float _OutLineScale;//边缘线范围
    public float OutLineScale
    {
        set
        {
            _OutLineScale = value; 
            GetComponent<MeshRenderer>().material.SetFloat("_OutLineScale",_OutLineScale);
        }
    }

    private Color _color;

    public Color Color
    {
        set
        {
            _color = value;
            GetComponent<MeshRenderer>().material.SetColor("_Color",_color);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
