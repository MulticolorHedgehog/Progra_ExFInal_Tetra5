using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStart : MonoBehaviour
{
    public string MusicaPorSonar;
    
    
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.Play(MusicaPorSonar);
    }

    // Update is called once per frame
    
    public void EndMusic()
    {
        AudioManager.Instance.Stop(MusicaPorSonar);
    }

}
