[cmdletbinding()]
Param([string]$url)

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

# Auth headers
$headers = @{"ApiKey" = "Password"}

#Zone JSON
$zoneJson = @{
    "Zone" = "test.se"
} | ConvertTo-Json
Invoke-RestMethod -ContentType "application/json" -Method Delete -Uri "$($url)/v1.0/dns/DnsZone" -Body $zoneJson -Headers $headers

#A Record JSON
$AJson = @{
    "Hostname"="abc"
    "IPAddress"="1.1.1.1"
    "Zone"="test.se"
} | ConvertTo-Json
Invoke-RestMethod -ContentType "application/json" -Method Delete -Uri "$($url)/v1.0/dns/ARecord" -Body $AJson -Headers $headers

#Cname record JSON
$CNameJson = @{
    "Hostname"="abcde"
    "PrimaryName"="google.se"
    "Zone"="test.se"
} | ConvertTo-Json
Invoke-RestMethod -ContentType "application/json" -Method Delete -Uri "$($url)/v1.0/dns/CnameRecord" -Body $CNameJson -Headers $headers

#TXT Record JSON
$TXTJson = @{
    "Hostname"="aa"
    "DescriptiveText"="hej--123"
    "Zone"="test.se"
} | ConvertTo-Json
Invoke-RestMethod -ContentType "application/json" -Method Delete -Uri "$($url)/v1.0/dns/TxtRecord" -Body $TXTJson -Headers $headers