using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //获取角色数值
    private CharacterStats stats;
    private CharacterAnimator characterAnimator;

    //面朝方向
    private float lastLookX = 0f;
    private float lastLookY = 0f;


    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        characterAnimator = GetComponent<CharacterAnimator>();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (stats.IsDead) return;
        Move();
    }

    //移动
    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        float move = Mathf.Abs(x) + Mathf.Abs(y);
        Vector2 position = transform.position;
        position.x += x * stats.CurrentSpeed * Time.deltaTime;
        position.y += y * stats.CurrentSpeed * Time.deltaTime;
        transform.position = position;

        if(move > 0)
        {
            lastLookX = x;
            lastLookY = y;
            stats.SetState(CharacterState.Walk);
            characterAnimator.FaceDiraction(lastLookX, lastLookY);
        }
        else
        {
            stats.SetState(CharacterState.Idle);
        }
    }
}
