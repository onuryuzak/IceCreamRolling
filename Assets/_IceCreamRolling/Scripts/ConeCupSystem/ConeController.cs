using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ConeController : Area
{
    public ScriptManager ScriptManager { get; private set; }
    public float timeRemaining = 1f;
    [SerializeField] List<GameObject> coneObjects;
    [SerializeField] GameObject _conePrefab;
    [SerializeField] Image _whiteTriggerVisual;
    [SerializeField] Image _greenTriggerVisual;
    private bool _startTiming;
    private PlayerBehaviour _playerBehaviour;

    private void Start()
    {
        ScriptManager = ScriptManager.Instance;
        _whiteTriggerVisual.enabled = true;
        _greenTriggerVisual.enabled = false;

    }
    private void Update()
    {
        if (_startTiming)
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
                GameStateEnums.currentGameState = GameStateEnums.GameState.InConeOrCupArea;
                ConeGoPoint(_playerBehaviour);
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
        if (playerBehaviour.ReachMaxSize && playerBehaviour.IceCreamOnHand)
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

    private void ConeGoPoint(PlayerBehaviour playerBehaviour)
    {
        GameObject _currentCone = coneObjects[Random.Range(0, coneObjects.Count - 1)];
        Vector3 tempConeInitPos = _currentCone.transform.localPosition;
        _currentCone.transform.DOLocalMoveY(tempConeInitPos.y + 0.2f, 1f).OnComplete(() =>
        {
            coneObjects.Remove(_currentCone);
            playerBehaviour.CurrentIceCreamGoCone(_currentCone);
            StartCoroutine(WaitForNewConeSpawn(tempConeInitPos));

        });
    }

    IEnumerator WaitForNewConeSpawn(Vector3 spawnPos)
    {
        yield return new WaitForSeconds(3f);
        GameObject newCone = Instantiate(_conePrefab, spawnPos + new Vector3(0, 5f, 0), _conePrefab.transform.localRotation, transform);
        newCone.transform.DOLocalMove(spawnPos, 2f).SetEase(Ease.OutBounce);
        coneObjects.Add(newCone);
    }

    protected override void SetFeeText(int price)
    {

    }
}
