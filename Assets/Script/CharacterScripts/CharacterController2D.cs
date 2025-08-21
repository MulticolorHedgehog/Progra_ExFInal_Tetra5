using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public static Rigidbody2D rb;

    public static CircleCollider2D circulo;
    public int velocidad;
    private int velocidadog;

    private Transform LimiteCheck1;
    private Transform LimiteCheck2;

    public bool cosa;

    public static bool isDeath;

    public LayerMask whatisWall;

    private float movX;
    private float movY;



    // Start is called before the first frame update
    void Start()
    {
        //LimiteCheck1 = transform.GetChild(0);
        //LimiteCheck2 = transform.GetChild(1);
        rb = GetComponent<Rigidbody2D>();
        velocidadog = velocidad;
        circulo = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //cosa = Physics2D.OverlapArea(LimiteCheck1.position, LimiteCheck2.position, whatisWall);

        

        movX = Input.GetAxis("Horizontal");
        movY = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            velocidad /= 5;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            velocidad = velocidadog;
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(movX * velocidad * Time.deltaTime, movY * velocidad * Time.deltaTime, 0);
        

    }
}
