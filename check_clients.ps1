$clientCsvPath = "C:\Users\loren\OneDrive\Desktop\ELECTIVA 1\Archivo CSV Análisis de Opiniones de Clientes-20260529\clients.csv"
$surveysPath = "C:\Users\loren\OneDrive\Desktop\ELECTIVA 1\Archivo CSV Análisis de Opiniones de Clientes-20260529\surveys_part1.csv"
$webPath = "C:\Users\loren\OneDrive\Desktop\ELECTIVA 1\Archivo CSV Análisis de Opiniones de Clientes-20260529\web_reviews.csv"
$socialPath = "C:\Users\loren\OneDrive\Desktop\ELECTIVA 1\Archivo CSV Análisis de Opiniones de Clientes-20260529\social_comments.csv"

$clients = Import-Csv $clientCsvPath
$clientDict = @{}
foreach ($c in $clients) {
    $idStr = $c.id_cliente -replace '\D',''
    if ($idStr) { $clientDict[[int]$idStr] = $true }
}
Write-Host "Total clients in clients.csv: $($clientDict.Count)"

$surveys = Import-Csv $surveysPath
$sValid = 0; $sInvalid = 0
foreach ($s in $surveys) {
    $idStr = $s.id_cliente -replace '\D',''
    if ($idStr -and $clientDict.ContainsKey([int]$idStr)) { $sValid++ } else { $sInvalid++ }
}
Write-Host "Surveys total: $($surveys.Count) -> Valid clients: $sValid, Invalid clients: $sInvalid"

$web = Import-Csv $webPath
$wValid = 0; $wInvalid = 0
foreach ($w in $web) {
    $idStr = $w.id_cliente -replace '\D',''
    if ($idStr -and $clientDict.ContainsKey([int]$idStr)) { $wValid++ } else { $wInvalid++ }
}
Write-Host "WebReviews total: $($web.Count) -> Valid clients: $wValid, Invalid clients: $wInvalid"

$social = Import-Csv $socialPath
$scValid = 0; $scInvalid = 0
foreach ($sc in $social) {
    $idStr = $sc.id_cliente -replace '\D',''
    if ($idStr -and $clientDict.ContainsKey([int]$idStr)) { $scValid++ } else { $scInvalid++ }
}
Write-Host "SocialComments total: $($social.Count) -> Valid clients: $scValid, Invalid clients: $scInvalid"

$totalValid = $sValid + $wValid + $scValid
Write-Host "TOTAL VALID OPINIONS (having existing Client ID): $totalValid"
