using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public GameObject PuntoA;
    public GameObject PuntoB;
    private Rigidbody2D rb;
    
    private Transform PuntoActual;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        PuntoActual = PuntoB.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = PuntoActual.position - transform.position;
        if(PuntoActual == PuntoB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        if(Vector2.Distance(transform.position, PuntoActual.position) < 0.5f && PuntoActual == PuntoB.transform)
        {
            flip();
            PuntoActual = PuntoA.transform;
        }
        if (Vector2.Distance(transform.position, PuntoActual.position) < 0.5f && PuntoActual == PuntoA.transform)
        {
            flip();
            PuntoActual = PuntoB.transform;
        }


    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= 1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(PuntoA.transform.position, 0.05f);
        Gizmos.DrawWireSphere(PuntoB.transform.position, 0.05f);
        Gizmos.DrawLine(PuntoA.transform.position, PuntoB.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce((Vector2.up * bounce)/9.28f, ForceMode2D.Impulse);
        }
    }
}
