using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DataUser : MonoBehaviour
{
   
    public string Username;
    public float Monney;
    public float Level;
    public float Tsu;
    
    public static DataUser DU;
    public bool LoggedIn = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (DU == null)
        {
            DU = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Debug.Log("Called DontDestroyOnLoad");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
