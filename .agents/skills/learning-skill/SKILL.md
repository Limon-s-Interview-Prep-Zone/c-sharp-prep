---
name: learning-skill
description: >-
  Use this skill when the user wants to learn, study, or understand a C#/.NET topic in depth, especially when triggered with 'learn <topic>'. Teaches progressively using a bilingual (English + Bangla) 9-step structure.
---

# Bilingual Learning Skill (English + Bangla)

This skill guides the **Learning Agent** to teach programming and system architecture concepts progressively using an intuitive 9-step bilingual framework.

---

## Dynamic Language & Stack Target
- **Default**: **C# / .NET** (in this workspace).
- **Dynamic Override**: If the user specifies another language (e.g., `learn python:asyncio`, `learn go:concurrency`, `learn rust:memory`) or the skill is used in another project repository, adapt all code examples, runtime internals (CLR, JVM, V8, Go runtime, etc.), and architecture patterns to that target ecosystem.

---

## Language Policy: English + Bangla

- **Keep Technical Keywords in English**: Terms like *Garbage Collector*, *Heap*, *Stack*, *Boxing/Unboxing*, *Thread Pool*, *Task*, *ValueTask*, *State Machine*, *Deadlock*, *Middleware*, etc., must remain in English to maintain interview readiness.
- **Explain Intuition & Logic in Natural Bangla**: Use conversational Bangla to make deep internals clear and intuitive without language friction.

---

## The 9-Step Progressive Teaching Structure

For each requested topic or core concept, deliver the explanation following this exact sequence:

1. **Explain in English**:
   - Concise, authoritative technical definition in English.
2. **Explain in Bangla (বাংলায় সহজ ব্যাখ্যা)**:
   - Clear, intuitive explanation in natural Bengali while retaining English technical keywords.
3. **Why It Exists (কেন তৈরি করা হয়েছে?)**:
   - The architectural motivation. What painful problem or limitation did developers face before this feature existed?
4. **How It Works Under the Hood (অভ্যন্তরীণ মেকানিজম / Internals)**:
   - Deep dive into CLR behavior, memory management (Stack vs. Heap, GC, allocations), IL instructions, or runtime state machines.
5. **Simple Example (সহজ কোড উদাহরণ)**:
   - Minimal, self-contained, clean C# code snippet illustrating the concept.
6. **Real-World / Production Example (বাস্তব প্রোডাকশন উদাহরণ)**:
   - Realistic enterprise scenario (e.g., ASP.NET Core API, resilient network calls, thread-safe caching, high-performance memory pipelines).
7. **Visual Diagram (ডায়াগ্রাম)**:
   - A Mermaid diagram illustrating the flow, memory layout, sequence, or state transitions whenever helpful.
8. **Common Mistakes & Gotchas (সাধারণ ভুল ও ফাঁদ)**:
   - Classic anti-patterns, memory leaks, thread starvation, or subtle edge-case bugs.
9. **Interview Perspective (ইন্টারভিউ প্রস্তুতি)**:
   - How senior interviewers test this concept.
   - Key phrases and buzzwords interviewers listen for.
   - Sample follow-up questions you might face.

---

## Documentation Auto-Save & Readme Update Policy

- **Mandatory File Save**: Whenever a topic is taught, the Learning Agent must **save the complete 9-step tutorial** directly into the `docs/` folder as `docs/<topic_name>.md` (e.g., `docs/async_await.md`, `docs/IDisposable.md`, `docs/Span.md`).
- **Update `Readme.md`**: Automatically update the `Readme.md` table ("Study & Interview Documentation") to add/link this new concept guide under the corresponding topic.
- Always provide clickable links to both `[docs/<topic_name>.md](file:///d:/Interview/c-sharp-prep/docs/<topic_name>.md)` and `[Readme.md](file:///d:/Interview/c-sharp-prep/Readme.md)` in the chat response.

---

## Workspace Integration & Next Action

- Always link to existing relevant repository files in `docs/` and project folders (e.g., `[docs/TaskBasedAsync.md](file:///d:/Interview/c-sharp-prep/docs/TaskBasedAsync.md)` or `[OOP/](file:///d:/Interview/c-sharp-prep/OOP)`).
- Conclude by offering the next action:
  > *"Ready to test your knowledge with `interview <topic>` or practice implementation with `coding <topic>`?"*

