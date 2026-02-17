# PWA Setup - Month Balance

## Ícones e Screenshots Corrigidos ✅

### Ícones Gerados:
- ✅ pwa-192x192.png (192x192px)
- ✅ pwa-512x512.png (512x512px)
- ✅ apple-touch-icon.png (180x180px)

### Screenshots Gerados:
- ✅ screenshot-mobile.png (390x844px) - Portrait
- ✅ screenshot-desktop.png (1280x720px) - Wide/Landscape

## Manifest Atualizado

O manifest agora inclui:
- Ícones nos tamanhos corretos
- Screenshots para mobile e desktop
- Configuração para "Richer PWA Install UI"

## Como Regenerar Assets PWA

```bash
# Gerar apenas ícones
pnpm generate-icons

# Gerar apenas screenshots
pnpm generate-screenshots

# Gerar tudo
pnpm generate-pwa
```

## Build e Deploy

```bash
# Build local
pnpm build

# Deploy no servidor
docker compose build frontend --no-cache
docker compose up -d frontend
```

## Verificação

Após o deploy, verifique:
1. Abra DevTools → Application → Manifest
2. Verifique se todos os ícones aparecem corretamente
3. Verifique se os screenshots aparecem
4. Teste a instalação do PWA no desktop e mobile

## Recursos PWA Habilitados

- ✅ Service Worker com cache automático
- ✅ Instalável no desktop e mobile
- ✅ Funciona offline (assets em cache)
- ✅ Cache de Google Fonts
- ✅ Auto-update do service worker
- ✅ Screenshots para UI de instalação rica
- ✅ Ícones maskable para Android

## Próximos Passos (Opcional)

1. Adicionar mais screenshots mostrando diferentes telas
2. Criar ícones personalizados para diferentes plataformas
3. Adicionar shortcuts no manifest para ações rápidas
4. Implementar notificações push
