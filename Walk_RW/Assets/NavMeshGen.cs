using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation; //导入NavMeshPlus命名空间
using UnityEngine.AI; // 导入正确的命名空间
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif


public class NavMeshGen : MonoBehaviour
{
    //预制体
    public GameObject prefab;

    [SerializeField] private NavMeshPlus.Components.NavMeshSurface[] navMeshSurfaces;
    public NavMeshPlus.Components.NavMeshSurface Surface2D;

    void Start()
    {
        // 获取NavMeshSurface组件
        Surface2D = GetComponent<NavMeshPlus.Components.NavMeshSurface>();

        if (Surface2D == null)
        {
            Debug.Log("未找到NavMeshSurface组件");
        }
        else
        {
            Debug.Log("已获取NavMeshSurface组件");
        }

        Surface2D.BuildNavMeshAsync();

        //生成预制体
        Instantiate(prefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        // Surface2D.UpdateNavMesh(Surface2D.navMeshData);
    }
}