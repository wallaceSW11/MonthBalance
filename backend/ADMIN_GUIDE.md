# 👨‍💼 Guia do Painel Admin - MonthBalance

## 🔐 Como se Tornar Admin

### 1. Crie sua conta normalmente pelo app

Acesse o frontend e registre-se com seu email.

### 2. Conecte no banco de dados via SSH Tunnel

```bash
ssh -i sua-chave.pem -L 5432:localhost:5432 ec2-user@seu-ip-ec2
```

### 3. Abra o Azure Data Studio ou pgAdmin

Conecte em:
- Host: `localhost`
- Port: `5432`
- Database: `monthbalance`
- Username: `postgres`
- Password: (a senha do seu .env)

### 4. Execute o SQL para se tornar Admin

```sql
UPDATE "Users" 
SET "Role" = 1  -- 1 = Admin, 0 = User
WHERE "Email" = 'seu_email@walltech.com.br';
```

### 5. Faça logout e login novamente

O novo token JWT terá a role "Admin" e você terá acesso ao painel.

---

## 📊 Endpoints do Painel Admin

Todos os endpoints requerem autenticação com role "Admin".

### 1. Dashboard (Visão Geral)

```http
GET /api/admin/dashboard
Authorization: Bearer seu_token_jwt
```

**Response:**
```json
{
  "totalUsers": 150,
  "newUsersToday": 3,
  "newUsersThisWeek": 12,
  "newUsersThisMonth": 35,
  "activeUsersToday": 45,
  "activeUsersThisWeek": 89,
  "activeUsersThisMonth": 120,
  "unreadFeedbacks": 5,
  "recentUsers": [
    {
      "id": 1,
      "name": "João Silva",
      "email": "joao@example.com",
      "createdAt": "2026-02-12T10:30:00Z",
      "lastLoginAt": "2026-02-12T14:20:00Z",
      "totalLogins": 15,
      "isActive": true
    }
  ]
}
```

### 2. Lista de Usuários

```http
GET /api/admin/users?search=joao&page=1&pageSize=20
Authorization: Bearer seu_token_jwt
```

**Query Parameters:**
- `search` (opcional): Busca por nome ou email
- `page` (opcional): Número da página (padrão: 1)
- `pageSize` (opcional): Itens por página (padrão: 20)

**Response:**
```json
{
  "users": [
    {
      "id": 1,
      "name": "João Silva",
      "email": "joao@example.com",
      "createdAt": "2026-01-15T10:30:00Z",
      "lastLoginAt": "2026-02-12T14:20:00Z",
      "totalLogins": 15,
      "isActive": true
    },
    {
      "id": 2,
      "name": "Maria Santos",
      "email": "maria@example.com",
      "createdAt": "2026-01-20T08:15:00Z",
      "lastLoginAt": "2026-02-10T09:30:00Z",
      "totalLogins": 8,
      "isActive": true
    }
  ],
  "totalCount": 150,
  "page": 1,
  "pageSize": 20
}
```

### 3. Detalhes de um Usuário

```http
GET /api/admin/users/1
Authorization: Bearer seu_token_jwt
```

**Response:**
```json
{
  "id": 1,
  "name": "João Silva",
  "email": "joao@example.com",
  "createdAt": "2026-01-15T10:30:00Z",
  "lastLoginAt": "2026-02-12T14:20:00Z",
  "totalLogins": 15,
  "isActive": true
}
```

### 4. Lista de Feedbacks

```http
GET /api/feedback?isRead=false&page=1&pageSize=20
Authorization: Bearer seu_token_jwt
```

**Query Parameters:**
- `isRead` (opcional): `true` para lidos, `false` para não lidos
- `page` (opcional): Número da página
- `pageSize` (opcional): Itens por página

**Response:**
```json
[
  {
    "id": 1,
    "userId": 5,
    "userName": "João Silva",
    "email": "joao@example.com",
    "subject": "Sugestão de melhoria",
    "message": "Seria legal ter...",
    "rating": 5,
    "createdAt": "2026-02-12T10:00:00Z",
    "isRead": false,
    "adminNotes": null
  }
]
```

### 5. Marcar Feedback como Lido

```http
PUT /api/feedback/1/mark-read
Authorization: Bearer seu_token_jwt
```

**Response:** `204 No Content`

### 6. Contador de Feedbacks Não Lidos

```http
GET /api/feedback/unread-count
Authorization: Bearer seu_token_jwt
```

**Response:**
```json
{
  "count": 5
}
```

---

## 📈 Métricas Explicadas

### Usuários
- **Total de Usuários**: Todos os usuários cadastrados
- **Novos Usuários (Hoje/Semana/Mês)**: Usuários criados no período
- **Usuários Ativos (Hoje/Semana/Mês)**: Usuários que fizeram login no período

### Atividade
- **Total de Logins**: Quantas vezes o usuário fez login
- **Último Acesso**: Data/hora do último login
- **Ativo**: Fez login nos últimos 7 dias

### Feedbacks
- **Não Lidos**: Feedbacks que ainda não foram visualizados

---

## 🎯 Casos de Uso

### Ver usuários mais ativos
```http
GET /api/admin/users?pageSize=50
```
Ordena por data de criação (mais recentes primeiro). Para ver os mais ativos, você pode filtrar no frontend pelo `totalLogins`.

### Buscar usuário específico
```http
GET /api/admin/users?search=joao@example.com
```

### Ver novos usuários da semana
No dashboard, veja `newUsersThisWeek` e `recentUsers`.

### Verificar feedbacks pendentes
```http
GET /api/feedback?isRead=false
```

---

## 🔒 Segurança

### Proteção de Rotas
Todos os endpoints `/api/admin/*` e alguns de `/api/feedback` são protegidos com:
```csharp
[Authorize(Roles = "Admin")]
```

### Validação no Frontend
O frontend deve:
1. Verificar a role no token JWT decodificado
2. Esconder menu/links admin para usuários normais
3. Redirecionar para home se tentar acessar sem permissão

### Token JWT
O token contém a claim:
```json
{
  "role": "Admin"
}
```

Decodifique no frontend para verificar.

---

## 🧪 Testando Localmente

### 1. Crie sua conta
```bash
POST http://localhost:5150/api/auth/register
{
  "name": "Wallace",
  "email": "wallace@walltech.com.br",
  "password": "senha123"
}
```

### 2. Torne-se Admin (SQL)
```sql
UPDATE "Users" SET "Role" = 1 WHERE "Email" = 'wallace@walltech.com.br';
```

### 3. Faça login novamente
```bash
POST http://localhost:5150/api/auth/login
{
  "email": "wallace@walltech.com.br",
  "password": "senha123"
}
```

### 4. Use o token para acessar o dashboard
```bash
GET http://localhost:5150/api/admin/dashboard
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## 📱 Próximos Passos (Frontend)

No frontend, você vai precisar:

1. **Rota `/admin`** protegida por role
2. **Dashboard** com cards de métricas
3. **Tabela de usuários** com busca e paginação
4. **Lista de feedbacks** com filtro lido/não lido
5. **Menu admin** visível apenas para admins

---

**Pronto!** Backend do painel admin completo e funcionando! 🎉
