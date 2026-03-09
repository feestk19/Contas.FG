param(
    [string]$BaseUrl = "http://localhost:8080"
)

$ErrorActionPreference = "Stop"

function Write-Info {
    param([string]$Message)
    Write-Output "[INFO] $Message"
}

function Write-Ok {
    param([string]$Step, [string]$Detail)
    Write-Output "[OK] $Step :: $Detail"
}

function To-JsonBody {
    param([object]$Body)
    return ($Body | ConvertTo-Json -Depth 10 -Compress)
}

function New-Conta {
    param(
        [string]$Descricao,
        [decimal]$Valor,
        [datetime]$DataVencimento,
        [long]$CategoriaId,
        [string]$BaseUrl
    )

    $body = To-JsonBody @{
        descricao = $Descricao
        valor = $Valor
        tipo = 2
        categoriaId = $CategoriaId
        dataVencimento = $DataVencimento.ToString("yyyy-MM-dd")
        status = 1
        observacao = "Smoke test HML"
    }

    return Invoke-RestMethod -Uri "$BaseUrl/api/contas" -Method Post -ContentType "application/json" -Body $body
}

try {
    Write-Info "Iniciando validacao funcional HML em $BaseUrl"

    $healthCode = curl.exe -s -o NUL -w "%{http_code}" "$BaseUrl/health"
    if ($healthCode -ne "200") {
        throw "Health check retornou status $healthCode."
    }

    Write-Ok "Health check" "status=200"

    $stamp = Get-Date -Format "yyyyMMddHHmmss"
    $today = Get-Date

    $categoria = Invoke-RestMethod -Uri "$BaseUrl/api/categorias" -Method Post -ContentType "application/json" -Body (To-JsonBody @{ nome = "Categoria HML $stamp" })
    Write-Ok "Criar categoria" "id=$($categoria.id)"

    $tipoConta = Invoke-RestMethod -Uri "$BaseUrl/api/tipos-conta" -Method Post -ContentType "application/json" -Body (To-JsonBody @{ nome = "Tipo HML $stamp" })
    Write-Ok "Criar tipo de conta" "id=$($tipoConta.id)"

    $fonte = Invoke-RestMethod -Uri "$BaseUrl/api/fontes-renda" -Method Post -ContentType "application/json" -Body (To-JsonBody @{ nome = "Fonte HML $stamp"; valorMensal = 4500.50; diaPrevistoRecebimento = 5 })
    Write-Ok "Criar fonte de renda" "id=$($fonte.id)"

    $usuario = Invoke-RestMethod -Uri "$BaseUrl/api/usuarios-permissoes/admin/existe" -Method Get
    Write-Ok "Validar usuario admin" "existe=$($usuario.existe)"

    $rotinasRaw = Invoke-WebRequest -Uri "$BaseUrl/api/usuarios-permissoes/rotinas" -Method Get -UseBasicParsing
    $rotinas = $rotinasRaw.Content | ConvertFrom-Json
    if (-not ($rotinas -is [System.Array])) {
        $rotinas = @($rotinas)
    }

    if ($rotinas.Count -lt 2) {
        throw "A API retornou menos de 2 rotinas para associacao."
    }

    $rotinasSelecionadas = @($rotinas[0], $rotinas[1])
    $associacaoBody = To-JsonBody @{ rotinas = $rotinasSelecionadas }
    $associacaoResp = Invoke-WebRequest -Uri "$BaseUrl/api/usuarios-permissoes/admin/rotinas" -Method Post -ContentType "application/json" -Body $associacaoBody -UseBasicParsing
    Write-Ok "Associar rotinas ao admin" "status=$($associacaoResp.StatusCode), total=$($rotinasSelecionadas.Count)"

    $contaPagar = New-Conta -Descricao "Conta Pagar $stamp" -Valor 200.00 -DataVencimento ($today.AddDays(7)) -CategoriaId ([long]$categoria.id) -BaseUrl $BaseUrl
    Write-Ok "Criar conta para pagar" "id=$($contaPagar.id)"

    $pagarBody = To-JsonBody @{ dataPagamento = $today.ToString("yyyy-MM-dd") }
    $pagarResp = Invoke-WebRequest -Uri "$BaseUrl/api/contas/$($contaPagar.id)/pagar" -Method Post -ContentType "application/json" -Body $pagarBody -UseBasicParsing
    Write-Ok "Pagar conta" "status=$($pagarResp.StatusCode)"

    $contaParcial = New-Conta -Descricao "Conta Parcial $stamp" -Valor 500.00 -DataVencimento ($today.AddDays(8)) -CategoriaId ([long]$categoria.id) -BaseUrl $BaseUrl
    Write-Ok "Criar conta para pagamento parcial" "id=$($contaParcial.id)"

    $parcialBody = To-JsonBody @{ valorPago = 150.00; dataPagamento = $today.ToString("yyyy-MM-dd"); dataProximoPagamento = $today.AddDays(15).ToString("yyyy-MM-dd") }
    $parcialResp = Invoke-RestMethod -Uri "$BaseUrl/api/contas/$($contaParcial.id)/pagamento-parcial" -Method Post -ContentType "application/json" -Body $parcialBody
    Write-Ok "Registrar pagamento parcial" "contaOriginal=$($contaParcial.id), novaConta=$($parcialResp.id)"

    $contaReagendar = New-Conta -Descricao "Conta Reagendar $stamp" -Valor 300.00 -DataVencimento ($today.AddDays(3)) -CategoriaId ([long]$categoria.id) -BaseUrl $BaseUrl
    Write-Ok "Criar conta para reagendar" "id=$($contaReagendar.id)"

    $reagendarBody = To-JsonBody @{ novaData = $today.AddDays(20).ToString("yyyy-MM-dd"); motivo = "Ajuste de fluxo"; usuario = "admin" }
    $reagendarResp = Invoke-WebRequest -Uri "$BaseUrl/api/contas/$($contaReagendar.id)/reagendar" -Method Post -ContentType "application/json" -Body $reagendarBody -UseBasicParsing
    Write-Ok "Reagendar conta" "status=$($reagendarResp.StatusCode)"

    $contaCancelar = New-Conta -Descricao "Conta Cancelar $stamp" -Valor 180.00 -DataVencimento ($today.AddDays(4)) -CategoriaId ([long]$categoria.id) -BaseUrl $BaseUrl
    Write-Ok "Criar conta para cancelar" "id=$($contaCancelar.id)"

    $cancelarBody = To-JsonBody @{ motivo = "Cancelamento de teste HML" }
    $cancelarResp = Invoke-WebRequest -Uri "$BaseUrl/api/contas/$($contaCancelar.id)/cancelar" -Method Post -ContentType "application/json" -Body $cancelarBody -UseBasicParsing
    Write-Ok "Cancelar conta" "status=$($cancelarResp.StatusCode)"

    $contaAtraso = New-Conta -Descricao "Conta Atraso $stamp" -Valor 130.00 -DataVencimento ($today.AddDays(-2)) -CategoriaId ([long]$categoria.id) -BaseUrl $BaseUrl
    Write-Ok "Criar conta vencida para atraso" "id=$($contaAtraso.id)"

    $atrasoBody = To-JsonBody @{ referencia = $today.ToString("yyyy-MM-dd") }
    $atrasoResp = Invoke-RestMethod -Uri "$BaseUrl/api/contas/marcar-atraso" -Method Post -ContentType "application/json" -Body $atrasoBody
    Write-Ok "Marcar contas em atraso" "totalAtualizadas=$($atrasoResp.total)"

    Write-Ok "RESULTADO_FINAL" "Fluxo funcional completo validado em HML"
    exit 0
}
catch {
    Write-Error "[ERRO] VALIDACAO_FUNCIONAL :: $($_.Exception.Message)"

    if ($_.ErrorDetails -and $_.ErrorDetails.Message) {
        Write-Error $_.ErrorDetails.Message
    }

    exit 1
}
