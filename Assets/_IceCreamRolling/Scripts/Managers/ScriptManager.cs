using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : Singleton<ScriptManager>
{
    public PlayerBehaviour PlayerBehaviour { get; private set; }
    public PlayerMovementController PlayerMovementController { get; private set; }
    public PlayerSnowShaderController PlayerSnowShaderController { get; private set; }
    public PlayerAnimationController PlayerAnimationController { get; private set; }
    public UIScript UIScript { get; private set; }

    private void Awake()
    {
        PlayerBehaviour = FindObjectOfType<PlayerBehaviour>();
        PlayerSnowShaderController = FindObjectOfType<PlayerSnowShaderController>();
        PlayerAnimationController = FindObjectOfType<PlayerAnimationController>();
        UIScript = FindObjectOfType<UIScript>();
        PlayerMovementController = FindObjectOfType<PlayerMovementController>();
    }
}
