using UnityEngine;

public class TestPhi : MonoBehaviour
{
    public float tiltAmount = 15f;    // เอียงสูงสุดกี่องศา
    public float smoothSpeed = 5f;   // ความเร็วในการคืนตัว

    private Rigidbody bikeRb;
    private Quaternion initialRotation;

    void Start()
    {
        // หา Rigidbody ของรถ (อยู่ชั้นบนของ Parent)
        bikeRb = GetComponentInParent<Rigidbody>();
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (bikeRb == null) return;

        // คำนวณแรงเหวี่ยงหนีศูนย์กลางจากการเลี้ยว (Angular Velocity)
        // หรือใช้ค่า Steer Input จากรถมาคำนวณก็ได้
        float turnSpeed = bikeRb.angularVelocity.y;

        // สร้างค่าการเอียงในแนวแกน Z (เอียงซ้าย-ขวา)
        float tiltZ = -turnSpeed * tiltAmount;

        // จำกัดองศาไม่ให้เอียงจนเกินไป
        tiltZ = Mathf.Clamp(tiltZ, -tiltAmount, tiltAmount);

        // คำนวณ Rotation เป้าหมาย
        Quaternion targetRot = initialRotation * Quaternion.Euler(0, 0, tiltZ);

        // ค่อยๆ หมุนไปหาเป้าหมาย (ให้ดูมีความนุ่มนวลเหมือนของเหลว)
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * smoothSpeed);
    }
}