using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Muerte : MonoBehaviour
{
    private Vector2 StartPos;

    private void Start()
    {
        StartPos = transform.position;
    }

    private void Update()
    {
        if(GameManager.Instance.Vidas <= 0)
        {
            Debug.Log("Muelto");
            StartCoroutine(MuerteEscena(0.3f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            GameManager.Instance.Vidas -= 1;
            Die();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("BalaEnemigo"))
        {
            GameManager.Instance.Vidas -= 1;
            AudioManager.Instance.Play("Muerte");
            Die();
        }
    }

   

    void Die()
    {
        StartCoroutine(Respawn(0.5f));
    }

    IEnumerator Respawn(float duration)
    {
        CharacterController2D.isDeath = true;
        CharacterController2D.rb.simulated = false;
        CharacterController2D.rb.velocity = new Vector2(0, 0);
        transform.localScale = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(duration);
        StartCoroutine(Hitbox(1.0f));
        CharacterController2D.isDeath = false;
        transform.position = StartPos;
        transform.localScale = new Vector3(1, 1, 1);
        CharacterController2D.rb.simulated = true;
    }

    IEnumerator MuerteEscena(float duration)
    {
        CharacterController2D.isDeath = true;
        CharacterController2D.rb.simulated = false;
        CharacterController2D.rb.velocity = new Vector2(0, 0);
        transform.localScale = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(duration);
        AudioManager.Instance.Stop("MusicaJueguito");
        SceneManager.LoadScene("Titulo");
    }

    IEnumerator Hitbox(float duration)
    {
        Debug.Log("HitBox apagada");
        CharacterController2D.circulo.enabled = false;
        yield return new WaitForSeconds(duration);
        Debug.Log("HitBox apagadan't");
        CharacterController2D.circulo.enabled = true;
    }
}
