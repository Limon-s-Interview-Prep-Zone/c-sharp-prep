# Multi-Agent Workflow Quickstart Guide

This `.agents/` directory contains an autonomous multi-agent system designed for technical learning, interview preparation, and hands-on coding practice.

---

## 📁 Where Are The Files Located?

Everything is already created directly inside this repository:

```text
d:\Interview\c-sharp-prep\
├── AGENTS.md                                # Root guidelines & routing overview
├── Readme.md                                # Auto-updated with generated docs
└── .agents/
    ├── README.md                            # This quickstart guide
    ├── rules/
    │   └── orchestrator.md                  # Routing logic & trigger rules
    └── skills/
        ├── learning-skill/
        │   └── SKILL.md                     # 9-Step bilingual teaching protocol
        ├── interview-skill/
        │   └── SKILL.md                     # Top interview Q&A with code snippets
        └── coding-skill/
            └── SKILL.md                     # Docs-first coding challenges & verification
```

---

## 🚀 How to Use the Workflow

Simply type any of the following commands directly in the chat:

### 1. Learning Mode (`learn <topic>`)
Teaches the concept using a **9-Step Bilingual (English + Bangla)** structure:
```text
learn async/await
learn IDisposable
learn Span<T>
```
* **Auto-Action**: Generates the complete lesson, saves it to `docs/<topic_name>.md`, and updates `Readme.md`.

---

### 2. Interview Mode (`interview <topic>`)
Generates the **Top 5-7 Interview Questions & Answers** in bilingual format with mandatory code snippets:
```text
interview async/await
interview polymorphism
interview garbage collection
```
* **Auto-Action**: Saves the full Q&A guide to `docs/<topic_name>_qa.md` and updates `Readme.md`.

---

### 3. Coding Mode (`coding <topic>`)
Presents a hands-on problem with the **Docs-First Rule**:
```text
coding lock vs semaphoreslim
coding memory efficient string parser
coding custom middleware
```
* **Auto-Action**: Checks if `docs/<topic>.md` exists (creates it if missing) ➔ creates an implementation plan ➔ writes code ➔ verifies with `dotnet build` / `dotnet run`.

---

## 🌐 Dynamic Multi-Language Support (Portability)

The system defaults to **C# / .NET** in this repository, but you can explicitly specify any language:

```text
learn python:asyncio
interview go:channels
coding rust:lru_cache
interview ts:generics
```

### How to Reuse in Another Project:
To use this exact workflow in a Python, Go, Node.js, or Rust repository:
1. **Copy the `.agents/` folder** into the root of your new project.
2. The orchestrator will automatically detect the project type (`package.json`, `go.mod`, `Cargo.toml`, etc.) and adapt its tools and explanations!
