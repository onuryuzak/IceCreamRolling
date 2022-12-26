using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;


public class UnlockTriggerIceCreamAreaController : Area
{
    public float timeRemaining = 1f;
    [SerializeField] IceCreamAreaPropertiesSO _iceCreamAreaPropertiesSO;
    [SerializeField] Image _whiteTriggerVisual;
    [SerializeField] Image _greenTriggerVisual;
    [SerializeField] Image _moneyImage;
    [SerializeField] TextMeshProUGUI areaFeeText;
    private bool _startTiming;
    private PlayerBehaviour _playerBehaviour;

    private BoxCollider _boxCollider;
    public ScriptManager ScriptManager { get; private set; }

    [Button("Reset area properties")]

    public void Reset()
    {
        _iceCreamAreaPropertiesSO.IceCreamUnlockFee = 100;
        _iceCreamAreaPropertiesSO.isUnlocked = false;
    }
    private void Awake()
    {
        _boxCollider = GetComponentInChildren<BoxCollider>();
    }
    private void Start()
    {
        ScriptManager = ScriptManager.Instance;
        SetFeeText(_iceCreamAreaPropertiesSO.IceCreamUnlockFee);
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
                GameStateEnums.currentGameState = GameStateEnums.GameState.InUnlockArea;
                StartCoroutine(UnlockIceCreamAreaProcess(_playerBehaviour));
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

    IEnumerator UnlockIceCreamAreaProcess(PlayerBehaviour playerBehaviour)
    {
        while (GameStateEnums.currentGameState == GameStateEnums.GameState.InUnlockArea
            && 0 < playerBehaviour.PlayerPropertiesSO.Money
            && !_iceCreamAreaPropertiesSO.isUnlocked)
        {
            areaFeeText.DOComplete();
            int currentPoint = int.Parse(areaFeeText.text);
            int nextPoint = _iceCreamAreaPropertiesSO.IceCreamUnlockFee - playerBehaviour.PlayerPropertiesSO.moneyDecreaseFactor;




            DOTween.To(() => currentPoint, x => currentPoint = x, nextPoint, 0f).SetEase(Ease.OutCubic).SetTarget(areaFeeText)
                .OnUpdate(() =>
                {
                    if (0 <= currentPoint)
                    {
                        ScriptManager.UIScript.DecreasePlayerMoneyUI();
                        playerBehaviour.MoneyRelease(Vector3.zero, transform);
                        SetFeeText(currentPoint);
                        _iceCreamAreaPropertiesSO.IceCreamUnlockFee = currentPoint;
                        if (currentPoint == 0) UnlockedIceCreamArea();
                    }



                });
            yield return new WaitForEndOfFrame();

        }
    }

    private void UnlockedIceCreamArea()
    {
        _iceCreamAreaPropertiesSO.isUnlocked = true;
        _boxCollider.enabled = false;
        GetComponentInParent<IceCreamAreaManager>().OpenNewIceCreamArea();

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
