---
inclusion: always
priority: highest
---

# 🚨 LEIA PRIMEIRO - HIERARQUIA DE REGRAS

## 📋 Ordem de Prioridade

### 1️⃣ `1_code-style.md` ← PRIORIDADE MÁXIMA
Regras técnicas de formatação e escrita de código C# / .NET. **NUNCA VIOLE!**

### 2️⃣ `2_kiro-guide.md` ← COMPORTAMENTO E METODOLOGIA
Persona, metodologia EPER, princípios fundamentais.

### 3️⃣ `3_project-reference.md` ← REFERÊNCIA TÉCNICA
Stack, estrutura, convenções, módulos do projeto.

### 4️⃣ `4_api-design.md` ← DESIGN DE API
REST, DTOs, Controllers, validações.

---

## ⚠️ REGRA DE OURO

**EM CASO DE CONFLITO:**

```
1_code-style.md > 2_kiro-guide.md > 3_project-reference.md > 4_api-design.md
```

Dúvida sobre formatação, nomenclatura, estrutura? Consulte **PRIMEIRO** o `1_code-style.md`.

---

## 🎯 Checklist Antes de Codar

- [ ] Li o `1_code-style.md`?
- [ ] Vou seguir TODAS as regras de formatação?
- [ ] Early returns?
- [ ] Async/await?
- [ ] DTOs para requests/responses?
- [ ] Validações com FluentValidation?
- [ ] Repositories para acesso a dados?
- [ ] Services para lógica de negócio?
- [ ] Controllers finos (apenas orquestração)?
- [ ] Migrations para mudanças no banco?

**Se SIM para tudo, bora codar! 🚀**

---

## 💡 Dica Rápida

Ao começar novo chat:

> "Lembre-se: siga RIGOROSAMENTE o `1_code-style.md`. É nossa regra MÁXIMA!"

---

**Versão:** 1.0 (Month Balance Backend)
