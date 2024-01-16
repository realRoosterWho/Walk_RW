using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPathfindeing : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform target;
    NavMeshAgent agent;
    private List<Vector3> visitedCellCenters = new List<Vector3>();
    private bool isOneMove = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        EventManager.Instance.OnGameEvent += OnMove; //订阅事件
        EventManager.Instance.OnGameEvent += OneMove; //订阅事件
    }

    // Update is called once per frame
    void Update()
    {

        float speed = agent.velocity.magnitude;

        if (speed >= 0.1f)
        {
            Debug.Log("AI - speed => 0.1f");
            isOneMove = false;
        }
     
        if (isOneMove == false)
        {
            //执行获取所有当前GameObject走过的Cell的中心点
            Debug.Log("AI - isOneMove");
            
            
            //获取当前速度
            
        }

    }

    void OnMove(EventManager.GameEvent gameEvent, GameEventArgs gameEventArgs) //事件处理函数
    { 
        if (target == null)
        {
            return;
        }
        agent.SetDestination(target.position);
        Debug.Log("AIOnMove");
    }
    
    void OneMove(EventManager.GameEvent gameEvent, GameEventArgs gameEventArgs) //事件处理函数
    { 
        Debug.Log("AI - OneMove");
        isOneMove = true;
    }
    
}
