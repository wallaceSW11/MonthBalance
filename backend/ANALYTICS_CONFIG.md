# 📊 Configuração de Analytics - MonthBalance

## 🎯 Visão Geral

O sistema de analytics pode ser configurado para rastrear apenas o essencial ou capturar dados detalhados de uso.

---

## ⚙️ Configuração

### appsettings.json / appsettings.Production.json

```json
{
  "Analytics": {
    "EnableDetailedTracking": false
  }
}
```

- **`false`** (Padrão): Tracking minimalista - apenas eventos críticos
- **`true`**: Tracking completo - todas as ações do usuário

---

## 📋 O que é Rastreado

### 🟢 SEMPRE Rastreado (independente da configuração)

Eventos críticos que você precisa para medir aceitação:

1. **UserRegistered** - Quando um usuário cria conta
2. **UserLogin** - Quando um usuário faz login
3. **FeedbackSent** - Quando um usuário envia feedback

**Tabelas usadas:**
- `UserSessions` - Logins/logouts (sempre ativo)
- `UserActivity` - Apenas os 3 eventos acima

**Dados capturados:**
- UserId
- Timestamp
- IP Address (opcional)
- User Agent (opcional)

---

### 🔵 Rastreado APENAS se `EnableDetailedTracking = true`

Ações detalhadas do usuário:

**MonthData:**
- MonthDataCreated
- MonthDataViewed
- MonthDataUpdated
- MonthDataDeleted

**Receitas:**
- IncomeCreated
- IncomeUpdated
- IncomeDeleted
- IncomeTypeCreated
- IncomeTypeUpdated
- IncomeTypeDeleted

**Despesas:**
- ExpenseCreated
- ExpenseUpdated
- ExpenseDeleted
- ExpenseTypeCreated
- ExpenseTypeUpdated
- ExpenseTypeDeleted

**Outros:**
- PasswordChanged
- PasswordResetRequested
- PasswordResetCompleted
- AdminPanelAccessed

---

## 📊 Impacto no Banco de Dados

### Cenário: 100 usuários ativos/dia

#### Com `EnableDetailedTracking = false` (Padrão)

**Registros por dia:**
- ~2 logins por usuário = 200 logins/dia
- ~1 registro por usuário = 100 registros/dia
- **Total: ~300 registros/dia**

**Por mês:** ~9.000 registros  
**Por ano:** ~108.000 registros

#### Com `EnableDetailedTracking = true`

**Registros por dia:**
- ~50 ações por usuário = 5.000 ações/dia
- ~2 logins por usuário = 200 logins/dia
- **Total: ~5.200 registros/dia**

**Por mês:** ~156.000 registros  
**Por ano:** ~1.872.000 registros

**Diferença:** 17x mais registros com tracking detalhado!

---

## 🎯 Métricas Disponíveis no Dashboard

### Com Tracking Minimalista (Padrão)

Você consegue ver:
- ✅ Total de usuários cadastrados
- ✅ Novos usuários (hoje/semana/mês)
- ✅ Usuários ativos (hoje/semana/mês) - baseado em logins
- ✅ Último acesso de cada usuário
- ✅ Total de logins de cada usuário
- ✅ Status ativo/inativo (logou nos últimos 7 dias)
- ✅ Quantidade de feedbacks recebidos

### Com Tracking Detalhado

Além do acima, você consegue ver:
- ✅ Quais features são mais usadas
- ✅ Quantas receitas/despesas são criadas
- ✅ Quantos tipos personalizados são criados
- ✅ Frequência de uso de cada feature
- ✅ Padrões de comportamento detalhados

---

## 🚀 Como Ativar/Desativar

### Desenvolvimento Local

Edite `backend/appsettings.json`:

```json
{
  "Analytics": {
    "EnableDetailedTracking": true  // ou false
  }
}
```

Reinicie o backend:
```bash
dotnet run
```

### Produção (Docker)

Edite `backend/appsettings.Production.json`:

```json
{
  "Analytics": {
    "EnableDetailedTracking": true  // ou false
  }
}
```

Rebuild e redeploy:
```bash
docker-compose down
docker-compose up -d --build
```

Ou via variável de ambiente no `docker-compose.yml`:
```yaml
backend:
  environment:
    - Analytics__EnableDetailedTracking=false
```

---

## 💡 Recomendações

### Fase Inicial (0-1000 usuários)
**Configuração:** `EnableDetailedTracking = false`

**Por quê:**
- Banco leve e rápido
- Você tem os dados essenciais
- Foco em crescimento, não em análise detalhada
- Economia de recursos

**Você consegue responder:**
- ✅ Quantos usuários tenho?
- ✅ Quantos estão usando ativamente?
- ✅ O app está sendo bem aceito?
- ✅ As pessoas estão voltando?

### Fase de Crescimento (1000+ usuários)
**Configuração:** `EnableDetailedTracking = true`

**Por quê:**
- Você quer otimizar o produto
- Precisa entender quais features usar
- Quer identificar gargalos
- Tem recursos para processar mais dados

**Você consegue responder:**
- ✅ Quais features são mais usadas?
- ✅ Onde os usuários passam mais tempo?
- ✅ Quais features são ignoradas?
- ✅ Como melhorar a experiência?

---

## 🔍 Verificando o Status

### Via Logs

Quando o tracking detalhado está desabilitado, você verá menos logs de "tracking activity".

### Via Banco de Dados

```sql
-- Ver quantos registros de atividade você tem
SELECT COUNT(*) FROM "UserActivities";

-- Ver tipos de atividade registrados
SELECT "ActivityType", COUNT(*) as Total
FROM "UserActivities"
GROUP BY "ActivityType"
ORDER BY Total DESC;

-- Ver registros de hoje
SELECT COUNT(*) FROM "UserActivities"
WHERE "Timestamp" >= CURRENT_DATE;
```

---

## 🧹 Limpeza de Dados Antigos (Opcional)

Se você ativar o tracking detalhado e depois quiser limpar dados antigos:

```sql
-- Deletar atividades antigas (mantém últimos 90 dias)
DELETE FROM "UserActivities"
WHERE "Timestamp" < NOW() - INTERVAL '90 days'
AND "ActivityType" NOT IN ('UserRegistered', 'UserLogin', 'FeedbackSent');

-- Deletar sessões antigas (mantém últimos 180 dias)
DELETE FROM "UserSessions"
WHERE "LoginAt" < NOW() - INTERVAL '180 days';
```

---

## ⚠️ Importante

### O que SEMPRE é mantido:
- ✅ Tabela `Users` (dados dos usuários)
- ✅ Tabela `UserSessions` (logins/logouts)
- ✅ Eventos críticos (registro, login, feedback)

### O que pode ser desabilitado:
- ⚙️ Tracking detalhado de ações (criar/editar/deletar)
- ⚙️ Middleware automático de tracking

### O que NUNCA é rastreado:
- ❌ Valores de receitas/despesas
- ❌ Nomes de receitas/despesas
- ❌ Dados financeiros específicos
- ❌ Conteúdo privado do usuário

---

## 🎉 Resumo

**Configuração Atual (Padrão):**
```json
"EnableDetailedTracking": false
```

**Você tem:**
- ✅ Dados essenciais para medir aceitação
- ✅ Banco leve e performático
- ✅ Possibilidade de ativar no futuro
- ✅ Nada foi perdido, só desabilitado

**Quando quiser mais dados:**
- Mude para `true`
- Reinicie o backend
- Pronto! Tracking detalhado ativo

---

**Versão:** 1.0  
**Data:** 12/02/2026
