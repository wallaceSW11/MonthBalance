#!/bin/bash

# Month Balance - Docker Helper Script

case "$1" in
  build)
    echo "🔨 Building Docker image..."
    docker compose build
    ;;
  
  up)
    echo "🚀 Starting container..."
    docker compose up -d
    echo "✅ Container started!"
    docker compose ps
    ;;
  
  down)
    echo "🛑 Stopping container..."
    docker compose down
    echo "✅ Container stopped!"
    ;;
  
  restart)
    echo "🔄 Restarting container..."
    docker compose restart
    echo "✅ Container restarted!"
    ;;
  
  logs)
    echo "📋 Showing logs (Ctrl+C to exit)..."
    docker compose logs -f month-balance
    ;;
  
  ps)
    docker compose ps
    ;;
  
  rebuild)
    echo "🔨 Rebuilding (no cache)..."
    docker compose build --no-cache
    ;;
  
  clean)
    echo "🧹 Cleaning up..."
    docker compose down -v
    echo "✅ Cleanup complete!"
    ;;
  
  shell)
    echo "🐚 Entering container shell..."
    docker exec -it month-balance sh
    ;;
  
  *)
    echo "Month Balance - Docker Helper"
    echo ""
    echo "Usage: ./docker.sh [command]"
    echo ""
    echo "Commands:"
    echo "  build    - Build Docker image"
    echo "  up       - Start container"
    echo "  down     - Stop container"
    echo "  restart  - Restart container"
    echo "  logs     - Show container logs"
    echo "  ps       - Show container status"
    echo "  rebuild  - Rebuild without cache"
    echo "  clean    - Stop and remove volumes"
    echo "  shell    - Enter container shell"
    ;;
esac
