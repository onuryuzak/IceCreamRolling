using UnityEngine;

public abstract class Area : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger && other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out PlayerBehaviour playerBehaviour))
        {
            OnEnterArea(playerBehaviour);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger && other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out PlayerBehaviour playerBehaviour))
        {
            OnExitArea(playerBehaviour);
        }
    }



    protected abstract void OnEnterArea(PlayerBehaviour playerBehaviour);
    protected abstract void OnExitArea(PlayerBehaviour playerBehaviour);

    protected abstract void SetFeeText(int price);
}

