# Technology Stack

## Programming Languages
- C# 10 - Unity scripting language (all 5 scripts)

## Frameworks
- Unity 6 (Unity Engine 6000.x) - Game engine / simulation platform
- Universal Render Pipeline (URP) - Render pipeline for realistic lighting
- TextMeshPro - Advanced text rendering cho LCD display
- Unity New Input System - Mouse click/input handling
- glTFast - GLTF/GLB 3D model loading at runtime

## Infrastructure
- Standalone PC build (Windows x64)
- No cloud/server infrastructure
- No database
- No networking

## Build Tools
- Unity Bee Build System (tundra) - C# compilation and asset pipeline
- Burst Compiler - DOTS/performance-critical code compilation
- Visual Studio / JetBrains Rider - IDE support

## Testing Tools
- Unity Test Runner (Unity.TestRunner) - Unit and play mode testing
- Unity Performance Testing (Unity.PerformanceTesting) - Performance benchmarks

## Asset Pipeline
- glTFast - Import GLTF/GLB models (Arduino, LCD, Pump sensor, Rain sensor)
- Unity FBX Importer - Import FBX models (ESP32, DHT22, Greenhouse)
- Unity ShaderGraph - Custom shader creation (URP-based)

## Version Control
- Git (.gitattributes, .gitignore present)
- Unity-specific LFS settings in .gitattributes

## Development Environment
- Unity 6 Editor
- VSCode (`.vscode/settings.json` present)
- JetBrains Rider (Rider editor package present)
- Windows 11 (development platform)
