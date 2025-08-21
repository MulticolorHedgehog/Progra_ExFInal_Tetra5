using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Contador : MonoBehaviour
{
    
    private TextMeshProUGUI textor;

    public string valor;

    private void Start()
    {
        textor = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        
        if(valor == "Vidas")
        {
            textor.text = "Lives " + GameManager.Instance.Vidas;
        }
        
        if(valor == "Score")
        {
            textor.text = "Score " + GameManager.Instance.Score;
        }

        if(valor == "HiScore")
        {
            textor.text = "HiScore " + GameManager.Instance.HiScore;
        }
    }
}
