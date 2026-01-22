# 🗄️ Comandos Úteis do Banco de Dados

## 🔄 Resetar Banco (Limpar Tudo)

```bash
# Parar o backend primeiro (se estiver rodando)
# Depois executar:

dotnet ef database drop --force
dotnet ef database update
```

Isso vai:
1. Deletar o banco `monthbalance` completamente
2. Recriar o banco vazio
3. Aplicar todas as migrations
4. **NÃO** criar dados de exemplo (banco limpo)

---

## 📋 Outros Comandos Úteis

### Ver Migrations Aplicadas
```bash
dotnet ef migrations list
```

### Criar Nova Migration
```bash
dotnet ef migrations add NomeDaMigration
```

### Remover Última Migration (se não foi aplicada)
```bash
dotnet ef migrations remove
```

### Aplicar Migrations
```bash
dotnet ef database update
```

### Voltar para Migration Específica
```bash
dotnet ef database update NomeDaMigration
```

### Gerar Script SQL das Migrations
```bash
dotnet ef migrations script
```

---

## 🎯 Workflow Comum

### Desenvolvimento Normal
1. Fazer mudanças nos Models
2. Criar migration: `dotnet ef migrations add DescricaoDaMudanca`
3. Aplicar: `dotnet ef database update`

### Resetar Tudo (Começar do Zero)
1. Parar backend
2. `dotnet ef database drop --force`
3. `dotnet ef database update`
4. Iniciar backend

### Desfazer Última Migration
1. Parar backend
2. `dotnet ef migrations remove`
3. Iniciar backend

---

## ⚠️ Importante

- **SEMPRE** pare o backend antes de rodar comandos de migration
- O banco está configurado para **NÃO** criar dados de exemplo automaticamente
- Migrations são aplicadas automaticamente quando o backend inicia (em Development)

---

**Versão:** 1.0  
**Data:** 22/01/2026
