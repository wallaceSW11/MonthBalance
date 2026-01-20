# 🚨 LEIA PRIMEIRO - HIERARQUIA DE REGRAS

## 📋 Ordem de Prioridade

### 1️⃣ `1_code-style.md` ← PRIORIDADE MÁXIMA
Regras técnicas de formatação e escrita de código. **NUNCA VIOLE!**

### 2️⃣ `2_kiro-guide.md` ← COMPORTAMENTO E METODOLOGIA
Persona, metodologia EPER, princípios fundamentais, idioma do código.

### 3️⃣ `3_project-reference.md` ← REFERÊNCIA TÉCNICA
Stack, estrutura, convenções, módulos do projeto.

### 7️⃣ `7_testing-best-practices.md` ← TESTES
Boas práticas de testes unitários e de interface.

---

## ⚠️ REGRA DE OURO

**EM CASO DE CONFLITO:**

```
1_code-style.md > 2_kiro-guide.md > 3_project-reference.md > 7_testing-best-practices.md
```

Dúvida sobre formatação, espaçamento, nomenclatura? Consulte **PRIMEIRO** o `1_code-style.md`.

---

## 🎯 Checklist Antes de Codar

- [ ] Li o `1_code-style.md`?
- [ ] Vou seguir TODAS as regras de formatação?
- [ ] PascalCase nos componentes do template?
- [ ] Pulos de linha antes de `if` e `return`?
- [ ] Evitar `watch`, `v-if/v-else` desnecessários, `switch/case`?
- [ ] Early returns?
- [ ] Async/await (não `.then()`)?
- [ ] Optional chaining (`?.`)?
- [ ] Metodologia EPER?
- [ ] Testes validando interface E lógica?

**Se SIM para tudo, bora codar! 🚀**

---

## 💡 Dica Rápida

Ao começar novo chat:

> "Lembre-se: siga RIGOROSAMENTE o `1_code-style.md`. É nossa regra MÁXIMA!"

---

## 📞 Checkpoint de Contexto

Quando contexto atingir ~80%, Kiro avisa:

> "Opa! Contexto chegando perto do limite (80%). Bora resumir e abrir chat novo?"

---

**Versão:** 3.0 (Unificado)
