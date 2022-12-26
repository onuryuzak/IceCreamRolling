using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _anim;
    private const string _speed = "Speed";
    private const string _scooping = "Scooping";
    private const string _carryCone = "CarryCone";
    private const string _carryCup = "CarryCup";


    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    public void WalkAndMoveBlend(float blendValue)
    {

        _anim.SetFloat(_speed, blendValue);
    }
    public void Scooping(bool state)
    {
        _anim.SetBool(_scooping, state);
    }
    public void CarryConeBlend(float blendValue)
    {
        _anim.SetFloat(_speed, blendValue);
    }
    public void CarryCone(bool state)
    {
        _anim.SetBool(_carryCone, state);
    }
}
