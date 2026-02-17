# Test Docker Build Locale

Write-Host "🐳 Testing Docker Build Localmente" -ForegroundColor Cyan
Write-Host ""

# 1. Build Backend
Write-Host "📦 Building Backend Docker Image..." -ForegroundColor Yellow
cd backend
docker build -t medical-appointments-backend:test .
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Backend build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "✅ Backend build successful!" -ForegroundColor Green
Write-Host ""

# 2. Build Frontend
Write-Host "📦 Building Frontend Docker Image..." -ForegroundColor Yellow
cd ../frontend
docker build --build-arg VITE_API_BASE_URL=http://localhost:8080/api -t medical-appointments-frontend:test .
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Frontend build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "✅ Frontend build successful!" -ForegroundColor Green
Write-Host ""

# 3. Crea network Docker
Write-Host "🌐 Creating Docker network..." -ForegroundColor Yellow
docker network create medical-network 2>$null
Write-Host "✅ Network ready!" -ForegroundColor Green
Write-Host ""

# 4. Run Backend
Write-Host "🚀 Starting Backend container..." -ForegroundColor Yellow
docker run -d `
    --name medical-backend-test `
    --network medical-network `
    -p 8080:8080 `
    -e ASPNETCORE_ENVIRONMENT=Production `
    -e ASPNETCORE_URLS=http://+:8080 `
    -e "JwtSettings__SecretKey=TestSecretKeyForLocalDockerTestingOnly1234567890!!" `
    -e "JwtSettings__Issuer=MedicalAppointmentsAPI" `
    -e "JwtSettings__Audience=MedicalAppointmentsClient" `
    -e "JwtSettings__ExpirationMinutes=1440" `
    -e "ConnectionStrings__DefaultConnection=Data Source=/app/data/medical_appointments.db" `
    -v medical-data:/app/data `
    medical-appointments-backend:test

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Backend container failed to start!" -ForegroundColor Red
    exit 1
}
Write-Host "✅ Backend container started!" -ForegroundColor Green
Write-Host ""

# 5. Wait for backend to be ready
Write-Host "⏳ Waiting for backend to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# 6. Test backend
Write-Host "🧪 Testing backend health..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:8080/api/Health" -TimeoutSec 5
    if ($response.StatusCode -eq 200) {
        Write-Host "✅ Backend health check passed!" -ForegroundColor Green
    }
} catch {
    Write-Host "❌ Backend health check failed!" -ForegroundColor Red
    Write-Host "Logs:" -ForegroundColor Yellow
    docker logs medical-backend-test
    docker stop medical-backend-test 2>$null
    docker rm medical-backend-test 2>$null
    exit 1
}
Write-Host ""

# 7. Run Frontend
Write-Host "🚀 Starting Frontend container..." -ForegroundColor Yellow
docker run -d `
    --name medical-frontend-test `
    --network medical-network `
    -p 3000:8080 `
    medical-appointments-frontend:test

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Frontend container failed to start!" -ForegroundColor Red
    docker stop medical-backend-test 2>$null
    docker rm medical-backend-test 2>$null
    exit 1
}
Write-Host "✅ Frontend container started!" -ForegroundColor Green
Write-Host ""

# 8. Wait for frontend
Write-Host "⏳ Waiting for frontend to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 5

# 9. Test frontend
Write-Host "🧪 Testing frontend..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:3000/health" -TimeoutSec 5
    if ($response.StatusCode -eq 200) {
        Write-Host "✅ Frontend health check passed!" -ForegroundColor Green
    }
} catch {
    Write-Host "❌ Frontend health check failed!" -ForegroundColor Red
    Write-Host "Logs:" -ForegroundColor Yellow
    docker logs medical-frontend-test
}
Write-Host ""

# 10. Summary
Write-Host "═══════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "✅ DOCKER TEST COMPLETATO!" -ForegroundColor Green
Write-Host "═══════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""
Write-Host "🌐 Servizi attivi:" -ForegroundColor Yellow
Write-Host "   Backend API:  http://localhost:8080" -ForegroundColor White
Write-Host "   Frontend:     http://localhost:3000" -ForegroundColor White
Write-Host "   Swagger:      http://localhost:8080/swagger" -ForegroundColor White
Write-Host ""
Write-Host "📊 Comandi utili:" -ForegroundColor Yellow
Write-Host "   Logs Backend:  docker logs medical-backend-test -f" -ForegroundColor White
Write-Host "   Logs Frontend: docker logs medical-frontend-test -f" -ForegroundColor White
Write-Host ""
Write-Host "🛑 Per fermare i container:" -ForegroundColor Yellow
Write-Host "   docker stop medical-backend-test medical-frontend-test" -ForegroundColor White
Write-Host "   docker rm medical-backend-test medical-frontend-test" -ForegroundColor White
Write-Host "   docker network rm medical-network" -ForegroundColor White
Write-Host "   docker volume rm medical-data" -ForegroundColor White
Write-Host ""
Write-Host "🎯 Credenziali per test:" -ForegroundColor Yellow
Write-Host "   admin / Admin123!" -ForegroundColor White
Write-Host "   mario.rossi / Mario123!" -ForegroundColor White
Write-Host ""
Write-Host "Press any key to open browser..." -ForegroundColor Green
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

# Open browser
Start-Process "http://localhost:3000"
Start-Process "http://localhost:8080/swagger"
