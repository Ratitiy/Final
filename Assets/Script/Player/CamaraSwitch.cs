
using Unity.Cinemachine;
using UnityEngine;

public class CamaraSwitch : MonoBehaviour
{
    public CinemachineCamera freeLook;

    public void SetTarget(Transform target)
    {
        if (freeLook == null) return;

        freeLook.Follow = target;
        freeLook.LookAt = target;
    }
}
