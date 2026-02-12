# 📧 Configuração de Email - MonthBalance

## 🔑 Configurar Senha de App do Gmail

Para usar o Gmail SMTP, você precisa criar uma "Senha de App":

### Passo a Passo:

1. **Acesse sua Conta Google**: https://myaccount.google.com/

2. **Ative a Verificação em 2 Etapas** (se ainda não tiver):
   - Vá em "Segurança"
   - Clique em "Verificação em duas etapas"
   - Siga as instruções

3. **Crie uma Senha de App**:
   - Ainda em "Segurança"
   - Procure por "Senhas de app" (pode estar em "Como fazer login no Google")
   - Clique em "Senhas de app"
   - Selecione "Outro (nome personalizado)"
   - Digite: "MonthBalance Backend"
   - Clique em "Gerar"
   - **Copie a senha gerada** (16 caracteres sem espaços)

4. **Configure no .env**:
   ```env
   EMAIL_USERNAME=walltech@gmail.com
   EMAIL_PASSWORD=abcd efgh ijkl mnop  # Cole a senha gerada aqui (sem espaços)
   ```

## 🐳 Configuração no Docker (Produção)

No seu `docker-compose.yml` ou ao rodar o container, adicione as variáveis de ambiente:

```yaml
services:
  backend:
    environment:
      - EMAIL_USERNAME=walltech@gmail.com
      - EMAIL_PASSWORD=sua_senha_de_app_aqui
```

Ou via linha de comando:
```bash
docker run -e EMAIL_USERNAME=walltech@gmail.com -e EMAIL_PASSWORD=sua_senha ...
```

## ✅ Testar Envio de Email

### 1. Recuperação de Senha

```bash
# Request
POST http://localhost:5150/api/auth/forgot-password
Content-Type: application/json

{
  "email": "seu_email@example.com"
}

# Response
{
  "message": "Se o email existir, um link de recuperação será enviado"
}
```

Verifique sua caixa de entrada!

### 2. Feedback

```bash
# Request (autenticado)
POST http://localhost:5150/api/feedback
Authorization: Bearer seu_token_jwt
Content-Type: application/json

{
  "subject": "Teste de Feedback",
  "message": "Testando o sistema de feedback!",
  "rating": 5
}

# Response
{
  "id": 1,
  "userId": 1,
  "userName": "Seu Nome",
  "email": "seu_email@example.com",
  "subject": "Teste de Feedback",
  "message": "Testando o sistema de feedback!",
  "rating": 5,
  "createdAt": "2026-02-12T...",
  "isRead": false,
  "adminNotes": null
}
```

Você receberá 2 emails:
- Um para você (confirmação)
- Um para o admin (walltech@gmail.com)

## 🔧 Troubleshooting

### Erro: "Authentication failed"
- Verifique se a senha de app está correta
- Certifique-se que a verificação em 2 etapas está ativa
- Tente gerar uma nova senha de app

### Erro: "SMTP connection failed"
- Verifique se a porta 587 está aberta
- Confirme que UseSsl está como "true"
- Teste a conexão de rede

### Email não chega
- Verifique a pasta de SPAM
- Confirme que o email do destinatário está correto
- Veja os logs do backend para erros

## 📝 Logs

O backend loga todas as tentativas de envio de email:

```
[Information] Email sent successfully to user@example.com
[Error] Error sending email to user@example.com: ...
```

## 🚀 Migrar para AWS SES (Futuro)

Quando tiver retorno financeiro, migre para AWS SES:

1. Configure o SES na AWS
2. Verifique o domínio walltech.app.br
3. Atualize o appsettings.Production.json:
   ```json
   "Email": {
     "SmtpHost": "email-smtp.us-east-1.amazonaws.com",
     "SmtpPort": "587",
     "UseSsl": "true",
     "FromEmail": "noreply@walltech.app.br",
     "FromName": "MonthBalance",
     "AdminEmail": "wallace@walltech.app.br"
   }
   ```
4. Use as credenciais SMTP do SES no .env

## 📧 Templates de Email

Os templates estão em `Services/EmailService.cs`:

- **Recuperação de Senha**: Design roxo (#4F46E5)
- **Feedback para Admin**: Design verde (#059669)
- **Confirmação de Feedback**: Design verde (#059669)

Todos são responsivos e funcionam em qualquer cliente de email.

---

**Pronto!** Sistema de email configurado e funcionando! 🎉
