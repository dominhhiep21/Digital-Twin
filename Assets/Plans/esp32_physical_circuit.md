# Project Overview
- **Game Title**: ESP32 Physical Breadboard Circuit Assembler (Digital Twin)
- **High-Level Concept**: A realistic, 3D physical electronic circuit assembly simulation. Instead of screen-space dashboards or algorithmic LineRenderers, this project focuses purely on the high-fidelity 3D modeling and integration of real-world electronic components. It positions the ESP32 directly onto a white breadboard (Protoboard) alongside a 9V battery, liquid crystal display (LCD 1602), DHT sensor, rain sensor, soil moisture (pump) sensor, a 5V relay module, and a water pump. All electrical connections are meticulously established using the actual 3D jumper wire models provided within the assets, mimicking a real-life hobbyist workbench.
- **Players**: Single player (educational simulator, technical showcase, hardware demonstration)
- **Inspiration / Reference Games**: Shenzhen I/O, PC Building Simulator, My Summer Car (wiring mechanics), real-world Fritzing/breadboard prototyping.
- **Tone / Art Direction**: Technical, photorealistic, hardware-focused workbench style with detailed wiring and emissive component indicators.
- **Target Platform**: PC / Standalone Windows
- **Screen Orientation / Resolution**: Landscape 1920x1080
- **Render Pipeline**: Universal Render Pipeline (URP)

---

# Game Mechanics
## Core Gameplay Loop
1. **Explore the Workspace**: The player is presented with a 3D electronics workbench. All classic DIY components are laid out physically on or around a white breadboard (Protoboard).
2. **Interact with Hardware (Physically Wired)**: 
   - The user observes the realistic 3D placement of the ESP32 on the white protoboard.
   - A 9V Battery with a snap-on clip provides main power to the board.
   - The Soil Moisture (Pump) Sensor is plugged into the soil bed, wired directly to its comparator module.
   - The Rain Sensor is mounted flat on the workspace or greenhouse.
   - The 5V Relay is wired as an intermediary switch to control the 12V submersible Water Pump.
   - The DHT sensor monitors ambient conditions.
   - The yellow-backlit parallel LCD 1602 display updates with readouts from the ESP32.
3. **No Screen-Space Dashboards**: The entire simulation runs directly on the 3D models. The LCD 1602 displays text using an on-mesh TextMeshPro Canvas, and the relay LED lights up physically.

## Controls and Input Methods
- **Camera Controls**: Mouse-based orbital camera (left-click/middle-click drag to orbit/pan, scroll to zoom) allowing the player to inspect the physical wiring from all angles.
- **Component Toggles**: Simple interaction (e.g., clicking on the rain sensor or dipping the soil sensor into water) physically triggers state changes in the circuit (e.g., the relay clicks, its LED turns on, the pump starts rotating/vibrating, and the LCD prints updated sensor values).

---

# UI
- **No Screen-Space Dashboard / UI Canvas**: In accordance with the user's instructions, all UI panels, sliders, and overlays are removed.
- **World Space LCD Display**: Text is printed directly onto the physical `LCD1602-Parallel-LCD-Display-Yellow-Backlight-ROBU_2` mesh's screen surface using a World Space Canvas.

---

# Key Asset & Context
## Existing Assets to Reuse
1. **Breadboard (Protoboard)**: Mesh named `Protoboard` from `Assets/Pump and soil sensor/scene.gltf`.
2. **ESP32 Microcontroller**: `Assets/ESP32/source/ESP32.fbx`.
3. **LCD 1602**: `LCD1602-Parallel-LCD-Display-Yellow-Backlight-ROBU_2` from `Assets/LCD/scene.gltf`.
4. **DHT Sensor**: `DHT11` from `Assets/Arduino/scene.gltf` or `DHT22` from `Assets/DHT22/source/DHT22.fbx`.
5. **Rain Sensor**: `Pieroelectric` from `Assets/Automated green house/source/Green House.fbx` or the custom roof sensor.
6. **9V Battery & Clip**: Meshes `Bateria` and `Clip` from `Assets/Pump and soil sensor/scene.gltf`.
7. **Relay Module**: `Modulo_Rele_5v_1` from `Assets/Pump and soil sensor/scene.gltf`.
8. **Soil Moisture Sensor & Comparator**: `sensor` (soil probe) and `modulo` (comparator board) from `Assets/Pump and soil sensor/scene.gltf`.
9. **Water Pump**: `Circle`, `Circle.002`, `Circle.003` from `Assets/Pump and soil sensor/scene.gltf`.
10. **Physical Jumper Wires**: 3D wire assets (`PRETO.001` - `PRETO.017`, `Jumper_0` - `Jumper_4`, `BezierCurve` nodes) from `Assets/Pump and soil sensor/scene.gltf` and `Assets/Arduino/scene.gltf`. These will be physically duplicated, positioned, scaled, and color-coded to connect pin holes on the breadboard.

## New Assets to Create
- **`PhysicalCircuitManager.cs`**: Reads simulation inputs (e.g. mouse clicks or automated cycles representing dry/wet soil and rain) and directly controls the hardware state (turning on the relay light, spinning/vibrating the pump, updating the physical LCD text).
- **Materials**: Emissive green LED for the relay board, yellow-backlight material for the LCD.

---

# Implementation Steps

### Step 1: Initialize Workbench & Clean Scene
- **Description**: Open `SampleScene.unity`. Remove the previously created Screen Space `Dashboard_Canvas` and algorithmic `Wire_Manager` LineRenderers. Disable or remove the old `Arduino/scene` hierarchy.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: No

### Step 2: Instantiate and Place the Core Breadboard & ESP32
- **Description**: 
  - Instantiate `Assets/Pump and soil sensor/scene.gltf` into the scene. Unpack the prefab completely.
  - Keep the `Protoboard`, `Bateria`, `Clip`, `sensor`, `modulo`, `Modulo_Rele_5v_1`, and the pump (`Circle` meshes) active. Disable the built-in `Arduino` model inside this prefab.
  - Position the `ESP32` mesh (`Assets/ESP32/source/ESP32.fbx`) directly onto the central partition of the white `Protoboard` so that its pins align with the breadboard's socket rows.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: No

### Step 3: Integrate LCD, DHT, and Rain Sensors
- **Description**:
  - Instantiate `Assets/LCD/scene.gltf` to get the physical yellow-backlight LCD 1602 display. Position it standing upright next to the breadboard.
  - Mount a World Space Canvas directly onto the physical LCD screen surface, using a dark gray monospaced font with a glowing yellow-backlight background material to match the ROBU LCD texture.
  - Position the `DHT11` (or DHT22) sensor and the `Rain Sensor` adjacent to the board.
- **Assigned role**: developer
- **Dependencies**: Step 2
- **Parallelizable**: Yes

### Step 4: Physical 3D Jumper Wiring Assembly (Real-Life Prototyping)
- **Description**: 
  - Instead of drawing line renderers, locate the 3D jumper wire models inside the `Pump and soil sensor` assets (such as `PRETO.003`, `PRETO.004`... which represent curved wires).
  - Duplicate, color-code (e.g., Red for VCC, Black for GND, Blue for Signal), rotate, and scale these 3D meshes to physically plug their metal pins into the breadboard ties:
    - **ESP32 Power**: Connect ESP32 VCC/GND to the Protoboard power rails.
    - **Battery**: Connect the 9V `Bateria` clip wires to the power regulator/relay pins.
    - **Soil Sensor**: Connect the probe `sensor` to its comparator `modulo`, then to the ESP32.
    - **Relay & Pump**: Route wires from ESP32 to the `Modulo_Rele_5v_1` inputs, and route the high-power circuit from the battery through the relay to the water pump (`Circle` meshes).
    - **LCD 1602**: Connect SDA/SCL pins of the LCD to ESP32 I2C pins.
- **Assigned role**: developer
- **Dependencies**: Step 3
- **Parallelizable**: No

### Step 5: Code the PhysicalCircuitManager & Actuator Logic
- **Description**: Write `PhysicalCircuitManager.cs` to run the hardware logic:
  - Toggle states based on player clicks (e.g., clicking the rain sensor toggles rain state; clicking the soil probe toggles wet/dry state).
  - If Soil is DRY:
    - Trigger the Relay (change material of the relay's on-board indicator LED to emissive green).
    - Activate the Pump (apply a vibration script or rotation to the pump's interior propeller/shaft).
    - Display alert `[!] PUMP ACTIVE` and `Soil: DRY` on the physical LCD.
  - If Soil is WET:
    - Deactivate Relay (Indicator LED off) and stop the Pump.
    - Update LCD to show normal readings.
- **Assigned role**: developer
- **Dependencies**: Step 4
- **Parallelizable**: No

### Step 6: Visual Polish and Camera Framing
- **Description**: Place the camera at an ideal angle framing the physical breadboard layout. Enable ambient occlusion and depth of field so that the copper pins, plastic board sockets, and colored wires look highly realistic.
- **Assigned role**: developer
- **Dependencies**: Step 5
- **Parallelizable**: Yes

---

# Verification & Testing
1. **Visual Inspection**:
   - Verify in the scene that no floating or algorithmic LineRenderers exist.
   - Verify that all wire connections are represented by actual 3D curved jumper meshes plugged directly into the physical sockets of the Protoboard, ESP32, Relay, LCD, and sensors.
2. **Functional Play Mode Test**:
   - Enter Play Mode.
   - Click the Soil Sensor to toggle dry soil. Verify that:
     - The relay LED material switches to emissive green.
     - The physical water pump meshes vibrate/rotate.
     - The physical LCD updates its text immediately with monospaced readouts.
   - Click the Rain Sensor to toggle rain. Verify that the LCD screen displays `Rain: YES` or `Rain: NO`.
