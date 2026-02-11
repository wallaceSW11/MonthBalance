# 🔐 WebAuthn Implementation - Backend Specification

## 📋 Overview

Implementar autenticação biométrica (Face ID / Touch ID) usando **WebAuthn** (Web Authentication API).

**Objetivo:** Permitir que usuários façam login usando biometria ao invés de senha.

---

## 🎯 Fluxo Completo

### 1. Registro de Credencial (Setup Inicial)

```
User → Frontend → Backend → Frontend → User
1. Usuário clica "Ativar Face ID"
2. Frontend solicita challenge ao backend
3. Backend gera challenge + retorna
4. Frontend chama navigator.credentials.create()
5. iOS mostra Face ID
6. Frontend envia credencial pública ao backend
7. Backend salva credencial no banco
```

### 2. Autenticação com Biometria

```
User → Frontend → Backend → Frontend → User
1. Usuário minimiza app (PWA)
2. Usuário volta pro app
3. Frontend detecta que precisa re-autenticar
4. Frontend solicita challenge ao backend
5. Backend gera challenge + retorna
6. Frontend chama navigator.credentials.get()
7. iOS mostra Face ID
8. Frontend envia assinatura ao backend
9. Backend valida assinatura
10. Backend retorna token JWT
11. Frontend marca como autenticado
```

---

## 🗄️ Database Schema

### Nova Tabela: `webauthn_credentials`

```sql
CREATE TABLE webauthn_credentials (
  id SERIAL PRIMARY KEY,
  user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
  credential_id TEXT NOT NULL UNIQUE,
  public_key TEXT NOT NULL,
  counter BIGINT NOT NULL DEFAULT 0,
  transports TEXT[], -- ['internal', 'usb', 'nfc', 'ble']
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  last_used_at TIMESTAMP,
  
  CONSTRAINT fk_user FOREIGN KEY (user_id) REFERENCES users(id)
);

CREATE INDEX idx_webauthn_user_id ON webauthn_credentials(user_id);
CREATE INDEX idx_webauthn_credential_id ON webauthn_credentials(credential_id);
```

**Campos:**
- `credential_id`: ID único da credencial (gerado pelo device)
- `public_key`: Chave pública (formato base64)
- `counter`: Contador de uso (previne replay attacks)
- `transports`: Como o device se comunica (iOS = 'internal')

---

## 🔌 API Endpoints

### 1. POST `/auth/webauthn/register/challenge`

Gerar challenge para registro de credencial.

**Auth:** ✅ Bearer Token (usuário já logado)

**Request Body:**
```json
{
  "userId": 1
}
```

**Response:** `200 OK`
```json
{
  "challenge": "base64_encoded_random_bytes",
  "userId": "base64_encoded_user_id",
  "rpId": "localhost",
  "rpName": "Month Balance",
  "timeout": 60000
}
```

**Lógica:**
1. Gerar 32 bytes aleatórios (challenge)
2. Salvar challenge em cache/sessão (expira em 5 min)
3. Retornar challenge + dados do RP (Relying Party)

---

### 2. POST `/auth/webauthn/register`

Salvar credencial pública do usuário.

**Auth:** ✅ Bearer Token

**Request Body:**
```json
{
  "credentialId": "base64_credential_id",
  "publicKey": "base64_public_key",
  "transports": ["internal"],
  "counter": 0
}
```

**Response:** `201 Created`
```json
{
  "success": true,
  "message": "Biometric authentication enabled"
}
```

**Lógica:**
1. Validar challenge (deve existir e não estar expirado)
2. Verificar se credentialId já existe (evitar duplicatas)
3. Salvar credencial no banco
4. Limpar challenge do cache

**Validações:**
- Challenge válido
- Usuário autenticado
- CredentialId único

---

### 3. POST `/auth/webauthn/authenticate/challenge`

Gerar challenge para autenticação.

**Auth:** ❌ Não requer (usuário ainda não autenticou)

**Request Body:**
```json
{
  "email": "user@example.com"
}
```

**Response:** `200 OK`
```json
{
  "challenge": "base64_encoded_random_bytes",
  "allowCredentials": [
    {
      "id": "base64_credential_id",
      "type": "public-key",
      "transports": ["internal"]
    }
  ],
  "timeout": 60000,
  "rpId": "localhost"
}
```

**Lógica:**
1. Buscar usuário por email
2. Buscar credenciais WebAuthn do usuário
3. Gerar challenge
4. Salvar challenge em cache (expira em 5 min)
5. Retornar challenge + lista de credenciais

---

### 4. POST `/auth/webauthn/authenticate`

Validar assinatura e autenticar usuário.

**Auth:** ❌ Não requer

**Request Body:**
```json
{
  "credentialId": "base64_credential_id",
  "authenticatorData": "base64_authenticator_data",
  "clientDataJSON": "base64_client_data",
  "signature": "base64_signature"
}
```

**Response:** `200 OK`
```json
{
  "token": "jwt_token",
  "user": {
    "id": 1,
    "name": "João Silva",
    "email": "joao@example.com",
    "avatar": null,
    "notificationsEnabled": true
  }
}
```

**Lógica:**
1. Buscar credencial no banco
2. Validar challenge (deve existir e não estar expirado)
3. Validar assinatura usando chave pública
4. Verificar counter (deve ser maior que o anterior)
5. Atualizar counter no banco
6. Gerar token JWT
7. Retornar token + dados do usuário

**Validações:**
- Challenge válido
- Credencial existe
- Assinatura válida
- Counter válido (previne replay)

---

## 🔐 Segurança

### Challenge
- **Tamanho:** 32 bytes (256 bits)
- **Formato:** Base64
- **Expiração:** 5 minutos
- **Uso único:** Após validação, deve ser deletado

### Counter
- **Propósito:** Prevenir replay attacks
- **Validação:** Novo counter DEVE ser maior que o anterior
- **Ação:** Se counter for menor/igual → rejeitar + alertar

### Public Key
- **Formato:** Base64
- **Algoritmo:** ES256 (ECDSA P-256) ou RS256 (RSA 2048)
- **Storage:** Banco de dados (não expor)

---

## 📦 Bibliotecas Recomendadas

### Node.js
```bash
npm install @simplewebauthn/server
```

**Exemplo:**
```typescript
import {
  generateRegistrationOptions,
  verifyRegistrationResponse,
  generateAuthenticationOptions,
  verifyAuthenticationResponse
} from '@simplewebauthn/server';
```

### Python
```bash
pip install webauthn
```

### Java
```xml
<dependency>
  <groupId>com.webauthn4j</groupId>
  <artifactId>webauthn4j-core</artifactId>
</dependency>
```

---

## 🧪 Testes

### Registro
1. Usuário logado solicita challenge
2. Challenge é gerado e salvo
3. Frontend envia credencial
4. Credencial é salva no banco
5. Verificar: credencial existe, counter = 0

### Autenticação
1. Usuário solicita challenge (com email)
2. Challenge é gerado
3. Frontend envia assinatura
4. Assinatura é validada
5. Token JWT é retornado
6. Verificar: counter incrementado

### Segurança
1. Challenge expirado → rejeitar
2. Challenge usado 2x → rejeitar
3. Counter menor → rejeitar
4. Assinatura inválida → rejeitar
5. Credencial inexistente → rejeitar

---

## 🚀 Deployment

### Requisitos
- **HTTPS obrigatório** (exceto localhost)
- **rpId:** Deve ser o domínio (ex: `monthbalance.com`)
- **origin:** Deve ser a URL completa (ex: `https://monthbalance.com`)

### Configuração
```env
WEBAUTHN_RP_ID=monthbalance.com
WEBAUTHN_RP_NAME=Month Balance
WEBAUTHN_ORIGIN=https://monthbalance.com
```

---

## 📝 Notas Importantes

1. **Usuário pode ter múltiplas credenciais** (iPhone + iPad)
2. **Credencial é device-specific** (não transferível)
3. **Backup:** Usuário DEVE poder usar senha também
4. **Revogação:** Permitir deletar credenciais antigas
5. **iOS PWA:** Funciona perfeitamente no Safari standalone

---

## 🔄 Fluxo de Migração

### Fase 1: Backend
1. Criar tabela `webauthn_credentials`
2. Implementar 4 endpoints
3. Testar com Postman/Insomnia

### Fase 2: Frontend
1. Descomentar TODOs em `authGuard.ts`
2. Criar tela de setup (ativar biometria)
3. Integrar com login

### Fase 3: UX
1. Mostrar opção "Ativar Face ID" na tela de conta
2. Ao voltar do background → mostrar Face ID
3. Fallback para senha se biometria falhar

---

## 📚 Referências

- [WebAuthn Spec (W3C)](https://www.w3.org/TR/webauthn-2/)
- [SimpleWebAuthn Docs](https://simplewebauthn.dev/)
- [MDN: Web Authentication API](https://developer.mozilla.org/en-US/docs/Web/API/Web_Authentication_API)
- [iOS Safari WebAuthn Support](https://webkit.org/blog/11312/meet-face-id-and-touch-id-for-the-web/)

---

**Versão:** 1.0  
**Data:** 06/02/2026  
**Autor:** Month Balance Team
