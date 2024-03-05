using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    
    public AudioSource Source;
    public AudioClip[] Clip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayBtnSound()
    {
        Source.PlayOneShot(Clip[0]);
    }
    public void PlayJump()
    {
        Source.PlayOneShot(Clip[1]);
    }
    public void PlayGameOver()
    {
        Source.Stop();
        Source.PlayOneShot(Clip[2]);
    }
    public void PlayHurt()
    {
        Source.PlayOneShot(Clip[3]);
    }
   
    public void PlayMonney()
    {
        Source.PlayOneShot(Clip[4]);
    }
    public void PlayItem()
    {
        Source.PlayOneShot(Clip[5]);
    }
}
