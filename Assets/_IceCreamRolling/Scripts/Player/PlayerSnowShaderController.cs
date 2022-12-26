using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerSnowShaderController : MonoBehaviour
{
    [Range(0, 1000)]
    public float BrushSize;
    [Range(0, 1000)]
    public float BrushStrength;

    public CustomRenderTexture InitialSplatMap;
    public Material DrawMaterial;
    public Material SnowMaterial;

    [SerializeField] private IceCreamAreaMaterialsSO _freezeAreaMaterialsSO;
    [SerializeField] GameObject _snowDigTarget;

    private RaycastHit _groundHit;
    private int _layerMask;
    [HideInInspector] public CustomRenderTexture _currentSplatMap;
    public ScriptManager ScriptManager { get; private set; }

    //Const String
    private const string _playerCoord = "_PlayerCoord";
    private const string _drawColor = "_DrawColor";
    private const string _splatMap = "_SplatMap";
    private const string _brushStrength = "_BrushStrength";
    private const string _brushSize = "_BrushSize";
    private const string _ground = "Ground";




    private void Awake()
    {
        foreach (var item in _freezeAreaMaterialsSO.areaMaterials)
        {
            InitialSplatMap = new CustomRenderTexture(1024, 1024, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);

            InitialSplatMap.updateMode = CustomRenderTextureUpdateMode.OnDemand;
            InitialSplatMap.material = DrawMaterial;
            InitialSplatMap.initializationSource = CustomRenderTextureInitializationSource.Material;
            InitialSplatMap.initializationMaterial = DrawMaterial;
            InitialSplatMap.doubleBuffered = true;

            DrawMaterial.SetVector(_drawColor, Color.red);
            DrawMaterial.SetVector(_playerCoord, Vector4.zero);
            DrawMaterial.SetTexture(_splatMap, InitialSplatMap);
            item.SetTexture(_splatMap, InitialSplatMap);
        }
        //InitialSplatMap = new CustomRenderTexture(2048, 2048, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);

        //InitialSplatMap.updateMode = CustomRenderTextureUpdateMode.OnDemand;
        //InitialSplatMap.material = DrawMaterial;
        //InitialSplatMap.initializationSource = CustomRenderTextureInitializationSource.Material;
        //InitialSplatMap.initializationMaterial = DrawMaterial;
        //InitialSplatMap.doubleBuffered = true;

        //DrawMaterial.SetVector(_drawColor, Color.red);
        //DrawMaterial.SetVector(_playerCoord, Vector4.zero);
        //DrawMaterial.SetTexture(_splatMap, InitialSplatMap);
        //SnowMaterial.SetTexture(_splatMap, InitialSplatMap);
    }

    void Start()
    {
        _layerMask = LayerMask.GetMask(_ground);
        ScriptManager = ScriptManager.Instance;

    }
    private void Update()
    {
        if (GameStateEnums.currentGameState != GameStateEnums.GameState.InIceCreamArea) return;
        if (!ScriptManager.PlayerBehaviour.ReachMaxSize)
        {
            DrawMaterial.SetFloat(_brushStrength, BrushStrength);
            DrawMaterial.SetFloat(_brushSize, BrushSize);
            if (_currentSplatMap == null) return;

            _currentSplatMap.Initialize();
            if (Physics.Raycast(_snowDigTarget.transform.position, Vector3.down, out _groundHit, Mathf.Infinity, _layerMask))
            {

                DrawMaterial.SetVector(_playerCoord, new Vector4(_groundHit.textureCoord.x, _groundHit.textureCoord.y, 0, 0));
                if (!(ScriptManager.UIScript.Joystick.Horizontal == 0) || !(ScriptManager.UIScript.Joystick.Vertical == 0))
                {
                    ScriptManager.PlayerBehaviour.IsStartGrownUp = true;
                    ScriptManager.PlayerAnimationController.Scooping(true);
                }
                else
                {
                    ScriptManager.PlayerAnimationController.Scooping(false);
                    ScriptManager.PlayerBehaviour.IsStartGrownUp = false;

                }
            }
            else
            {
                ScriptManager.PlayerBehaviour.IsStartGrownUp = false;
                ScriptManager.PlayerAnimationController.Scooping(false);
            }
            _currentSplatMap.Update();
        }
        else
        {
            ScriptManager.PlayerBehaviour.IsStartGrownUp = false;
            ScriptManager.PlayerAnimationController.Scooping(false);
        }
    }

    public void SetNewSplatMap(Material icecreamMaterial)
    {
        _currentSplatMap = (CustomRenderTexture)icecreamMaterial.GetTexture("_SplatMap");
        _currentSplatMap.material = DrawMaterial;
        _currentSplatMap.initializationMaterial = DrawMaterial;
        DrawMaterial.SetTexture(_splatMap, _currentSplatMap);

    }
    public void SetNewSplatMapWhenCustomer(CustomRenderTexture customRenderTexture)
    {
        _currentSplatMap = customRenderTexture;
        _currentSplatMap.material = DrawMaterial;
        _currentSplatMap.initializationMaterial = DrawMaterial;
        DrawMaterial.SetTexture(_splatMap, _currentSplatMap);
    }
    
}
