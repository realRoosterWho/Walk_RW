using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshGen : MonoBehaviour
{
    //预制体
    public GameObject prefab;
    
    // Start is called before the first frame update
    void Start()
    {
        //生成预制体
        Instantiate(prefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
