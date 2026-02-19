# MonthBalance - Analytics, Email e Painel Admin

## 📋 Visão Geral

Implementação de sistema de analytics, notificações por email e painel administrativo para monitorar a aceitação e uso do MonthBalance em produção.

---

## 🎯 Objetivos

1. **Analytics**: Rastrear comportamento dos usuários e métricas de engajamento
2. **Email**: Sistema de recuperação de senha e feedback
3. **Admin Panel**: Dashboard para visualizar métricas e gerenciar o app

---

## 📊 FASE 1: Sistema de Analytics

### 1.1 Modelos de Dados

#### UserActivity (Tabela de Atividades)
```csharp
- Id (int, PK)
- UserId (int, FK -> Users)
- ActivityType (enum: Login, Logout, CreateMonthData, CreateIncome, CreateExpense, etc)
- Timestamp (DateTime)
- IpAddress (string, nullable)
- UserAgent (string, nullable)
- Metadata (string, JSON, nullable) // dados extras específicos da ação
```

#### UserSession (Tabela de Sessões)
```csharp
- Id (int, PK)
- UserId (int, FK -> Users)
- LoginAt (DateTime)
- LogoutAt (DateTime, nullable)
- IpAddress (string, nullable)
- UserAgent (string, nullable)
- IsActive (bool)
```

#### UserRetention (View/Query calculada)
- Não precisa de tabela, calculamos via queries
- Métricas: DAU, WAU, MAU, retention rate

### 1.2 Enum ActivityType
```csharp
public enum ActivityType
{
    // Auth
    UserRegistered,
    UserLogin,
    UserLogout,
    PasswordChanged,
    PasswordResetRequested,
    PasswordResetCompleted,
    
    // MonthData
    MonthDataCreated,
    MonthDataViewed,
    MonthDataUpdated,
    
    // Income
    IncomeCreated,
    IncomeUpdated,
    IncomeDeleted,
    IncomeTypeCreated,
    
    // Expense
    ExpenseCreated,
    ExpenseUpdated,
    ExpenseDeleted,
    ExpenseTypeCreated,
    
    // Feedback
    FeedbackSent,
    
    // Admin
    AdminPanelAccessed
}
```

### 1.3 Middleware de Tracking
- Criar middleware para capturar automaticamente ações importantes
- Registrar IP e UserAgent de cada requisição autenticada
- Não bloquear a request se o log falhar (fire and forget)

### 1.4 Repositories e Services
- `IActivityRepository` / `ActivityRepository`
- `ISessionRepository` / `SessionRepository`
- `IAnalyticsService` / `AnalyticsService` (para queries agregadas)

---

## 📧 FASE 2: Sistema de Email

### 2.1 Configuração SMTP (Gmail)

#### appsettings.json / .env
```json
{
  "Email": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "UseSsl": true,
    "FromEmail": "walltech@gmail.com",
    "FromName": "MonthBalance",
    "AdminEmail": "walltech@gmail.com"
  }
}
```

#### .env (credenciais sensíveis)
```
EMAIL_USERNAME=walltech@gmail.com
EMAIL_PASSWORD=sua_senha_de_app_aqui
```

**Importante**: Usar "Senha de App" do Google, não a senha normal da conta.

### 2.2 Modelos de Email

#### PasswordResetToken
```csharp
- Id (int, PK)
- UserId (int, FK -> Users)
- Token (string, unique)
- ExpiresAt (DateTime)
- IsUsed (bool)
- CreatedAt (DateTime)
```

#### UserFeedback
```csharp
- Id (int, PK)
- UserId (int, FK -> Users, nullable) // pode ser anônimo
- Email (string)
- Subject (string)
- Message (string)
- Rating (int, nullable, 1-5)
- CreatedAt (DateTime)
- IsRead (bool)
- AdminNotes (string, nullable)
```

### 2.3 Templates de Email

#### Recuperação de Senha
- Assunto: "MonthBalance - Recuperação de Senha"
- Link com token válido por 1 hora
- Template HTML simples e responsivo

#### Feedback para Admin
- Assunto: "Novo Feedback - MonthBalance"
- Informações do usuário
- Conteúdo do feedback
- Link para responder (opcional)

#### Confirmação de Feedback (para usuário)
- Assunto: "Recebemos seu feedback!"
- Agradecimento
- Expectativa de resposta

### 2.4 Services
- `IEmailService` / `EmailService`
- `IPasswordResetService` / `PasswordResetService`
- `IFeedbackService` / `FeedbackService`

### 2.5 Endpoints

#### POST /api/auth/forgot-password
```json
Request: { "email": "user@example.com" }
Response: { "message": "Email enviado se o usuário existir" }
```

#### POST /api/auth/reset-password
```json
Request: { 
  "token": "abc123",
  "newPassword": "newpass123"
}
Response: { "message": "Senha alterada com sucesso" }
```

#### POST /api/feedback
```json
Request: {
  "subject": "Sugestão de melhoria",
  "message": "Seria legal ter...",
  "rating": 5
}
Response: { "message": "Feedback enviado com sucesso" }
```

---

## 👨‍💼 FASE 3: Painel Administrativo

### 3.1 Sistema de Roles

#### Atualizar Model User
```csharp
public enum UserRole
{
    User,
    Admin
}

// Adicionar ao User:
public UserRole Role { get; set; } = UserRole.User;
```

#### Atualizar JWT Claims
- Incluir claim "role" no token
- Criar atributo `[Authorize(Roles = "Admin")]` nos endpoints admin

### 3.2 Endpoints Admin (Backend)

#### GET /api/admin/dashboard
```json
Response: {
  "totalUsers": 150,
  "activeUsersToday": 45,
  "activeUsersWeek": 89,
  "activeUsersMonth": 120,
  "newUsersToday": 3,
  "newUsersWeek": 12,
  "newUsersMonth": 35,
  "totalMonthDataCreated": 450,
  "totalIncomes": 1200,
  "totalExpenses": 3400,
  "retentionRate": {
    "day1": 0.85,
    "day7": 0.65,
    "day30": 0.45
  },
  "topActivities": [
    { "activity": "ExpenseCreated", "count": 2500 },
    { "activity": "IncomeCreated", "count": 800 }
  ]
}
```

#### GET /api/admin/users
```json
Query params: ?page=1&pageSize=20&search=email
Response: {
  "users": [
    {
      "id": 1,
      "name": "João Silva",
      "email": "joao@example.com",
      "createdAt": "2026-01-15T10:30:00Z",
      "lastLoginAt": "2026-02-12T08:15:00Z",
      "totalMonthData": 5,
      "isActive": true
    }
  ],
  "totalCount": 150,
  "page": 1,
  "pageSize": 20
}
```

#### GET /api/admin/activities
```json
Query params: ?userId=1&startDate=2026-02-01&endDate=2026-02-12&activityType=Login
Response: {
  "activities": [
    {
      "id": 1,
      "userId": 1,
      "userName": "João Silva",
      "activityType": "Login",
      "timestamp": "2026-02-12T08:15:00Z",
      "ipAddress": "192.168.1.1"
    }
  ],
  "totalCount": 500
}
```

#### GET /api/admin/feedback
```json
Query params: ?isRead=false&page=1
Response: {
  "feedbacks": [
    {
      "id": 1,
      "userName": "João Silva",
      "email": "joao@example.com",
      "subject": "Sugestão",
      "message": "Seria legal ter...",
      "rating": 5,
      "createdAt": "2026-02-12T10:00:00Z",
      "isRead": false
    }
  ],
  "totalCount": 25
}
```

#### PUT /api/admin/feedback/{id}/mark-read
```json
Response: { "message": "Feedback marcado como lido" }
```

### 3.3 Frontend - Rota /admin

#### Estrutura de Páginas
```
/admin
  /admin/dashboard          -> Métricas gerais
  /admin/users              -> Lista de usuários
  /admin/activities         -> Log de atividades
  /admin/feedback           -> Feedbacks recebidos
```

#### Proteção de Rota
- Verificar role "Admin" no token JWT
- Redirect para home se não for admin
- Componente `<AdminRoute>` ou guard

#### Dashboard - Componentes
1. **Cards de Métricas**
   - Total de usuários
   - Usuários ativos (hoje/semana/mês)
   - Novos usuários (hoje/semana/mês)
   - Taxa de retenção

2. **Gráficos**
   - Linha: Novos usuários por dia (últimos 30 dias)
   - Barra: Atividades mais comuns
   - Pizza: Distribuição de tipos de despesas/receitas

3. **Tabela de Atividades Recentes**
   - Últimas 10 atividades
   - Link para ver todas

4. **Feedbacks Não Lidos**
   - Badge com contador
   - Link para página de feedbacks

#### Segurança no Frontend
- Não mostrar link/menu admin para usuários normais
- Validar role no componente
- Não confiar apenas no frontend (backend valida tudo)

---

## 🔐 Segurança

### Backend
- Todos endpoints `/api/admin/*` com `[Authorize(Roles = "Admin")]`
- Validar role no JWT em cada request
- Rate limiting em endpoints sensíveis
- Logs de todas ações admin

### Email
- Tokens de reset com expiração (1 hora)
- Token usado apenas uma vez
- Não revelar se email existe ou não (segurança)
- Validar força da nova senha

### Admin
- Apenas você terá role Admin (definir manualmente no banco inicialmente)
- Considerar 2FA no futuro se necessário
- Logs de acesso ao painel admin

---

## 📝 Checklist de Implementação

### Backend

#### Analytics
- [ ] Criar models: `UserActivity`, `UserSession`
- [ ] Criar enum `ActivityType`
- [ ] Criar migration
- [ ] Criar repositories e interfaces
- [ ] Criar `AnalyticsService` com queries agregadas
- [ ] Criar middleware de tracking
- [ ] Registrar atividades nos controllers existentes
- [ ] Testar tracking de atividades

#### Email
- [ ] Adicionar pacote NuGet: `MailKit` ou `System.Net.Mail`
- [ ] Criar models: `PasswordResetToken`, `UserFeedback`
- [ ] Criar migration
- [ ] Configurar SMTP no appsettings
- [ ] Criar `EmailService`
- [ ] Criar templates HTML de email
- [ ] Criar `PasswordResetService`
- [ ] Criar `FeedbackService`
- [ ] Criar endpoints de forgot/reset password
- [ ] Criar endpoint de feedback
- [ ] Testar envio de emails

#### Admin
- [ ] Adicionar `Role` ao model `User`
- [ ] Criar migration para adicionar coluna Role
- [ ] Atualizar `AuthService` para incluir role no JWT
- [ ] Criar `AdminController` com endpoints
- [ ] Implementar queries de analytics no `AnalyticsService`
- [ ] Testar todos endpoints admin
- [ ] Definir seu usuário como Admin no banco

### Frontend

#### Feedback
- [ ] Criar página/modal de feedback
- [ ] Integrar com endpoint de feedback
- [ ] Adicionar link no menu/footer

#### Recuperação de Senha
- [ ] Criar página "Esqueci minha senha"
- [ ] Criar página "Redefinir senha" (com token)
- [ ] Integrar com endpoints

#### Admin Panel
- [ ] Criar guard/proteção de rota admin
- [ ] Criar layout admin
- [ ] Criar página Dashboard
- [ ] Criar componentes de métricas/cards
- [ ] Criar gráficos (usar lib como recharts/chart.js)
- [ ] Criar página de usuários
- [ ] Criar página de atividades
- [ ] Criar página de feedbacks
- [ ] Adicionar menu admin (apenas para admins)
- [ ] Testar todas funcionalidades

---

## 🚀 Ordem de Implementação Sugerida

1. **Analytics Backend** (base para tudo)
2. **Email Backend** (recuperação de senha é importante)
3. **Admin Backend** (endpoints para consumir)
4. **Recuperação de Senha Frontend** (feature para usuários)
5. **Feedback Frontend** (feature para usuários)
6. **Admin Panel Frontend** (seu painel)

---

## 📌 Notas Importantes

- **Gmail SMTP**: Precisa habilitar "Senhas de app" na conta Google
- **Role Admin**: Definir manualmente no banco após criar sua conta
- **Middleware**: Não deve bloquear requests se logging falhar
- **Performance**: Considerar índices nas tabelas de log
- **Retenção de Dados**: Definir política de limpeza de logs antigos (opcional)
- **AWS SES**: Quando migrar, apenas trocar configuração SMTP

---

## 🎨 Sugestões de UI/UX

### Dashboard Admin
- Cards coloridos para métricas principais
- Gráficos interativos
- Filtros de data
- Export de dados (CSV) - futuro

### Feedback
- Rating com estrelas
- Campo de texto amplo
- Confirmação visual após envio
- Opcional: categorias de feedback

### Recuperação de Senha
- Validação de email em tempo real
- Feedback claro sobre email enviado
- Timer de expiração visível
- Validação de força da senha

---

Pronto para começar a implementação! 🚀
