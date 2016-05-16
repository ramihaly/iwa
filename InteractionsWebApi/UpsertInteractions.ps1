$Endpoint = "http://localhost:26046/api/Interactions/"
$JsonContentType = "application/json"

$interaction = @{
    Id=[guid]::NewGuid()
    Sent=0
    Delivered=0
}

$interactionJson = $interaction | ConvertTo-Json
Invoke-RestMethod -Uri $Endpoint -Method Post -Body $interactionJson -ContentType $JsonContentType
Write-Output "POST $Endpoint $interactionJson"

For ($i=0; $i -le 1000; $i++)
{
    $interaction.Sent += Get-Random -Minimum 1 -Maximum 20
    $interactionJson = $interaction | ConvertTo-Json
    $uri = "$Endpoint/$($interaction.Id)"
    Invoke-RestMethod -Uri $uri -Method Put -Body $interactionJson -ContentType $JsonContentType
    Write-Output "PUT $uri $($interaction.Sent)"
    If ($i % 5 -eq 0) {
        start-sleep -Seconds 1
    }
}

