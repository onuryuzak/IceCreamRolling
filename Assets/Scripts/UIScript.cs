using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIScript : MonoBehaviour
{

    public ScriptManager ScriptManager { get; private set; }
    [SerializeField] FloatingText _floatingTextPrefab;
    public TextMeshProUGUI PlayerMoneyText;
    public Joystick Joystick;

    void Start()
    {
        ScriptManager = ScriptManager.Instance;
        PlayerMoneyText.SetText(Mathf.CeilToInt(ScriptManager.PlayerBehaviour.PlayerPropertiesSO.Money).ToString());
        GameStateEnums.currentGameState = GameStateEnums.GameState.Waiting;
    }

    private void Update()
    {
        if (GameStateEnums.currentGameState == GameStateEnums.GameState.Waiting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameStateEnums.currentGameState = GameStateEnums.GameState.PlayerCanMove;
            }
        }

    }

    public void DecreasePlayerMoneyUI()
    {
        ScriptManager.PlayerBehaviour.PlayerPropertiesSO.Money -= ScriptManager.PlayerBehaviour.PlayerPropertiesSO.moneyDecreaseFactor;
        int currentPoint = int.Parse(PlayerMoneyText.text);
        int nextPoint = ScriptManager.PlayerBehaviour.PlayerPropertiesSO.Money;

        PlayerMoneyText.DOComplete();

        DOTween.To(() => currentPoint, x => currentPoint = x, nextPoint, 0f).SetEase(Ease.OutCubic).SetTarget(PlayerMoneyText)
            .OnUpdate(() => PlayerMoneyText.SetText(Mathf.CeilToInt(currentPoint).ToString()));
    }
    public void IncreasePlayerMoneyUI(int money)
    {
        int currentPoint = int.Parse(PlayerMoneyText.text);
        int nextPoint = money;

        PlayerMoneyText.DOComplete();

        DOTween.To(() => currentPoint, x => currentPoint = x, nextPoint, 0f).SetEase(Ease.OutCubic).SetTarget(PlayerMoneyText)
            .OnUpdate(() => PlayerMoneyText.SetText(Mathf.CeilToInt(currentPoint).ToString()));
    }

    public void EnableJoystick() //Open Joystick Controller
    {
        Joystick.gameObject.SetActive(true);
    }
    public void DisableJoystick() //Close Joystick Controller
    {
        Joystick.gameObject.SetActive(false);
    }

    public void showMoneyText(Transform targetPoint, int money)
    {
        FloatingText floatingText = Instantiate(_floatingTextPrefab);
        floatingText.transform.position = targetPoint.position + Vector3.up * 10;

        floatingText.setText(money);
    }


}
