using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    public float MoveSpeed = 10f;
    public ScriptManager ScriptManager { get; private set; }

    private void Start()
    {
        ScriptManager = ScriptManager.Instance;
        MoveSpeed = ScriptManager.PlayerBehaviour.PlayerPropertiesSO.MovementSpeed;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (GameStateEnums.currentGameState == GameStateEnums.GameState.InConeOrCupArea
                || GameStateEnums.currentGameState == GameStateEnums.GameState.InSauceMachineArea) return;
            if (ScriptManager.UIScript.Joystick.Horizontal == 0 || ScriptManager.UIScript.Joystick.Vertical == 0) return;

            Vector3 pos = -(new Vector3(ScriptManager.UIScript.Joystick.Horizontal * MoveSpeed * Time.deltaTime, 0,
                ScriptManager.UIScript.Joystick.Vertical * MoveSpeed * Time.deltaTime));

            transform.position += pos;
            if (!ScriptManager.PlayerBehaviour.IsConeOnHand)
            {
                ScriptManager.PlayerAnimationController.CarryCone(false);
                ScriptManager.PlayerAnimationController.WalkAndMoveBlend(Vector3.Normalize(transform.position).magnitude);
            }
            else
            {
                ScriptManager.PlayerAnimationController.CarryCone(true);
                ScriptManager.PlayerAnimationController.CarryConeBlend(Vector3.Normalize(transform.position).magnitude);
            }


            Vector3 lookDir = (transform.position + new Vector3(ScriptManager.UIScript.Joystick.Horizontal, 0,
                ScriptManager.UIScript.Joystick.Vertical)) - transform.position;

            if (lookDir.magnitude > 0.05f)
            {
                transform.localRotation = (Quaternion.RotateTowards(transform.localRotation, Quaternion.LookRotation(-lookDir), _rotateSpeed));

            }
        }
        else
        {
            if (!ScriptManager.PlayerBehaviour.IsConeOnHand)
            {
                ScriptManager.PlayerAnimationController.CarryCone(false);
                ScriptManager.PlayerAnimationController.WalkAndMoveBlend(0);
            }
            else
            {
                ScriptManager.PlayerAnimationController.CarryCone(true);
                ScriptManager.PlayerAnimationController.CarryConeBlend(0);
            }

        }
    }

    public void SetNewSpeed(float speed)
    {
        MoveSpeed = speed;
    }
}
