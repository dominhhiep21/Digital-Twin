using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PhysicalCircuitManager : MonoBehaviour
{
    [Header("Peripherals")]
    public Transform soilSensor;
    public Transform rainSensor;
    public Transform relayModule;
    public Transform waterPump;
    public TextMeshProUGUI lcdText;

    [Header("Simulation State")]
    public bool isSoilDry = false;
    public bool isRaining = false;

    [Header("Actuator Feedback")]
    public Color activeRelayColor = Color.green;
    public Color idleRelayColor = Color.red;
    private Material relayLedMaterial;
    private Light relayLedLight;

    [Header("Rain Sensor Feedback")]
    public Color activeRainColor = Color.green;
    public Color idleRainColor = Color.red;
    private Material rainLedMaterial;
    private Light rainLedLight;

    // Inside values
    private float simulatedTemp = 26.4f;
    private float simulatedHumi = 62.0f;
    private float simulatedSoilMoisture = 48.0f;
    private float blinkTimer = 0f;
    private bool blinkState = false;
    private Vector3 initialPumpPos;

    private void Start()
    {
        // 1. Programmatically add Colliders to make sensors clickable
        AddColliderIfMissing(soilSensor);
        AddColliderIfMissing(rainSensor);

        // Also add colliders specifically to children for high-fidelity clicking
        if (soilSensor != null)
        {
            foreach (Transform child in soilSensor) AddColliderIfMissing(child);
        }
        if (rainSensor != null)
        {
            foreach (Transform child in rainSensor) AddColliderIfMissing(child);
            // Search under group
            Transform group = rainSensor.Find("Collada visual scene group");
            if (group != null)
            {
                foreach (Transform child in group) AddColliderIfMissing(child);
            }
        }

        if (waterPump != null)
        {
            initialPumpPos = waterPump.localPosition;
        }

        // 2. Set up a physical light on the Relay board to represent its status LED
        if (relayModule != null)
        {
            Transform lightTrans = relayModule.Find("RelayLEDLight");
            if (lightTrans == null)
            {
                GameObject lightObj = new GameObject("RelayLEDLight");
                lightObj.transform.SetParent(relayModule, false);
                lightObj.transform.localPosition = new Vector3(-0.02f, 0.04f, 0.05f);
                relayLedLight = lightObj.AddComponent<Light>();
                relayLedLight.type = LightType.Point;
                relayLedLight.range = 0.5f;
                relayLedLight.intensity = 1.0f;
            }
            else
            {
                relayLedLight = lightTrans.GetComponent<Light>();
            }

            Transform vidro = relayModule.Find("vidro_019-material");
            if (vidro != null)
            {
                var mr = vidro.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    relayLedMaterial = mr.material;
                    relayLedMaterial.EnableKeyword("_EMISSION");
                }
            }
        }

        // 3. Set up a physical light on the Rain Sensor Module board for status LED
        if (rainSensor != null)
        {
            Transform group = rainSensor.Find("Collada visual scene group");
            if (group != null)
            {
                Transform rainLedObj = group.Find("Led_placa.002");
                if (rainLedObj != null)
                {
                    Transform lightTrans = rainLedObj.Find("RainLEDLight");
                    if (lightTrans == null)
                    {
                        GameObject lightObj = new GameObject("RainLEDLight");
                        lightObj.transform.SetParent(rainLedObj, false);
                        lightObj.transform.localPosition = new Vector3(0f, 0.02f, 0f);
                        rainLedLight = lightObj.AddComponent<Light>();
                        rainLedLight.type = LightType.Point;
                        rainLedLight.range = 0.5f;
                        rainLedLight.intensity = 1.0f;
                    }
                    else
                    {
                        rainLedLight = lightTrans.GetComponent<Light>();
                    }

                    Transform vidro = rainLedObj.Find("vidro-material");
                    if (vidro != null)
                    {
                        var mr = vidro.GetComponent<MeshRenderer>();
                        if (mr != null)
                        {
                            rainLedMaterial = mr.material;
                            rainLedMaterial.EnableKeyword("_EMISSION");
                        }
                    }
                }
            }
        }

        UpdateActuators();
    }

    private void Update()
    {
        // --- New Input System Raycast Click Selection ---
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform == soilSensor || hit.transform.IsChildOf(soilSensor))
                    {
                        isSoilDry = !isSoilDry;
                        simulatedSoilMoisture = isSoilDry ? 12.0f : 48.0f;
                        UpdateActuators();
                        Debug.Log("Physical Circuit: Clicked Soil Sensor! Dry State: " + isSoilDry);
                    }
                    else if (hit.transform == rainSensor || hit.transform.IsChildOf(rainSensor))
                    {
                        isRaining = !isRaining;
                        UpdateActuators();
                        Debug.Log("Physical Circuit: Clicked Rain Sensor! Rain State: " + isRaining);
                    }
                }
            }
        }

        // --- Simulated Fluctuations ---
        simulatedTemp += Random.Range(-0.02f, 0.02f);
        simulatedTemp = Mathf.Clamp(simulatedTemp, 25.5f, 27.5f);
        simulatedHumi += Random.Range(-0.05f, 0.05f);
        simulatedHumi = Mathf.Clamp(simulatedHumi, 58.0f, 65.0f);

        // --- Animate active actuators ---
        if (isSoilDry && waterPump != null)
        {
            // Pump is active - vibrate mesh
            float speed = 80f;
            float amount = 0.0015f;
            waterPump.localPosition = initialPumpPos + new Vector3(
                Mathf.Sin(Time.time * speed) * amount,
                Mathf.Cos(Time.time * speed) * amount,
                0f
            );
        }
        else if (waterPump != null)
        {
            waterPump.localPosition = initialPumpPos;
        }

        // --- LCD Text Display Printing ---
        blinkTimer += Time.deltaTime;
        if (blinkTimer >= 0.5f)
        {
            blinkTimer = 0f;
            blinkState = !blinkState;
        }

        if (lcdText != null)
        {
            string line1 = $"T:{simulatedTemp:F1}C H:{simulatedHumi:F0}%";
            string line2 = "";

            if (isSoilDry)
            {
                if (blinkState)
                {
                    line2 = "[!] SOIL DRY!";
                }
                else
                {
                    line2 = "PUMP: ACTIVE (ON)";
                }
            }
            else
            {
                string rainStatus = isRaining ? "YES" : "NO";
                line2 = $"S:{simulatedSoilMoisture:F0}% RAIN:{rainStatus}";
            }

            lcdText.text = $"{line1}\n{line2}";
        }
    }

    private void UpdateActuators()
    {
        if (relayLedLight != null)
        {
            relayLedLight.color = isSoilDry ? activeRelayColor : idleRelayColor;
            relayLedLight.intensity = isSoilDry ? 3.0f : 1.0f;
        }

        if (relayLedMaterial != null)
        {
            Color targetColor = isSoilDry ? activeRelayColor : idleRelayColor;
            relayLedMaterial.color = targetColor;
            relayLedMaterial.SetColor("_EmissionColor", targetColor * 1.5f);
        }

        if (rainLedLight != null)
        {
            rainLedLight.color = isRaining ? activeRainColor : idleRainColor;
            rainLedLight.intensity = isRaining ? 3.0f : 1.0f;
        }

        if (rainLedMaterial != null)
        {
            Color targetColor = isRaining ? activeRainColor : idleRainColor;
            rainLedMaterial.color = targetColor;
            rainLedMaterial.SetColor("_EmissionColor", targetColor * 1.5f);
        }
    }

    private void AddColliderIfMissing(Transform t)
    {
        if (t == null) return;
        
        Collider c = t.GetComponentInChildren<Collider>();
        if (c == null)
        {
            MeshFilter mf = t.GetComponentInChildren<MeshFilter>();
            if (mf != null)
            {
                BoxCollider bc = mf.gameObject.AddComponent<BoxCollider>();
                bc.size = bc.size * 1.2f;
            }
            else
            {
                t.gameObject.AddComponent<BoxCollider>();
            }
        }
    }
}