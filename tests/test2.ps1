[cmdletbinding()]
Param([string]$url)

#Zone JSON
$zoneJson = @{
    "Zone" = "test.se"
} | ConvertTo-Json
Invoke-RestMethod -Method Delete -Uri "$($url)/v1/dns/DnsZone" -Body $zoneJson

#A Record JSON
$AJson = @{
    "Hostname"="abc"
    "IPAddress"="1.1.1.1"
    "Zone"="test.se"
} | ConvertTo-Json
Invoke-RestMethod -Method Delete -Uri "$($url)/v1/dns/ARecord" -Body $AJson

#Cname record JSON
$CNameJson = @{
    "Hostname"="abcde"
    "PrimaryName"="google.se"
    "Zone"="test.se"
} | ConvertTo-Json
Invoke-RestMethod -Method Delete -Uri "$($url)/v1/dns/CnameRecord" -Body $CNameJson

#TXT Record JSON
$TXTJson = @{
    "Hostname"="aa"
    "DescriptiveText"="hej--123"
    "Zone"="test.se"
} | ConvertTo-Json
Invoke-RestMethod -Method Delete -Uri "$($url)/v1/dns/TxtRecord" -Body $TXTJson