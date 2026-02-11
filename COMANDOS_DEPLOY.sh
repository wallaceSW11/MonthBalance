#!/bin/bash

# ============================================================================
# COMANDOS DE DEPLOY - Month Balance AWS EC2
# ============================================================================
# Este arquivo contém todos os comandos necessários para fazer o deploy
# Copie e cole os comandos conforme necessário
# ============================================================================

# ============================================================================
# PARTE 1: NO SEU COMPUTADOR LOCAL
# ============================================================================

echo "=== PARTE 1: BUILD E PUSH DAS IMAGENS ==="

# Substitua SEU_USUARIO pelo seu usuário do GitHub
GITHUB_USER="SEU_USUARIO"
BACKEND_IMAGE="ghcr.io/${GITHUB_USER}/month-balance-backend:latest"
FRONTEND_IMAGE="ghcr.io/${GITHUB_USER}/month-balance-frontend:latest"

# Login no GitHub Container Registry
echo "Fazendo login no GitHub Container Registry..."
echo $GITHUB_TOKEN | docker login ghcr.io -u $GITHUB_USER --password-stdin

# Build e push do BACKEND
echo "Building backend..."
cd backend
docker build -t $BACKEND_IMAGE .
docker push $BACKEND_IMAGE
cd ..

# Build e push do FRONTEND (IMPORTANTE: com VITE_API_BASE_URL=/api)
echo "Building frontend..."
cd frontend
docker build --build-arg VITE_API_BASE_URL=/api -t $FRONTEND_IMAGE .
docker push $FRONTEND_IMAGE
cd ..

echo "✅ Imagens built e pushed com sucesso!"

# ============================================================================
# PARTE 2: COPIAR ARQUIVOS PARA O EC2
# ============================================================================

echo ""
echo "=== PARTE 2: COPIAR ARQUIVOS PARA O EC2 ==="

# Substitua pelos seus valores
EC2_KEY="sua-chave.pem"
EC2_USER="ec2-user"
EC2_IP="SEU_IP_ELASTICO"

# Copiar docker-compose.yml
echo "Copiando docker-compose.yml..."
scp -i $EC2_KEY docker-compose.yml ${EC2_USER}@${EC2_IP}:~/month-balance/

# Copiar .env
echo "Copiando .env..."
scp -i $EC2_KEY .env.production ${EC2_USER}@${EC2_IP}:~/month-balance/.env

echo "✅ Arquivos copiados com sucesso!"

# ============================================================================
# PARTE 3: NO SERVIDOR EC2
# ============================================================================

echo ""
echo "=== PARTE 3: COMANDOS PARA EXECUTAR NO EC2 ==="
echo "Conecte-se ao EC2 e execute os comandos abaixo:"
echo ""

cat << 'EOF'
# Conectar ao EC2
ssh -i sua-chave.pem ec2-user@SEU_IP_ELASTICO

# Criar diretório (se não existir)
mkdir -p ~/month-balance
cd ~/month-balance

# Editar .env com suas credenciais
nano .env

# Conteúdo do .env:
# DB_NAME=monthbalance
# DB_USER=postgres
# DB_PASSWORD=SuaSenhaSeguraAqui123!
# JWT_SECRET=sua_chave_jwt_super_secreta_minimo_32_caracteres_aqui

# Editar docker-compose.yml para usar suas imagens
nano docker-compose.yml

# Substituir:
# ghcr.io/SEU_USUARIO/month-balance-backend:latest
# ghcr.io/SEU_USUARIO/month-balance-frontend:latest

# Login no GitHub Container Registry (se imagens são privadas)
docker login ghcr.io -u SEU_USUARIO

# Pull das imagens
docker-compose pull

# Iniciar os containers
docker-compose up -d

# Verificar status
docker-compose ps

# Ver logs
docker-compose logs -f

# Verificar que está tudo funcionando
docker-compose logs backend | grep -i "now listening"
docker-compose logs postgres | grep -i "ready"

EOF

# ============================================================================
# PARTE 4: TESTES
# ============================================================================

echo ""
echo "=== PARTE 4: TESTES ==="
echo "Execute estes comandos para testar:"
echo ""

cat << 'EOF'
# ===== NO EC2 =====

# Teste 1: Backend direto (deve funcionar)
curl http://localhost:5150/api/health

# Teste 2: Via proxy (deve funcionar)
curl http://localhost/api/health

# Teste 3: Verificar portas abertas
sudo netstat -tlnp | grep LISTEN
# Deve mostrar apenas porta 80 exposta

# Teste 4: Verificar conectividade interna
docker-compose exec frontend ping -c 3 backend
docker-compose exec backend ping -c 3 postgres

# ===== NO SEU COMPUTADOR =====

# Teste 5: Frontend (deve funcionar)
curl http://SEU_IP_ELASTICO/

# Teste 6: API via proxy (deve funcionar)
curl http://SEU_IP_ELASTICO/api/health

# Teste 7: Backend direto (DEVE FALHAR - isso é correto!)
curl http://SEU_IP_ELASTICO:5150/api/health
# Esperado: Connection refused ou timeout

# Teste 8: No navegador
# Abra: http://SEU_IP_ELASTICO
# DevTools → Network → Faça login
# Verifique que as requisições vão para /api/* (não :5150)
# Verifique que NÃO há erros de CORS

EOF

# ============================================================================
# PARTE 5: COMANDOS ÚTEIS
# ============================================================================

echo ""
echo "=== PARTE 5: COMANDOS ÚTEIS ==="
echo ""

cat << 'EOF'
# Ver status dos containers
docker-compose ps

# Ver logs em tempo real
docker-compose logs -f

# Ver logs de um serviço específico
docker-compose logs -f backend
docker-compose logs -f frontend
docker-compose logs -f postgres

# Reiniciar um serviço
docker-compose restart backend

# Reiniciar tudo
docker-compose restart

# Parar tudo
docker-compose down

# Atualizar para nova versão
docker-compose pull
docker-compose up -d

# Ver uso de recursos
docker stats

# Executar comando dentro de um container
docker-compose exec backend bash
docker-compose exec postgres psql -U postgres -d monthbalance

# Backup do banco
docker-compose exec postgres pg_dump -U postgres monthbalance > backup_$(date +%Y%m%d).sql

# Restaurar backup
cat backup_20260211.sql | docker-compose exec -T postgres psql -U postgres monthbalance

# Ver networks
docker network ls
docker network inspect month-balance_month-balance-network

# Limpar tudo (CUIDADO: apaga volumes!)
docker-compose down -v

EOF

# ============================================================================
# PARTE 6: TROUBLESHOOTING
# ============================================================================

echo ""
echo "=== PARTE 6: TROUBLESHOOTING ==="
echo ""

cat << 'EOF'
# Problema: Backend não responde
docker-compose logs backend
docker-compose restart backend

# Problema: Erro 502 Bad Gateway
docker-compose exec frontend curl http://backend:5150/api/health
docker-compose logs frontend

# Problema: Erro de CORS (não deveria acontecer)
# Verificar se o frontend foi built com VITE_API_BASE_URL=/api
docker-compose exec frontend cat /etc/nginx/conf.d/default.conf | grep proxy_pass

# Problema: Banco não conecta
docker-compose logs postgres
docker-compose exec postgres psql -U postgres -d monthbalance -c "SELECT 1;"

# Problema: Container não inicia
docker-compose ps
docker-compose logs NOME_DO_SERVICO

# Verificar configuração do nginx
docker-compose exec frontend nginx -t

# Verificar variáveis de ambiente
docker-compose exec backend env | grep -E "ASPNETCORE|ConnectionStrings"

EOF

echo ""
echo "============================================================================"
echo "✅ Guia completo gerado!"
echo "============================================================================"
echo ""
echo "Próximos passos:"
echo "1. Execute os comandos da PARTE 1 (build e push)"
echo "2. Execute os comandos da PARTE 2 (copiar arquivos)"
echo "3. Conecte ao EC2 e execute os comandos da PARTE 3"
echo "4. Execute os testes da PARTE 4"
echo ""
echo "Documentação completa em:"
echo "- DEPLOY_AWS.md (guia detalhado)"
echo "- CHECKLIST_DEPLOY.md (checklist passo a passo)"
echo "- RESUMO_ALTERACOES.md (resumo das mudanças)"
echo ""
