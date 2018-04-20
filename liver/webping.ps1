### My Ping
DO
{
    try {
        (Invoke-WebRequest 'http://localhost:5000/health' -TimeoutSec 2).Content
    }
    catch {
        Write-Host "Service not responding..." -ForegroundColor "Red";
    }
    
    start-sleep 1
}
while (1 -eq 1) 

