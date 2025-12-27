# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Alpha 5.0] - 2025-12-28

### ✨ Added
- **Manual Throw Mechanic**: Players can now throw packages by clicking the mouse at any time when holding a package
- **Aiming Line**: Added a dashed white line showing throw trajectory from player to mouse cursor
- **Aim Assist**: Using OverlapCircle (1m radius) for forgiving hit detection
- **Throw Range**: Maximum throw distance clamped to 12 meters
- **Miss Penalty System**: 
  - Throwing at empty ground or full customers results in a miss
  - Score penalty: -1 point for each miss
  - Visual feedback: "-1" text appears when package is destroyed
- **Package Destruction Animation**: 
  - Missed packages fly to target, roll on ground, then fade out
  - 3-phase animation: Fly → Roll to stop → Fade out
- **Package Stacking System**: 
  - Successfully delivered packages now stack next to customer areas
  - Boxes pile up like a tower as more deliveries are made
  - Visual indicator of delivery progress
- **Footprint Effects**: Added procedural footprint particles when player moves
- **GitHub Actions CI/CD**: 
  - Automatic WebGL build on push/PR to main
  - Automatic deployment to GitHub Pages

### 🔧 Changed
- **WebGL Compatibility**: 
  - Replaced all `System.IO.File` operations with `PlayerPrefs`
  - Score saving now works in WebGL builds (stored in IndexedDB)
- **Aiming Line**: Removed color change on targeting (always white)
- **Throw Animation**: Packages fly in a parabolic arc with spin effect

### 🐛 Fixed
- Fixed package snapping back to player after throw
- Fixed Z-axis issues causing packages to disappear behind ground
- Fixed duplicate variable declarations causing compilation errors
- Fixed score not updating after successful delivery with stacking system

### 📁 Files Modified
- `Assets/ScriptAll/Delivery.cs` - Major refactoring for throw mechanics
- `Assets/ScriptAll/Driver.cs` - Added footprint effects
- `Assets/ScriptOfMenu/SaveName.cs` - WebGL compatibility
- `Assets/ScriptOfMenu/ShowHightScore.cs` - WebGL compatibility
- `Assets/ScriptOfMenu/ShowNameMenue.cs` - WebGL compatibility
- `Assets/Scenes/Menu.unity` - Version update
- `.github/workflows/build-and-deploy.yml` - New CI/CD workflow

---

## [Alpha 4.0] - Previous Release

### Features
- Basic delivery gameplay
- Blue and Black customer types
- Timer-based gameplay
- High score system
- Localization support (English/Thai)

---

## [Alpha 3.1] - Earlier Release

### Features
- Initial game mechanics
- Basic UI implementation
- Sound effects

