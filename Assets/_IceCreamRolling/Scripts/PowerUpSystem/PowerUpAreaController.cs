using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PowerUpAreaController : Area
{
    public float timeRemaining = 1f;
    public ScriptManager ScriptManager { get; private set; }
    [SerializeField] PowerUPSSO _powerUPSSO;
    [SerializeField] Image _whiteTriggerVisual;
    [SerializeField] Image _greenTriggerVisual;
    [SerializeField] TextMeshProUGUI areaFeeText;
    int tempAreaFee;

    private bool _startTiming;
    private PlayerBehaviour _playerBehaviour;
    private void Start()
    {
        ScriptManager = ScriptManager.Instance;
        SetFeeText(_powerUPSSO.SpeedPowerUpPrice);

    }

    private void Update()
    {
        if (_startTiming && 0 < _playerBehaviour.PlayerPropertiesSO.Money)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                if (_greenTriggerVisual.fillAmount <= 1)
                {
                    _greenTriggerVisual.fillAmount += Time.deltaTime;
                }


            }
            else
            {
                timeRemaining = 0;
                _startTiming = false;
                _greenTriggerVisual.fillAmount = 0;
                GameStateEnums.currentGameState = GameStateEnums.GameState.InPowerUpArea;
                StartCoroutine(COPowerUpProcess(_playerBehaviour));
            }

        }
        else
        {
            _greenTriggerVisual.fillAmount = 0;
        }
    }

    protected override void OnEnterArea(PlayerBehaviour playerBehaviour)
    {
        _playerBehaviour = playerBehaviour;
        if (0 < _playerBehaviour.PlayerPropertiesSO.Money)
        {
            _startTiming = true;
            _whiteTriggerVisual.enabled = false;
            _greenTriggerVisual.enabled = true;
        }
    }

    protected override void OnExitArea(PlayerBehaviour playerBehaviour)
    {
        _startTiming = false;
        timeRemaining = 1f;
        _greenTriggerVisual.enabled = false;
        _whiteTriggerVisual.enabled = true;
        GameStateEnums.currentGameState = GameStateEnums.GameState.PlayerCanMove;
    }

    IEnumerator COPowerUpProcess(PlayerBehaviour playerBehaviour)
    {
        while (GameStateEnums.currentGameState == GameStateEnums.GameState.InPowerUpArea
            && 0 < playerBehaviour.PlayerPropertiesSO.Money)
        {
            areaFeeText.DOComplete();
            int currentPoint = int.Parse(areaFeeText.text);
            int nextPoint = currentPoint - playerBehaviour.PlayerPropertiesSO.moneyDecreaseFactor;


            DOTween.To(() => currentPoint, x => currentPoint = x, nextPoint, 0f).SetEase(Ease.OutCubic).SetTarget(areaFeeText)
                .OnUpdate(() =>
                {
                    if (0 <= currentPoint)
                    {
                        ScriptManager.UIScript.DecreasePlayerMoneyUI();
                        playerBehaviour.MoneyRelease(Vector3.zero, transform);
                        SetFeeText(currentPoint);
                        if (currentPoint == 0)
                        {
                            GetPowerUp(_powerUPSSO.PowerUpType, playerBehaviour);
                        }
                    }
                });

            yield return new WaitForEndOfFrame();

        }
    }

    private void GetPowerUp(PowerUp type, PlayerBehaviour playerBehaviour)
    {
        switch (type)
        {

            case PowerUp.Speed:
                if (_powerUPSSO.SpeedPowerUpPrice <= playerBehaviour.PlayerPropertiesSO.Money)
                {
                    GameStateEnums.currentGameState = GameStateEnums.GameState.PlayerCanMove;
                    playerBehaviour.PlayerPropertiesSO.MovementSpeed += _powerUPSSO.SpeedPowerUp;
                    _powerUPSSO.SpeedPowerUpPrice += 5;
                    SetFeeText(_powerUPSSO.SpeedPowerUpPrice);
                    ScriptManager.PlayerMovementController.SetNewSpeed(playerBehaviour.PlayerPropertiesSO.MovementSpeed);

                }

                break;
            case PowerUp.DiggingSpeed:
                if (_powerUPSSO.DiggingSpeedPowerUpPrice <= playerBehaviour.PlayerPropertiesSO.Money)
                {
                    GameStateEnums.currentGameState = GameStateEnums.GameState.PlayerCanMove;
                    playerBehaviour.PlayerPropertiesSO.DiggingSpeed += _powerUPSSO.DiggingSpeedPowerUp;
                    _powerUPSSO.DiggingSpeedPowerUpPrice += 5;
                    SetFeeText(_powerUPSSO.DiggingSpeedPowerUpPrice);
                    playerBehaviour.SetNewDiggingSpeed(playerBehaviour.PlayerPropertiesSO.DiggingSpeed);

                }

                break;
            case PowerUp.IncomeMoney:
                if (_powerUPSSO.IncomeMoneyPowerUpPrice <= playerBehaviour.PlayerPropertiesSO.Money)
                {
                    GameStateEnums.currentGameState = GameStateEnums.GameState.PlayerCanMove;
                    playerBehaviour.PlayerPropertiesSO.IncomeIncreaseFactor += _powerUPSSO.IncomeMoneyPowerUp;
                    _powerUPSSO.IncomeMoneyPowerUpPrice += 5;
                    SetFeeText(_powerUPSSO.IncomeMoneyPowerUpPrice);
                    playerBehaviour.SetNewIncomeMoneyPowerUp(playerBehaviour.PlayerPropertiesSO.IncomeIncreaseFactor);


                }
                break;
            default:
                break;
        }
    }

    protected override void SetFeeText(int price)
    {
        areaFeeText.SetText(Mathf.CeilToInt(price).ToString());
    }
}
