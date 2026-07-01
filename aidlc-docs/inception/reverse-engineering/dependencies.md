# Dependencies

## Internal Dependencies

```
SmartCircuitManager
    +---> ActuatorController (SetFansSpinning, SetLightActive)
    +---> UnityEngine.UI.Slider (4 sliders)
    +---> TextMeshPro (lcdText)
    +---> Camera (mainCamera)
    +---> UnityEngine.UI.Button (camButton)

PhysicalCircuitManager
    +---> TextMeshPro (lcdText)
    +---> UnityEngine.InputSystem.Mouse (click detection)
    +---> Transform (soilSensor, rainSensor, relayModule, waterPump)
    +---> UnityEngine.Light (relayLedLight, rainLedLight)
    +---> Material (relayLedMaterial, rainLedMaterial)

ActuatorController
    +---> Transform (fan1, fan2)
    +---> UnityEngine.Light (bulbLight)
    +---> UnityEngine.Renderer (bulbRenderer)
    +---> Material (glowMaterial - emission control)

JumperWireRenderer
    +---> LineRenderer (per wire connection)
    +---> Material (wireMaterial)
    +---> Transform (startPoint, endPoint per connection)

FanBladesRotator
    +---> MeshFilter (split mesh)
    +---> MeshRenderer (material copy)
    +---> (No external script dependencies)
```

## External Dependencies

### Unity Engine Packages (All via Package Manager)
- **com.unity.render-pipelines.universal** - URP rendering
  - Usage: PC_RPAsset, Mobile_RPAsset, all materials, post-processing
- **com.unity.textmeshpro** - Text rendering
  - Usage: LCD display text in both manager scripts
- **com.unity.inputsystem** - Input handling
  - Version: see packages-lock.json
  - Usage: `Mouse.current.leftButton` in PhysicalCircuitManager
- **com.atteneder.gltfast** - GLTF asset loading
  - Usage: All GLTF/GLB 3D models (Arduino, LCD, Pump, RainSensor)
- **com.unity.splines** - Spline curves
  - Usage: Wire curves (potential use in JumperWireRenderer)
- **com.unity.burst** - Performance compilation
- **com.unity.collections** - Native collections
- **com.unity.mathematics** - Math utilities
- **com.unity.ai.navigation** - NavMesh (available, not currently used)
- **com.unity.timeline** - Animation timeline

## Dependency Notes

### PhysicalCircuitManager vs SmartCircuitManager
- Hai script này phục vụ hai chế độ khác nhau của cùng một scene
- Không gọi lẫn nhau - hoạt động độc lập
- Cả hai đều cập nhật TextMeshPro LCD display

### FanBladesRotator vs ActuatorController
- FanBladesRotator: Xử lý mesh phức tạp từ GLTF (split fan blades)
- ActuatorController: Rotate toàn bộ Transform của fan
- Hai cách tiếp cận khác nhau cho cùng mục tiêu (fan animation)
- FanBladesRotator dùng mesh splitting DSU, ActuatorController dùng Transform.Rotate đơn giản
