using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoodText : MonoBehaviour
{
    
    public TextMeshPro foodText;
    public int foodnum ;
	public bool isRandomFood = true;
    
    private Vector2 currenToScreenPoint;
    // Start is called before the first frame update
    void Start()
    {

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
    private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Wall")
		{
			//在RangeFood脚本中调用AddFood方法
			GameObject.Find("RangeFood").GetComponent<RangeFood>().AddFood();
			//销毁自己
			Destroy(gameObject);
		}
	}

    // Update is called once per frame
    void Update()
    {

        
        
    }
}
