using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //获取角色数值
    public CharacterStats stats;
    //获取动画控制器
    private Animator anim;

    //面朝方向
    private float lastLookX = 0f;
    private float lastLookY = 0f;


    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        anim = GetComponent<Animator>();

        if (stats == null)
        {
            Debug.LogError("CharacterStats component not found on Player!");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //移动函数
    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        float move = Mathf.Abs(x) + Mathf.Abs(y);
        Vector2 position = transform.position;
        position.x += x * stats.CurrentSpeed * Time.deltaTime;
        position.y += y * stats.CurrentSpeed * Time.deltaTime;
        transform.position = position;

        if(x != 0 || y != 0)
        {
            lastLookX = x;
            lastLookY = y;
        }
        if(move > 1)
        {
            move = Mathf.Sqrt(2) / 2f;
        }

        anim.SetFloat("LookX", lastLookX);
        anim.SetFloat("LookY", lastLookY);
        anim.SetFloat("Speed", move * stats.CurrentSpeed / stats.Speed);
    }
}
