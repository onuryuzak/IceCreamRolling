using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using LiquidVolumeFX;

public class SauceMachineController : Area
{
    public float timeRemaining = 1f;
    [SerializeField] SauceMachineSO _sauceMachinePropertiesSO;
    [SerializeField] GameObject _sauceMachineGlass;
    [SerializeField] Image _whiteTriggerVisual;
    [SerializeField] Image _greenTriggerVisual;
    //[SerializeField] LiquidVolume liquidVolume;
    [SerializeField] GameObject _mudLiquidParticle;
    private bool _startTiming;
    public Collider Trigger;

    public ScriptManager ScriptManager { get; private set; }

    private void Start()
    {
        _mudLiquidParticle.SetActive(false);
        ScriptManager = ScriptManager.Instance;

    }

    private void Update()
    {
        if (_startTiming && ScriptManager.PlayerBehaviour.IsConeOnHand)
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
                //ScriptManager.PlayerAnimationController.WalkAndMoveBlend(0);
                //GameStateEnums.currentGameState = GameStateEnums.GameState.InSauceMachineArea;
                SauceProcess();
            }

        }
        else
        {
            _greenTriggerVisual.fillAmount = 0;
        }
    }
    protected override void OnEnterArea(PlayerBehaviour playerBehaviour)
    {
        if (ScriptManager.PlayerBehaviour.IsConeOnHand)
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



    public void SauceProcess()
    {
        //if (0 < liquidVolume.level)
        //{
        //    DOVirtual.Float(liquidVolume.level, liquidVolume.level - 0.2f, 1f, t =>
        //    {
        //        liquidVolume.level = t;
        //        if (liquidVolume.level == 0)
        //        {
        //            DOVirtual.Float(liquidVolume.level, 1, 1f, t =>
        //            {
        //                liquidVolume.level = t;
        //            });
        //        }
        //    });

        //}
        StartCoroutine(SauceCountDown());



    }
    IEnumerator SauceCountDown()
    {
        _mudLiquidParticle.gameObject.SetActive(true);
        float _time = 3f;
        while (_time > 0)
        {
            _time -= Time.deltaTime;
            yield return null;
        }
        _mudLiquidParticle.gameObject.SetActive(false);
        GameStateEnums.currentGameState = GameStateEnums.GameState.PlayerCanMove;

    }
    public void IsSauceMachineWillOpen(bool state)
    {
        if (state == true)
        {
            _sauceMachineGlass.transform.DOLocalMoveY(_sauceMachineGlass.transform.localPosition.y + 45f, 5).SetSpeedBased()
                .OnComplete(() => _sauceMachineGlass.SetActive(false));
        }
        Trigger.enabled = state;
        _greenTriggerVisual.enabled = false;
        _whiteTriggerVisual.enabled = true;
    }

    public void StartGameProgress(bool state)
    {
        _greenTriggerVisual.enabled = false;
        _whiteTriggerVisual.enabled = state;
        Trigger.enabled = state;
        _sauceMachineGlass.SetActive(!state);
    }

    protected override void SetFeeText(int price)
    {

    }
}
