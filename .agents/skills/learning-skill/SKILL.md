---
name: learning-skill
description: >-
  Use this skill when the user wants to learn, study, or understand a C#/.NET topic in depth, especially when triggered with 'learn <topic>'. Teaches progressively using a fully paired bilingual (English + Bangla) 9-step structure, including framework built-in types.
---

# Bilingual Learning Skill (English + Bangla)

This skill guides the **Learning Agent** to teach programming and system architecture concepts progressively using an intuitive 9-step, **fully-paired bilingual (English first, then Bangla)** framework.

---

## Dynamic Language & Stack Target
- **Default**: **C# / .NET** (in this workspace).
- **Dynamic Override**: If the user specifies another language (e.g., `learn python:asyncio`, `learn go:concurrency`, `learn rust:memory`) or the skill is used in another project repository, adapt all code examples, runtime internals (CLR, JVM, V8, Go runtime, etc.), and architecture patterns to that target ecosystem.

---

## Language Policy: Strict Paired Bilingualism (English ➔ Bangla)

- **For EVERY section and concept**, always provide:
  1. The complete **English explanation** first (professional, standard terminology).
  2. The complete **Bangla explanation (বাংলায় ব্যাখ্যা)** immediately following it (intuitive, conversational, retaining English technical keywords like *Garbage Collector*, *Heap*, *Stack*, *Boxing/Unboxing*, *Thread Pool*, *Task*, *ValueTask*, *State Machine*, *Deadlock*).
- No section should be English-only or Bangla-only. Both must be present side-by-side or block-by-block.

---

## The 9-Step Progressive Teaching Structure

For each requested topic, deliver the explanation following this exact sequence:

### 1. English Definition & Concept
- Authoritative, senior-level definition in clear English.

### 2. Bangla Explanation (বাংলায় সহজ ব্যাখ্যা)
- Intuitive, crystal-clear explanation in Bengali breaking down the mental model.

### 3. Why It Exists (কেন তৈরি করা হয়েছে?)
- **English**: The architectural motivation, problems it solved, and consequences of not having it.
- **Bangla (বাংলায় কারণ)**: কেন এটি তৈরি করা হয়েছিল এবং এটি না থাকলে অতীতে কী ধরনের সমস্যার মুখোমুখি হতে হতো।

### 4. How It Works Under The Hood (অভ্যন্তরীণ মেকানিজম / Internals)
- **English**: CLR runtime behavior, memory layout (Stack vs. Heap, struct/class metadata, GC pressure), IL instructions, invocation mechanics.
- **Bangla (বাংলায় অভ্যন্তরীণ মেকানিজম)**: ব্যাকগ্রাউন্ডে CLR এবং কম্পাইলার কীভাবে এটি হ্যান্ডেল করে, মেমোরিতে কী ঘটে।

### 5. Built-in Types & Simple Example (বিল্ট-ইন টাইপ ও সহজ উদাহরণ)
- **Framework Built-in Types**: Always detail the standard built-in types/variants provided by the framework (e.g., for Delegates: `Action<T>`, `Func<T, TResult>`, `Predicate<T>`, `EventHandler`, `Comparison<T>`).
- **English & Bangla description** of each built-in type.
- Clean, minimal, self-contained C# code snippet demonstrating both custom declarations and built-in types.

### 6. Real-World Production Example (বাস্তব প্রোডাকশন উদাহরণ)
- Realistic enterprise scenario (e.g., ASP.NET Core middleware, high-throughput caching, event-driven architecture, resilient pipelines).
- Explanation of why this pattern is used in production (English + Bangla).

### 7. Visual Diagram (ডায়াগ্রাম)
- A Mermaid diagram illustrating the architecture, memory layout, sequence, or state transitions.

### 8. Common Mistakes & Gotchas (সাধারণ ভুল ও ফাঁদ)
- For every pitfall:
  - **English**: Description of the anti-pattern, memory leak, or performance trap.
  - **Bangla (বাংলায় সতর্কতা)**: কেন এই ভুলটি ঘটে এবং এটি কীভাবে এড়াতে হবে (Senior solution).

### 9. Interview Perspective (ইন্টারভিউ প্রস্তুতি)
- Top tricky interview questions and expected answers:
  - **English Pitch**: Crisp, interview-ready response.
  - **Bangla Deep Explanation (বাংলায় ব্যাখ্যা)**: The underlying reasoning that demonstrates Senior-to-Principal mastery.

---

## Documentation Auto-Save & Readme Update Policy

- **Mandatory File Save**: Always save the complete 9-step tutorial directly into `docs/<topic_name>.md`.
- **Update `Readme.md`**: Automatically update the `Readme.md` table ("Study & Interview Documentation") to add/link this new guide.
- Always provide clickable links to both `[docs/<topic_name>.md](file:///d:/Interview/c-sharp-prep/docs/<topic_name>.md)` and `[Readme.md](file:///d:/Interview/c-sharp-prep/Readme.md)` in the chat response.
