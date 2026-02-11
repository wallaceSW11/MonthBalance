# Gerar Ícones PWA

## Opção 1: Online (Mais Fácil)

1. Acesse: https://www.pwabuilder.com/imageGenerator
2. Faça upload do arquivo `public/logo-dark.png` ou `public/logo-light.png`
3. Baixe os ícones gerados
4. Coloque na pasta `public/`:
   - `pwa-192x192.png`
   - `pwa-512x512.png`
   - `apple-touch-icon.png` (180x180)

## Opção 2: ImageMagick (Local)

Se tiver ImageMagick instalado:

```bash
# Instalar ImageMagick (se necessário)
# Windows: choco install imagemagick
# Mac: brew install imagemagick
# Linux: sudo apt-get install imagemagick

# Gerar ícones
magick public/logo-dark.png -resize 192x192 public/pwa-192x192.png
magick public/logo-dark.png -resize 512x512 public/pwa-512x512.png
magick public/logo-dark.png -resize 180x180 public/apple-touch-icon.png
```

## Opção 3: Photoshop/GIMP

1. Abra `public/logo-dark.png` ou `public/logo-light.png`
2. Redimensione para:
   - 192x192 → salve como `pwa-192x192.png`
   - 512x512 → salve como `pwa-512x512.png`
   - 180x180 → salve como `apple-touch-icon.png`
3. Coloque todos na pasta `public/`

## Verificar

Após gerar os ícones, você deve ter na pasta `public/`:
- ✅ favicon.ico
- ✅ pwa-192x192.png
- ✅ pwa-512x512.png
- ✅ apple-touch-icon.png

## Testar PWA

1. Build: `pnpm build`
2. Preview: `pnpm preview`
3. Abra no navegador e teste a instalação do PWA
4. No Chrome: Menu → Instalar Month Balance
