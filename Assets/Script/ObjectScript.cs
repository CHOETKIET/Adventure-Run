using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public PlayerScript Player;

    public float MinSpeed;
    public float MaxSpeed;
    public float currentSpeed;
    public GameObject TrapG, TrapC,ItemH,ItemF,ground,pit;
    public GameObject[] Ground;
    public float CurrentPlatform = 1;

    int WhatToSpawn;
    public Transform[] SpawnPoint;
    public float ItemSpawnCD ,MaxItemSpawnCD;

    public float SpeedMultiplier;
    
    public float spawnCooldown , maxCooldown;

    // Start is called before the first frame update
    private void Awake()
    {
        currentSpeed = MinSpeed;
        
        
        ItemSpawnCD = MaxItemSpawnCD;
    }
    public void GenerateNextPlatform()
    {
        float randomWait = Random.Range(1f, 3f);
        Invoke("generateGround", randomWait);
    }
    public void GenerateItemObj()
    {       
        if(ItemSpawnCD <1 && Player.Immune == false)
        {
            generateItem();
            ItemSpawnCD = MaxItemSpawnCD;
        }

    }
   
    /*public void generateSpike()
    {      
         WhatToSpawn = Random.Range(1, 3);

        switch (WhatToSpawn)
        {
            case 1:
                GameObject trap_C = Instantiate(TrapC, SpawnPoint[0].position, SpawnPoint[0].rotation);
                trap_C.GetComponent<ObjectMovement>().objectScript = this;
                break;

            case 2:
                GameObject trap_G = Instantiate(TrapG, SpawnPoint[1].position, SpawnPoint[1].rotation);
                trap_G.GetComponent<ObjectMovement>().objectScript = this;
                break;
        }
    }*/

    public void generateItem()
    {
        WhatToSpawn = Random.Range(1, 4);
        switch (WhatToSpawn)
        {
            case 1:
                GameObject item_H = Instantiate(ItemH, transform.position, transform.rotation);
                item_H.GetComponent<ObjectMovement>().objectScript = this;
                break;

            case 2:
                GameObject item_F = Instantiate(ItemF, transform.position, transform.rotation);
                item_F.GetComponent<ObjectMovement>().objectScript = this;
                break;
           
        }
    }
    public void generateGround()
    {
       
                WhatToSpawn = Random.Range(1,6);
                switch (WhatToSpawn)
                {
                    case 1:
                        GameObject A_Ground = Instantiate(Ground[0], SpawnPoint[2].position, SpawnPoint[2].rotation);
                        A_Ground.GetComponent<ObjectMovement>().objectScript = this;
                        CurrentPlatform++;
                        break;

                    case 2:
                        GameObject B_Ground = Instantiate(Ground[1], SpawnPoint[2].position, SpawnPoint[2].rotation);
                        B_Ground.GetComponent<ObjectMovement>().objectScript = this;
                        CurrentPlatform++;
                        break;
                    case 3:
                        GameObject C_Ground = Instantiate(Ground[2], SpawnPoint[2].position, SpawnPoint[2].rotation);
                        C_Ground.GetComponent<ObjectMovement>().objectScript = this;
                        CurrentPlatform++;
                        break;
                    case 4:
                        GameObject D_Ground = Instantiate(Ground[3], SpawnPoint[2].position, SpawnPoint[2].rotation);
                        D_Ground.GetComponent<ObjectMovement>().objectScript = this;
                        CurrentPlatform++;
                        break;
                    case 5:
                        GameObject F_Ground = Instantiate(Ground[4], SpawnPoint[2].position, SpawnPoint[2].rotation);
                        F_Ground.GetComponent<ObjectMovement>().objectScript = this;
                        CurrentPlatform++;
                        break;
                   

        }
    }
   

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Player.Timer >0)
        {
            if (currentSpeed < MaxSpeed)
            {
                currentSpeed += SpeedMultiplier;
            }
            if (ItemSpawnCD > 1)
            {
                ItemSpawnCD -= Time.deltaTime;
            }

            GenerateItemObj();

            if (Player.Immune == true)
            {
                spawnCooldown -= Time.deltaTime*2;
            }
            else
            {
                spawnCooldown -= Time.deltaTime;
            }
            
            if (spawnCooldown < 1)
            {
                generateGround();
                spawnCooldown = maxCooldown;
            }
        }
        
    }
}
