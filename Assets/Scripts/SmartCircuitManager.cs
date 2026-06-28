using UnityEngine;
using TMPro;

public class SmartCircuitManager : MonoBehaviour
{
    [Header("UI Sliders")]
    public UnityEngine.UI.Slider tempSlider;
    public UnityEngine.UI.Slider humiSlider;
    public UnityEngine.UI.Slider soilSlider;
    public UnityEngine.UI.Slider rainSlider;

    [Header("Outputs & Actuators")]
    public TextMeshProUGUI lcdText;
    public ActuatorController actuatorController;

    [Header("Camera Control")]
    public UnityEngine.UI.Button camButton;
    public Camera mainCamera;
    public Vector3 boardViewPosition = new Vector3(-3.2f, -16.8f, -2.5f);
    public Vector3 boardViewRotation = new Vector3(25f, 25f, 0f);
    public Vector3 greenhouseViewPosition = new Vector3(-4.5f, -14f, -9f);
    public Vector3 greenhouseViewRotation = new Vector3(15f, 15f, 0f);
    public float cameraLerpSpeed = 5f;

    private bool isBoardView = true;
    private Vector3 targetCamPosition;
    private Quaternion targetCamRotation;

    private float blinkTimer = 0f;
    private bool blinkState = false;

    private void Start()
    {
        // Set up initial camera targets
        if (mainCamera != null)
        {
            targetCamPosition = boardViewPosition;
            targetCamRotation = Quaternion.Euler(boardViewRotation);
            mainCamera.transform.position = boardViewPosition;
            mainCamera.transform.rotation = targetCamRotation;
        }

        // Add click listener to Camera button
        if (camButton != null)
        {
            camButton.onClick.AddListener(ToggleCameraView);
        }

        // Initialize display
        UpdateSimulation(0f);
    }

    private void Update()
    {
        // Smoothly interpolate camera position and rotation
        if (mainCamera != null)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCamPosition, Time.deltaTime * cameraLerpSpeed);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetCamRotation, Time.deltaTime * cameraLerpSpeed);
        }

        // Update blink state for alerts (blinks every 0.5 seconds)
        blinkTimer += Time.deltaTime;
        if (blinkTimer >= 0.5f)
        {
            blinkTimer = 0f;
            blinkState = !blinkState;
        }

        // Gather slider values and run logic
        float temp = tempSlider != null ? tempSlider.value : 24f;
        float humi = humiSlider != null ? humiSlider.value : 60f;
        float soil = soilSlider != null ? soilSlider.value : 45f;
        float rain = rainSlider != null ? rainSlider.value : 0f;

        // --- threshold logic ---
        bool fanOn = temp > 30f;
        bool soilDry = soil < 30f;
        bool lightOn = rain > 15f || soilDry; // bulb lights up if dry or raining (dark/cloudy simulation)

        if (actuatorController != null)
        {
            actuatorController.SetFansSpinning(fanOn);
            actuatorController.SetLightActive(lightOn);
        }

        // --- lcd formatting ---
        if (lcdText != null)
        {
            string line1 = $"T:{temp:F1}C H:{humi:F0}%";
            string line2 = "";

            if (soilDry)
            {
                if (blinkState)
                {
                    line2 = "[!] SOIL DRY!";
                }
                else
                {
                    line2 = $"Soil:{soil:F0}% Rain:{rain:F0}%";
                }
            }
            else
            {
                string rainStatus = rain > 10f ? "RAIN" : "DRY";
                line2 = $"Soil:{soil:F0}% R:{rainStatus}";
            }

            lcdText.text = $"{line1}\n{line2}";
        }
    }

    public void ToggleCameraView()
    {
        isBoardView = !isBoardView;
        if (isBoardView)
        {
            targetCamPosition = boardViewPosition;
            targetCamRotation = Quaternion.Euler(boardViewRotation);
        }
        else
        {
            targetCamPosition = greenhouseViewPosition;
            targetCamRotation = Quaternion.Euler(greenhouseViewRotation);
        }
    }

    private void UpdateSimulation(float dummy)
    {
        // Force evaluation on start
    }
}