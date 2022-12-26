using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class MoneyItem : CollectableItem
{

    public int moneyValue;
    Rigidbody _ridigBody;
    public ScriptManager ScriptManager { get; private set; }

    private void Start()
    {
        canCollect = true;
        _ridigBody = GetComponent<Rigidbody>();
        ScriptManager = ScriptManager.Instance;

    }

    //private void Update()
    //{
    //    if (!collected && canCollect)
    //    {
    //        float distance = (ScriptManager.PlayerBehaviour.transform.position - transform.position).magnitude;

    //        if (distance < distanceToCollect)
    //        {

    //            ScriptManager.UIScript.showMoneyText(transform, moneyValue);
    //        }
    //    }
    //}

    public override void OnCollected(Vector3 targetPoint, Transform parent = null)
    {
        _ridigBody.isKinematic = true;
        collected = true;
        transform.SetParent(parent);
        transform.DOComplete();
        transform.DOLocalJump(targetPoint, 5, 1, .4f).OnComplete(() =>
        {
            ScriptManager.PlayerBehaviour.PlayerPropertiesSO.Money += moneyValue + ScriptManager.PlayerBehaviour.PlayerPropertiesSO.IncomeIncreaseFactor;
            ScriptManager.Instance.UIScript.IncreasePlayerMoneyUI(ScriptManager.PlayerBehaviour.PlayerPropertiesSO.Money);
            Destroy(gameObject);
        });
    }
}
