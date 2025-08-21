using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruccionBala : MonoBehaviour
{
    

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Limite"))
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            GameManager.Instance.Score += 50;
            Destroy(this.gameObject);
        }
    }


}
