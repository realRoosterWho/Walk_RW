using UnityEngine;
using System.Collections;
using System;

public class MyCharacterController : MonoBehaviour
{
    public float maxHeadMoveDistance = 5.0f;
    public float headMoveSpeed = 10.0f;
    public float tailMoveInterval = 0.5f;
    public float tailMoveStep = 2f;
    public float tailRotationSpeed = 5f;
    public float headMoveStep = 0.5f;
    public float roundingInterval = 1.0f; // 新增变量，用于自定义取整的单位间隔
    

    private Vector3 headStartPosition;
    private Vector3 headTargetPosition;
    private Vector3 tailTargetPosition;
    private bool isHeadMoving = false;
    private bool isTailMoving = false;
    private bool isTailRotating = false;
    private float tailMoveTimer = 0.0f;
    private float keyPressDuration = 0.0f;
    private Vector2 lastDirectionInputPrevious = Vector2.zero; // 新增变量，用于存储0.1秒前的lastDirectionInput
    
    
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
        HandleHeadMovement();
        HandleTailMovement();
        HandleDirectionInput();
    }
    


void HandleDirectionInput()
{
    // 如果头部和尾部都不在移动，则开始处理方向输入
    if (!isHeadMoving && !isTailMoving)
    {
        if (directionTimer < inputTimeWindow) // 只有当directionTimer小于inputTimeWindow时，才检测输入
        {
            Debug.Log(isHeadMoving + "<--isHeading iaTailMoving-->" + isTailMoving);
            Debug.Log("lastDirectionInput" + lastDirectionInput);
            // 检测所有八个方向的输入
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (Vector2.Distance(input, Vector2.zero) > 0.01f && (Vector2.Distance(input, lastDirectionInputPrevious) < 0.01f || Vector2.Distance(lastDirectionInputPrevious, Vector2.zero) < 0.01f)) // 只有当input不为零，并且与0.1秒前的lastDirectionInput一致时，才执行
            {
                lastDirectionInput = input;
                keyPressDuration += Time.deltaTime; // 计算按键持续时间

                

                // 明确头部的起始位置和目标位置
                headStartPosition = head.position;
                float moveDistance = Mathf.Clamp((float)Math.Floor((keyPressDuration * headMoveStep) / roundingInterval) * roundingInterval, 0, maxHeadMoveDistance); // 使用按键持续时间计算移动距离
                headTargetPosition = headStartPosition + new Vector3(lastDirectionInput.normalized.x, lastDirectionInput.normalized.y, 0) * moveDistance;

                // 显示预测落点
                //Debug.DrawLine(headStartPosition, headTargetPosition, Color.red, 2.0f);

                // 使用LineRenderer组件来显示预测落点
                //Debug.Log("lineRenderer" + headTargetPosition + " " + headTargetPosition);
                lineRenderer.SetPosition(0, headStartPosition);
                lineRenderer.SetPosition(1, headTargetPosition);
            }
            else
            {
                if (Vector2.Distance(lastDirectionInput, Vector2.zero) > 0.01f)
                {
                    // 定义头部目标位置
                    StartHeadMovement(headTargetPosition);
                    Debug.Log("LastDirectionInput_Normalized" + lastDirectionInput.normalized);
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
            lastDirectionInputPrevious = lastDirectionInput; // 在重置方向输入和计时器时，更新lastDirectionInputPrevious
            lastDirectionInput = Vector2.zero;
        }
    }
}

void StartHeadMovement(Vector2 headTargetPosition)
{
    Debug.Log("StartHeadMovement");
    
    
    // 重置lineRenderer
    lineRenderer.SetPosition(0, head.position);
    lineRenderer.SetPosition(1, head.position);
    
    // Debug输出落点
    Debug.Log("落点"+headTargetPosition);
}
    

    void HandleHeadMovement()
    {
        if (isHeadMoving)
        {
            // 头部的线性移动
            head.position = Vector3.MoveTowards(head.position, headTargetPosition, headMoveSpeed * Time.deltaTime);

            // 检查头部是否已经到达目标位置
            if (Vector3.Distance(head.position, headTargetPosition) < 0.01f)
            {
                isHeadMoving = false;
                isTailMoving = true;
                tailMoveTimer = 0.0f;
                tailTargetPosition = head.position;

                // 重置lineRenderer
                lineRenderer.SetPosition(0, head.position);
                lineRenderer.SetPosition(1, head.position);
            }
        }
    }

void HandleTailMovement()
{
    if (isTailMoving)
    {
        if (isTailRotating)
        {
            // 计算尾巴当前的方向和目标方向
            Vector3 currentDirection = tail.forward;
            Vector3 targetDirection = (tailTargetPosition - tail.position).normalized;

            // 计算尾巴当前的Z轴旋转角度和目标Z轴旋转角度
            float currentAngle = tail.eulerAngles.z;
            float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

            // 使用Mathf.MoveTowardsAngle方法来逐渐改变尾巴的Z轴旋转角度
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, tailRotationSpeed);

            // 更新尾巴的旋转
            tail.rotation = Quaternion.Euler(0, 0, newAngle);

            // 检查尾巴的旋转角度是否已经到达目标
            if (Mathf.Abs(Mathf.DeltaAngle(tail.eulerAngles.z, targetAngle)) < 0.01f)
            {
                isTailRotating = false;
                StartCoroutine(DelayBeforeMoving());
            }
        }
        else
        {
            tailMoveTimer += Time.deltaTime;

            if (tailMoveTimer >= tailMoveInterval)
            {
                // 尾部的分段移动
                Vector3 targetPosition = Vector3.MoveTowards(tail.position, tailTargetPosition, tailMoveStep);
                tailMoveTimer = 0.0f;

                // 更新尾巴的位置
                tail.position = targetPosition;

                // 检查尾巴的位置是否已经到达目标
                if (Vector3.Distance(tail.position, tailTargetPosition) < 0.01f)
                {
                    isTailMoving = false;
                    directionTimer = 0.0f;
                    lastDirectionInput = Vector2.zero;
                    lastDirectionInputPrevious = lastDirectionInput; // 在重置方向输入和计时器时，更新lastDirectionInputPrevious
                }
            }
        }
    }
}

IEnumerator DelayBeforeMoving()
{
    yield return new WaitForSeconds(0.5f); // 延迟0.5秒
}
}
