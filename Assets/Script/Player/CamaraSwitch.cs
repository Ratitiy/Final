using Unity.Cinemachine; 
using UnityEngine;

public class CamaraSwitch : MonoBehaviour
{
   
    public CinemachineCamera tpsCamera; 

    public CinemachineCamera fpsCamera; 

    public int activePriority = 20;
    public int inactivePriority = 10;

    public void SetTarget(Transform target)
    {
        if (tpsCamera != null)
        {
            tpsCamera.Follow = target;
            tpsCamera.LookAt = target;
        }
    }

    public void SwitchToFPS(Transform fpsTarget)
    {
        if (fpsCamera != null && tpsCamera != null)
        {
            fpsCamera.Follow = fpsTarget;

            fpsCamera.Priority = activePriority;
            tpsCamera.Priority = inactivePriority;
        }
    }

    public void SwitchToTPS(Transform tpsTarget)
    {
        if (fpsCamera != null && tpsCamera != null)
        {
            tpsCamera.Follow = tpsTarget;
            tpsCamera.LookAt = tpsTarget;

            tpsCamera.Priority = activePriority;
            fpsCamera.Priority = inactivePriority;
        }
    }
}