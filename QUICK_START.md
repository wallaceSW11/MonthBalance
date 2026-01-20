# 🚀 Quick Start - Novo Projeto

## 1️⃣ Clonar ou Copiar o BaseProject

```bash
# Copie este projeto para uma nova pasta
cp -r BaseProject MeuNovoProjeto
cd MeuNovoProjeto
```

## 2️⃣ Personalizar package.json

Atualize as seguintes propriedades no `package.json`:

```json
{
  "name": "meu-novo-projeto",
  "version": "0.1.0"
}
```

## 3️⃣ Personalizar Theme

Edite `public/theme.json`:

```json
{
  "customization": {
    "appName": "Meu App",
    "appDescription": "Descrição do meu app",
    "copyrightText": "© 2025 Minha Empresa"
  }
}
```

## 4️⃣ Atualizar Traduções

Edite os arquivos em `src/locales/`:

- `pt-BR.ts`
- `en-US.ts`

## 5️⃣ Instalar e Rodar

```bash
pnpm install
pnpm dev
```

## 6️⃣ Limpar Exemplos (Opcional)

Se não precisar da página de demos:

1. Remova `src/views/DemoView.vue`
2. Remova a rota em `src/router/index.ts`
3. Atualize a navegação em `src/App.vue`

## ✅ Pronto!

Seu projeto está configurado e pronto para desenvolvimento!
