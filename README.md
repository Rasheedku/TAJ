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

## Troubleshooting: Clone shows only `.git` / `.gitkeep`
If you cloned the repo and the folder is empty (only `.git` or `.gitkeep`), use these steps:
1. Start clean:
   ```bash
   cd "C:\"
   rmdir /s /q "Taj App"
   mkdir "Taj App"
   cd "Taj App"
   ```
2. Clone the main branch explicitly:
   ```bash
   git clone --branch main https://github.com/Rasheedku/Taj/ .
   ```
3. Verify files exist:
   ```bash
   dir
   ```
   You should see `TAJ.sln`, `README.md`, and `src\`.

If you still see an empty folder, run:
```bash
git remote -v
git branch -a
```
Then check out the default branch (for example `main`):
```bash
git checkout -b main origin/main
```
If the output of `git branch -a` shows a different default branch, replace `main` accordingly.

## Troubleshooting: Packaging project not selectable as Startup
If **Taj.BuildingCostApp.Packaging** shows in Solution Explorer but is missing from **Startup Project** options:
1. Close Visual Studio.
2. Make sure your local solution is up to date:
   ```bash
   cd "C:\Taj App"
   git pull
   ```
3. Re-open `TAJ.sln`, then use **Build → Configuration Manager** and ensure:
   - **Active solution configuration** = `Debug`
   - **Active solution platform** = `x64`
   - The **Taj.BuildingCostApp.Packaging** row has **Build** and **Deploy** checked

If the packaging project still does not appear in **Startup Project** options, re-add it to the solution:
1. Right-click **Solution 'TAJ'** → **Add → Existing Project...**
2. Select `src\Taj.BuildingCostApp.Packaging\Taj.BuildingCostApp.Packaging.wapproj`.

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
