using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoodText : MonoBehaviour
{
    
    public TextMeshPro foodText;
    public int foodnum ;
	public bool isRandomFood = true;
	public float bornTime;
    
    private Vector2 currenToScreenPoint;
    // Start is called before the first frame update
    void Start()
    {
	    //关闭spriteRenderer组件
	    SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	    if (spriteRenderer != null)
	    {
		    spriteRenderer.enabled = false;
	    }
	        
	    //关闭textMeshPro组件
	    if (foodText != null)
	    {
		    foodText.enabled = false;
	    }
	    
	    //记录出生时间
	    bornTime = Time.time;
	    
	    

		if (isRandomFood == true)
		{
        // 随机生成1~3的整数
        foodnum = Random.Range(1, 4);
		}
		
		//获取物体上的TextMeshPro,并将其赋值给foodText
		foodText = gameObject.GetComponent<TextMeshPro>();

        //将TextMeshProUGUI组件的文本设置为foodnum
		if (foodText != null)
		{
        foodText.text = foodnum.ToString();
		}
    }
    
    //如果和标签为“Wall”的东西碰撞，在RangeFood脚本中调用AddFood方法，随后销毁自己
    private void OnTriggerEnter2D(Collider2D other) //需要当前的物体勾选上Is Trigger
	{
		if (other.tag == "Wall")
		{
			//在RangeFood脚本中调用AddFood方法
			GameObject.Find("RangeFood").GetComponent<RangeFood>().AddFood();
			//销毁自己
			Destroy(gameObject);
		}
	}
    
 //    void OnCollisionEnter2D(Collision2D collision)
	// {
	//     // 检查碰撞的游戏对象是否是Tilemap
	//     if (collision.gameObject.CompareTag("Wall"))
	//     {
	//         // 如果是Tilemap，那么在控制台打印一条消息
	//         Debug.Log("Collided with Tilemap");
	//     }
	// }
    
    

    // Update is called once per frame
    void Update()
    {

        //该物体在生成0.1秒后才打开spriteRenderer和textMeshPro组件(如果有)
        if (Time.time - bornTime> 0.1f)
		{
			//打开spriteRenderer组件
			SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
			if (spriteRenderer != null)
			{
				spriteRenderer.enabled = true;
			}
			
			//打开textMeshPro组件
			if (foodText != null)
			{
				foodText.enabled = true;
			}
		}
        else
        {
	        //关闭spriteRenderer组件
	        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	        if (spriteRenderer != null)
	        {
		        spriteRenderer.enabled = false;
	        }
	        
	        //关闭textMeshPro组件
	        if (foodText != null)
	        {
		        foodText.enabled = false;
	        }
        }
        
    }
}
