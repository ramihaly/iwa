$Endpoint = "http://localhost:26046/api/Interactions/"
$JsonContentType = "application/json"

$interaction = @{
    Id="f6997816-665f-47e8-a5d3-8d1c0cb74ced" #[guid]::NewGuid()
    Sent=0
    Delivered=0
}

$interactionJson = $interaction | ConvertTo-Json
<#
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
#>

$upsertEndpoint = "$Endpoint/upsert"
Invoke-RestMethod -Uri $upsertEndpoint -Method Put -Body $interactionJson -ContentType $JsonContentType | Out-Null
For ($i=0; $i -le 1000; $i++)
{
    $interaction.Sent += Get-Random -Minimum 1 -Maximum 20
    $interactionJson = $interaction | ConvertTo-Json
    Invoke-RestMethod -Uri $upsertEndpoint -Method Put -Body $interactionJson -ContentType $JsonContentType | Out-Null
}
