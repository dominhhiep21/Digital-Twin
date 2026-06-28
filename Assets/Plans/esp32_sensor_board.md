# Project Overview
- **Game Title**: ESP32 Smart Greenhouse Digital Twin
- **High-Level Concept**: An interactive 3D digital twin of a smart greenhouse controlled by an ESP32 microcontroller mounted on a protoboard, integrated directly onto the Greenhouse asset structure. The ESP32 gathers real-time data from three integrated sensors (DHT11 temperature/humidity, soil moisture inside the soil bed, and a rain sensor on the roof), displays them on an on-board World Space LCD screen mounted on the greenhouse, and triggers smart actuators (spinning the cooling fans and lighting up the greenhouse bulb) based on automated threshold logic.
- **Players**: Single player (interactive technical simulation / educational demo)
- **Inspiration / Reference Games**: Shenzhen I/O, Kerbal Space Program (UI style), interactive web-based digital twins, and industrial IoT dashboards.
- **Tone / Art Direction**: Clean, modern, technical, realistic lighting with emissive displays and distinct color-coded electronic wiring.
- **Target Platform**: PC / Standalone Windows
- **Screen Orientation / Resolution**: Landscape 1920x1080 (aspect ratio 16:9)
- **Render Pipeline**: Universal Render Pipeline (URP)

---

# Game Mechanics
## Core Gameplay Loop
1. **Observe**: The player views the beautifully lit Green House asset, which now features the integrated ESP32 circuit board, sensors, and LCD display panel mounted directly onto its main structure.
2. **Interact / Simulate**: The player uses a Screen Space overlay containing interactive UI sliders to simulate real-world environmental changes:
   - Temperature (e.g., 10°C to 50°C)
   - Ambient Humidity (e.g., 20% to 100%)
   - Soil Moisture (e.g., 0% to 100%)
   - Rain Intensity (e.g., 0% to 100%)
3. **Analyze Feedback**: 
   - The virtual sensors in the 3D scene detect the simulated values.
   - The World Space LCD 1602 screen mounted on the Greenhouse front frame displays real-time formatting (e.g. `T: 25C  H: 60% \n Soil:45% Rain:0%`).
   - If **Temperature > 30°C**, the 3D **Cooler / Fans** inside the Greenhouse automatically start spinning.
   - If **Soil Moisture < 30%**, an alert displays on the LCD: `[!] Soil Dry!`.
   - If **Rain Intensity > 10%**, the LCD displays `Rain: YES (X%)` and can trigger a subtle rain sound or particle system.

## Controls and Input Methods
- **Mouse Click & Drag**: Adjust Screen Space UI Sliders.
- **Camera Controls**: Mouse scroll to zoom, middle mouse hold to drag/pan, or simple toggle buttons to switch between:
  - **View 1: Board Close-up** (Frames the integrated ESP32, Protoboard, LCD, and DHT11 on the Greenhouse structure).
  - **View 2: Greenhouse Overview** (Frames the entire greenhouse setup to watch the fans spin and light turn on).

---

# UI
## Screen Space Dashboard (Control Panel)
- Situated on the left/right side of the screen as a transparent, high-tech panel:
  - **DHT11 Environment**: Sliders for Temperature and Humidity.
  - **Soil Moisture Environment**: Slider for Soil Moisture level.
  - **Precipitation Environment**: Slider for Rain intensity.
  - **Camera Controller**: Buttons to switch between "Board View" and "Greenhouse View".

## World Space LCD 1602 Display (Integrated into Green House)
- Built directly into the 3D Green House structure (e.g. mounted on the front pillar or outer wall) using a flat Cube and an overlaying World Space Canvas with **TextMeshPro (TMP) Text**:
  - Uses a monospaced digital font (or standard TMP font) with a cyan-blue/greenish background light (using material emission).
  - Shows dynamic, formatted data updated per frame from the simulation manager.

---

# Key Asset & Context
## Existing Assets to Reuse
- **ESP32 Board**: `Assets/ESP32/source/ESP32.fbx`
- **Protoboard & DHT11**: Located inside `Assets/Arduino/scene.gltf` (specifically nodes `Protoboard`, `DHT11`, and `Resistor`).
- **Soil Moisture Sensor**: `Soil Sensor` GameObject (already in `SampleScene` under `Green House` hierarchy).
- **Rain Sensor**: `Pieroelectric` model inside `Assets/Automated green house/source/Green House.fbx` (which acts as a rain sensing pad).
- **Actuators (Cooler Fans & Light)**: `Cooler`, `Fan 1`, `Fan 2`, and `Bulb` inside `Green House` scene hierarchy.

## New Assets to Create
1. **LCD Display Prefab**: Built from primitive Cubes (PCB backboard, screen bevel, screen glass) and a World Space UI Canvas with TextMeshPro. Reparented under the `Green House` GameObject.
2. **C# Scripts**:
   - `SmartCircuitManager.cs` (Core Controller): Handles environmental state, connects Screen Space UI sliders, computes thresholds, updates the LCD text, and drives actuators.
   - `JumperWireRenderer.cs`: Draws colored wires connecting the ESP32 pins to the sensors and LCD using `LineRenderer` components.
   - `ActuatorController.cs`: Rotates the fans (`Fan 1`, `Fan 2`, `Cooler`) when triggered, and controls the emission color of the Greenhouse `Bulb` material.
3. **Materials**:
   - `LCD_Backlight_Mat`: Cyan-blue emissive material for the LCD screen background.
   - `Wire_Red_Mat`, `Wire_Black_Mat`, `Wire_Blue_Mat`: Plain colored unlit materials for the jumper wires.

---

# Implementation Steps

### Step 1: Deactivate Arduino, Reparent & Position ESP32 Board
- **Description**: Open `SampleScene.unity`. 
  - Navigate to `scene/Collada visual scene group/` and disable the `Arduino` GameObject.
  - Extract the `Protoboard` GameObject and reparent it directly under the `Green House` GameObject so it forms a single integrated prefab/asset hierarchy.
  - Drag the `ESP32` GameObject (`Assets/ESP32/source/ESP32.fbx`) into the scene and make it a child of `Green House`.
  - Position and scale the `Protoboard` and `ESP32` to sit on an available flat section or side shelf of the `Green House` (e.g. on the wooden support frame or next to the plant buckets).
  - Recommended scale for ESP32 on the Protoboard: approximately `(4.5, 4.5, 4.5)` (or adjusted to fit the Protoboard pins perfectly).
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: No

### Step 2: Set Up Sensors & Actuators inside Green House
- **Description**: Mount all sensors and hook up greenhouse components.
  - **DHT11**: Reparent it under `Green House`. Position it directly on the `Protoboard` to monitor internal greenhouse air temperature.
  - **Soil Sensor**: Keep its existing position inside the soil bed/bucket of the `Green House`.
  - **Rain Sensor**: Copy the `Pieroelectric` mesh from `Green House.fbx` and place it on the roof of the `Green House` structure (where rain would actually land). Parent it to the `Green House` GameObject and rename it to `Rain Sensor`.
  - **Greenhouse Actuators**: Locate `Fan 1`, `Fan 2`, and `Bulb` inside the `Green House` hierarchy. Add an `ActuatorController.cs` script to spin the fans and handle the bulb emissive material toggling.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: Yes

### Step 3: Create and Mount the Virtual LCD 1602 Display
- **Description**: Build the 3D World Space LCD display and mount it.
  - Create an empty GameObject named `Virtual_LCD_1602` as a child of `Green House`.
  - Create a child Cube (`PCB_Base`, scale: `(1.6, 0.8, 0.05)`) with a dark green or blue material.
  - Create another child Cube (`Screen_Area`, scale: `(1.4, 0.5, 0.06)`) with a cyan-blue emissive material.
  - Create a child Canvas (Render Mode: **World Space**, Event Camera: Main Camera). Position it exactly on the front face of `Screen_Area`.
  - Create a TextMeshPro - Text element under the Canvas. Format it to use monospaced text, color it dark blue or black, and adjust the font size to fit inside the screen area (simulating a 16x2 LCD).
  - Mount this LCD unit on the outer frame or front pillar of the `Green House` as an integrated display panel.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: Yes

### Step 4: Implement Jumper Wires (JumperWireRenderer)
- **Description**: Add realistic wiring routed along the greenhouse frame.
  - Create an empty GameObject `Wire_Manager` as a child of `Green House`.
  - Create child GameObjects with `LineRenderer` components representing wire jumpers:
    - **VCC (5V/3V3)**: Red wires from ESP32 power pins to Protoboard power rails and sensor VCC pins.
    - **GND**: Black wires from ESP32 GND pins to Protoboard ground rails and sensor GND pins.
    - **DHT11 Signal**: Blue wire from DHT11 Pin to ESP32 Pin.
    - **Soil Sensor Signal**: Yellow wire routed from the soil bed Soil Sensor along the wooden pillars to the ESP32 Pin.
    - **Rain Sensor Signal**: Green wire routed from the roof Rain Sensor down to the ESP32 Pin.
    - **LCD I2C (SDA/SCL)**: Orange and White wires from LCD to ESP32 Pin 21 and Pin 22.
  - Configure `JumperWireRenderer.cs` to set the point positions of these LineRenderers to form nice, slightly drooping wire arcs that hug the structure.
- **Assigned role**: developer
- **Dependencies**: Step 2, Step 3
- **Parallelizable**: Yes

### Step 5: Implement Simulation Dashboard (Screen Space UI)
- **Description**: Create the user interface for adjusting variables.
  - Add a Canvas (Render Mode: Screen Space - Overlay).
  - Add a Panel with 4 sliders:
    - Temperature Slider: Range `[10, 50]`, label "Temperature (°C)"
    - Humidity Slider: Range `[20, 100]`, label "Humidity (%)"
    - Soil Moisture Slider: Range `[0, 100]`, label "Soil Moisture (%)"
    - Rain Intensity Slider: Range `[0, 100]`, label "Rain Intensity (%)"
  - Add a camera switch toggle button.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes

### Step 6: Code the SmartCircuitManager & Core Logic
- **Description**: Write `SmartCircuitManager.cs` to connect everything.
  - Read input values from the 4 UI Sliders.
  - Check thresholds:
    - If `Temperature > 30f`, call `ActuatorController.SpinFans(true, speedFactor)`. Otherwise, `ActuatorController.SpinFans(false)`.
    - If `Soil Moisture < 30f`, set a flag `isSoilDry = true`.
    - If `Rain Intensity > 10f`, set a flag `isRaining = true`.
  - Format the string for the LCD Text:
    - Line 1: `"Temp: " + temp.ToString("F1") + "C  Humi: " + humi.ToString("F0") + "%"`
    - Line 2: `"Soil: " + soil.ToString("F0") + "% " + (isRaining ? "Rain:YES" : "Rain:NO")`
    - Alternate line 2 with `"[!] DRY SOIL!"` if `isSoilDry` is true to create a blinking alarm effect.
  - Update the TextMeshPro on `Virtual_LCD_1602`.
- **Assigned role**: developer
- **Dependencies**: Step 3, Step 5
- **Parallelizable**: No

### Step 7: Camera Framing & Post-Processing (Visual Polish)
- **Description**: Enhance the visual digital twin look.
  - Reposition the `Main Camera` to look down on the `Green House` asset at an angled, aesthetic perspective (e.g., position `(-4.5, -14, -8)` looking towards the Greenhouse).
  - Implement a simple camera controller script to smooth-glide the camera between the Board/LCD Panel Focus and the full Greenhouse Focus when the UI toggle is clicked.
  - Configure the URP Global Volume profile with Bloom (to make the LCD screen and bulb glow beautifully) and Tonemapping.
- **Assigned role**: developer
- **Dependencies**: Step 6
- **Parallelizable**: Yes

---

# Verification & Testing
1. **Interactive Test**:
   - Run the scene in Play Mode.
   - Adjust the Temperature slider to `40°C`. Verify that:
     - The LCD displays `Temp: 40.0C`.
     - The `Fan 1` and `Fan 2` models in the Greenhouse spin continuously.
   - Adjust the Temperature slider back to `20°C`. Verify that:
     - The fans slow down and stop spinning.
2. **Soil Moisture Threshold Test**:
   - Drag the Soil Moisture slider to `15%`. Verify that:
     - The LCD screen periodically blinks `[!] DRY SOIL!` or shows a dry soil alert.
3. **Rain Sensor Test**:
   - Drag the Rain slider to `80%`. Verify that:
     - The LCD screen changes `Rain:NO` to `Rain:YES` or shows the rain percentage.
4. **Wiring Verification**:
   - Verify in the scene view that all `LineRenderer` wires connect the correct pin positions on the ESP32 to the corresponding nodes (DHT11, LCD, Sensors) without floating in empty space.
