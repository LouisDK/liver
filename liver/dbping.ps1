### My Ping
DO
{
    try {
        (Invoke-WebRequest 'http://52.170.94.3/dbhealth' -TimeoutSec 1).Content
    }
    catch {
        Write-Host "Service not responding..." -ForegroundColor "Red";
    }
    
    start-sleep 1
}
while (1 -eq 1) 

