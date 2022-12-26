using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CostumerController : Area
{
    public GameObject orderCanvasGameObject;
    private Animator animator;
    PlayerBehaviour behaviour;
    public GameObject ConePos;



    private void Awake()
    {
        orderCanvasGameObject.SetActive(false);
        animator = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        transform.DOLocalMoveY(10.75f, 1.5f).SetEase(Ease.OutBounce).OnComplete(() => orderCanvasGameObject.SetActive(true));
    }
    protected override void OnEnterArea(PlayerBehaviour playerBehaviour)
    {
        behaviour = playerBehaviour;
        if (playerBehaviour.IsConeOnHand)
        {
            playerBehaviour.IsConeOnHand = false;
            playerBehaviour.Scope.SetActive(true);
            playerBehaviour.ReachMaxSize = false;
            playerBehaviour.IsStartGrownUp = true;
            animator.SetBool("Excited", true);
            EventManager.Tick(true);
            playerBehaviour.CurrentCone.transform.SetParent(orderCanvasGameObject.transform);
            playerBehaviour.CurrentCone.transform.DOLocalJump(Vector3.zero, 1, 1, 1);
        }

    }
    public void AnimEvent1()
    {
        animator.SetBool("Excited", false);

        ReleaseMoneyCostumers();
    }
    public void AnimEvent2()
    {
        behaviour.CurrentCone.transform.SetParent(ConePos.transform);
        behaviour.CurrentCone.transform.localPosition = Vector3.zero;
        behaviour.CurrentCone.transform.localRotation = Quaternion.Euler(Vector3.zero);


    }
    private void ReleaseMoneyCostumers()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject money = ObjectPooler.Instance.GetPooledObject("Money");
            money.transform.position = orderCanvasGameObject.transform.position;
            money.SetActive(true);
        }
        EventManager.NewCustomer();
        Destroy(behaviour.CurrentCone);
        Destroy(gameObject);

    }

    protected override void OnExitArea(PlayerBehaviour playerBehaviour)
    {

    }

    protected override void SetFeeText(int price)
    {

    }


}
