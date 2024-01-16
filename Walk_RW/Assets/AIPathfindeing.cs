using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class AIPathfindeing : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform target;
    NavMeshAgent agent;
    private List<Vector3> visitedCellCenters = new List<Vector3>();
    private bool isOneMove = false;
    private bool isPassPath = false;
    public Grid grid;
    
    private Queue<Vector3> path = new Queue<Vector3>();

    private float cellSize;
    
    private Vector3 lastCellCenterPos = Vector3.zero; // 记录上一次移动的位置


    


    void Start()
    {
        
        //获取场景中名字叫springHead的并且把他的Transform赋值给Target
        target = GameObject.Find("springHead").GetComponent<Transform>();
        
        //自己的位置跟随springHead
        transform.position = target.position;
        
        //获取场景中名字叫Grid的东西上面的Grid组件
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        
        //检查网格的cellSizeXY是否相等
        if (grid.cellSize.x != grid.cellSize.y)
        {
            Debug.LogError("The cellSizeXY of the grid is not equal.");
            return;
        }
        
        //记录网格的cellSize
        cellSize = grid.cellSize.x;
        
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        EventManager.Instance.OnGameEvent += OnMove; //订阅事件
        EventManager.Instance.OnGameEvent += OneMove; //订阅事件
        EventManager.Instance.OnGameEvent += MoveInitial; //订阅事件
    }

    // Update is called once per frame
    void Update()
    {

        if (isOneMove) //在球已经出手而三角形还没有就位的时候
        {
            //获取AI所经过的所有格子的中心点

            Debug.Log("AI - isOneMove");
            Vector3 currentPos = transform.position;
            Vector3Int cellPos = grid.WorldToCell(currentPos);
            Vector3 cellCenterPos = grid.GetCellCenterWorld(cellPos);

            if (cellCenterPos == lastCellCenterPos)
            {
                // 退出if isOneMove
                goto ContinueUpdate;
            }

            // 如果上一次移动的位置和这一次的位置之间的距离大于一个cellSize，那么就在这两个路径节点之间插入一个路径节点，
            // 这个路径节点需要是瓷砖中心，并且取第一个路径节点的x坐标，取第二个路径节点的y坐标
            if (Vector3.Distance(lastCellCenterPos, cellCenterPos) > grid.cellSize.x)
            {
                Vector3Int lastCellPos = grid.WorldToCell(lastCellCenterPos);
                // 计算中心点所在的格子的坐标
                Vector3Int currentCellPos = grid.WorldToCell(cellCenterPos);
                // 计算中心点所在的格子的中心点
                Vector3Int insertCellPos = new Vector3Int(lastCellPos.x, currentCellPos.y);

                Vector3 insertCellCenterPos = grid.GetCellCenterWorld(insertCellPos);

                path.Enqueue(insertCellCenterPos);

            }

            // 将中心点所在的格子的中心点设置为上一次移动的位置
            path.Enqueue(cellCenterPos);
            lastCellCenterPos = cellCenterPos;


        }
        else
        {
            
        }

        ContinueUpdate:
        //Debug path的长度
        // Debug.Log("path.Count = " + path.Count);

        if (isPassPath)
        {
            // 如果路径长度是0，那么就不执行下面的代码
            if (path.Count != 0)
            {
                // //通过事件将path返回给ThrowHead
                EventManager.Instance.TriggerGameEvent(EventManager.GameEvent.AIReady, new GameEventArgs {Vector3QueueValue = path});
                Debug.Log("PATH PASSED" + path.Count);
            }
            isPassPath = false;
            path.Clear();
        }



    }

    void OnMove(EventManager.GameEvent gameEvent, GameEventArgs gameEventArgs) //事件处理函数
    {
        if (gameEvent == EventManager.GameEvent.OnMove)
        {
            //agent.isStopped = false;
            agent.SetDestination(target.position);
            Debug.Log("AIOnMove");
        }
            
            

    }
    
    void OneMove(EventManager.GameEvent gameEvent, GameEventArgs gameEventArgs) //事件处理函数
    { 
        if (gameEvent == EventManager.GameEvent.OneMove)
        {
            Debug.Log("AI - 结束AI移动 - OneMove");
            isOneMove = false;
            isPassPath = true;
        }

    }
    
    void MoveInitial(EventManager.GameEvent gameEvent, GameEventArgs gameEventArgs) //事件处理函数
    { 
        if (gameEvent == EventManager.GameEvent.MoveInitial)
        {
            //agent.isStopped = true;
            Debug.Log("AI - 开始AI移动 - Move Initial");
            isOneMove = true;
        }

    }
    
}
