$csvDir = (Get-ChildItem -Path "C:\Users\loren\OneDrive\Desktop\ELECTIVA 1" -Directory -Filter "*Archivo CSV*").FullName
$clients = Import-Csv (Join-Path $csvDir "clients.csv")
$surveys = Import-Csv (Join-Path $csvDir "surveys_part1.csv")
$web = Import-Csv (Join-Path $csvDir "web_reviews.csv")
$social = Import-Csv (Join-Path $csvDir "social_comments.csv")

Write-Host "--- Analizando cómo se obtienen 848 o ~800+ hechos ---"
# Total: 500 + 200 + 200 = 900
# En social_comments: 200 filas, 112 tienen IdCliente no vacio, 88 vacios (o 52 descartados)
# 900 - 52 = 848!

$socialEmpty = 0
foreach ($sc in $social) {
    if (-not $sc.IdCliente -or $sc.IdCliente.Trim() -eq "") {
        $socialEmpty++
    }
}
Write-Host "Social comments con IdCliente vacio: $socialEmpty"
Write-Host "Total opiniones (900) - $socialEmpty = $(900 - $socialEmpty)"

# Veamos en surveys y webreviews si alguno tiene cliente vacio
$surveysEmpty = 0
foreach ($s in $surveys) {
    if (-not $s.IdCliente -or $s.IdCliente.Trim() -eq "") { $surveysEmpty++ }
}
Write-Host "Surveys con IdCliente vacio: $surveysEmpty"

$webEmpty = 0
foreach ($w in $web) {
    if (-not $w.IdCliente -or $w.IdCliente.Trim() -eq "") { $webEmpty++ }
}
Write-Host "Web reviews con IdCliente vacio: $webEmpty"

# Veamos si en el Stored Procedure o en C# se filtraba BETWEEN 1 AND 5
# En el SP de Bianca: WHERE op.[PuntajeSatisfaccion] BETWEEN 1 AND 5
# O en C# si se descartan los que no tienen IdCliente o puntaje inválido:
$validPuntaje = 0
foreach ($s in $surveys) {
    [int]$p = 0
    if ([int]::TryParse($s.PuntajeSatisfaccion, [ref]$p)) {
        if ($p -ge 1 -and $p -le 5) { $validPuntaje++ }
    }
}
Write-Host "Surveys con puntaje 1-5: $validPuntaje de $($surveys.Count)"
