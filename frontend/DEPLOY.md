# 🚀 Deploy Guide - Month Balance

## Vercel Deployment

### Quick Deploy

[![Deploy with Vercel](https://vercel.com/button)](https://vercel.com/new/clone)

### Manual Deployment

1. **Install Vercel CLI** (if not installed):
```bash
pnpm add -g vercel
```

2. **Login to Vercel**:
```bash
vercel login
```

3. **Deploy**:
```bash
vercel
```

4. **Deploy to Production**:
```bash
vercel --prod
```

### Configuration

The project is pre-configured with `vercel.json`:

- ✅ SPA routing (all routes redirect to index.html)
- ✅ Optimized caching for assets
- ✅ Service Worker support
- ✅ PWA ready

### Environment Variables

No environment variables required for basic deployment.

### Build Settings (Vercel Dashboard)

- **Framework Preset**: Vite
- **Build Command**: `pnpm run build`
- **Output Directory**: `dist`
- **Install Command**: `pnpm install`
- **Node Version**: 20.x

### Performance Optimizations

✅ Code splitting by vendor
✅ CSS code splitting
✅ Terser minification
✅ Console logs removed in production
✅ Gzip compression
✅ Asset caching (1 year)
✅ Service Worker caching

### Post-Deployment Checklist

- [ ] Test PWA installation
- [ ] Verify favicon loads correctly
- [ ] Check theme (dark mode by default)
- [ ] Test offline functionality
- [ ] Verify all routes work
- [ ] Check mobile responsiveness

### Troubleshooting

**Issue**: Routes return 404
- **Solution**: Ensure `vercel.json` is in the root directory

**Issue**: PWA not installing
- **Solution**: Check HTTPS is enabled (required for PWA)

**Issue**: Assets not loading
- **Solution**: Verify `dist` folder contains all assets after build

---

## Other Platforms

### Netlify

Create `netlify.toml`:
```toml
[build]
  command = "pnpm run build"
  publish = "dist"

[[redirects]]
  from = "/*"
  to = "/index.html"
  status = 200
```

### GitHub Pages

1. Update `vite.config.ts`:
```typescript
export default defineConfig({
  base: '/repository-name/',
  // ... rest of config
})
```

2. Deploy:
```bash
pnpm run build
cd dist
git init
git add -A
git commit -m 'deploy'
git push -f git@github.com:username/repository-name.git main:gh-pages
```

---

**Version**: 1.0.0
**Last Updated**: 2026-01-20
