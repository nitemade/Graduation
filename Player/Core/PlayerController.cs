using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
[RequireComponent(typeof(CharacterStats))]
public class PlayerController : MonoBehaviour
{
    private MoveController moveController;
    private CharacterStats stats;

    private readonly Vector2 _zeroInput = Vector2.zero;


    private void Awake()
    {
        moveController = GetComponent<MoveController>();
        stats = GetComponent<CharacterStats>();
    }


    // Update is called once per frame
    void Update()
    {
        if (stats.IsDead || MenuManager.Instance.IsPaused) return;
        MoveInput();
    }

    //移动
    void MoveInput()
    {
        // 读取一次输入轴并缓存，避免重复调用Input.GetAxisRaw
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            // 使用临时变量创建Vector2，减少GC（Unity的Vector2是值类型，栈上分配，GC影响极小）
            Vector2 input = new Vector2(horizontal, vertical);
            moveController.SetInput(input);
        }
        else
        {
            moveController.SetInput(_zeroInput);
        }
    }
}
