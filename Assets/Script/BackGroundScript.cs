using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScript : MonoBehaviour
{
    public PlayerScript Player;
    // Start is called before the first frame update
   
    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Player.Timer > 0)
        {
            transform.position += new Vector3(-2 * Time.deltaTime, 0);
            if (transform.position.x < -26.82)
            {
                transform.position = new Vector3(44.8f, transform.position.y);
            }
        }
       

    }
} 

