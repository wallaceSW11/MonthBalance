#!/bin/bash

echo "🚀 Month Balance - Iniciando..."

# Verificar se .env existe
if [ ! -f .env ]; then
    echo "⚠️  Arquivo .env não encontrado!"
    echo "📝 Criando .env a partir do .env.example..."
    cp .env.example .env
    echo "✅ Arquivo .env criado!"
    echo "⚠️  IMPORTANTE: Edite o arquivo .env com senhas seguras antes de continuar!"
    echo "   nano .env"
    exit 1
fi

# Build e start
echo "🔨 Building containers..."
docker-compose build

echo "🚀 Starting containers..."
docker-compose up -d

echo "⏳ Aguardando containers iniciarem..."
sleep 5

# Verificar status
echo ""
echo "📊 Status dos containers:"
docker-compose ps

echo ""
echo "✅ Aplicação iniciada!"
echo ""
echo "📡 Endpoints disponíveis:"
echo "   - API: http://localhost:5000"
echo "   - Swagger: http://localhost:5000/swagger"
echo ""
echo "📝 Ver logs:"
echo "   docker-compose logs -f"
echo ""
echo "🛑 Parar aplicação:"
echo "   docker-compose down"
