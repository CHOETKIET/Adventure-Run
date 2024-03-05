using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MainSceneScript : MonoBehaviour
{
    public DataUser DU;
    public FirebaseManager FBM;


    public GameObject[] Stage;
    public GameObject[] HPUpgrade;
    public float UpgradePrice;
    public TMP_Text PriceTag;
    public TMP_Text SystemMassage;
    // Start is called before the first frame update
    void Start()
    {
        DU = GameObject.Find("UserData").GetComponent<DataUser>();
    }

    // Update is called once per frame
    void Update()
    {

        if (DU.Tsu == 1)
        {
            Stage[0].SetActive(false);
            Stage[1].SetActive(false);
            Stage[2].SetActive(false);
            Stage[3].SetActive(false);
        }
        if (DU.Tsu == 2)
        {
            Stage[0].SetActive(true);
            Stage[1].SetActive(false);
            Stage[2].SetActive(false);
            Stage[3].SetActive(false);
        }

        if (DU.Tsu == 3)
        {
            Stage[0].SetActive(true);
            Stage[1].SetActive(true);
            Stage[2].SetActive(false);
            Stage[3].SetActive(false);
        }
        if (DU.Tsu == 4)
        {
            Stage[0].SetActive(true);
            Stage[1].SetActive(true);
            Stage[2].SetActive(true);
            Stage[3].SetActive(false);
        }
        if (DU.Tsu == 5)
        {
            Stage[0].SetActive(true);
            Stage[1].SetActive(true);
            Stage[2].SetActive(true);
            Stage[3].SetActive(true);
        }


        if (DU.Level == 1)
        {
            HPUpgrade[0].SetActive(true);
            HPUpgrade[1].SetActive(false);
            HPUpgrade[2].SetActive(false);
            HPUpgrade[3].SetActive(false);
            HPUpgrade[4].SetActive(false);
            UpgradePrice = 5000;
            PriceTag.text = "5000";
        }
        if (DU.Level == 2)
        {
            HPUpgrade[0].SetActive(true);
            HPUpgrade[1].SetActive(true);
            HPUpgrade[2].SetActive(false);
            HPUpgrade[3].SetActive(false);
            HPUpgrade[4].SetActive(false);
            UpgradePrice = 10000;
            PriceTag.text = "10000";
        }
        if (DU.Level == 3)
        {
            HPUpgrade[0].SetActive(true);
            HPUpgrade[1].SetActive(true);
            HPUpgrade[2].SetActive(true);
            HPUpgrade[3].SetActive(false);
            HPUpgrade[4].SetActive(false);
            UpgradePrice = 15000;
            PriceTag.text = "15000";
        }
        if (DU.Level == 4)
        {
            HPUpgrade[0].SetActive(true);
            HPUpgrade[1].SetActive(true);
            HPUpgrade[2].SetActive(true);
            HPUpgrade[3].SetActive(true);
            HPUpgrade[4].SetActive(false);
            UpgradePrice = 20000;
            PriceTag.text = "20000";
        }
        if (DU.Level == 5)
        {
            HPUpgrade[0].SetActive(true);
            HPUpgrade[1].SetActive(true);
            HPUpgrade[2].SetActive(true);
            HPUpgrade[3].SetActive(true);
            HPUpgrade[4].SetActive(true);
            PriceTag.text = "MAX";

        }

        if (DU.Monney > UpgradePrice)
        {           
            SystemMassage.text = "";
        }
        else
        {
            SystemMassage.text = "Not enough monney";
        }

    }
   

    public void UpgradeBtn()
    {
       if(DU.Level !=5)
        {
            if (DU.Monney > UpgradePrice)
            {
                DU.Monney -= UpgradePrice;
                DU.Level++;
                FBM.UpdatePlayerFirebase();

            }
            else
            {

            }
        }
    }
       
}
