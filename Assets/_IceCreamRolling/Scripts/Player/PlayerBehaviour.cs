using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _fakeBallPrefab;
    [SerializeField] private Transform _ballSpawn;
    [SerializeField] private Vector3 _ballMaxSize = Vector3.one;
    [SerializeField] private Transform _carryPos;

    [SerializeField] private float _ballScaleMultipier;
    [SerializeField] private float _ballScaleSpeed;
    public PlayerPropertiesSO PlayerPropertiesSO;
    [HideInInspector] public GameObject _currentIceCreamBall;
    [HideInInspector] public Texture _currentIceCreamBallTexture;
    public GameObject Scope;
    public bool IceCreamOnHand;
    public bool IsStartGrownUp;
    public bool ReachMaxSize;
    public bool IsConeOnHand;
    public GameObject CurrentCone;

    private bool _initProcessIsOver;
    private bool _firstWork;
    private GameObject _growingIceCreamBall;

    [HideInInspector] public Texture _currentAreaTexture;


    public ScriptManager ScriptManager { get; private set; }

    private void Start()
    {
        ScriptManager = ScriptManager.Instance;
        _ballScaleSpeed = ScriptManager.PlayerBehaviour.PlayerPropertiesSO.DiggingSpeed;
    }

    private void Update()
    {
        if (ScriptManager.UIScript.Joystick.Horizontal == 0 || ScriptManager.UIScript.Joystick.Vertical == 0) return;

        if (GameStateEnums.currentGameState != GameStateEnums.GameState.InIceCreamArea) return;

        IceCreamBallProcess();


    }

    private void IceCreamBallProcess()
    {
        if (IsStartGrownUp && !IsConeOnHand)
        {
            if (_initProcessIsOver && !ReachMaxSize && _currentAreaTexture == _currentIceCreamBallTexture) //check if iceCream still on hand
            {
                if (_growingIceCreamBall != null)
                    BallGrowingUp(_growingIceCreamBall);
            }
            else //if there is no ice cream
            {

                if (!_firstWork)
                {

                    _firstWork = true;
                    _currentIceCreamBall = InstantiateFakeBall();
                    IceCreamOnHand = true;
                    _currentIceCreamBall.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", _currentAreaTexture);
                    _currentIceCreamBallTexture = _currentIceCreamBall.GetComponent<MeshRenderer>().material.GetTexture("_MainTex");
                    _currentIceCreamBall.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    _currentIceCreamBall.transform.localPosition = Vector3.zero;


                    DOVirtual.Float(0, 255, 1f, t =>
                    {
                        Quaternion temp = _currentIceCreamBall.transform.rotation;
                        temp.x = t;
                        _currentIceCreamBall.transform.rotation = temp;
                    }).OnComplete(() =>
                    {
                        ReachMaxSize = false;
                        _growingIceCreamBall = _currentIceCreamBall.transform.GetChild(0).gameObject;
                        _growingIceCreamBall.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", _currentIceCreamBallTexture);

                        _growingIceCreamBall.GetComponent<PaintIn3D.P3dPaintableTexture>().Texture = _currentIceCreamBallTexture;

                        _growingIceCreamBall.GetComponent<MeshRenderer>().enabled = true;
                        _initProcessIsOver = true;
                    });

                    Vector3 tempPos = _currentIceCreamBall.transform.localScale;
                    _currentIceCreamBall.transform.localScale = tempPos * 0.2f;
                    _currentIceCreamBall.transform.DOScale(tempPos, 1);






                }
            }

            if (_growingIceCreamBall != null)
            {
                if (_growingIceCreamBall.transform.localScale.magnitude >= _ballMaxSize.magnitude) //checking currentball reach max size.
                {
                    _currentIceCreamBall.transform.DOPunchScale(_currentIceCreamBall.transform.localScale + Vector3.zero, 0.5f);
                    ReachMaxSize = true;
                    _initProcessIsOver = false;
                    IsStartGrownUp = false;
                }
            }

        }
    }
    public void CurrentIceCreamGoCone(GameObject currentCone)
    {
        IceCreamOnHand = false;
        _firstWork = false;
        Scope.SetActive(false); //külahı bırakınca tekrar active true
        _currentIceCreamBall.transform.SetParent(currentCone.transform);
        _currentIceCreamBall.transform.DOLocalJump(currentCone.transform.GetChild(0).transform.localPosition, 0.007f, 1, 1)
            .OnComplete(() => CurrentConeGoHand(currentCone));

    }
    private void CurrentConeGoHand(GameObject currentCone)
    {

        currentCone.transform.SetParent(_carryPos.transform);

        currentCone.transform.DOLocalRotate(Vector3.zero, 0.7f);
        currentCone.transform.DOJump(_carryPos.transform.position, 1f, 1, 0.7f).OnComplete(() =>
        {
            CurrentCone = currentCone;
            IsConeOnHand = true;
            ScriptManager.PlayerAnimationController.CarryConeBlend(0);
            GameStateEnums.currentGameState = GameStateEnums.GameState.PlayerCanMove;
        });

    }
    private void BallGrowingUp(GameObject ball)
    {
        ball.transform.localScale = Vector3.Lerp(ball.transform.localScale,
                    ball.transform.localScale + (_ballScaleMultipier * Vector3.one), Time.deltaTime * _ballScaleSpeed);

    }
    private GameObject InstantiateFakeBall()
    {
        return Instantiate(_fakeBallPrefab, _ballSpawn);
    }
    public void MoneyRelease(Vector3 targetPoint, Transform parent = null)
    {
        GameObject money = ObjectPooler.Instance.GetPooledObject("Money");
        money.transform.position = transform.position;
        money.GetComponent<Collider>().enabled = false;
        money.SetActive(true);

        MoneyItem moneyItem = money.GetComponent<MoneyItem>();
        money.transform.SetParent(parent);
        money.transform.DOLocalJump(targetPoint, 0.7f, 1, 1.5f).OnComplete(() =>
        {
            ObjectPooler.Instance.ReleasePooledObject(money);
        });

    }

    public void SetNewIncomeMoneyPowerUp(int money)
    {
        PlayerPropertiesSO.IncomeIncreaseFactor = money;
    }

    public void SetCurrentIceCreamAreaTexture(Material currentAreaMat)
    {
        _currentAreaTexture = currentAreaMat.GetTexture("_SnowTexture");
    }

    public void SetNewDiggingSpeed(float diggingSpeed)
    {
        _ballScaleSpeed = diggingSpeed;
    }
}
