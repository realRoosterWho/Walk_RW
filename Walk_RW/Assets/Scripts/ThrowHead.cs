using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHead : MonoBehaviour
{
    public float maxDragDistance = 2f;
    public float power = 10f;

    private Rigidbody2D rb;
    private Vector2 startPos;
    private LineRenderer line;
    private Coroutine moveCoroutine;
    
    public float Timer; //游戏速度
    public int step; //蛇头的移动距离
    private int X; //移动的增量值
    private int Y; //移动的增量值
    private Vector3 HeadPos; //蛇头的坐标
    private Vector3 OldHeadPos; //蛇头的坐标
    private bool isDragging = false;
    private bool isOneMove = true;
    private float releaseTime = 0f;

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

        //初始化direction为向上
        direction = Vector2.up;

        OnMove();
    }


    public void OnMove()
    {
        Debug.Log("OnMove");
        // 计算蛇头的新位置
        HeadPos = gameObject.transform.position;

        // 计算蛇头的移动方向，也就是现在的第一节身体的位置减去蛇头的位置
        Vector3 moveDirection = bodyList.Count > 0 ? bodyList[0].position - HeadPos : direction * step;
        
        // 如果蛇身存在且蛇头和第一节身体的距离小于或等于一个步长，那么不执行移动
        if (bodyList.Count > 0 && Vector3.Distance(HeadPos, bodyList[0].position) <= (step * 1.5))
        {
            path.Clear();
            return;
        }


        if (isOneMove == false)
        {
            //清空当前路径
            path.Clear();
            
            // 将蛇头的移动路径分解为一系列的正交步骤
            int steps = Mathf.RoundToInt(Mathf.Max(Mathf.Abs(moveDirection.x), Mathf.Abs(moveDirection.y)));
            Vector3 stepDirection = moveDirection / steps;
            for (int i = 0; i < steps; i++)
            {
                // 将每个步骤添加到路径中
                path.Enqueue(HeadPos + stepDirection * i);
            }
        
            // 倒序
            path = new Queue<Vector3>(new Stack<Vector3>(path));
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
    
    
    public void Grow()
    {
        // 创建新的蛇身部分
        GameObject newBodyPart = Instantiate(bodyPrefab);

        // 将新的蛇身部分的位置设置为蛇身列表中的最后一个元素的位置
        newBodyPart.transform.position = bodyList[bodyList.Count - 1].position;

        // 根据蛇身的长度来选择纹理
        int index = bodyList.Count % 2;
        newBodyPart.GetComponent<SpriteRenderer>().sprite = bodySprites[index];

        // 将新的蛇身部分添加到蛇身列表的末尾
        bodyList.Add(newBodyPart.transform);
    }
    
    //碰撞
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("碰撞");
        if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject); //销毁食物
            this.Grow(); //生长尾巴
            RangeFood.Instance.AddFood(); //添加食物
        }
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0)) //按下鼠标左键
        {
            startPos = rb.position;
            line.enabled = true;
            line.SetPosition(0, startPos);
        }

        if (Input.GetMouseButton(0)) //按住鼠标左键
        {
            Vector2 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = startPos - currentPos;
            float distance = direction.magnitude;

            if (distance > maxDragDistance)
            {
                direction = direction.normalized * maxDragDistance;
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
        }

        // 检测“当头不再动”
        if (!(Input.GetMouseButton(0)) && rb.velocity.magnitude < 0.1f && Time.time - releaseTime > 0.1f)
        {
            if (moveCoroutine == null)
            {
                moveCoroutine = StartCoroutine(MoveRepeatedly());
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
}