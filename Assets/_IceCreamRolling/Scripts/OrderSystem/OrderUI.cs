using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    public Image coneImage;
    public Image icecreamImage;
    public Image icecreamTick;
    public Image sauce;

    public GameObject Customer;
    public GameObject CustomerInstantiatePos;
    public CustomRenderTexture cs;

    private void OnEnable()
    {
        EventManager.OnNewCostumer += Delay;
        EventManager.OnTick += Tick;
    }
    private void OnDisable()
    {
        EventManager.OnNewCostumer -= Delay;
        EventManager.OnTick -= Tick;
    }

    private void Start()
    {
        coneImage.enabled = false;
        icecreamImage.enabled = false;
        icecreamTick.enabled = false;
        sauce.enabled = false;
        NewCostumer();
        StartCoroutine(Order());
    }

    IEnumerator Order()
    {
        yield return new WaitForSeconds(1.5f);
        coneImage.enabled = true;
        icecreamImage.enabled = true;
        sauce.enabled = true;

    }



    private void Tick(bool state)
    {
        ScriptManager.Instance.PlayerSnowShaderController.SetNewSplatMapWhenCustomer(cs);
        icecreamTick.enabled = state;
    }

    private void Delay()
    {
        Invoke("NewCostumer", 3f);
    }

    private void NewCostumer()
    {
        Instantiate(Customer, CustomerInstantiatePos.transform.position, Customer.transform.rotation, CustomerInstantiatePos.transform);
        Invoke("tickCall", 1f);
    }
    private void tickCall()
    {
        Tick(false);
    }
}
