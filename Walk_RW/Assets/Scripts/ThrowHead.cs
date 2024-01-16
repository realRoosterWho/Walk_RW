using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHead : MonoBehaviour
{
    public float maxDragDistance = 2f;
    public float power = 10f;
    public Grid grid;

    private Rigidbody2D rb;
    private Vector2 startPos;
    private LineRenderer line;
    private Coroutine moveCoroutine;
    
    public float Timer; //游戏速度
    public float step; //蛇头的移动距离
    private int X; //移动的增量值
    private int Y; //移动的增量值
    private Vector3 HeadPos; //蛇头的坐标
    private Vector3 OldHeadPos; //蛇头的坐标
    private bool isDragging = false;
    private bool isOneMove = true;
    private bool isCanDrag = true;
    private float releaseTime = 0f;
    private float currentMaxDragDistance = 2f;
    private int totalGrowth = 0;
    private float cellSize;
    private bool isAIReady = false;
    private bool isPassPath = false;

    private Vector2 direction;

    public GameObject bodyPrefab; //蛇尾的预设

    //蛇身列表
    public List<Transform> bodyList = new List<Transform>();
    public Sprite[] bodySprites = new Sprite[2]; //图片

    // 蛇头的移动路径
    private Queue<Vector3> path = new Queue<Vector3>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is not found on the game object.");
            return;
        }

        line = GetComponent<LineRenderer>();
        if (line == null)
        {
            Debug.LogError("LineRenderer component is not found on the game object.");
            return;
        }

        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        //将line的图层设置高
        line.sortingOrder = 1;

        //初始化direction为向上
        direction = Vector2.up;
        
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
        
        
        EventManager.Instance.OnGameEvent += AIReady; //订阅事件

        OnMove();
    }


    public void OnMove()
    {
        Debug.Log("OnMove");
        // 计算蛇头的新位置
        HeadPos = gameObject.transform.position;

        // 计算蛇头的移动方向，也就是现在的第一节身体的位置减去蛇头的位置
        Vector3 moveDirection = bodyList.Count > 0 ? bodyList[0].position - HeadPos : direction * step;
        
        // 计算蛇头的新位置
        Vector3 headPos = gameObject.transform.position;

        // 将蛇头的新位置转换为最近的瓷砖中心
        Vector3Int cellPos = grid.WorldToCell(headPos);
        Vector3 cellCenterPos = grid.GetCellCenterWorld(cellPos);
        // 
        // 使用瓷砖中心作为蛇头的新位置
        headPos = cellCenterPos;
        gameObject.transform.position = headPos;
        // 速度变成0
        rb.velocity = Vector2.zero;


        
        // 如果蛇身存在且蛇头和第一节身体的距离小于或等于一个步长，那么不执行移动
        if (bodyList.Count > 0 && Vector3.Distance(HeadPos, bodyList[0].position) <= (step * 2))
        {
            isCanDrag = true;
            path.Clear();
            return;
        }


        if (isOneMove == false)
        {
            EventManager.Instance.TriggerGameEvent(EventManager.GameEvent.OneMove);
            // //清空当前路径
            // path.Clear();
            //
            // //修改路径设计，使得路径也按照网格走
            // int steps = Mathf.RoundToInt(Mathf.Max(Mathf.Abs(moveDirection.x), Mathf.Abs(moveDirection.y))); // 计算移动的步数
            // Vector3 stepDirection = moveDirection / steps;
            // Vector3 lastCellCenterPos = Vector3.zero; // 记录上一次移动的位置
            // for (int i = 0; i < steps; i++)
            // {
            //     // 计算每一步的世界坐标
            //     Vector3 worldPos = HeadPos + stepDirection * i;
            //
            //     // 将世界坐标转换为最近的瓷砖中心
            //     cellPos = grid.WorldToCell(worldPos);
            //     cellCenterPos = grid.GetCellCenterWorld(cellPos);
            //     
            //     // 如果上一次移动的位置和这一次移动的位置一样，那么不要添加
            //     if (cellCenterPos == lastCellCenterPos)
            //     {
            //         continue;
            //     }
            //
            //     // 如果上一次移动的位置和这一次的位置之间的距离大于一个cellSize，那么就在这两个路径节点之间插入一个路径节点，这个路径节点需要是瓷砖中心，并且取第一个路径节点的x坐标，取第二个路径节点的y坐标
            //     if (Vector3.Distance(lastCellCenterPos, cellCenterPos) > cellSize)
            //     {
            //         Vector3Int lastCellPos = grid.WorldToCell(lastCellCenterPos);
            //         Vector3Int currentCellPos = grid.WorldToCell(cellCenterPos);
            //         Vector3Int insertCellPos = new Vector3Int(lastCellPos.x, currentCellPos.y, 0);
            //         Vector3 insertCellCenterPos = grid.GetCellCenterWorld(insertCellPos);
            //         path.Enqueue(insertCellCenterPos);
            //     }
            //
            //     // 将瓷砖中心添加到路径中
            //     path.Enqueue(cellCenterPos);
            //
            //     // 记录这一次的移动位置，以便下一次循环时使用
            //     lastCellCenterPos = cellCenterPos;
            // }

            // // 倒序
            // path = new Queue<Vector3>(new Stack<Vector3>(path));
            isOneMove = true;
        }
        
       
        

        // 如果蛇身存在
        if (bodyList.Count > 0)
        {
            if (path.Count == 0)
            {
                isOneMove = false;
                return;
            }
            
            // 从后往前开始移动蛇身
            for (int i = bodyList.Count - 2; i >= 0; i--)
            {
                // 每一个蛇身都移动到它前面一个节点的位置
                bodyList[i + 1].position = bodyList[i].position;
            }

            // 第一个蛇身移动到路径中的下一个位置
            if (path.Count > 0)
            {
                bodyList[0].position = path.Dequeue();
            }
        }
    }
    
    
    public void Grow(int growthCount = 5)
    {
        totalGrowth++; // 记录总共生长的次数
        for (int i = 0; i < growthCount; i++)
        {
            // 创建新的蛇身部分，每个新部分的初始位置都有所不同
            Vector3 initialPosition = new Vector3(2000 * totalGrowth + i * 10, 2000 * totalGrowth + i * 10, 0);
            GameObject newBodyPart = Instantiate(bodyPrefab, initialPosition, Quaternion.identity);

            // 根据蛇身的长度来选择纹理
            int index = bodyList.Count % 2;
            newBodyPart.GetComponent<SpriteRenderer>().sprite = bodySprites[index];

            // 将新的蛇身部分添加到蛇身列表的末尾
            bodyList.Add(newBodyPart.transform);
        }
    }
    
    //碰撞
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("碰撞");
        if (other.CompareTag("Food"))
        {
            int growthCount = other.GetComponent<FoodText>().foodnum;
            Destroy(other.gameObject); //销毁食物
            this.Grow(growthCount); //生长尾巴
            //添加食物
            GameObject.Find("RangeFood").GetComponent<RangeFood>().AddFood();
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        // 获取在蛇头位置和一定半径内的所有碰撞器
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

        // 遍历所有碰撞器
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("AINav"))
            {
                isAIReady = true;
                Debug.Log("碰撞AINav");
                return; // 如果找到了带有"AINav"标签的物体，就直接返回
            }
        }

        // 如果没有找到带有"AINav"标签的物体，就将isAIReady设置为false
        isAIReady = false;
    }

    private void FixedUpdate()
    {
        isAIReady = false;

        
        
    }
    
    private void Update()
    {
        
        
        if(isCanDrag == true)
        {
            if (Input.GetMouseButtonDown(0)) //按下鼠标左键
            {
                Debug.Log("按下鼠标左键");
                startPos = rb.position;
                line.enabled = true;
                line.SetPosition(0, startPos);
            }

            if (Input.GetMouseButton(0)) //按住鼠标左键
            {
                Vector2 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                direction = startPos - currentPos;
                float distance = direction.magnitude;
                currentMaxDragDistance = maxDragDistance;

                // 使用RaycastAll检测拖拽路径上所有的物体
                RaycastHit2D[] hits = Physics2D.RaycastAll(startPos, -direction, maxDragDistance);
                Debug.DrawRay(startPos, -direction, Color.red);

                // 找到距离最近的墙壁
                RaycastHit2D firstWallHit = default;
                float minDistance = float.MaxValue;
                foreach (var hit in hits)
                {
                    if (hit.collider.CompareTag("Wall"))
                    {
                        float hitDistance = Vector2.Distance(startPos, hit.point);
                        if (hitDistance < minDistance)
                        {
                            minDistance = hitDistance;
                            firstWallHit = hit;
                        }
                    }
                }

                // 如果有墙壁，那么限制拖拽的距离
                if (firstWallHit.collider != null)
                {
                    currentMaxDragDistance = Mathf.Min(minDistance, maxDragDistance);
                }

                if (distance > currentMaxDragDistance)
                {
                    direction = direction.normalized * currentMaxDragDistance;
                }

                rb.position = startPos - direction;
                line.SetPosition(1, rb.position);
                isDragging = true;
            }

            if (Input.GetMouseButtonUp(0) && isDragging) //松开鼠标左键
            {
                Vector2 force = (startPos - rb.position) * power;

                //如果force没有到达最小值，那么设定为最小值
                if (force.magnitude < 1f)
                {
                    force = force.normalized * 1f;
                }

                rb.AddForce(force, ForceMode2D.Impulse);
                line.enabled = false;
                releaseTime = Time.time;

                /*
                // 将蛇头的移动路径添加到队列中
                path.Enqueue(rb.position);
                */
                isDragging = false;
                isCanDrag = false;
                EventManager.Instance.TriggerGameEvent(EventManager.GameEvent.MoveInitial);
            }
        }



        // 检测“当头不再动”
        if (!(Input.GetMouseButton(0)) && rb.velocity.magnitude < 0.5f && Time.time - releaseTime > 0.1f)
        {
            Debug.Log("头不再动");
            tileHeadPos();
            EventManager.Instance.TriggerGameEvent(EventManager.GameEvent.OnMove);
                
            if (isAIReady == true)
            {
                if (moveCoroutine == null)
                {
                    moveCoroutine = StartCoroutine(MoveRepeatedly());
                }

            }   

        }
        else
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
        }

        IEnumerator MoveRepeatedly()
        {
            while (true)
            {
                OnMove();
                yield return new WaitForSeconds(Timer); //等待Timer秒
            }
        }
    }

    public void tileHeadPos()
    {
        // 计算蛇头的新位置
        HeadPos = gameObject.transform.position;
        
        // 计算蛇头的新位置
        Vector3 headPos = gameObject.transform.position;

        // 将蛇头的新位置转换为最近的瓷砖中心
        Vector3Int cellPos = grid.WorldToCell(headPos);
        Vector3 cellCenterPos = grid.GetCellCenterWorld(cellPos);
        // 
        // 使用瓷砖中心作为蛇头的新位置
        headPos = cellCenterPos;
        gameObject.transform.position = headPos;
        // 速度变成0
        rb.velocity = Vector2.zero;
    }

    public void AIReady(EventManager.GameEvent gameEvent, GameEventArgs eventArgs)
    {
    
        if (gameEvent == EventManager.GameEvent.AIReady)
        {
            Debug.Log("PathPassed - SpringHeadGet");
            path = eventArgs.Vector3QueueValue;
            isPassPath = true;
        }
    
    }
}