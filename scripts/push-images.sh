#!/bin/bash
# Script para push das imagens Docker

GITHUB_USER="wallacesw11"
VERSION=${1:-latest}

echo "🚀 Pushing Docker images..."
echo "Version: $VERSION"
echo ""

# Push backend
echo "📤 Pushing backend..."
docker push ghcr.io/$GITHUB_USER/month-balance-backend:$VERSION
if [ $? -ne 0 ]; then
    echo "❌ Backend push failed"
    exit 1
fi

# Push frontend
echo "📤 Pushing frontend..."
docker push ghcr.io/$GITHUB_USER/month-balance-frontend:$VERSION
if [ $? -ne 0 ]; then
    echo "❌ Frontend push failed"
    exit 1
fi

echo ""
echo "✅ Push completed!"
echo ""
echo "📋 Próximos passos no EC2:"
echo "   docker compose pull"
echo "   docker compose up -d"
