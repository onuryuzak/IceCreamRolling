using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IceCreamAreaController : Area
{
    [SerializeField] IceCreamAreaPropertiesSO _iceCreamAreaPropertiesSO;
    [SerializeField] GameObject _iceCreamGlassModelHolder;

    public ScriptManager ScriptManager { get; private set; }
    private CustomRenderTexture _areaCurrentSplatMap;

    private void Start()
    {
        ScriptManager = ScriptManager.Instance;
    }
    protected override void OnEnterArea(PlayerBehaviour playerBehaviour)
    {
        GameStateEnums.currentGameState = GameStateEnums.GameState.InIceCreamArea;
        ScriptManager.PlayerSnowShaderController.SetNewSplatMap(_iceCreamAreaPropertiesSO.AreaIceCreamMaterial);

        playerBehaviour.SetCurrentIceCreamAreaTexture(_iceCreamAreaPropertiesSO.AreaIceCreamMaterial);
    }

    protected override void OnExitArea(PlayerBehaviour playerBehaviour)
    {
        GameStateEnums.currentGameState = GameStateEnums.GameState.PlayerCanMove;
        SetAreaCurrentSplatMap();

    }

    private void SetAreaCurrentSplatMap()
    {
        _areaCurrentSplatMap = ScriptManager.PlayerSnowShaderController._currentSplatMap;
        _areaCurrentSplatMap.material = _iceCreamAreaPropertiesSO.AreaIceCreamMaterial;
        _areaCurrentSplatMap.initializationMaterial = _iceCreamAreaPropertiesSO.AreaIceCreamMaterial;
        _iceCreamAreaPropertiesSO.AreaIceCreamMaterial.SetTexture("_SplatMap", _areaCurrentSplatMap);
    }

    public void IsIceCreamAreaWillOpen(bool state)
    {

        if (state == true)
        {
            _iceCreamGlassModelHolder.transform.DOLocalMoveY(_iceCreamGlassModelHolder.transform.localPosition.y + 45f, 5).SetSpeedBased()
                .OnComplete(() => _iceCreamGlassModelHolder.SetActive(false));
        }
    }
    public void StartGameProcess(bool state)
    {
        _iceCreamGlassModelHolder.SetActive(state);
    }

    protected override void SetFeeText(int price)
    {

    }
}
