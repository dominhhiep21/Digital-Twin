# API Documentation

## Internal Script APIs

### PhysicalCircuitManager (Public Interface)
```csharp
// Inspector-configured references
public Transform soilSensor;
public Transform rainSensor;
public Transform relayModule;
public Transform waterPump;
public TextMeshProUGUI lcdText;

// Simulation state (Inspector-readable)
public bool isSoilDry = false;
public bool isRaining = false;

// Visual feedback configuration
public Color activeRelayColor = Color.green;
public Color idleRelayColor = Color.red;
public Color activeRainColor = Color.green;
public Color idleRainColor = Color.red;
```

**Behavior**:
- `Start()` - Adds MeshColliders to sensors, sets up LED lights/materials
- `Update()` - Raycast click detection, fluctuate simulated values, animate pump, update LCD
- `UpdateActuators()` - Updates relay and rain LED colors/intensity

---

### SmartCircuitManager (Public Interface)
```csharp
// UI References
public UnityEngine.UI.Slider tempSlider;    // Range: 10-50 C
public UnityEngine.UI.Slider humiSlider;    // Range: 20-100 %
public UnityEngine.UI.Slider soilSlider;    // Range: 0-100 %
public UnityEngine.UI.Slider rainSlider;    // Range: 0-100 %

// Outputs
public TextMeshProUGUI lcdText;
public ActuatorController actuatorController;

// Camera control
public Camera mainCamera;
public Vector3 boardViewPosition;
public Vector3 boardViewRotation;
public Vector3 greenhouseViewPosition;
public Vector3 greenhouseViewRotation;
public float cameraLerpSpeed = 5f;

// Public method
public void ToggleCameraView();  // Toggle between board/greenhouse camera views
```

**Threshold Logic**:
- `temp > 30f` => fans ON
- `soil < 30f` => soilDry alert + LCD blink
- `rain > 15f || soilDry` => light ON

---

### ActuatorController (Public Interface)
```csharp
public Transform fan1;
public Transform fan2;
public float spinSpeed = 500f;
public Light bulbLight;
public Renderer bulbRenderer;
public int glowMaterialIndex = 3;
public Color activeEmissionColor;

// Public methods
public void SetFansSpinning(bool active);
public void SetLightActive(bool active);
```

---

### JumperWireRenderer (Public Interface)
```csharp
[System.Serializable]
public class WireConnection {
    public string label;
    public Transform startPoint;
    public Transform endPoint;
    public Color wireColor;
    public float droop;   // 0.01 to 1.0
    public int segments;  // wire smoothness
}

public List<WireConnection> connections;
public Material wireMaterial;
public float wireWidth = 0.015f;

// Public methods
public void GenerateWires();   // Create LineRenderer GameObjects
public void UpdateWires();     // Update wire positions per frame
// Context menu: "Rebuild Wires"
```

---

### FanBladesRotator (Public Interface)
```csharp
public float rotationSpeed = 600f;  // degrees per second

// No public methods - self-contained
// Automatically splits mesh on Start() and rotates blades on Update()
```

## Data Models

### WireConnection (JumperWireRenderer.cs)
```csharp
string label          // Wire name/label
Transform startPoint  // World space start position
Transform endPoint    // World space end position
Color wireColor       // Wire color (Red/Black/Blue/etc.)
float droop           // Sag amount (0.01 = tight, 1.0 = very droopy)
int segments          // Smoothness (15 = default)
LineRenderer lineRenderer  // [HideInInspector] cached renderer
```
