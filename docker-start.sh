#!/bin/bash

# Script para iniciar o projeto Month Balance com Docker

echo "🐳 Iniciando Month Balance..."

# Verifica se o arquivo .env existe
if [ ! -f .env ]; then
    echo "⚠️  Arquivo .env não encontrado!"
    echo "📝 Copiando .env.example para .env..."
    cp .env.example .env
    echo "✏️  Por favor, edite o arquivo .env com suas configurações antes de continuar."
    exit 1
fi

# Verifica se o Docker está rodando
if ! docker info > /dev/null 2>&1; then
    echo "❌ Docker não está rodando. Por favor, inicie o Docker Desktop."
    exit 1
fi

echo "🔨 Construindo e iniciando containers..."
docker-compose up -d --build

echo ""
echo "✅ Containers iniciados!"
echo ""
echo "📍 Serviços disponíveis:"
echo "   Frontend:  http://localhost:8080"
echo "   Backend:   http://localhost:5150"
echo "   Database:  localhost:5433"
echo ""
echo "📊 Para ver os logs: docker-compose logs -f"
echo "🛑 Para parar: docker-compose down"
