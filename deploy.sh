#!/bin/bash

# Script de Deploy Rápido - AWS EC2 com Docker Compose
# Uso: ./deploy.sh [build|pull|restart|logs|status]

set -e

ACTION=${1:-pull}

echo "🚀 Month Balance - Deploy Script"
echo "================================"

case $ACTION in
  build)
    echo "📦 Building images..."
    docker-compose build --no-cache
    echo "✅ Build completed!"
    ;;
    
  pull)
    echo "📥 Pulling latest images..."
    docker-compose pull
    echo "✅ Pull completed!"
    ;;
    
  restart)
    echo "🔄 Restarting services..."
    docker-compose down
    docker-compose up -d
    echo "✅ Services restarted!"
    ;;
    
  logs)
    echo "📋 Showing logs..."
    docker-compose logs -f --tail=100
    ;;
    
  status)
    echo "📊 Service status:"
    docker-compose ps
    echo ""
    echo "📈 Resource usage:"
    docker stats --no-stream
    ;;
    
  *)
    echo "❌ Unknown action: $ACTION"
    echo "Usage: $0 [build|pull|restart|logs|status]"
    exit 1
    ;;
esac

echo ""
echo "🎉 Done!"
