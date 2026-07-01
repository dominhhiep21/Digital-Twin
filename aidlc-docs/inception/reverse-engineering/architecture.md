# System Architecture

## System Overview

Dự án "Digital Twin" là một Unity 6 application mô phỏng nhà kính thông minh với hai chế độ hoạt động song song:

1. **Physical Circuit Mode** - Mô phỏng bàn làm việc điện tử vật lý với ESP32, breadboard, dây jumper 3D
2. **Smart Greenhouse Mode** - Mô phỏng nhà kính với UI slider để điều chỉnh môi trường

## Architecture Diagram

```
+--------------------------------------------------------------------+
|                     Unity SampleScene                              |
|                                                                    |
|  +------------------+          +---------------------------+       |
|  | Physical Circuit |          |   Smart Greenhouse        |       |
|  | Mode             |          |   Mode                    |       |
|  |                  |          |                           |       |
|  | PhysicalCircuit  |          | SmartCircuit              |       |
|  | Manager.cs       |          | Manager.cs                |       |
|  | - Click Input    |          | - Slider UI Input         |       |
|  | - Raycast        |          | - Threshold Logic         |       |
|  | - LED feedback   |          | - Camera Control          |       |
|  +--------+---------+          +----------+----------------+       |
|           |                               |                        |
|           +----------+  +----------------+                         |
|                      |  |                                          |
|             +--------v--v--------+                                 |
|             | ActuatorController |                                  |
|             | - Fan Spin         |                                  |
|             | - Bulb Emission    |                                  |
|             +--------+-----------+                                 |
|                      |                                             |
|          +-----------+------------+                                |
|          |                        |                                |
|  +-------v-------+    +-----------v------+                        |
|  | FanBladesRot  |    | JumperWire       |                        |
|  | ator.cs       |    | Renderer.cs      |                        |
|  | - Mesh split  |    | - LineRenderer   |                        |
|  | - DSU algo    |    | - Catenary curve |                        |
|  +---------------+    +------------------+                        |
|                                                                    |
|  +-------------------------+    +-----------------------------+    |
|  | 3D Assets               |    | World Space LCD Canvas      |    |
|  | - Arduino (gltf)        |    | - TextMeshPro               |    |
|  | - ESP32 (fbx)           |    | - Emissive background       |    |
|  | - DHT22 (fbx)           |    +-----------------------------+    |
|  | - Pump+Soil (gltf)      |                                       |
|  | - LCD (gltf)            |                                       |
|  | - RainSensor (gltf)     |                                       |
|  | - Greenhouse (fbx)      |                                       |
|  | - Tree (source)         |                                       |
|  +-------------------------+                                       |
+--------------------------------------------------------------------+
```

## Component Descriptions

### PhysicalCircuitManager
- **Purpose**: Điều khiển chế độ mạch điện vật lý
- **Responsibilities**: Mouse raycast click, toggle soil/rain state, animate pump, LED feedback, LCD text
- **Dependencies**: Unity Input System (Mouse), TextMeshPro, UnityEngine.Light
- **Type**: Application (MonoBehaviour)

### SmartCircuitManager
- **Purpose**: Điều khiển chế độ nhà kính thông minh với UI slider
- **Responsibilities**: Read UI sliders, compute thresholds, drive ActuatorController, update LCD, camera lerp
- **Dependencies**: UnityEngine.UI.Slider, TextMeshPro, ActuatorController
- **Type**: Application (MonoBehaviour)

### ActuatorController
- **Purpose**: Cơ cấu chấp hành vật lý (quạt, đèn)
- **Responsibilities**: Fan1/Fan2 rotation, bulb light toggle, emission material control
- **Dependencies**: UnityEngine.Light, UnityEngine.Renderer
- **Type**: Application (MonoBehaviour)

### JumperWireRenderer
- **Purpose**: Vẽ dây kết nối 3D
- **Responsibilities**: Generate/update LineRenderers với drooping parabolic curve
- **Dependencies**: UnityEngine.LineRenderer, configurable wire material
- **Type**: Application (MonoBehaviour, ExecuteAlways)

### FanBladesRotator
- **Purpose**: Xử lý phần cứng quạt từ mesh GLTF nguyên khối
- **Responsibilities**: DSU mesh splitting, blade isolation, continuous rotation
- **Dependencies**: UnityEngine.MeshFilter, UnityEngine.MeshRenderer
- **Type**: Application (MonoBehaviour)

## Data Flow

```
User Input (Mouse Click / UI Slider)
        |
        v
PhysicalCircuitManager / SmartCircuitManager
        |
        +---------> ActuatorController (Fan/Light)
        |                   |
        |                   v
        |           FanBladesRotator (3D Fan mesh)
        |
        +---------> LCD TextMeshPro (World Space display)
        |
        +---------> LED Material Emissive (Relay indicator)
        |
        +---------> JumperWireRenderer (Wire visualization)
```

## Integration Points

- **External APIs**: None (standalone Unity simulation)
- **Databases**: None
- **Third-party Services**: None
- **Unity Packages Used**: TextMeshPro, New Input System, URP, glTFast (for GLTF loading), Splines

## Infrastructure Components

- **Deployment Model**: Standalone PC (Windows)
- **Render Pipeline**: Universal Render Pipeline (URP)
- **Build System**: Unity Bee Build System (tundra)
- **Target Resolution**: 1920x1080 Landscape
