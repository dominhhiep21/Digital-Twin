# Code Structure

## Build System
- **Type**: Unity 6 (Bee Build System)
- **Configuration**: `Assembly-CSharp.csproj`, `Assembly-CSharp-Editor.csproj`
- **Primary Assembly**: `Assembly-CSharp.dll` (88KB compiled)
- **Editor Assembly**: `Assembly-CSharp-Editor.dll` (10KB compiled)

## Source File Inventory

### Application Scripts (Assets/Scripts/)
- `Assets/Scripts/FanBladesRotator.cs` - Mesh splitting + fan blade rotation (DSU algorithm)
- `Assets/Scripts/PhysicalCircuitManager.cs` - Physical circuit simulation (click-based interaction)
- `Assets/Scripts/SmartCircuitManager.cs` - Smart greenhouse simulation (UI slider-based)
- `Assets/Scripts/ActuatorController.cs` - Fan spinning + bulb light control
- `Assets/Scripts/JumperWireRenderer.cs` - LineRenderer-based wire visualization

### Scene Files
- `Assets/Scenes/SampleScene.unity` - Main and only scene (621KB)

### 3D Asset Files
- `Assets/Arduino/scene.gltf` + `scene.bin` (28MB) - Arduino board model with Protoboard, DHT11
- `Assets/ESP32/source/ESP32.fbx` - ESP32 microcontroller model
- `Assets/DHT22/source/DHT22.fbx` - DHT22 sensor model
- `Assets/LCD/scene.gltf` + `scene.bin` (5.4KB) - LCD 1602 display model
- `Assets/Pump and soil sensor/scene.gltf` + `scene.bin` (48MB) - Pump, soil sensor, relay, breadboard, battery, jumper wires
- `Assets/RainSensor/scene.gltf` + `scene.bin` (2.9MB) - Rain sensor model
- `Assets/Green house/` - Greenhouse 3D model with materials
- `Assets/Automated green house/source/` - Automated greenhouse variant
- `Assets/Tree/source/` - Tree decorative models

### Plan Documents (Assets/Plans/)
- `Assets/Plans/esp32_physical_circuit.md` - Physical circuit assembly plan
- `Assets/Plans/esp32_sensor_board.md` - Smart greenhouse sensor board plan

## Design Patterns

### DSU (Disjoint Set Union) - Mesh Splitting
- **Location**: `FanBladesRotator.cs` - `SplitFanMesh()` method
- **Purpose**: Tách mesh GLTF nguyên khối thành phần tĩnh (frame) và phần động (cánh quạt)
- **Implementation**: DSU để tìm connected components, dùng threshold bán kính 2D để phân loại vertex

### Observer/Event Pattern (Simplified)
- **Location**: `SmartCircuitManager.cs` - `ToggleCameraView()` method + UI Button listener
- **Purpose**: Camera view toggle khi user click button
- **Implementation**: `camButton.onClick.AddListener(ToggleCameraView)`

### Component Pattern
- **Location**: `SmartCircuitManager.cs` + `ActuatorController.cs`
- **Purpose**: Tách logic simulation khỏi actuator control
- **Implementation**: SmartCircuitManager gọi `actuatorController.SetFansSpinning()` và `SetLightActive()`

### World Space Display
- **Location**: LCD TextMeshPro trong scene
- **Purpose**: Hiển thị dữ liệu sensor trực tiếp trong không gian 3D thay vì overlay 2D
- **Implementation**: World Space Canvas + TextMeshPro trên bề mặt LCD 3D model

## Critical Dependencies

### TextMeshPro (TMPro)
- **Version**: Bundled with Unity 6
- **Usage**: LCD display text trong PhysicalCircuitManager, SmartCircuitManager
- **Location**: `using TMPro;` in both managers

### Unity New Input System
- **Version**: com.unity.inputsystem (bundled)
- **Usage**: Mouse click detection trong PhysicalCircuitManager
- **Location**: `using UnityEngine.InputSystem;`, `Mouse.current.leftButton.wasPressedThisFrame`

### glTFast
- **Version**: com.atteneder.gltfast (package cache)
- **Usage**: Loading GLTF/GLB 3D assets (Arduino, LCD, Pump, RainSensor)
- **Location**: `Library/PackageCache/com.atteneder.gltfast@66aa58252baf`

### Universal Render Pipeline (URP)
- **Version**: com.unity.render-pipelines.universal (bundled)
- **Usage**: Render pipeline, emissive materials, lighting
- **Location**: PC_RPAsset.asset, Mobile_RPAsset.asset trong Assets/Settings/
