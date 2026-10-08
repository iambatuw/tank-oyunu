@echo off
chcp 65001 >nul
cd /d "%~dp0"
if not exist "Build\TankDuel.exe" (
    echo Oyun dosyaları eksik. İndirdiğiniz ZIP dosyasının tamamını bir klasöre çıkartın.
    pause
    exit /b 1
)
start "" "Build\TankDuel.exe"
