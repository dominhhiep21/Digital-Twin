# Business Overview

## Business Context Diagram

```
+------------------------------------------------------------------+
|              ESP32 Digital Twin - Smart Greenhouse               |
|                                                                  |
|  +------------------+        +-----------------------------+     |
|  |  Physical Circuit|        |  Smart Greenhouse Simulator |     |
|  |  (Workbench Mode)|        |  (Greenhouse Mode)          |     |
|  +------------------+        +-----------------------------+     |
|         |                              |                         |
|         +----------+  +----------------+                         |
|                    |  |                                          |
|              +-----v--v------+                                   |
|              | Sensor System |                                    |
|              | - Soil Sensor |                                    |
|              | - Rain Sensor |                                    |
|              | - DHT22/11    |                                    |
|              +-------+-------+                                   |
|                      |                                           |
|              +-------v-------+                                   |
|              | Actuator Sys  |                                    |
|              | - Relay/Pump  |                                    |
|              | - Fans        |                                    |
|              | - Bulb/Light  |                                    |
|              +-------+-------+                                   |
|                      |                                           |
|              +-------v-------+                                   |
|              |  LCD Display  |                                    |
|              |  (World Space)|                                    |
|              +---------------+                                   |
+------------------------------------------------------------------+
```

## Business Description

- **Business Description**: Ứng dụng Unity 3D mô phỏng hệ thống Digital Twin của nhà kính thông minh (Smart Greenhouse) được điều khiển bởi ESP32. Hệ thống tái hiện trung thực môi trường phần cứng vật lý gồm mạch điện, cảm biến, và cơ cấu chấp hành trong không gian 3D.

- **Business Transactions**:
  1. **Soil Moisture Monitoring** - Đọc giá trị độ ẩm đất từ cảm biến và kích hoạt máy bơm nước khi đất khô (< 30%)
  2. **Rain Detection** - Phát hiện mưa từ cảm biến mưa và cập nhật trạng thái hiển thị
  3. **Temperature/Humidity Monitoring** - Đọc nhiệt độ/độ ẩm không khí từ DHT11/22 và kích hoạt quạt làm mát khi nhiệt độ > 30°C
  4. **LCD Status Display** - Hiển thị dữ liệu cảm biến real-time trên màn hình LCD 1602 nhúng trong scene 3D
  5. **Actuator Control** - Điều khiển cơ cấu chấp hành: relay, máy bơm, quạt, đèn dựa trên ngưỡng cảm biến
  6. **Physical Circuit Interaction** - Cho phép người dùng click vào cảm biến để toggle trạng thái (chế độ Physical Circuit)
  7. **UI Slider Simulation** - Cho phép người dùng điều chỉnh slider trên Screen Space UI để mô phỏng thay đổi môi trường (chế độ Smart Greenhouse)
  8. **Camera View Toggle** - Chuyển đổi góc nhìn camera giữa chế độ Board View và Greenhouse View

- **Business Dictionary**:
  - **Digital Twin**: Bản sao kỹ thuật số của hệ thống vật lý trong môi trường 3D
  - **ESP32**: Vi điều khiển nhúng WiFi/Bluetooth, trung tâm của mạch
  - **DHT11/DHT22**: Cảm biến nhiệt độ và độ ẩm không khí
  - **Soil Moisture Sensor**: Cảm biến độ ẩm đất (analog) + module so sánh (digital)
  - **Rain Sensor**: Cảm biến mưa piezoelectric
  - **Relay Module 5V**: Module relay để đóng/ngắt mạch điện công suất cao
  - **LCD 1602**: Màn hình hiển thị 16x2 ký tự, giao tiếp I2C
  - **Protoboard/Breadboard**: Bảng mạch thử nghiệm trắng
  - **Actuator**: Cơ cấu chấp hành (quạt, bơm, đèn)
  - **World Space Canvas**: Canvas Unity hiển thị trong không gian 3D thay vì overlay 2D

## Component Level Business Descriptions

### PhysicalCircuitManager (Assets/Scripts/PhysicalCircuitManager.cs)
- **Purpose**: Mô phỏng chế độ "vật lý" - người dùng click trực tiếp vào model cảm biến 3D để toggle trạng thái
- **Responsibilities**: Raycast click detection, soil/rain state toggling, relay LED feedback, pump vibration animation, LCD text update

### SmartCircuitManager (Assets/Scripts/SmartCircuitManager.cs)
- **Purpose**: Mô phỏng chế độ "thông minh" - người dùng dùng UI slider để thay đổi giá trị môi trường
- **Responsibilities**: UI slider reading, threshold logic (fan/light on/off), LCD text formatting, camera view switching

### ActuatorController (Assets/Scripts/ActuatorController.cs)
- **Purpose**: Điều khiển các cơ cấu chấp hành vật lý trong scene
- **Responsibilities**: Fan spinning animation, light bulb emission toggle

### JumperWireRenderer (Assets/Scripts/JumperWireRenderer.cs)
- **Purpose**: Vẽ dây jumper kết nối giữa các component điện tử sử dụng LineRenderer
- **Responsibilities**: Wire generation, drooping catenary curve simulation, real-time update

### FanBladesRotator (Assets/Scripts/FanBladesRotator.cs)
- **Purpose**: Xử lý mesh phức tạp của quạt - tách phần cánh quạt ra khỏi phần khung để có thể xoay độc lập
- **Responsibilities**: Mesh splitting via DSU (Disjoint Set Union), blade detection by radius threshold, rotation animation
