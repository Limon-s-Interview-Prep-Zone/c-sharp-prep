---
name: coding-skill
description: >-
  Use this skill when the user triggers 'coding <topic>' or wants a hands-on C# coding problem, bug fix, or refactoring task.
---

# Coding Skill - Hands-On Implementation & Code Review

This skill guides the agent in providing practical, interview-grade programming challenges, planning, generating code, and reviewing solutions.

---

## Dynamic Language & Stack Target
- **Default**: **C# / .NET** (Runner: `dotnet build` / `dotnet run`).
- **Dynamic Override & Portability**: If another language is requested (e.g., `coding python:lru_cache`, `coding go:worker_pool`) or used in another repository, adapt to that ecosystem:
  - **C# / .NET**: `dotnet build` / `dotnet run --project <path>`
  - **TypeScript / JavaScript**: `npm test` / `npm start`
  - **Go**: `go test ./...` / `go run .`
  - **Python**: `pytest` / `python <script>.py`
  - **Rust**: `cargo test` / `cargo run`

---

## Workflow Rules & Guidelines

1. **Pre-requisite Check: Documentation First (Docs-First Rule)**:
   - Before generating any code or implementation plan, check if the topic is already documented in `docs/` (e.g., `docs/<topic_name>.md`).
   - **If the document does NOT exist**:
     - Create the documentation first in `docs/<topic_name>.md` detailing the core concept, architectural design, requirements, and constraints.
     - Provide a clickable link to `[docs/<topic_name>.md](file:///d:/Interview/c-sharp-prep/docs/<topic_name>.md)`.
   - **If the document DOES exist (or once created)**:
     - Proceed to step 2 (Plan & Code Generation).

2. **Implementation Planning & Problem Formulation**:
   - Formulate a clear step-by-step execution plan.
   - Define:
     - **Functional Requirements**: What the code must accomplish.
     - **Constraints & Edge Cases**: E.g., zero unnecessary heap allocation, O(1) complexity, thread-safety, cancellation support.
     - **Starter / Target Architecture**: Target classes, signatures, and interfaces.

3. **Code Generation & Workspace Integration**:
   - Write or update code in the matching workspace project (e.g. `StringOperations/`, `TaskBasedAsync/`, `Multithreading/`, `OOP/`).
   - Verify code using the .NET CLI:
     ```powershell
     dotnet build <project_path>
     dotnet run --project <project_path>
     ```

4. **Senior Code Review Checklist**:
   When reviewing submitted or generated code:
   - **Correctness & Edge Cases**: Null safety, boundary conditions, proper exception handling.
   - **Performance & Allocations**: Boxing/unboxing avoidance, minimal GC pressure, `Span<T>`/`Memory<T>` where appropriate.
   - **Concurrency Safety**: Race conditions, deadlock avoidance, proper lock primitive selection (`lock`, `SemaphoreSlim`, `Interlocked`).
   - **Modern C# Idioms**: Pattern matching, `using` declarations, record types, and expressive LINQ.
