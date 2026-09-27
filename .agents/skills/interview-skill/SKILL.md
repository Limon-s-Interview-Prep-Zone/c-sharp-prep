---
name: interview-skill
description: >-
  Use this skill when the user triggers 'interview <topic>' or requests interview questions on a C#/.NET topic. Generates a curated set of Top Interview Questions & Answers in bilingual format (English + Bangla) and saves them to docs/<topic_name>_qa.md.
---

# Bilingual Interview Skill - Top Interview Questions & Answers

This skill guides the **Interview Agent** in generating high-yield, senior-level interview questions and comprehensive answers on any requested topic, delivered in a bilingual (English + Bangla) format and saved permanently to project documentation.

---

## Dynamic Language & Stack Target
- **Default**: **C# / .NET** (in this workspace).
- **Dynamic Override**: If another language is requested (e.g., `interview python:decorators`, `interview go:channels`) or this skill is mounted in a different project repository, formulate the questions, code snippets, and internal evaluation criteria for that specific language and runtime.

---

## Language Policy: English + Bangla

- **Questions**: Formulated in standard, professional English as used by tech interviewers worldwide.
- **Bangla Context**: Explains what hiring managers are really probing for and why the question matters.
- **Answers**: 
  - **English Pitch**: A crisp, senior-level interview response designed for the interview room.
  - **Bangla Deep Explanation**: Intuitive Bengali breakdown of the underlying CLR/memory mechanics and design rationale.
- **Technical Keywords**: Kept in English (e.g., *SynchronizationContext*, *ThreadPool*, *Boxing*, *ValueTask*, *State Machine*, *Deadlock*).

---

## Workflow Rules & Guidelines

1. **Generate Comprehensive Top Q&A**:
   - Generate the **Top 5 to 7 most essential and critical interview questions** for the given topic, progressing from foundational to senior edge cases.
   - Do NOT just provide a single question; provide a complete, robust question-and-answer guide.

2. **Structure of Each Q&A Item**:
   For every question in the set, include:
   - **Question (English)**: The realistic interview question.
   - **Context & Intent (বাংলায় প্রেক্ষাপট)**: Why interviewers ask this and what trap they are looking for.
   - **Ideal Senior Answer (English)**: The polished 30-to-60 second pitch ready to say in an interview.
   - **Code Snippet (Mandatory / কোড উদাহরণ)**: A concise, realistic C# snippet demonstrating the concept, best practice, or anti-pattern to connect theory to implementation.
   - **In-Depth Explanation (বাংলায় গভীর ব্যাখ্যা)**: Clear explanation in Bangla covering CLR internals, memory, or architectural trade-offs.
   - **Follow-up / Probing Edge Case (ফলো-আপ প্রশ্ন)**: The deeper question an interviewer typically asks next.

3. **Documentation Auto-Save & Readme Update Policy (`topic_name_qa.md`)**:
   - **Mandatory File Save**: Always save the complete Top Q&A guide directly into the `docs/` folder as `docs/<topic_name>_qa.md` (e.g., `docs/async_await_qa.md`, `docs/Span_qa.md`, `docs/OOP_qa.md`).
   - **Update `Readme.md`**: Automatically update the `Readme.md` table ("Study & Interview Documentation") to add/link this new Q&A guide under the corresponding topic.
   - **Clickable File Link**: Always output direct clickable links to `[docs/<topic_name>_qa.md](file:///d:/Interview/c-sharp-prep/docs/<topic_name>_qa.md)` and `[Readme.md](file:///d:/Interview/c-sharp-prep/Readme.md)` in your response.

4. **Next Step Suggestions**:
   - Conclude by asking if the user wants to practice coding any of the scenarios with `coding <topic>`, or learn another concept with `learn <next_topic>`.
