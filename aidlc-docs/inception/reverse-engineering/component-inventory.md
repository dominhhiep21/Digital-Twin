# Component Inventory

## Application Scripts
- `PhysicalCircuitManager.cs` - Physical circuit simulation (click interaction mode)
- `SmartCircuitManager.cs` - Smart greenhouse simulation (UI slider mode)
- `ActuatorController.cs` - Fan rotation + bulb lighting actuators
- `JumperWireRenderer.cs` - Wire visualization via LineRenderer
- `FanBladesRotator.cs` - Fan mesh splitting and rotation

## 3D Asset Components

### Sensors
- `Assets/DHT22/` - DHT22 temperature/humidity sensor model
- `Assets/RainSensor/` - Rain sensor (piezoelectric) model
- (DHT11 embedded in) `Assets/Arduino/` - Arduino scene with DHT11

### Electronic Boards
- `Assets/ESP32/` - ESP32 microcontroller
- `Assets/Arduino/` - Arduino board with breadboard, DHT11, resistors
- `Assets/LCD/` - LCD 1602 display module

### Mechanical Assets (inside Pump and soil sensor gltf)
- Protoboard - White breadboard
- Bateria + Clip - 9V battery with snap clip
- sensor - Soil moisture probe
- modulo - Soil sensor comparator module
- Modulo_Rele_5v_1 - 5V relay module
- Circle / Circle.002 / Circle.003 - Water pump
- PRETO.001 to PRETO.017, Jumper_0 to Jumper_4 - Physical jumper wire meshes

### Environment Assets
- `Assets/Green house/` - Greenhouse 3D structure
- `Assets/Automated green house/` - Automated greenhouse variant with Rain Sensor (Pieroelectric)
- `Assets/Tree/` - Decorative tree models

## Unity Package Dependencies
- com.unity.render-pipelines.universal - URP
- com.unity.textmeshpro - TextMeshPro
- com.unity.inputsystem - New Input System
- com.atteneder.gltfast - GLTF asset loader
- com.unity.splines - Spline curves
- com.unity.burst - Burst compiler (performance)
- com.unity.collections - Unity collections
- com.unity.mathematics - Math library
- com.unity.ai.navigation - AI Navigation
- com.unity.timeline - Timeline animation
- com.unity.visualscripting - Visual Scripting

## Total Count
- **Total Scripts**: 5 C# MonoBehaviour scripts
- **Total 3D Asset Folders**: 9 (Arduino, ESP32, DHT22, LCD, Pump and soil sensor, RainSensor, Green house, Automated green house, Tree)
- **Total Scenes**: 1 (SampleScene.unity)
- **Total Plan Documents**: 2 (esp32_physical_circuit.md, esp32_sensor_board.md)
