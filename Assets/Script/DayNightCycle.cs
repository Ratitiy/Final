using Unity.VisualScripting;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Sun (Directional Light)")]
    [SerializeField] private Light sunLight;

    [Header("Time Settings")]
    [Tooltip("1 = 1 วันใช้เวลา 24 นาทีจริง, 24 = 1 วันใช้เวลา 1 นาทีจริง")]
    [SerializeField] private float timeScale = 1f;
    [Tooltip("เวลาเริ่มต้น 0-24")]
    [SerializeField][Range(0f, 24f)] private float startTime = 8f;

    [Header("Rotation Axis")]
    [Tooltip("แกนที่ดวงอาทิตย์หมุนรอบ")]
    [SerializeField] private RotationAxis rotationAxis = RotationAxis.X;
    [Tooltip("offset การหมุนบนแกนอื่น เช่น Y = ทิศที่ดวงอาทิตย์ขึ้น")]
    [SerializeField] private float axisOffset = 170f;

    public enum RotationAxis { X, Y, Z }

    [Header("Sun Color")]
    [SerializeField] private Gradient sunColor;

    [Header("Sun Intensity")]
    [SerializeField] private AnimationCurve sunIntensity = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private float maxSunIntensity = 1.5f;

    [Header("Ambient Light")]
    [SerializeField] private Gradient ambientColor;

    public float CurrentTime { get; private set; }

    void Start()
    {
        CurrentTime = startTime;
        if (sunLight == null) sunLight = GetComponent<Light>();
    }

    void Update()
    {
        CurrentTime += Time.deltaTime * timeScale / 60f;
        if (CurrentTime >= 24f) CurrentTime -= 24f;

        UpdateSun();
        UpdateAmbient();
    }

    void UpdateSun()
    {
        if (sunLight == null) return;

        float sunAngle = (CurrentTime / 24f) * 360f - 90f;

        Quaternion rotation = rotationAxis switch
        {
            RotationAxis.X => Quaternion.Euler(sunAngle, axisOffset, 0f),
            RotationAxis.Y => Quaternion.Euler(axisOffset, sunAngle, 0f),
            RotationAxis.Z => Quaternion.Euler(axisOffset, 0f, sunAngle),
            _ => Quaternion.Euler(sunAngle, axisOffset, 0f)
        };

        sunLight.transform.rotation = rotation;

        float t = CurrentTime / 24f;
        sunLight.color = sunColor.Evaluate(t);
        sunLight.intensity = sunIntensity.Evaluate(t) * maxSunIntensity;
        sunLight.enabled = sunLight.intensity > 0.01f;
    }

    void UpdateAmbient()
    {
        float t = CurrentTime / 24f;
        RenderSettings.ambientLight = ambientColor.Evaluate(t);
    }

    public void SetTime(float hour) => CurrentTime = Mathf.Clamp(hour, 0f, 24f);
}