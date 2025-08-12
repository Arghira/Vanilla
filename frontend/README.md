# Vanilla UI (React + TypeScript + Tailwind via Vite)

## How to run
```powershell
cd frontend
npm install
npm run dev
```
- API base defaults to the same origin. For local dev, override:
```powershell
$env:VITE_API_BASE="https://localhost:7044"; npm run dev
```

## Build and copy into ASP.NET `wwwroot`
```powershell
npm run build
robocopy dist ..\Vanilla\wwwroot /MIR
```

## Deploy with Docker on Render
- Add a Node build stage that copies `/ui/dist` into `Vanilla/wwwroot` before `dotnet publish`.
- See the Dockerfile snippet provided in chat.
