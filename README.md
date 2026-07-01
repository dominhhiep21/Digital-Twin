# 🌿 Digital Twin — ESP32 Smart Greenhouse

A high-fidelity 3D **Digital Twin** simulation of an ESP32-based smart greenhouse, built with **Unity 6** and **Universal Render Pipeline (URP)**. The project replicates the physical hardware setup — microcontroller, sensors, actuators, and wiring — entirely in interactive 3D space.

---

## 📸 Overview

The simulation features two parallel interaction modes:

| Mode | Description |
|------|-------------|
| **Physical Circuit** | Click directly on 3D sensor models (soil probe, rain sensor) to toggle states. The relay LED, water pump, and LCD respond physically. |
| **Smart Greenhouse** | Use on-screen UI sliders to simulate environmental changes (temperature, humidity, soil moisture, rain). Fans, lights, and LCD react automatically based on thresholds. |

---

## 🔧 Hardware Components Simulated

| Component | Asset |
|-----------|-------|
| ESP32 Microcontroller | `Assets/ESP32/` |
| Breadboard (Protoboard) | Inside `Assets/Pump and soil sensor/` |
| DHT22 Temperature & Humidity Sensor | `Assets/DHT22/` |
| Soil Moisture Sensor + Comparator Module | Inside `Assets/Pump and soil sensor/` |
| Rain Sensor (Piezoelectric) | `Assets/RainSensor/` |
| 5V Relay Module | Inside `Assets/Pump and soil sensor/` |
| Water Pump | Inside `Assets/Pump and soil sensor/` |
| LCD 1602 Display | `Assets/LCD/` |
| 9V Battery + Snap Clip | Inside `Assets/Pump and soil sensor/` |
| Greenhouse Structure | `Assets/Green house/` |
| Cooling Fans & Bulb | Inside Greenhouse hierarchy |
| 3D Jumper Wires | Physical meshes from GLTF assets |

---

## ⚙️ Logic & Thresholds

```
Temperature  > 30°C    →  Cooling fans ON
Soil Moisture < 30%    →  Relay activates, water pump ON, LCD alert "[!] SOIL DRY!"
Rain Intensity > 15%   →  Grow light (bulb) ON
Rain Sensor click      →  Toggle rain state on LCD
Soil Sensor click      →  Toggle dry/wet state, pump vibrates
```

---

## 🗂️ Project Structure

```
Assets/
├── Scripts/
│   ├── PhysicalCircuitManager.cs   # Click-based physical circuit simulation
│   ├── SmartCircuitManager.cs      # UI slider-based greenhouse simulation
│   ├── ActuatorController.cs       # Fan rotation + light bulb control
│   ├── JumperWireRenderer.cs       # 3D wire visualization (LineRenderer + catenary sag)
│   └── FanBladesRotator.cs         # GLTF mesh splitting for fan blade animation
├── Scenes/
│   └── SampleScene.unity           # Main scene
├── Plans/
│   ├── esp32_physical_circuit.md   # Physical circuit design document
│   └── esp32_sensor_board.md       # Smart greenhouse design document
├── ESP32/                          # ESP32 FBX model
├── DHT22/                          # DHT22 sensor FBX model
├── LCD/                            # LCD 1602 GLTF model
├── Pump and soil sensor/           # Breadboard, pump, relay, sensors, jumper wires (GLTF)
├── RainSensor/                     # Rain sensor GLTF model
├── Arduino/                        # Arduino board + DHT11 GLTF model
├── Green house/                    # Greenhouse 3D structure
├── Automated green house/          # Greenhouse variant with integrated sensors
└── Tree/                           # Decorative trees
```

---

## 🚀 Getting Started

### Prerequisites

- **Unity 6** — version `6000.4.8f1` or later
- **Universal Render Pipeline** (included via Package Manager)

### Opening the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/dominhhiep21/Digital-Twin.git
   ```
2. Open Unity Hub → **Add project from disk** → select the cloned folder.
3. Wait for Unity to import all assets (first import may take a few minutes due to large GLTF files).
4. Open `Assets/Scenes/SampleScene.unity`.
5. Press **Play**.

---

## 🎮 Controls

| Action | Input |
|--------|-------|
| Orbit camera | Left-click + drag |
| Pan camera | Middle-click + drag |
| Zoom | Mouse scroll wheel |
| Toggle soil dry/wet | Left-click on the Soil Sensor model |
| Toggle rain state | Left-click on the Rain Sensor model |
| Adjust environment (Smart mode) | UI Sliders (Temperature / Humidity / Soil / Rain) |
| Switch camera view | "Camera" button on UI (Board View ↔ Greenhouse View) |

---

## 📦 Key Dependencies

| Package | Version |
|---------|---------|
| Universal Render Pipeline | 17.4.0 |
| Input System | 1.19.0 |
| TextMeshPro | (bundled) |
| glTFast | latest (git) |
| Splines | 2.8.4 |
| AI Navigation | 2.0.12 |
| Timeline | 1.8.12 |

---

## 🏗️ Technical Highlights

- **DSU Mesh Splitting** — `FanBladesRotator` uses Disjoint Set Union algorithm to identify and separate rotating blade geometry from static fan housing at runtime, enabling smooth animation without manual mesh editing.
- **World Space LCD** — The LCD 1602 display is rendered directly in 3D space using a World Space Canvas + TextMeshPro, with emissive materials simulating the yellow backlight.
- **Parabolic Wire Sag** — `JumperWireRenderer` simulates realistic wire drooping using the catenary formula `sag = 4 * droop * t * (1 - t)` applied along each LineRenderer.
- **Dual Simulation Modes** — Two independent managers handle different interaction paradigms without coupling.

---

## 📄 License

3D assets (Arduino, ESP32, LCD, sensors) sourced from third-party providers — see individual `license.txt` files within each asset folder.

Project code (`Assets/Scripts/`) — free to use for educational purposes.
