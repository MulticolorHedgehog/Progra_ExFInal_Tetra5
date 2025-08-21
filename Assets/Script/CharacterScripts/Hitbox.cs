using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            spriteRenderer.enabled = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            spriteRenderer.enabled = false;
        }
    }
}
