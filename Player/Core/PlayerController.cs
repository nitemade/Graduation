using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
[RequireComponent(typeof(CharacterStats))]
public class PlayerController : MonoBehaviour
{
    private MoveController moveController;
    private CharacterStats stats;



    private void Awake()
    {
        moveController = GetComponent<MoveController>();
        stats = GetComponent<CharacterStats>();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (stats.IsDead) return;
        MoveInput();
    }

    //ÒÆ¶¯
    void MoveInput()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical")!= 0)
        {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveController.SetInput(input);
        }

    }
}
