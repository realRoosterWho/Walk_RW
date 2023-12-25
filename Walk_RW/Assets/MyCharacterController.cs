using UnityEngine;
using System.Collections;
using System;

public class MyCharacterController : MonoBehaviour
{
    public float maxHeadMoveDistance = 5.0f;
    public float headMoveSpeed = 10.0f;
    public float tailMoveSpeed = 5.0f;
    public float tailMoveInterval = 0.5f;
    public float tailMoveStep = 0.25f;
    public float headMoveStep = 5f;
    private float keyPressDuration = 0.0f;

    private Vector3 headStartPosition;
    private Vector3 headTargetPosition;
    private Vector3 tailTargetPosition;
    private bool isHeadMoving = false;
    private bool isTailMoving = false;
    private float tailMoveTimer = 0.0f;
    
    
    private Vector2 lastDirectionInput = Vector2.zero;
    private float inputTimeWindow = 0.5f; // 允许的输入时间窗口，0.5秒
    private float directionTimer = 0.0f; // 方向键计时器

    // 用于表示角色的头部和尾部
    public Transform head;
    public Transform tail;

    // 添加一个LineRenderer组件的引用
    public LineRenderer lineRenderer;
    void Update()
    {
        //HandleHeadMovement();
        //HandleTailMovement();
        HandleDirectionInput();
    }
    
void HandleDirectionInput()
{
    // 如果头部和尾部都不在移动，则开始处理方向输入
    if (!isHeadMoving && !isTailMoving)
    {
        if (directionTimer < inputTimeWindow)
        {
            Debug.Log(isHeadMoving + " " + isTailMoving);
            // 检测所有八个方向的输入
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (input != Vector2.zero)
            {
                lastDirectionInput = input;
                keyPressDuration += Time.deltaTime; // 计算按键持续时间
                
                // 明确头部的起始位置和目标位置
                headStartPosition = head.position;
                float moveDistance = Mathf.Clamp((float)Math.Floor(keyPressDuration * headMoveStep), 0, maxHeadMoveDistance); // 使用按键持续时间计算移动距离
                headTargetPosition = headStartPosition + new Vector3(lastDirectionInput.normalized.x, lastDirectionInput.normalized.y, 0) * moveDistance;
                // 显示预测落点
                Debug.DrawLine(headStartPosition, headTargetPosition, Color.red, 2.0f);
                // 使用LineRenderer组件来显示预测落点
                Debug.Log("lineRenderer" + headTargetPosition + " " + headTargetPosition);
                lineRenderer.SetPosition(0, headStartPosition);
                lineRenderer.SetPosition(1, headTargetPosition);
            }
            else
            {
                if (lastDirectionInput != Vector2.zero) 
                {
                    // 定义头部目标位置
                    //StartHeadMovement(lastDirectionInput.normalized);
                    Debug.Log(lastDirectionInput.normalized);
                    isHeadMoving = true; // 在这里设置isHeadMoving为true

                }
                keyPressDuration = 0.0f; // 重置按键持续时间
            }
            directionTimer += Time.deltaTime;
        }
        else
        {
            // 重置方向输入和计时器
            directionTimer = 0.0f;
            lastDirectionInput = Vector2.zero;
        }
    }
}

void StartHeadMovement(Vector2 direction)
{
    Debug.Log("StartHeadMovement");
    headStartPosition = head.position;
    float moveDistance = Mathf.Clamp(keyPressDuration, 0, maxHeadMoveDistance); // 使用按键持续时间计算移动距离
    headTargetPosition = headStartPosition + new Vector3(direction.x, direction.y, 0) * moveDistance;

    // Debug输出落点
    Debug.Log("落点"+headTargetPosition);
}
    

    void HandleHeadMovement()
    {
        if (isHeadMoving)
        {
            // 头部的线性移动
            head.position = Vector3.MoveTowards(head.position, headTargetPosition, headMoveSpeed * Time.deltaTime);
            
            if (head.position == headTargetPosition)
            {
                isHeadMoving = false;
                isTailMoving = true;
                tailMoveTimer = 0.0f;
                tailTargetPosition = head.position;
            }
        }
        else if (!isTailMoving && Input.GetKeyDown(KeyCode.Space)) // 使用空格键来触发头部移动
        {
            // 确定头部目标位置
            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection.Normalize();
            headStartPosition = head.position;
            headTargetPosition = headStartPosition + moveDirection * maxHeadMoveDistance;
            

            isHeadMoving = true;
        }
    }

    void HandleTailMovement()
    {
        if (isTailMoving)
        {
            tailMoveTimer += Time.deltaTime;

            if (tailMoveTimer >= tailMoveInterval)
            {
                // 尾部的分段移动
                tail.position = Vector3.MoveTowards(tail.position, tailTargetPosition, tailMoveStep);
                tailMoveTimer = 0.0f;
                
                if (tail.position == tailTargetPosition)
                {
                    isTailMoving = false;
                }
            }
        }
    }
}
