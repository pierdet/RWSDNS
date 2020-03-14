[cmdletbinding()]
Param([string]$url)

# @Todo Rewrite to support new API Endpoints and the new URL:s

#skip SSL
add-type @"
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    public class TrustAllCertsPolicy : ICertificatePolicy {
        public bool CheckValidationResult(
            ServicePoint srvPoint, X509Certificate certificate,
            WebRequest request, int certificateProblem) {
            return true;
        }
    }
"@
[System.Net.ServicePointManager]::CertificatePolicy = New-Object TrustAllCertsPolicy

#Zone JSON
$zoneJson = @{
    "Zone" = "test.se"
} | ConvertTo-Json
Invoke-RestMethod -ContentType "application/json" -Method Post -Uri "$($url)/v1/dns/DnsZone" -Body $zoneJson

#A Record JSON
$AJson = @{
    "Hostname"="abc"
    "IPAddress"="1.1.1.1"
    "Zone"="test.se"
} | ConvertTo-Json
Invoke-RestMethod -ContentType "application/json" -Method Post -Uri "$($url)/v1/dns/ARecord" -Body $AJson

#Cname record JSON
$CNameJson = @{
    "Hostname"="abcde"
    "PrimaryName"="google.se"
    "Zone"="test.se"
} | ConvertTo-Json
Invoke-RestMethod -ContentType "application/json" -Method Post -Uri "$($url)/v1/dns/CnameRecord" -Body $CNameJson

#TXT Record JSON
$TXTJson = @{
    "Hostname"="aa"
    "DescriptiveText"="hej--123"
    "Zone"="test.se"
} | ConvertTo-Json
Invoke-RestMethod -ContentType "application/json" -Method Post -Uri "$($url)/v1/dns/TxtRecord" -Body $TXTJson