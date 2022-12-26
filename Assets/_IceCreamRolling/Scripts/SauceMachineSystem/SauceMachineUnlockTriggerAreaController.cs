using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SauceMachineUnlockTriggerAreaController : Area
{
    public float timeRemaining = 1f;
    [SerializeField] SauceMachineSO _SauceMachineAreaPropertiesSO;
    [SerializeField] Image _whiteTriggerVisual;
    [SerializeField] Image _greenTriggerVisual;
    [SerializeField] Image _moneyImage;
    private bool _startTiming;
    private PlayerBehaviour _playerBehaviour;
    [SerializeField] TextMeshProUGUI areaFeeText;

    private BoxCollider _boxCollider;
    public ScriptManager ScriptManager { get; private set; }

    private void Awake()
    {
        _boxCollider = GetComponentInChildren<BoxCollider>();
    }
    private void Start()
    {
        ScriptManager = ScriptManager.Instance;
        SetFeeText(_SauceMachineAreaPropertiesSO.ChocolateSauceUnlockFee); //change with current


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
                ScriptManager.PlayerAnimationController.WalkAndMoveBlend(0);
                GameStateEnums.currentGameState = GameStateEnums.GameState.InUnlockArea;
                StartCoroutine(COUnlockMachineProgress(_playerBehaviour));
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
        _startTiming = true;
        _whiteTriggerVisual.enabled = false;
        _greenTriggerVisual.enabled = true;
    }

    protected override void OnExitArea(PlayerBehaviour playerBehaviour)
    {
        _startTiming = false;
        timeRemaining = 1f;
        _greenTriggerVisual.enabled = false;
        _whiteTriggerVisual.enabled = true;
        GameStateEnums.currentGameState = GameStateEnums.GameState.PlayerCanMove;

    }

    IEnumerator COUnlockMachineProgress(PlayerBehaviour playerBehaviour)
    {
        while (GameStateEnums.currentGameState == GameStateEnums.GameState.InUnlockArea
            && 0 < playerBehaviour.PlayerPropertiesSO.Money
            && !_SauceMachineAreaPropertiesSO.isUnlocked)
        {
            areaFeeText.DOComplete();
            int currentPoint = int.Parse(areaFeeText.text);
            int nextPoint = _SauceMachineAreaPropertiesSO.ChocolateSauceUnlockFee - playerBehaviour.PlayerPropertiesSO.moneyDecreaseFactor;




            DOTween.To(() => currentPoint, x => currentPoint = x, nextPoint, 0f).SetEase(Ease.OutCubic).SetTarget(areaFeeText)
                .OnUpdate(() =>
                {
                    if (0 <= currentPoint)
                    {
                        ScriptManager.UIScript.DecreasePlayerMoneyUI();
                        playerBehaviour.MoneyRelease(Vector3.zero, transform);
                        SetFeeText(currentPoint);
                        _SauceMachineAreaPropertiesSO.ChocolateSauceUnlockFee = currentPoint; // change with current
                        if (currentPoint == 0) UnlockSauceMachineArea();
                    }



                });
            yield return new WaitForEndOfFrame();

        }
    }

    private void UnlockSauceMachineArea()
    {
        _SauceMachineAreaPropertiesSO.isUnlocked = true;
        _boxCollider.enabled = false;
        GetComponentInParent<SauceMachineManager>().OpenNewSauceMachineArea();

    }

    public void IsUnlockAreaWillOpen(bool state)
    {
        _boxCollider.enabled = state;
        areaFeeText.gameObject.SetActive(state);
        _moneyImage.gameObject.SetActive(state);
        _greenTriggerVisual.gameObject.SetActive(state);
        _whiteTriggerVisual.gameObject.SetActive(state);

    }

    protected override void SetFeeText(int price)
    {
        areaFeeText.SetText(Mathf.CeilToInt(price).ToString());
    }
}
