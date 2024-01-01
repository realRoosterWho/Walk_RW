using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SnakeHeadController : MonoBehaviour
{
 public float Timer;                //游戏速度
    public int step;//蛇头的移动距离
    private int X;//移动的增量值
    private int Y;//移动的增量值
    private Vector3 HeadPos;//蛇头的坐标
    public GameObject bodyPrefab;    //蛇尾的预设
    //蛇身列表
    public List<Transform> bodyList = new List<Transform>();
    public Sprite[] bodySprites = new Sprite[2];//图片

    private void Start()
    {
        InvokeRepeating("OnMove", 0, Timer);
        SetSnakeHeadMoveOffset(step, 0); //设置初始移动的方向
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && Y != -step)
        {
            SetSnakeHeadMoveOffset(0, step);
        }
        if (Input.GetKeyDown(KeyCode.S) && Y != step)
        {
            SetSnakeHeadMoveOffset(0, -step);
        }
        if (Input.GetKeyDown(KeyCode.A) && X != step)
        {
            SetSnakeHeadMoveOffset(-step, 0);
        }
        if (Input.GetKeyDown(KeyCode.D) && X != -step)
        {
            SetSnakeHeadMoveOffset(step, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))//加速
        {
            CancelInvoke();
            InvokeRepeating("OnMove", 0, Timer - 0.2f);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke();
            InvokeRepeating("OnMove", 0, Timer);
        }
    }
    //监听移动
    public void OnMove()
    {
        HeadPos = gameObject.transform.localPosition;//获取头的位置
        gameObject.transform.localPosition = new Vector3(HeadPos.x + X, HeadPos.y + Y, HeadPos.z);//移动
        if (bodyList.Count > 0)
        { 
            for (int i = bodyList.Count - 2; i >= 0; i--)                                           //从后往前开始移动蛇身
            {
                bodyList[i + 1].localPosition = bodyList[i].localPosition;                          //每一个蛇身都移动到它前面一个节点的位置
            }
            bodyList[0].localPosition = HeadPos;                                                    //第一个蛇身移动到蛇头移动前的位置
        }
    }
    //设置偏移量
    public void SetSnakeHeadMoveOffset(int x, int y)
    {
        X = x;
        Y = y;
    }
    //生长尾巴
    public void Grow()
    {
        int index = (bodyList.Count % 2 == 0) ? 0 : 1;
        GameObject body = Instantiate(bodyPrefab, new Vector3(2000, 2000, 0), Quaternion.identity);
        body.GetComponent<Image>().sprite = bodySprites[index];
        body.transform.SetParent(GameObject.Find("Canvas").transform, false);
        bodyList.Add(body.transform);
    }
   //碰撞
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject);//销毁食物
            this.Grow();//生长尾巴
            RangeFood.instance.AddFood();//添加食物
        }
    }
}
