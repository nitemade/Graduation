using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float movex = Input.GetAxis("Horizontal");//水平方向移动
        float movey = Input.GetAxis("Vertical");//垂直方向移动

        Vector2 position = transform.position;
        position.x += movex * speed * Time.deltaTime;
        position.y += movey * speed * Time.deltaTime;

        transform.position = position;
    }
}
