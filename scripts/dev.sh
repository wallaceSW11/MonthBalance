#!/bin/bash
# Script para iniciar o ambiente de desenvolvimento

echo "🚀 Iniciando Month Balance - Desenvolvimento"
echo ""

# Verificar se Docker está rodando
if ! docker info > /dev/null 2>&1; then
    echo "❌ Docker não está rodando. Inicie o Docker e tente novamente."
    exit 1
fi

# Verificar se .env existe
if [ ! -f .env ]; then
    echo "📝 Criando arquivo .env..."
    cp .env.development .env
fi

# Subir banco e backend
echo "🐘 Iniciando PostgreSQL e Backend..."
docker compose -f docker-compose.dev.yml up -d

echo ""
echo "✅ Backend rodando em: http://localhost:5000"
echo "✅ PostgreSQL rodando em: localhost:5432"
echo ""
echo "📱 Para iniciar o frontend:"
echo "   cd frontend"
echo "   npm install"
echo "   npm run dev"
echo ""
echo "📊 Ver logs: docker compose -f docker-compose.dev.yml logs -f"
echo "🛑 Parar: docker compose -f docker-compose.dev.yml down"
