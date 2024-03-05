using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{

    public ObjectScript objectScript;
    public PlayerScript Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerScript>();
        objectScript = GameObject.Find("ObjectGenerator").GetComponent<ObjectScript>();

    }

    // Update is called once per frame
    void Update()
    {
       if (Player.Timer >0)
        {
            if (Player.Immune == true)
            {
                transform.Translate(Vector2.left * objectScript.currentSpeed * Time.deltaTime*2);
            }
            else
            {
                transform.Translate(Vector2.left * objectScript.currentSpeed * Time.deltaTime);
            }

            
        }
            
        


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.tag == "ground")
        {           
            if (collision.gameObject.CompareTag("endLine"))
            {
                objectScript.CurrentPlatform--;
                Destroy(this.gameObject);
            }
        }
      


        if (this.gameObject.tag == "FireballItem")
        {
            if (collision.gameObject.CompareTag("Player"))
            {               
                Player.Immune = true;
                Player.BoostTime = Player.MaxBoost;
                
                Destroy(this.gameObject);
            }
        }
        if (this.gameObject.tag == "HPItem")
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player.Timer += 20;
                Destroy(this.gameObject);
            }
        }

        /*if (this.gameObject.tag == "ground")
        {
            
            if (collision.gameObject.CompareTag("endLine"))
            {
                objectScript.GenerateGround();
                Destroy(this.gameObject);
            }
        }*/
    }
}
