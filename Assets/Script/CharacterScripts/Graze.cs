using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graze : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo") || collision.gameObject.CompareTag("BalaEnemigo"))
        {
            GameManager.Instance.Score += 15;
            Debug.Log("Graizeo");
        }
    }
}
