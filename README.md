# TAJ Building Cost App

TAJ is a packaged-only WinUI 3 desktop application (Windows App SDK on .NET 8) for building cost estimation. The app ships as an MSIX via a packaging project and does **not** support unpackaged execution.

## Prerequisites
- Windows 10/11
- Visual Studio 2022 with:
  - .NET Desktop Development
  - Universal Windows Platform development
  - Windows App SDK tooling
- .NET 8 SDK

## Run (Packaged Only)
1. Open `TAJ.sln` in Visual Studio.
2. Set **Taj.BuildingCostApp.Packaging** as the Startup Project.
3. Select **Debug | x64** and run.

## Seeded Admin Credentials
If `users.json` is missing, the app seeds:
- **Username:** `admin`
- **Password:** `temp1234`
- **Role:** `Admin`

Change this password in the **Admin → Users** screen after first login.

## Data Storage
JSON files are stored in the app's local data folder (Windows app data):
- `users.json`
- `prices.json`
- `settings.json`

Default seeds are packaged under `src/Taj.BuildingCostApp/Data/` and copied to local storage on first run.

## Pricing & Settings
Admin users can manage prices and constants from **Admin → Pricing**. Changes persist to `prices.json`.

