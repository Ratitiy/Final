using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class GPS : MonoBehaviour
{
    private LineRenderer line;
    private NavMeshPath path;
    public float yOffset = 0.15f; 

    void Start()
    {
        line = GetComponent<LineRenderer>();
        path = new NavMeshPath();

        // ตั้งค่าความสวยงามของเส้น
        line.startWidth = 0.5f;
        line.endWidth = 0.5f;
        line.textureMode = LineTextureMode.Tile;
    }

    void Update()
    {
        
        if (DeliveryManager.Instance != null && DeliveryManager.Instance.hasActiveDelivery && DeliveryManager.Instance.currentTarget != null)
        {
            line.enabled = true;

            
            NavMesh.CalculatePath(transform.position, DeliveryManager.Instance.currentTarget.position, NavMesh.AllAreas, path);

            DrawPath();
        }
        else
        {
            line.enabled = false;
        }
    }

    void DrawPath()
    {
        if (path.corners.Length < 2) return;

        line.positionCount = path.corners.Length;
        for (int i = 0; i < path.corners.Length; i++)
        {
            line.SetPosition(i, path.corners[i] + Vector3.up * yOffset);
        }
    }
}