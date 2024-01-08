using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shop : MonoBehaviour
{
    public int cost1;
    public int cost2;
    public GameObject buyButton1;
    public GameObject buyButton2;
    private void OnEnable()
    {
        CheckBuy();
    }
    public void Buy1()
    {
        if (PlayerPrefs.GetInt("Money")>=cost1)
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money")-cost1);
            PlayerPrefs.SetInt("Buy1", 1);
            PlayerPrefs.Save();
            CheckBuy();
        }
    }
    public void Buy2()
    {
        if (PlayerPrefs.GetInt("Money") >= cost2)
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - cost2);
            PlayerPrefs.SetInt("Buy2", 1);
            PlayerPrefs.Save();
            CheckBuy();
        }
    }
    private void CheckBuy()
    {
        if (PlayerPrefs.HasKey("Buy1"))
        {
            buyButton1.SetActive(false);
        }
        if (PlayerPrefs.HasKey("Buy2"))
        {
            buyButton2.SetActive(false);
        }
        Manager.instance1.UpdateCoinCount(0);
        PlayerController.instance.CheckBuy();
    }
}
