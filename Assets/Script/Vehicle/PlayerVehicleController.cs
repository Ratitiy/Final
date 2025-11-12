
using Unity.Cinemachine;
using UnityEngine;

public class PlayerVehicleController : MonoBehaviour
{
    [Header("Player Components")]
    public CharacterController characterController;
    public MonoBehaviour playerMovementScript;  

    [Header("Cinemachine VCam")]
    public CinemachineVirtualCameraBase playerVCam;   
    public CinemachineVirtualCameraBase vehicleVCam;  

    [Header("Input")]
    public KeyCode exitKey = KeyCode.E;

    bool inVehicle;
    CarInteractable currentCar;
    Transform savedParent;

    void Start()
    {
        if (!characterController) characterController = GetComponent<CharacterController>();
        if (!playerVCam) playerVCam = GameObject.FindWithTag("PlayerVCam")?.GetComponent<CinemachineVirtualCameraBase>();
        if (!vehicleVCam) vehicleVCam = GameObject.FindWithTag("VehicleVCam")?.GetComponent<CinemachineVirtualCameraBase>();
    }

    void Update()
    {
        if (inVehicle && Input.GetKeyDown(exitKey))
            ExitVehicle();
    }

    public void EnterVehicle(CarInteractable car)
    {
        if (inVehicle) return;
        inVehicle = true;
        currentCar = car;
        characterController.enabled = false;

        if (playerMovementScript) playerMovementScript.enabled = false;
        if (characterController) characterController.enabled = false;

        savedParent = transform.parent;
        transform.SetParent(car.seat);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (vehicleVCam)
        {
            vehicleVCam.Follow = car.cameraTarget ? car.cameraTarget : car.transform;
            vehicleVCam.LookAt = car.cameraTarget ? car.cameraTarget : car.transform;
            vehicleVCam.Priority = 20;
        }
        if (playerVCam) playerVCam.Priority = 10;

        if (car.controller) car.controller.SetControlEnabled(true);
    }

    public void ExitVehicle()
    {
        if (!inVehicle || currentCar == null) return;
        inVehicle = false;
        characterController.enabled = true;
        if (currentCar.controller) currentCar.controller.SetControlEnabled(false);

        Transform exit = currentCar.exitPoint ? currentCar.exitPoint : currentCar.transform;
        transform.SetParent(savedParent);
        transform.position = exit.position;
        transform.rotation = exit.rotation;

        if (characterController) characterController.enabled = true;
        if (playerMovementScript) playerMovementScript.enabled = true;

        if (playerVCam) playerVCam.Priority = 20;
        if (vehicleVCam) vehicleVCam.Priority = 10;

        // Cursor.visible = true;
        // Cursor.lockState = CursorLockMode.None;
        currentCar.SetOccupied(false);
        currentCar = null;
    }
}
