using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;


public class PlayerScript : MonoBehaviour
{
    public float JumpForce;

    bool isGrounded = false;
    Rigidbody2D RB;
    public float MaxTimer;
    public float Timer; //HP
    public Image timerSlider;//HPbar
    public Text TimeText;//HPtext

    public float score,scoremonney,Extrascoremonney;
    public Text ScoreText,ScoreResult,Monney;
    public float StageNumber;
    public float GoalScore;
    public float ExtraGoalScore;
    public GameObject SystemText;
    public bool[] Check;
    


    public float invicibilityLegnth;
    public float invicibilityCounter;

    public Renderer playerRenderer;
    public Collider2D Hitbox;
    private float flashCounter;
    public float flashLength = 0.1f;

    public float BoostTime , MaxBoost;
    public bool Immune;

    public Animator anim;
    public Animator[] UIanim;

    public float fallMultiplier =2.5f;
    public float JumpCount, MaxJump;
    
    public Rigidbody2D rb;
   

    public DataUser DU;
    public SoundManager SM;
    public DatabaseReference DBreference;



    // Start is called before the first frame update
    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        DU = GameObject.Find("UserData").GetComponent<DataUser>();
        
        
        score = 0;
        Immune = false;

        if (DU.Level == 1)
        {
            Timer = 100;
            MaxTimer = 100;
        }

        if (DU.Level == 2)
        {
            Timer = 110;
            MaxTimer = 110;
        }
        if (DU.Level == 3)
        {
            Timer = 125;
            MaxTimer = 125;
        }
        if (DU.Level == 4)
        {
            Timer = 135;
            MaxTimer = 135;
        }
        if (DU.Level == 5)
        {
            Timer = 150;
            MaxTimer = 150;
        }




    }


    // Update is called once per frame
    void Update()
    {
       
        if(rb.velocity.y<0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

        }
        scoremonney = score / 1000;
        scoremonney = Mathf.Round(scoremonney * 1.0f) * 1f;


        if (Timer > 0)
        {
            timerSlider.fillAmount = (float)Timer / (float)MaxTimer;
            Timer -= Time.deltaTime;


            if (Input.GetKeyDown(KeyCode.Space) )
            {
                if (isGrounded == true || JumpCount <2 )
                {
                    RB.velocity =(Vector2.up * JumpForce);
                    isGrounded = false;
                    JumpCount++;
                   
                    anim.SetBool("isGrounded", false);
                    SM.PlayJump();
                }
               

                
            }
            if (JumpCount == 2)
            {
                anim.SetBool("isDoubleJump", true);
            }

            if (Input.GetKey(KeyCode.S))
            {
                anim.SetBool("isSlide", true);
            }
            else
            {
                anim.SetBool("isSlide", false);
            }



            if (Immune == true)
            {
                score += Time.deltaTime * 3000;
            }
            else
            {
                score += Time.deltaTime * 1500;
            }
           



           

            

            if (BoostTime > 0)
            {
                BoostTime -= Time.deltaTime;
            }
            else
            {
               
                Immune = false;

                
            }
        }
        else
        {
            Timer = 0;
            timerSlider.fillAmount = 0;
            invicibilityCounter = 0;

            GameOver();        
            AddMonney();
            UnlockingStage();
            ExtraMonney();
            
        }
        if (invicibilityCounter > 0)
        {
            invicibilityCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0)
            {
                playerRenderer.enabled = !playerRenderer.enabled;

                flashCounter = flashLength;
            }
            if (invicibilityCounter <= 0)
            {
                playerRenderer.enabled = true;



            }
        }

        if (Timer > MaxTimer)
        {
            Timer = MaxTimer;
        }

        ScoreText.text = "" + score.ToString("F0");
        ScoreResult.text = "" + score.ToString("F0");
        Monney.text = "" + scoremonney.ToString("F0");
        DisplayTime(Timer);

        



    }
    public void AddMonney()
    {
        if (Check[0] == false)
        {
            DU.Monney += scoremonney;
            Check[0] = true;
        }
       
    }

    public void UnlockingStage()
    {
        if(Check[1] == false)
        {
            if (score >= GoalScore)
            {
                if (StageNumber == DU.Tsu)
                {
                    DU.Tsu++;
                    SystemText.SetActive(true);
                    Check[1] = true;
                }
            }
        }
       
    }
    public void ExtraMonney()
    {
        if (Check[3] == false)
        {
            if (score >= ExtraGoalScore)
            {
                DU.Monney += Extrascoremonney;
                Check[3] = true;
            }
        }
    }

    void DisplayTime(float Timer)
    {
        

         float minutes = Mathf.FloorToInt(Timer / 60);
         float seconds = Mathf.FloorToInt(Timer % 60);


        TimeText.text = Timer.ToString("F0");

        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            if (isGrounded == false)
            {
                isGrounded = true;
                anim.SetBool("isGrounded", true);
                anim.SetBool("isDoubleJump", false);
                JumpCount = 0;
            }
        }
     
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Spike")
        {
            if (invicibilityCounter < 1 && Immune == false)
            {
                Timer -= 20;
                invicibilityCounter = invicibilityLegnth;
                anim.SetTrigger("isHurt");
                SM.PlayHurt();
            }
        }

        if (other.gameObject.tag == "Daimond")
        {
            score += 1000;
           Destroy(other.gameObject);
            SM.PlayMonney();
        }
        if(other.gameObject.tag == "pit")
        {
            Timer = 0;
        }
        if (other.gameObject.tag == "FireballItem")
        {
           
            SM.PlayItem();
        }
        if (other.gameObject.tag == "HPItem")
        {

            SM.PlayItem();
        }
    }

   public void GameOver()
    {
        if (Check[2] == false)
        {
            SM.PlayGameOver();
            Check[2] = true;
        }
       
        anim.SetBool("isDead", true);
        UIanim[0].SetBool("isGameOver", true);
        UIanim[1].SetBool("isGameOver", true);
    }

    
}
