#!/bin/bash
# Script para build e push das imagens Docker

GITHUB_USER="wallacesw11"
VERSION=${1:-latest}

echo "🏗️  Building Docker images..."
echo "Version: $VERSION"
echo ""

# Build backend
echo "📦 Building backend..."
docker build -t ghcr.io/$GITHUB_USER/month-balance-backend:$VERSION ./backend
if [ $? -ne 0 ]; then
    echo "❌ Backend build failed"
    exit 1
fi

# Build frontend
echo "📦 Building frontend..."
docker build -t ghcr.io/$GITHUB_USER/month-balance-frontend:$VERSION ./frontend
if [ $? -ne 0 ]; then
    echo "❌ Frontend build failed"
    exit 1
fi

echo ""
echo "✅ Build completed!"
echo ""
echo "🚀 Para fazer push das imagens:"
echo "   docker push ghcr.io/$GITHUB_USER/month-balance-backend:$VERSION"
echo "   docker push ghcr.io/$GITHUB_USER/month-balance-frontend:$VERSION"
echo ""
echo "Ou execute: ./scripts/push-images.sh $VERSION"
