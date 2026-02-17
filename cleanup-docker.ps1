# Cleanup Docker Test Containers

Write-Host "🧹 Cleaning up Docker test environment..." -ForegroundColor Cyan
Write-Host ""

# Stop containers
Write-Host "🛑 Stopping containers..." -ForegroundColor Yellow
docker stop medical-backend-test 2>$null
docker stop medical-frontend-test 2>$null
Write-Host "✅ Containers stopped" -ForegroundColor Green

# Remove containers
Write-Host "🗑️  Removing containers..." -ForegroundColor Yellow
docker rm medical-backend-test 2>$null
docker rm medical-frontend-test 2>$null
Write-Host "✅ Containers removed" -ForegroundColor Green

# Remove network
Write-Host "🌐 Removing network..." -ForegroundColor Yellow
docker network rm medical-network 2>$null
Write-Host "✅ Network removed" -ForegroundColor Green

# Remove volume (optional - chiedi conferma)
Write-Host ""
$response = Read-Host "Vuoi eliminare anche il volume del database? (y/N)"
if ($response -eq 'y' -or $response -eq 'Y') {
    Write-Host "🗃️  Removing volume..." -ForegroundColor Yellow
    docker volume rm medical-data 2>$null
    Write-Host "✅ Volume removed" -ForegroundColor Green
} else {
    Write-Host "ℹ️  Volume mantenuto (medical-data)" -ForegroundColor Blue
}

# Remove images (optional - chiedi conferma)
Write-Host ""
$response = Read-Host "Vuoi eliminare anche le immagini Docker? (y/N)"
if ($response -eq 'y' -or $response -eq 'Y') {
    Write-Host "🖼️  Removing images..." -ForegroundColor Yellow
    docker rmi medical-appointments-backend:test 2>$null
    docker rmi medical-appointments-frontend:test 2>$null
    Write-Host "✅ Images removed" -ForegroundColor Green
} else {
    Write-Host "ℹ️  Immagini mantenute" -ForegroundColor Blue
}

Write-Host ""
Write-Host "✅ Cleanup completato!" -ForegroundColor Green
