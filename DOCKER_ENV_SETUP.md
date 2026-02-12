# 🐳 Configuração de Variáveis de Ambiente - Docker

## 📋 Variáveis Necessárias

O docker-compose agora usa variáveis de ambiente do arquivo `.env` na raiz do projeto.

### Variáveis Obrigatórias

```env
# Database
DB_NAME=monthbalance
DB_USER=postgres
DB_PASSWORD=sua_senha_segura_aqui

# JWT (mínimo 32 caracteres)
JWT_SECRET=sua_chave_secreta_jwt_aqui_minimo_32_chars

# Email (SMTP Gmail)
EMAIL_USERNAME=walltechappbr@gmail.com
EMAIL_PASSWORD=sua_senha_de_app_do_gmail
```

---

## 🚀 Setup Local (Desenvolvimento)

### 1. Copie o arquivo de exemplo
```bash
cp .env.example .env
```

### 2. Edite o .env
```bash
nano .env
# ou
vim .env
```

### 3. Configure as variáveis
```env
DB_NAME=monthbalance
DB_USER=postgres
DB_PASSWORD=postgres123

JWT_SECRET=dev_secret_key_for_local_only_min_32_chars_here

EMAIL_USERNAME=walltechappbr@gmail.com
EMAIL_PASSWORD=abcd efgh ijkl mnop  # Senha de app do Gmail (sem espaços)
```

### 4. Suba os containers
```bash
docker-compose up -d
```

---

## ☁️ Setup EC2 (Produção)

### 1. Conecte no EC2
```bash
ssh -i sua-chave.pem ec2-user@seu-ip-ec2
```

### 2. Navegue até o diretório do projeto
```bash
cd /caminho/do/projeto
```

### 3. Crie/Edite o arquivo .env
```bash
nano .env
```

### 4. Configure com valores de PRODUÇÃO
```env
# Database
DB_NAME=monthbalance
DB_USER=postgres
DB_PASSWORD=SENHA_SUPER_SEGURA_AQUI_PRODUCAO

# JWT (GERE UMA NOVA CHAVE SEGURA!)
# Gere com: openssl rand -base64 48
JWT_SECRET=CHAVE_SUPER_SEGURA_GERADA_ALEATORIAMENTE_MIN_32_CHARS

# Email
EMAIL_USERNAME=walltechappbr@gmail.com
EMAIL_PASSWORD=sua_senha_de_app_do_gmail_aqui
```

### 5. Proteja o arquivo .env
```bash
chmod 600 .env
```

### 6. Suba os containers
```bash
docker-compose up -d
```

### 7. Verifique os logs
```bash
docker-compose logs -f backend
```

---

## 🔐 Gerando JWT Secret Seguro

### Opção 1: OpenSSL (Recomendado)
```bash
openssl rand -base64 48
```

### Opção 2: PowerShell (Windows)
```powershell
[Convert]::ToBase64String((1..48 | ForEach-Object { Get-Random -Maximum 256 }))
```

### Opção 3: Online (Use com cuidado)
https://generate-secret.vercel.app/32

---

## 📧 Configurando Senha de App do Gmail

### 1. Acesse sua Conta Google
https://myaccount.google.com/

### 2. Ative Verificação em 2 Etapas
- Vá em "Segurança"
- Ative "Verificação em duas etapas"

### 3. Crie Senha de App
- Ainda em "Segurança"
- Procure "Senhas de app"
- Selecione "Outro (nome personalizado)"
- Digite: "MonthBalance Backend"
- Clique em "Gerar"
- **Copie a senha** (16 caracteres)

### 4. Cole no .env
```env
EMAIL_PASSWORD=abcdefghijklmnop  # Cole aqui (sem espaços)
```

---

## ✅ Verificando Configuração

### 1. Verifique se as variáveis estão carregadas
```bash
docker-compose config
```

### 2. Verifique os logs do backend
```bash
docker-compose logs backend | grep -i email
```

### 3. Teste o envio de email
Acesse o app e teste a recuperação de senha.

---

## 🔄 Atualizando Variáveis

### 1. Edite o .env
```bash
nano .env
```

### 2. Reinicie apenas o backend
```bash
docker-compose restart backend
```

### 3. Ou reinicie tudo
```bash
docker-compose down
docker-compose up -d
```

---

## 🚨 Segurança

### ⚠️ NUNCA faça:
- ❌ Commitar o arquivo `.env` no Git
- ❌ Compartilhar senhas em texto plano
- ❌ Usar senhas fracas em produção
- ❌ Reutilizar senhas entre ambientes

### ✅ SEMPRE faça:
- ✅ Use `.env.example` como template
- ✅ Gere senhas fortes e únicas
- ✅ Proteja o arquivo `.env` (chmod 600)
- ✅ Use senhas de app do Gmail (não a senha da conta)
- ✅ Mantenha backups seguros das credenciais

---

## 📝 Checklist de Deploy

### Antes de subir em produção:
- [ ] Arquivo `.env` criado
- [ ] Senha do banco forte e única
- [ ] JWT Secret gerado aleatoriamente (min 32 chars)
- [ ] Senha de app do Gmail configurada
- [ ] Arquivo `.env` protegido (chmod 600)
- [ ] `.env` NÃO está no Git (.gitignore)
- [ ] Testado localmente
- [ ] Backup das credenciais em local seguro

---

## 🆘 Troubleshooting

### Erro: "EMAIL_USERNAME not configured"
- Verifique se o `.env` existe na raiz
- Verifique se a variável está escrita corretamente
- Reinicie o container: `docker-compose restart backend`

### Erro: "Authentication failed" (Email)
- Verifique se a senha de app está correta
- Certifique-se que não há espaços na senha
- Verifique se a verificação em 2 etapas está ativa
- Tente gerar uma nova senha de app

### Variáveis não carregam
- Verifique se o `.env` está na mesma pasta do `docker-compose.yml`
- Verifique a sintaxe do arquivo (sem espaços extras)
- Use `docker-compose config` para validar

---

**Pronto!** Configuração de ambiente completa e segura! 🎉
