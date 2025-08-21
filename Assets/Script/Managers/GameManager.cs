using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Points;

    public int Graze;

    public int Score;

    public int HiScore;

    public int Vidas;

    public int ScoreforExtraLife;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if(Score >= HiScore)
        {
            HiScore = Score;
        }

        if(Score >= ScoreforExtraLife)
        {
            ScoreforExtraLife += ScoreforExtraLife;
            Vidas = Vidas + 1;
        }
    }
}
