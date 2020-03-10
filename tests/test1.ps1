Function Test1 {
    [cmdletbinding()]
    Param([string]$url)

    #Zone JSON
    $zoneJson = @{
        "Zone" = "test.se"
    } | ConvertTo-Json
    Invoke-RestMethod -Method Post -Uri "$($url)/v1/dns/DnsZone" -Body $zoneJson

    #A Record JSON
    $AJson = @{
        "Hostname"="abc"
        "IPAddress"="1.1.1.1"
        "Zone"="test.se"
    } | ConvertTo-Json
    Invoke-RestMethod -Method Post -Uri "$($url)/v1/dns/ARecord" -Body $AJson

    # @Todo implement tests for the other methods
}