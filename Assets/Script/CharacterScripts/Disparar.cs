using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class Disparar : MonoBehaviour
{
    public Transform balaPrefab;
    public Transform shootPoint;
    public float fuerza;

    private float tiempo = 0f;

    [SerializeField]
    private float balacooldown;

    private void Update()
    {
        tiempo -= Time.deltaTime;
        
        if (Input.GetKey(KeyCode.Z))
        {
            if (tiempo <= 0f)
            {
                Disparo();
                
            }
        }
    }

    private void Disparo()
    {
        if(CharacterController2D.isDeath == true)
        {
            Debug.Log("No");
        }
        else
        {
            AudioManager.Instance.Play("DisparoReimu");
            Transform clon = Instantiate(balaPrefab, shootPoint.position, shootPoint.rotation);
            clon.GetComponent<Rigidbody2D>().AddForce(transform.up * fuerza);
            tiempo += balacooldown;
        }
            
    }

    




}
