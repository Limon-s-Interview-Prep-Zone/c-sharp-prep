# C# / .NET Core Master Interview & Engineering Guide (Simple English + বাংলা)

A complete, unified interview preparation and technical reference manual merging core runtime fundamentals, modern C# language features, practical coding algorithms, and production-grade asynchronous and memory management challenges.

---

## Master Table of Contents
1. [Part 1: Core C# & .NET Runtime Fundamentals](#part-1-core-c--net-runtime-fundamentals)
   - [1.1 Platform & Execution Engine](#11-platform--execution-engine) (C#, .NET, CLR, JIT vs. AOT)
   - [1.2 Types & Memory Foundations](#12-types--memory-foundations) (Value vs. Reference, Boxing, Stack vs. Heap, const vs. readonly, var vs. dynamic, Nullables)
   - [1.3 Object-Oriented Programming (OOP)](#13-object-oriented-programming-oop) (4 Pillars, Class vs. Struct, Constructors, Properties vs. Fields, Enums)
   - [1.4 Polymorphism & Method Modifiers](#14-polymorphism--method-modifiers) (virtual/override/new, Abstract vs. Interface, ref/out/in, params)
   - [1.5 Collections, Flow & Generics](#15-collections-flow--generics) (Arrays vs. List, Dictionary, HashSet, foreach, Generics)
2. [Part 2: Advanced C# Runtime & Language Mechanics](#part-2-advanced-c-runtime--language-mechanics)
   - [2.1 Delegates, Lambdas & Events](#21-delegates-lambdas--events)
   - [2.2 LINQ & Custom Iterators](#22-linq--custom-iterators) (Deferred Execution, IEnumerable vs. IQueryable, yield return)
   - [2.3 Memory Management, GC & Resource Cleanup](#23-memory-management-gc--resource-cleanup) (Generational GC, LOH, IDisposable, Finalizers)
   - [2.4 Asynchronous Programming & Concurrency](#24-asynchronous-programming--concurrency) (async/await, Task vs. Thread, ValueTask, ConfigureAwait, Cancellation)
   - [2.5 Thread Synchronization & Thread-Safety](#25-thread-synchronization--thread-safety) (lock, SemaphoreSlim, Interlocked, ConcurrentDictionary)
   - [2.6 Equality, Comparison & Copying](#26-equality-comparison--copying) (== vs. Equals, GetHashCode, Shallow vs. Deep Copy, Variance)
   - [2.7 Reflection, Attributes & Design Patterns](#27-reflection-attributes--design-patterns)
   - [2.8 ASP.NET Core & Runtime Error Handling](#28-aspnet-core--runtime-error-handling) (DI Lifetimes, Middleware Pipeline, throw vs. throw ex)
3. [Part 3: Core Language Details & Modern C# Features (C# 9 to C# 12+)](#part-3-core-language-details--modern-c-features-c-9-to-c-12)
   - [3.1 Core Language Mechanics](#31-core-language-mechanics) (Extension Methods, Indexers, Operator Overloading, Partial Types, Tuples, Pattern Matching)
   - [3.2 Modern C# Features](#32-modern-c-features) (Records, init-only, required, with expressions, Primary Constructors, Collection Expressions, Raw Strings)
4. [Part 4: High-Performance Memory Engineering (Span, Memory & Pools)](#part-4-high-performance-memory-engineering-span-memory--pools)
5. [Part 5: Production Asynchronous & Memory Challenges](#part-5-production-asynchronous--memory-challenges)
   - [Challenge 1: Bounded-Concurrency Async Map](#challenge-1-bounded-concurrency-async-map)
   - [Challenge 2: Timeout Without Losing Caller Cancellation](#challenge-2-timeout-without-losing-caller-cancellation)
   - [Challenge 3: Bounded Producer/Consumer Pipeline with Channel<T>](#challenge-3-bounded-producerconsumer-pipeline-with-channelt)
   - [Challenge 4: Single-Flight Concurrent Request Deduplication](#challenge-4-single-flight-concurrent-request-deduplication)
   - [Challenge 5: Stream Large File Using Pooled Memory](#challenge-5-stream-large-file-using-pooled-memory)
   - [Challenge 6: Zero-Allocation Delimited String Parser](#challenge-6-zero-allocation-delimited-string-parser)
   - [Challenge 7: Async Enumeration with Cancellation & Cleanup](#challenge-7-async-enumeration-with-cancellation--cleanup)
   - [Challenge 8: Hot-Path Allocation Optimization & stackalloc](#challenge-8-hot-path-allocation-optimization--stackalloc)
   - [Challenge 9: Diagnosing & Fixing Event Subscription Leaks](#challenge-9-diagnosing--fixing-event-subscription-leaks)
6. [Part 6: Common Coding & Algorithm Interview Problems](#part-6-common-coding--algorithm-interview-problems)
   - [Problem 1: Reverse a String](#problem-1-reverse-a-string)
   - [Problem 2: Palindrome Validation](#problem-2-palindrome-validation)
   - [Problem 3: Find Maximum in an Array](#problem-3-find-maximum-in-an-array)
   - [Problem 4: Binary Search](#problem-4-binary-search)
   - [Problem 5: Remove Duplicates Preserving Order](#problem-5-remove-duplicates-preserving-order)
7. [Part 7: Code Review Checklist & Senior Interview Strategy](#part-7-code-review-checklist--senior-interview-strategy)

---

# Part 1: Core C# & .NET Runtime Fundamentals

## 1.1 Platform & Execution Engine

### Q1: What is C#?
- **English:** C# (C-sharp) is a strongly typed, modern, object-oriented, and component-oriented programming language developed by Microsoft. It is designed for building enterprise web services, mobile apps, desktop applications, games, and cloud-native microservices on the .NET platform.
- **বাংলা:** C# (সি-শার্প) হলো Microsoft-এর তৈরি একটি strongly typed, modern, object-oriented প্রোগ্রামিং ভাষা। এটি .NET প্ল্যাটফর্মে সুরক্ষিত এন্টারপ্রাইজ ওয়েব সার্ভিস, ক্লাউড-নেটিভ মাইক্রোসার্ভিস, ডেস্কটপ ও মোবাইল অ্যাপ তৈরির জন্য ব্যবহৃত হয়।

### Q2: What is .NET, and how is it related to C#?
- **English:** .NET is a cross-platform, open-source developer framework providing the execution engine (CLR), runtime libraries (BCL), SDK, and developer CLI tools. C# is the premier programming language that compiles down to Intermediate Language (IL) targeting the .NET runtime.
- **বাংলা:** .NET হলো একটি ক্রস-প্ল্যাটফর্ম ওপেন-সোর্স ফ্রেমওয়ার্ক যা রানটাইম ইঞ্জিন (CLR), বেস ক্লাস লাইব্রেরি (BCL), SDK ও বিল্ড টুলস সরবরাহ করে। C# হলো প্রধান ভাষা যা ইন্টারমিডিয়েট ল্যাঙ্গুয়েজে (IL) কম্পাইল হয়ে .NET রানটাইমে চলে।

### Q3: What is the Common Language Runtime (CLR) and why is it important?
- **English:** The CLR is the heart of .NET. It executes Intermediate Language (IL) bytecode and provides essential system services including automatic Garbage Collection (GC), Just-In-Time (JIT) compilation to native CPU code, thread pooling, type verification, and structured exception handling.
- **বাংলা:** CLR হলো .NET-এর কোর এক্সিকিউশন ইঞ্জিন। এটি IL কোডকে রানটাইমে JIT কম্পাইলার দিয়ে নেটিভ মেমোরি কোডে রূপান্তর করে, মেমরি ম্যানেজমেন্ট (GC), থ্রেড ম্যানেজমেন্ট, টাইপ সেফটি এবং এক্সেপশন হ্যান্ডলিং পরিচালনা করে।

### Q4: What is the difference between a compiler and a runtime?
- **English:** The compiler (e.g., Roslyn for C#) verifies syntax, type safety, and semantic correctness at build time, outputting Intermediate Language (IL) binaries (.dll). The runtime (CLR) executes those binaries, loading types, performing JIT compilation, managing heap memory, and invoking OS APIs.
- **বাংলা:** কম্পাইলার (যেমন Roslyn) কোডের গঠন ও সিনট্যাক্স যাচাই করে বিল্ড টাইমে IL বাইনারি তৈরি করে। রানটাইম (CLR) সেই বাইনারিগুলো চালায়, মেমোরি বরাদ্দ করে এবং মেথড এক্সিকিউট করে।

### Q5: What is JIT compilation vs. Ahead-Of-Time (AOT) compilation?
- **English:** JIT (Just-In-Time) compilation translates IL into native machine instructions on the fly as each method is first called at runtime, enabling hardware-specific runtime optimizations. Native AOT compiles C# directly into platform-specific machine code during build time, resulting in instantaneous application startup, reduced memory footprint, and no JIT overhead.
- **বাংলা:** JIT কম্পাইলেশন রানটাইমে প্রতিটি মেথড প্রথমবার কলের সময় IL-কে নেটিভ মেশিন কোডে রূপান্তর করে। Native AOT বিল্ড টাইমে পুরো কোডকে সরাসরি মেশিন কোডে কম্পাইল করে, ফলে স্টার্টআপ টাইম অত্যন্ত দ্রুত হয় এবং মেমরি ব্যবহার কমে।

---

## 1.2 Types & Memory Foundations

### Q6: What are value types and reference types?
- **English:** Value types (`int`, `bool`, `struct`, `enum`) directly hold their data value and are copied by value (typically allocated on the execution stack or inline within containing objects). Reference types (`class`, `string`, `delegate`, arrays) hold a pointer to an object allocated on the managed heap; copying a reference variable copies only the pointer, not the actual object.
- **বাংলা:** Value type-এর ভেরিয়েবলে সরাসরি আসল ডেটা মান থাকে এবং অ্যাসাইন করলে মান কপি হয় (সাধারণত স্ট্যাকে বরাদ্দ)। Reference type-এর ভেরিয়েবলে হিপ মেমরিতে থাকা অবজেক্টের মেমরি রেফারেন্স (পয়েন্টার) থাকে; এটি কপি করলে শুধু পয়েন্টার কপি হয়, নতুন অবজেক্ট তৈরি হয় না।

### Q7: What is boxing and unboxing?
- **English:** Boxing is the implicit conversion of a value type to `object` or any interface it implements, wrapping the value in a heap-allocated box. Unboxing is the explicit extraction of the value type from that heap object. Excessive boxing causes high heap allocation and GC pause overhead.
- **বাংলা:** Boxing হলো একটি ভ্যালু টাইপকে `object` বা ইন্টারফেসে রূপান্তর করে হিপ মেমরিতে স্থানান্তর করা। Unboxing হলো সেই বক্সড অবজেক্ট থেকে কাস্টিংয়ের মাধ্যমে আসল ভ্যালু বের করে আনা। অপ্রয়োজনীয় বক্সিং হিপে চাপ ফেলে এবং অ্যাপ স্লো করে।

```csharp
int val = 100;
object boxed = val;         // Boxing (heap allocation)
int unboxed = (int)boxed;   // Unboxing (explicit cast)
```

### Q8: What is the difference between `stack` and `heap` allocation?
- **English:** The stack is a high-speed, thread-specific LIFO memory structure where local method call frames, primitive variables, and references live; memory is reclaimed instantly when the method exits. The heap is a large, shared pool of memory managed by the Garbage Collector where reference objects live until collected.
- **বাংলা:** স্ট্যাক হলো প্রতিটি থ্রেডের নিজস্ব অতি দ্রুতগতির LIFO মেমরি, যেখানে লোকাল ভ্যারিয়েবল থাকে এবং মেথড শেষ হলে সাথে সাথে মুছে যায়। হিপ হলো অ্যাপ্লিকেশনের শেয়ার্ড মেমরি স্পেস যেখানে রেফারেন্স অবজেক্ট সংরক্ষিত থাকে এবং GC তা মুক্ত করে।

### Q9: What is the difference between `const` and `readonly`?
- **English:** `const` is evaluated at compile time, inlined directly into call sites, implicitly static, and restricted to primitives and strings. `readonly` is evaluated at runtime, can hold complex objects, and can be initialized at declaration or within instance/static constructors.
- **বাংলা:** `const` কম্পাইল-টাইম কনস্ট্যান্ট যা কল সাইটে সরাসরি মান হিসেবে বসে যায় এবং কেবল প্রিমিটিভ/স্ট্রিং-এ কাজ করে। `readonly` রানটাইমে নির্ধারিত হয়, এতে জটিল অবজেক্ট রাখা যায় এবং ডিক্লারেশন বা কনস্ট্রাক্টরে ইনিশিয়ালাইজ করা যায়।

```csharp
public class Settings {
    public const int MaxRetries = 3;                // Compile-time literal
    public readonly DateTime BootTime = DateTime.UtcNow; // Evaluated at runtime
}
```

### Q10: What is the difference between `var` and `dynamic`?
- **English:** `var` uses compile-time type inference; the compiler determines the static type from the right-hand initialization, maintaining full compile-time type safety. `dynamic` bypasses compile-time type checking altogether; member resolution and method invocations are deferred to the runtime Dynamic Language Runtime (DLR).
- **বাংলা:** `var` কম্পাইল-টাইম টাইপ ইনফারেন্স ব্যবহার করে—ডানপাশের মান দেখে কম্পাইলার টাইপ ঠিক করে এবং পূর্ণ টাইপ-সেফটি বজায় থাকে। `dynamic` কম্পাইল টাইমে টাইপ যাচাই এড়িয়ে যায় এবং রানটাইমে DLR-এর মাধ্যমে মেথড কল সম্পন্ন করে।

### Q11: What are nullable value types and nullable reference types?
- **English:** Nullable value types (`int?`, `Nullable<T>`) wrap a struct to allow it to hold `null` via an internal `HasValue` flag. Nullable reference types (`string?`) are a C# 8+ static analysis feature where the compiler warns if an unannotated reference could evaluate to null, without runtime overhead.
- **বাংলা:** Nullable value type (`int?`) একটি স্ট্রাক্ট যা `null` ধারণ করতে পারে। Nullable reference type (`string?`) C# 8-এর একটি স্ট্যাটিক অ্যানালাইসিস ফিচার যা রানটাইমে কোনো বাড়তি খরচ ছাড়া কম্পাইল টাইমে সম্ভাব্য নাল সংক্রান্ত সতর্কতা দেয়।

---

## 1.3 Object-Oriented Programming (OOP)

### Q12: What are the four pillars of object-oriented programming?
- **English:** Encapsulation hides internal state and exposes safe operations via access modifiers and properties. Abstraction exposes essential contracts while concealing implementation details. Inheritance creates derived classes from base classes to reuse functionality. Polymorphism allows derived types to be treated through common base abstractions with customized behavior.
- **বাংলা:** Encapsulation ডেটা ও আচরণ একত্রিত করে প্রবেশাধিকার নিয়ন্ত্রণ করে। Abstraction ভেতরের জটিলতা আড়াল করে কেবল প্রয়োজনীয় চুক্তি প্রকাশ করে। Inheritance বিদ্যমান ক্লাস থেকে নতুন ক্লাসে কোড পুনর্ব্যবহার করতে দেয়। Polymorphism একই ইন্টারফেস বা বেজ ক্লাসের অধীনে ভিন্ন ভিন্ন আচরণ প্রকাশ করতে দেয়।

### Q13: What is the difference between a class and a struct?
- **English:** A `class` is a reference type allocated on the heap, supporting inheritance, finalizers, and reference equality. A `struct` is a value type allocated inline or on the stack, copied by value, cannot inherit other classes, and should generally be small (under 16 bytes), immutable, and short-lived.
- **বাংলা:** `class` হলো রেফারেন্স টাইপ যা হিপে বরাদ্দ হয় এবং ইনহেরিটেন্স সমর্থন করে। `struct` হলো হালকা ভ্যালু টাইপ যা সরাসরি স্ট্যাকে বা মেমোরিতে ইনলাইন থাকে এবং মান কপি হয়; এটি ইনহেরিট করা যায় না এবং ছোট ও অপরিবর্তনীয় ডেটায় উপযোগী।

### Q14: How do constructors and static constructors work?
- **English:** Instance constructors initialize new instances of a type. A `static` constructor initializes type-level static data; it is invoked automatically by the CLR exactly once per AppDomain before any instance is created or any static member is accessed, and is inherently thread-safe.
- **বাংলা:** ইনস্ট্যান্স কনস্ট্রাক্টর অবজেক্ট তৈরি করার সময় ফিল্ড ইনিশিয়ালাইজ করে। `static` কনস্ট্রাক্টর কোনো ক্লাসের সদস্য প্রথমবার ব্যবহারের আগে CLR দ্বারা ঠিক একবারই কল হয় এবং এটি থ্রেড-সেফ।

### Q15: What is the difference between a field and a property?
- **English:** A field is a raw variable declared directly on a type. A property provides controlled accessor methods (`get` and `set`/`init`), enabling validation, computed values, change notification, and interface implementation while preserving encapsulation.
- **বাংলা:** ফিল্ড হলো সরাসরি ক্লাসে ঘোষিত একটি ভেরিয়েবল। প্রোপার্টি `get` ও `set` অ্যাক্সেসরের মাধ্যমে ডেটা ফিল্টারিং, ভ্যালিডেশন বা হিসাব করে মান ফেরত দেয় এবং এনক্যাপসুলেশন বজায় রাখে।

### Q16: What are access modifiers?
- **English:** Access modifiers dictate visibility: `public` (any code), `private` (containing type only), `protected` (containing and derived types), `internal` (same assembly), `protected internal` (same assembly OR derived classes outside), and `private protected` (derived classes within the same assembly only).
- **বাংলা:** Access modifier কোডের দৃশ্যমানতা নির্ধারণ করে: `public` (সবার জন্য), `private` (শুধু নিজের ক্লাসে), `protected` (নিজের ও চাইল্ড ক্লাসে), `internal` (একই অ্যাসেম্বলিতে), `protected internal` (একই অ্যাসেম্বলি অথবা বাইরের চাইল্ড ক্লাসে), এবং `private protected` (একই অ্যাসেম্বলির চাইল্ড ক্লাসে)।

### Q17: What is an `enum`?
- **English:** An `enum` defines a strongly typed set of named integral constants (defaulting to `int`). Enums improve code readability. Bitwise combinations can be created using the `[Flags]` attribute with powers-of-two values.
- **বাংলা:** `enum` হলো নামযুক্ত ইন্টিজার কনস্ট্যান্টের সেট। একাধিক অপশন বিটওয়াইজ সমন্বয় করতে `[Flags]` অ্যাট্রিবিউট ব্যবহার করা হয়।

---

## 1.4 Polymorphism & Method Modifiers

### Q18: What is inheritance? Does C# support multiple class inheritance?
- **English:** Inheritance allows a derived class to acquire members of a base class. C# allows single class inheritance to prevent ambiguity (the Diamond Problem), but allows a class to implement multiple interfaces.
- **বাংলা:** ইনহেরিটেন্সের মাধ্যমে চাইল্ড ক্লাস প্যারেন্ট ক্লাসের আচরণ পায়। C#-এ মাল্টিপল ক্লাস ইনহেরিটেন্স নেই (ডায়মন্ড প্রবলেম এড়াতে), তবে একটি ক্লাস একাধিক ইন্টারফেস ইমপ্লিমেন্ট করতে পারে।

### Q19: Abstract class vs. Interface: what is the difference?
- **English:** An abstract class defines identity ("is-a"), can maintain instance state (fields), constructors, and default implementations, but limits inheritance to one class. An interface defines a behavioral contract ("can-do"), supports multiple implementations on a class, and facilitates loose coupling and mocking in unit tests.
- **বাংলা:** অ্যাবস্ট্রাক্ট ক্লাস একটি সাধারণ ভিত্তি বা পরিচয় দেয় ("is-a"), এতে স্টেট ও কনস্ট্রাক্টর থাকতে পারে, তবে ক্লাস কেবল একটি ইনহেরিট করতে পারে। ইন্টারফেস একটি চুক্তি নির্ধারণ করে ("can-do"), একাধিক ইন্টারফেস বাস্তবায়ন করা যায় এবং টেস্টিং ও লুজ কাপলিং-এ সাহায্য করে।

### Q20: Explain `virtual`, `override`, and `new` (Method Hiding).
- **English:** `virtual` marks a base method as open to polymorphic customization. `override` provides a new implementation that permanently updates the virtual method table (vtable), ensuring the derived method is called even through a base pointer. `new` hides the base method without overriding; dispatch depends purely on the compile-time type of the reference.
- **বাংলা:** `virtual` প্যারেন্ট মেথডকে চাইল্ড ক্লাসে বদলানোর অনুমতি দেয়। `override` মেথডটিকে স্থায়ীভাবে প্রতিস্থাপন করে, ফলে বেজ রেফারেন্স দিয়ে কল করলেও চাইল্ডের কোডই চলে। `new` কিওয়ার্ড মেথডটিকে কেবল লুকিয়ে রাখে; বেজ রেফারেন্সে কাস্ট করলে পুরনো বেজ মেথডটিই কল হয়।

```csharp
public class Base { public virtual void Show() => Console.WriteLine("Base"); }
public class Over : Base { public override void Show() => Console.WriteLine("Over"); }
public class Hide : Base { public new void Show() => Console.WriteLine("Hide"); }

Base b1 = new Over(); b1.Show(); // "Over" (polymorphic)
Base b2 = new Hide(); b2.Show(); // "Base" (hidden member bypassed)
```

### Q21: What are `ref`, `out`, and `in` parameter modifiers?
- **English:** All three pass variables by reference (memory pointer). `ref` requires prior initialization and allows the callee to read and modify the variable. `out` does not require caller initialization but mandates assignment by the callee before returning. `in` passes a reference read-only to avoid copying large structs while guaranteeing immutability.
- **বাংলা:** তিনটিই ভেরিয়েবলকে মেমরি রেফারেন্স হিসেবে পাঠায়। `ref`-এ আগে থেকেই মান থাকা আবশ্যক এবং মেথড তা বদলাতে পারে। `out`-এ আগে মান থাকার দরকার নেই কিন্তু মেথড শেষ করার আগে মান এসাইন করা বাধ্যতামূলক। `in` রেফারেন্স আকারে পাঠায় পারফরম্যান্সের জন্য (লার্জ স্ট্রাক্ট কপি এড়াতে) কিন্তু মান পরিবর্তন করা যায় না (read-only)।

### Q22: What are `params` and optional parameters?
- **English:** `params` allows a method to accept a comma-separated list or array of arguments of a specified type. Optional parameters define default values used when omitted by callers. Defaults are baked into the caller's assembly at compile-time.
- **বাংলা:** `params` একটি মেথডে অনির্দিষ্ট সংখ্যক আর্গুমেন্ট গ্রহণ করতে দেয়। Optional প্যারামিটারে ডিফল্ট মান দেওয়া থাকে যা আর্গুমেন্ট না পাঠালে কার্যকর হয়।

---

## 1.5 Collections, Flow & Generics

### Q23: Arrays vs. `List<T>`: what is the difference?
- **English:** An array (`T[]`) is a fixed-size, contiguous block of memory allocated once. `List<T>` is a dynamic wrapper over an internal array that automatically doubles its capacity when filled. Arrays are ideal for fixed-size allocations; `List<T>` is preferred for collections that grow or shrink.
- **বাংলা:** Array নির্দিষ্ট দৈর্ঘ্যের মেমোরি ব্লক যা পরিবর্তনের সুযোগ নেই। `List<T>` একটি ডায়নামিক কালেকশন যা অভ্যন্তরীণ অ্যারে পূর্ণ হলে স্বয়ংক্রিয়ভাবে দ্বিগুণ ধারণক্ষমতা বৃদ্ধি করে।

### Q24: What are the common .NET collections and their Big-O complexities?
- **English:** `List<T>` provides $O(1)$ index lookup and $O(1)$ amortized append, but $O(n)$ search/remove. `Dictionary<TKey, TValue>` uses a hash table providing $O(1)$ average key lookups and inserts. `HashSet<T>` provides $O(1)$ set membership and duplicate elimination.
- **বাংলা:** `List<T>` ইনডেক্স দিয়ে $O(1)$ অ্যাক্সেস দেয়। `Dictionary<TKey, TValue>` হ্যাশ টেবিল ব্যবহার করে $O(1)$ সময়ে ডেটা সার্চ ও ইনসার্ট করতে পারে। `HashSet<T>` ইউনিক ডেটা যাচাইয়ের জন্য $O(1)$ লুকআপ দেয়।

### Q25: What are Generics and Generic Constraints?
- **English:** Generics allow classes, interfaces, and methods to be parameterized with a type placeholder (`List<T>`), achieving compile-time safety and zero boxing overhead. Constraints (`where T : class`, `where T : struct`, `where T : new()`) restrict valid type arguments so members can be safely accessed.
- **বাংলা:** জেনেরিক্স কোডকে টাইপ-সেফ ও পুনঃব্যবহারযোগ্য করে এবং বক্সিং রোধ করে। `where T : class` বা `where T : new()` এর মতো কনস্ট্রেইন্ট দিয়ে জেনেরিক টাইপের ওপর শর্ত আরোপ করা যায়।

---

# Part 2: Advanced C# Runtime & Language Mechanics

## 2.1 Delegates, Lambdas & Events

### Q26: What is a delegate and what are `Action`, `Func`, and `Predicate`?
- **English:** A delegate is a type-safe object-oriented function pointer holding a reference to a method with a specific signature. `Action<...>` represents a method returning `void`. `Func<..., TResult>` represents a method returning a value. `Predicate<T>` represents a method returning `bool`.
- **বাংলা:** ডেলিগেট হলো একটি টাইপ-সেফ মেথড পয়েন্টার। `Action` কোনো রিটার্ন ভ্যালু ছাড়া মেথড চালায় (`void`)। `Func` কাজ শেষে নির্দিষ্ট টাইপের মান রিটার্ন করে। `Predicate` সত্য বা মিথ্যা (`bool`) রিটার্ন করে।

```csharp
Action<string> log = msg => Console.WriteLine(msg);
Func<int, int, int> add = (a, b) => a + b;
Predicate<int> isEven = n => n % 2 == 0;
```

### Q27: What is an event and how does it prevent memory leaks?
- **English:** An event is an encapsulation layer over a multicast delegate that enforces the publisher-subscriber pattern: external code can only subscribe (`+=`) or unsubscribe (`-=`), while only the publisher can invoke it. Failing to unsubscribe a short-lived subscriber from a long-lived publisher prevents the subscriber from being garbage-collected, causing a memory leak.
- **বাংলা:** ইভেন্ট হলো ডেলিগেটের একটি সুরক্ষিত মোড়ক যাতে বাইরের কোড কেবল সাবস্ক্রাইব (`+=`) বা আনসাবস্ক্রাইব (`-=`) করতে পারে। দীর্ঘস্থায়ী পাবলিশার থেকে স্বল্পস্থায়ী সাবস্ক্রাইবার আনসাবস্ক্রাইব না করলে মেমোরি লিক ঘটে কারণ GC অবজেক্টটিকে মুক্ত করতে পারে না।

---

## 2.2 LINQ & Custom Iterators

### Q28: What is deferred execution in LINQ?
- **English:** Deferred (lazy) execution means a LINQ query pipeline is not executed when defined; it builds an iterator execution plan that runs only when enumerated (e.g., via `foreach`, `ToList()`, `Count()`). This avoids intermediate allocations and allows dynamic query composition.
- **বাংলা:** ডিফার্ড এক্সিকিউশন মানে LINQ কোয়েরি ঘোষণার সাথে সাথে রান হয় না; এটি কেবল একটি পরিকল্পনা তৈরি করে। যখন কোয়েরির ওপর `foreach`, `ToList()`, বা `Count()` চালানো হয়, ঠিক তখনই ফিল্টারিং ঘটে।

### Q29: What is the difference between `IEnumerable<T>` and `IQueryable<T>`?
- **English:** `IEnumerable<T>` executes in-memory using compiled delegates (`Func<T, bool>`), loading full records into application memory before filtering. `IQueryable<T>` parses queries into expression trees (`Expression<Func<T, bool>>`), allowing ORMs (like EF Core) to translate the query into native database SQL before execution.
- **বাংলা:** `IEnumerable<T>` অ্যাপ্লিকেশনের মেমরিতে পুরো ডেটা এনে লোকাল ফিল্টার চালায়। `IQueryable<T>` এক্সপ্রেশন ট্রি ব্যবহার করে কুয়েরিকে সরাসরি SQL-এ রূপান্তর করে ডাটাবেজ সার্ভারে চালায়, ফলে অপ্রয়োজনীয় ডেটা অ্যাপে আসে না।

### Q30: How does `yield return` work under the hood?
- **English:** `yield return` produces elements lazily on demand without allocating an intermediate `List<T>`. The compiler synthesizes a state-machine class implementing `IEnumerator<T>`, suspending execution at each `yield` and resuming on the next `MoveNext()` call.
- **বাংলা:** `yield return` কোনো ইন্টারমিডিয়েট লিস্ট তৈরি না করেই একে একে ডেটা স্ট্রিম করে। কম্পাইলার এর জন্য ব্যাকগ্রাউন্ডে একটি স্টেট-মেশিন ক্লাস তৈরি করে যা `MoveNext()` কলের মাধ্যমে প্রতি ধাপে ডেটা রিটার্ন করে।

---

## 2.3 Memory Management, GC & Resource Cleanup

### Q31: How does Garbage Collection (GC) work in .NET?
- **English:** .NET uses a generational, mark-sweep-compact Garbage Collector. The heap is organized into Generation 0 (short-lived objects), Generation 1 (buffer generation), Generation 2 (long-lived objects), and the Large Object Heap (LOH). GC triggers when Gen 0 fills up, sweeping unreachable objects and compacting surviving objects into older generations.
- **বাংলা:** .NET-এ জেনারেশনভিত্তিক গারবেজ কালেক্টর কাজ করে। হিপ তিনটি প্রজন্মে বিভক্ত: Gen 0 (নতুন অবজেক্ট), Gen 1 (বাফার), এবং Gen 2 (দীর্ঘস্থায়ী অবজেক্ট), এবং ৮৫,০০০ বাইটের বড় অবজেক্টের জন্য LOH। অল্পবয়সী প্রজন্মগুলো ঘন ঘন পরিষ্কার করে অ্যাপ্লিকেশন পজ টাইম হ্রাস করা হয়।

### Q32: What is the Large Object Heap (LOH)?
- **English:** The Large Object Heap (LOH) holds objects $\ge$ 85,000 bytes (primarily large byte arrays and strings). The LOH is collected only during full Gen 2 sweeps and was historically not compacted to prevent high CPU costs, leading to memory fragmentation. Modern .NET supports background collection and optional LOH compaction via `GCSettings.LargeObjectHeapCompactionMode`.
- **বাংলা:** ৮৫,০০০ বাইট বা তার বড় অবজেক্টগুলো LOH-এ বরাদ্দ হয়। LOH মেমোরি সাধারণত ডিফল্টভাবে কম্প্যাক্ট হয় না, যার ফলে মেমোরি ফ্র্যাগমেন্টেশন হতে পারে। বড় বাফার বারবার তৈরি না করে `ArrayPool<T>` ব্যবহার করা উচিত।

### Q33: How do `IDisposable` and the `using` statement manage unmanaged resources?
- **English:** `IDisposable` provides a deterministic pattern to release unmanaged OS resources (file handles, database connections, sockets) immediately. The `using` statement (or `using var`) wraps the resource in a compiler-generated `try-finally` block ensuring `Dispose()` is invoked even if an unhandled exception occurs.
- **বাংলা:** `IDisposable` ইন্টারফেস আনম্যানেজড রিসোর্স (যেমন ফাইল হ্যান্ডেল, ডিবি কানেকশন) সাথে সাথে মুক্ত করার সুনির্দিষ্ট উপায় প্রদান করে। `using` স্টেটমেন্ট একটি `try-finally` ব্লক তৈরি করে নিশ্চয়তা দেয় যে কোডে এরর হলেও `Dispose()` কল হবে।

### Q34: What is a finalizer (`~ClassName`) and why should it be rare?
- **English:** A finalizer is a nondeterministic fallback executed by the GC finalizer thread before an object's memory is reclaimed. Finalizers promote objects to older GC generations, increase GC pause times, and delay memory deallocation. In modern C#, use `SafeHandle` wrappers instead of custom finalizers.
- **বাংলা:** ফাইনালাইজার হলো GC দ্বারা পরিচালিত অনির্ধারিত ক্লিনআপ মেকানিজম। এটি অবজেক্টকে পরের প্রজন্মে ঠেলে দেয় এবং মেমরি মুক্ত করতে দেরি করায়। আধুনিক C#-এ সরাসরি ফাইনালাইজার না লিখে `SafeHandle` বা `IDisposable` ব্যবহার করা উচিত।

---

## 2.4 Asynchronous Programming & Concurrency

### Q35: How do `async` and `await` work internally?
- **English:** The Roslyn compiler rewrites an `async` method into a state-machine struct implementing `IAsyncStateMachine`. When an uncompleted task is awaited, execution yields control back to the caller while the method registers a continuation callback with the task, freeing the calling thread to process other work.
- **বাংলা:** `async` মেথডকে কম্পাইলার একটি স্টেট-মেশিন স্ট্রাক্টে রূপান্তর করে। `await` কলের সময় কাজটি অসম্পূর্ণ থাকলে মেথডটি বর্তমান থ্রেড ছেড়ে দেয় এবং ব্যাকগ্রাউন্ড টাস্ক শেষ হলে কন্টিনিউয়েশন কলের মাধ্যমে বাকি কোড সম্পন্ন করে।

### Q36: What is the difference between `Task` and `Thread`?
- **English:** A `Thread` is a dedicated, heavyweight OS execution unit with its own 1MB stack and high context-switching cost. A `Task` is a lightweight abstraction scheduled onto the managed `ThreadPool`, reusing background worker threads efficiently and supporting compositional combinators like `WhenAll` and `WhenAny`.
- **বাংলা:** `Thread` হলো ওএস লেভেলের ব্যয়বহুল এক্সিকিউশন ইউনিট যা প্রায় ১ মেগাবাইট মেমরি নেয়। `Task` হলো হালকা অ্যাবস্ট্রাকশন যা `ThreadPool`-এর বিদ্যমান থ্রেডগুলো পুনরায় ব্যবহার করে অত্যন্ত দ্রুত ও কম খরচে কাজ চালায়।

### Q37: Why should `async void` generally be avoided?
- **English:** `async void` cannot be awaited, cannot be tracked, and unhandled exceptions bypass `try-catch` blocks and crash the process directly via the synchronization context. The only valid use case for `async void` is top-level UI event handlers. Always return `Task` or `Task<T>`.
- **বাংলা:** `async void` কে `await` করা যায় না এবং এর ভেতর ঘটা এক্সেপশন সাধারণ `try-catch` দিয়ে ধরা যায় না, ফলে পুরো অ্যাপ ক্র্যাশ করে। কেবল UI ইভেন্ট হ্যান্ডলার ছাড়া অন্য কোথাও `async void` লেখা সম্পূর্ণ নিষিদ্ধ; সর্বদা `Task` ব্যবহার করুন।

### Q38: When should you use `ValueTask<T>` instead of `Task<T>`?
- **English:** `Task<T>` is a reference type that always allocates an object on the heap. `ValueTask<T>` is a value type (`struct`) designed to eliminate heap allocation when an asynchronous method completes synchronously most of the time (e.g., returning from an in-memory cache).
- **বাংলা:** `Task<T>` একটি ক্লাস হওয়ায় প্রতিবার হিপ মেমরি নেয়। `ValueTask<T>` একটি স্ট্রাক্ট যা কোনো মেথড বেশিরভাগ সময় ক্যাশ বা সিঙ্ক্রোনাসভাবে রিটার্ন করলে কোনো মেমরি অ্যালোকেশন ছাড়াই কাজ সম্পন্ন করে।

### Q39: What is `ConfigureAwait(false)` and why is it used?
- **English:** By default, `await` captures the current `SynchronizationContext` to resume execution on the originating thread. In library and backend code, using `ConfigureAwait(false)` tells the runtime to resume continuation on any available `ThreadPool` thread, boosting throughput and preventing UI/ASP.NET deadlocks.
- **বাংলা:** ডিফল্টভাবে `await` মূল থ্রেডের `SynchronizationContext` ধরে রাখে। ক্লাস লাইব্রেরি কোডে `ConfigureAwait(false)` ব্যবহার করলে টাস্ক যেকোনো ফ্রি `ThreadPool` থ্রেডে শেষ হতে পারে, যা কর্মক্ষমতা বাড়ায় এবং UI ডেডলক রোধ করে।

### Q40: How do you handle cooperative cancellation with `CancellationToken`?
- **English:** Asynchronous cancellation in .NET is cooperative: callers supply a `CancellationToken` produced by a `CancellationTokenSource` (CTS), and long-running tasks periodically check `token.ThrowIfCancellationRequested()` or pass it to underlying async APIs.
- **বাংলা:** .NET-এ ক্যান্সেলেশন হলো কো-অপারেটিভ: কলার একটি `CancellationToken` পাঠায় এবং মেথডটি নিজে থেকে `ThrowIfCancellationRequested()` চেক করে কাজ থামিয়ে দেয়।

---

## 2.5 Thread Synchronization & Thread-Safety

### Q41: What is a Race Condition and how is it prevented?
- **English:** A race condition occurs when concurrent threads access and modify shared state without synchronization, producing unpredictable outcomes based on execution timing. It is prevented using thread synchronization primitives (`lock`, `SemaphoreSlim`), thread-safe collections, or immutable data models.
- **বাংলা:** যখন একাধিক থ্রেড একই সাথে কোনো শেয়ার্ড ডেটা পরিবর্তন করতে চায় এবং টাইমিংয়ের ওপর ভুল ফলাফল নির্ভর করে, তাকে রেস কন্ডিশন বলে। এটি রোধ করতে `lock`, `SemaphoreSlim` বা ইমিউটেবল অবজেক্ট ব্যবহার করা হয়।

### Q42: What is `lock` and what should you avoid locking on?
- **English:** `lock` provides exclusive synchronization using `Monitor.Enter`/`Monitor.Exit` on an object reference. Always lock on a `private readonly object _lock = new();`. Never lock on `this`, public objects, types (`typeof(X)`), or strings, as external code could lock on the same reference and cause deadlocks. Never `await` inside a `lock`.
- **বাংলা:** `lock` নির্দিষ্ট অবজেক্টে একটি সময়ে কেবল একটি থ্রেডকে কোড ব্লকে ঢুকতে দেয়। সর্বদা একটি `private readonly object` লক করুন। কখনো `this`, পাবলিক অবজেক্ট বা স্ট্রিং লক করবেন না। `lock`-এর ভেতরে কখনো `await` কল করা যায় না।

### Q43: How do `lock`, `SemaphoreSlim`, and `Interlocked` compare?
- **English:** `lock` is for synchronous mutual exclusion on a single thread. `SemaphoreSlim` limits concurrent access to a resource across threads and supports `WaitAsync()` for asynchronous code. `Interlocked` provides atomic, lock-free CPU hardware instructions (`Increment`, `Exchange`) with maximum performance.
- **বাংলা:** `lock` সিঙ্ক্রোনাস কোডে এক্সক্লুসিভ লক দেয়। `SemaphoreSlim` অ্যাসিনক্রোনাস কোডে `WaitAsync()` সমর্থন করে এবং নির্দিষ্ট সংখ্যক থ্রেড একসাথে চালাতে পারে। `Interlocked` কোনো লক ছাড়াই সরাসরি CPU হার্ডওয়্যারের মাধ্যমে অতি দ্রুত অ্যাটমিক অপারেশন সম্পন্ন করে।

### Q44: What is `ConcurrentDictionary<TKey, TValue>`?
- **English:** A thread-safe collection using fine-grained bucket-level locking. Multiple reader threads can read concurrently without blocking, while writes lock only the relevant bucket. Multi-step business logic should utilize atomic methods like `GetOrAdd` or `AddOrUpdate`.
- **বাংলা:** এটি একটি থ্রেড-সেফ ডিকশনারি যা বাকেট-লেভেল লকিং ব্যবহার করে। রিডাররা কোনো ব্লক ছাড়াই একসাথে ডেটা পড়তে পারে এবং রাইটাররা কেবল নির্দিষ্ট বাকেট লক করে।

---

## 2.6 Equality, Comparison & Copying

### Q45: What is the difference between `==` and `Equals()`?
- **English:** `==` is an operator resolved at compile-time (checking reference identity for classes unless explicitly overloaded). `Equals()` is a virtual method resolved at runtime (checking reference identity by default on `object`, but overridden by types like `string` and `record` for value equality).
- **বাংলা:** `==` হলো কম্পাইল-টাইম অপারেটর যা ডিফল্টভাবে মেমরি রেফারেন্স তুলনা করে (ওভারলোড না থাকলে)। `Equals()` হলো ভার্চুয়াল মেথড যা রানটাইমে অবজেক্টের কনটেন্ট বা মান সমান কি না তা যাচাই করতে ওভাররাইড করা হয়।

### Q46: What is the contract between `Equals()` and `GetHashCode()`?
- **English:** If two objects are equal according to `Equals()`, they **must** return the same hash code from `GetHashCode()`. Hash-based collections (`Dictionary`, `HashSet`) use the hash code to assign buckets; unequal hash codes prevent equal objects from ever being matched.
- **বাংলা:** দুটি অবজেক্ট `Equals()`-এ সমান হলে তাদের `GetHashCode()` অবশ্যই একই ইন্টিজার রিটার্ন করতে হবে। তা না হলে `Dictionary` বা `HashSet`-এ ডেটা খুঁজে পাওয়া অসম্ভব হয়ে যাবে।

### Q47: What is the difference between a shallow copy and a deep copy?
- **English:** A shallow copy (`MemberwiseClone()`) duplicates value-type fields, but references point to the same original heap instances. A deep copy recursively duplicates the entire object graph so modifications to the clone never impact the original.
- **বাংলা:** শ্যালো কপিতে অবজেক্টের ফিল্ডগুলো কপি হলেও ভেতরের রেফারেন্স অবজেক্টগুলো আগের মূল মেমরি শেয়ার করে। ডিপ কপিতে মূল অবজেক্টের পাশাপাশি ভেতরের সমস্ত রেফারেন্সড অবজেক্টের সম্পূর্ণ নতুন কপি তৈরি হয়।

### Q48: What are Covariance (`out`) and Contravariance (`in`)?
- **English:** Covariance (`IEnumerable<out T>`) preserves assignment compatibility, allowing a more-derived type to be used where a less-derived type is expected (read-only output). Contravariance (`Action<in T>`) allows a less-derived type to be used where a more-derived type is expected (write-only input).
- **বাংলা:** Covariance (`out`) চাইল্ড টাইপকে প্যারেন্ট টাইপের জায়গায় ব্যবহার করতে দেয় (আউটপুট বা রিড-অনলি)। Contravariance (`in`) প্যারেন্ট টাইপকে চাইল্ড টাইপের ইনপুটে ব্যবহার করতে দেয় (ইনপুট বা রাইট-অনলি)।

---

## 2.7 Reflection, Attributes & Design Patterns

### Q49: What is Reflection and what are its performance costs?
- **English:** Reflection allows runtime type inspection, dynamic invocation, and metadata discovery via `System.Type` and `Assembly`. It incurs performance penalties due to late binding and type-lookup overhead, and complicates trimming and Native AOT compilation.
- **বাংলা:** রিফ্লেকশন রানটাইমে টাইপের মেটাডাটা পরীক্ষা ও ডায়নামিক মেথড কল করতে দেয়। এটি দেরিতে মেথড কল ও টাইপ সার্চ করার কারণে পারফরম্যান্স কমায় এবং Native AOT তৈরিতে বাধা দেয়।

### Q50: What are Attributes?
- **English:** Attributes attach declarative metadata to classes, methods, or properties. They do not execute behavior on their own; instead, frameworks (like EF Core, ASP.NET Core) or reflection routines read that metadata to modify behavior dynamically.
- **বাংলা:** অ্যাট্রিবিউট ক্লাসে বা মেথডে ঘোষণামূলক মেটাডাটা যুক্ত করে। ফ্রেমওয়ার্ক বা রিফ্লেকশন কোড এই মেটাডাটা পড়ে নির্দিষ্ট আচরণ নির্ধারণ করে।

### Q51: What is an Expression Tree (`Expression<Func<T, bool>>`)?
- **English:** An expression tree represents code as a data structure (AST) that can be inspected, traversed, and modified at runtime. Database providers (like EF Core) parse expression trees to translate C# lambda conditions directly into SQL queries.
- **বাংলা:** এক্সপ্রেশন ট্রি কোডকে একটি ডেটা স্ট্রাকচার হিসেবে প্রকাশ করে। ORM (যেমন EF Core) এই ট্রি পড়ে C# ল্যাম্বডাকে সরাসরি ডাটাবেজ SQL কোয়েরিতে রূপান্তর করে।

### Q52: Explain Singleton, Factory, and Strategy patterns in C#.
- **English:** Singleton guarantees a class has only one instance with a global access point. Factory centralizes object creation to decouple callers from concrete implementations. Strategy encapsulates interchangeable algorithms behind a common interface, allowing behavior to switch at runtime.
- **বাংলা:** Singleton পুরো অ্যাপ্লিকেশনে একটিমাত্র শেয়ার্ড অবজেক্ট নিশ্চিত করে। Factory নির্দিষ্ট ক্লাসের অবজেক্ট তৈরির দায়িত্ব এক জায়গায় রাখে। Strategy একই ইন্টারফেসের আড়ালে বিভিন্ন অ্যালগরিদম রেখে রানটাইমে আচরণ পরিবর্তনের সুযোগ দেয়।

---

## 2.8 ASP.NET Core & Runtime Error Handling

### Q53: Explain the three service lifetimes in ASP.NET Core DI.
- **English:** `Transient` creates a brand new instance every time it is injected. `Scoped` creates one shared instance per HTTP request scope. `Singleton` creates a single instance on initial resolution that lives throughout the application's entire process lifetime. Capturing a `Scoped` service inside a `Singleton` causes a captive dependency bug.
- **বাংলা:** `Transient` প্রতিবার নতুন অবজেক্ট তৈরি করে। `Scoped` প্রতিটি HTTP রিকোয়েস্টের জন্য একটি নির্দিষ্ট ইনস্ট্যান্স রাখে। `Singleton` পুরো অ্যাপ্লিকেশনে একটিমাত্র ইনস্ট্যান্স রাখে। সিঙ্গেলটনের ভেতর স্কোপড সার্ভিস ইনজেক্ট করা ক্ষতিকর (Captive Dependency)।

### Q54: How does custom middleware work and where should it be placed?
- **English:** Middleware components form an execution pipeline handling HTTP requests and responses sequentially. Each component receives `HttpContext` and calls `await next()`. Error handling and HTTPS redirection should be placed early at the top, while routing, authentication, and endpoint handlers are placed downstream.
- **বাংলা:** মিডলওয়্যার রিকোয়েস্ট ও রেসপন্স পাইপলাইনে ধারাবাহিকভাবে কাজ করে। প্রতিটি মিডলওয়্যার `HttpContext` গ্রহণ করে এবং `next()` কল করে পরেরটিতে পাঠায়। এরর হ্যান্ডলিং ও রিডাইরেকশন পাইপলাইনের শুরুতে এবং রাউটিং ও অথেনটিকেশন নিচের দিকে রাখা হয়।

### Q55: What is the difference between `throw;` and `throw ex;`?
- **English:** `throw;` preserves the original exception call stack trace from where the exception was initially triggered, allowing developers to see the true origin in logs. `throw ex;` resets the stack trace, making the catch block appear as the root cause and hiding the actual error call stack. Always prefer `throw;`.
- **বাংলা:** `throw;` এক্সেপশনের মূল কল স্ট্যাক অক্ষুণ্ন রেখে পুনরায় থ্রো করে, যা আসল ত্রুটির লাইন খুঁজে পেতে সাহায্য করে। অপরদিকে `throw ex;` পূর্বের কল স্ট্যাক মুছে ফেলে বর্তমান ক্যাচ ব্লকের লাইনকে ত্রুটির শুরু হিসেবে দেখায়। তাই সবসময় `throw;` ব্যবহার করা উচিত।

```csharp
try {
    ProcessOrder();
} catch (Exception ex) {
    _logger.LogError(ex, "Failed order");
    throw; // Preserves complete original stack trace
}
```

---

# Part 3: Core Language Details & Modern C# Features (C# 9 to C# 12+)

## 3.1 Core Language Mechanics

### Q56: What is an extension method?
- **English:** An extension method allows developers to add new methods to existing types without modifying their source code or creating derived classes. Declared as a `static` method inside a non-nested `static` class using the `this` modifier on the first parameter.
- **বাংলা:** এক্সটেনশন মেথড মূল সোর্স কোড বা ইনহেরিটেন্স ছাড়াই বিদ্যমান ক্লাসে নতুন মেথড যুক্ত করার সুযোগ দেয়। এটি একটি `static` ক্লাসের ভেতর `static` মেথড হিসেবে ডিক্লেয়ার করা হয় এবং প্রথম প্যারামিটারের আগে `this` থাকে।

```csharp
public static class StringExtensions {
    public static bool HasText(this string? str) => !string.IsNullOrWhiteSpace(str);
}
// Usage: if ("hello".HasText()) { ... }
```

### Q57: What is an indexer?
- **English:** An indexer enables an object to be indexed like an array using `[]` syntax. Declared using the `this` keyword and can accept any parameter type (e.g., `int`, `string`), supporting custom `get` and `set` validation logic.
- **বাংলা:** ইনডেক্সার কোনো অবজেক্টকে অ্যারের মতো `[]` ব্র্যাকেট সিনট্যাক্স দিয়ে ডেটা অ্যাক্সেস করতে দেয়। এটি `this` কিওয়ার্ড দিয়ে ডিক্লেয়ার করা হয়।

```csharp
public class KeyStore {
    private readonly Dictionary<string, string> _data = new();
    public string this[string key] {
        get => _data[key];
        set => _data[key] = value;
    }
}
```

### Q58: What is operator overloading?
- **English:** Operator overloading defines how built-in operators (like `+`, `-`, `==`, `!=`) behave when applied to user-defined types (e.g., vectors, complex numbers). Must be declared as `public static` and symmetric comparison operators must be overloaded in pairs.
- **বাংলা:** অপারেটর ওভারলোডিং কোনো কাস্টম ক্লাসে বিল্ট-ইন অপারেটরের (`+`, `==`) নিজস্ব আচরণ নির্ধারণ করে। এটি `public static` মেথড হিসেবে তৈরি করতে হয়।

### Q59: What are partial classes and partial methods?
- **English:** The `partial` keyword allows a class, struct, or interface definition to be split across multiple source files, unified into a single type by the compiler during build time. Crucial for separating autogenerated code (e.g., EF Core, source generators) from human-maintained business logic.
- **বাংলা:** `partial` কিওয়ার্ডের মাধ্যমে একটি ক্লাসকে একাধিক ফাইলে ভাগ করে লেখা যায় এবং বিল্ড টাইমে কম্পাইলার তাদের একসাথে যুক্ত করে। কোড জেনারেটরের সাথে কাজ করতে এটি অপরিহার্য।

### Q60: What are tuples and deconstruction?
- **English:** ValueTuples `(int, string)` provide lightweight, stack-allocated data structures for returning multiple values from a method without declaring a dedicated class. Deconstruction unpacks tuple elements or object properties into separate variables.
- **বাংলা:** টাপল একাধিক ডেটা ভ্যালু কোনো আলাদা ক্লাস না বানিয়েই মেথড থেকে রিটার্ন করতে দেয়। ডিকনস্ট্রাকশন টাপলের মানগুলোকে আলাদা ভ্যারিয়েবলে ভেঙে সংরক্ষণ করে।

```csharp
(string Name, int Age) GetUser() => ("Rahim", 28);
var (name, age) = GetUser();
```

### Q61: What is pattern matching in modern C#?
- **English:** Pattern matching tests expressions against specific shapes, types, or conditions cleanly. Supports type patterns, constant patterns, relational patterns (`> 100`), property patterns (`{ Status: "Active" }`), and concise switch expressions.
- **বাংলা:** প্যাটার্ন ম্যাচিং কোনো ডেটার টাইপ, মান বা প্রোপার্টির শর্ত খুব সহজে যাচাই করতে দেয়। আধুনিক C#-এর switch expression কোডকে অনেক সংক্ষিপ্ত করে।

```csharp
string Describe(object val) => val switch {
    int n when n > 0 => "Positive number",
    string { Length: > 0 } s => $"Text: {s}",
    null => "Null object",
    _ => "Unknown"
};
```

### Q62: What is a local function vs. a lambda expression?
- **English:** A local function is a named function declared within the scope of an enclosing method; it is compiled into a regular private method, supports recursion cleanly, and avoids delegate allocation when called directly. A lambda expression produces an anonymous delegate instance or expression tree.
- **বাংলা:** লোকাল ফাংশন মেথডের ভেতর ডিক্লেয়ার করা নামযুক্ত ফাংশন যা কোনো বাড়তি ডেলিগেট অবজেক্ট তৈরি না করেই সরাসরি রান হতে পারে। ল্যাম্বডা সাধারণত ডেলিগেট তৈরিতে ব্যবহৃত হয়।

---

## 3.2 Modern C# Features

### Q63: What is a `record` and a `record struct`?
- **English:** A `record` is a type offering compiler-synthesized value-based equality, formatted string output (`ToString()`), and non-destructive cloning (`with` expressions). `record class` is a heap-allocated reference type; `record struct` is a stack-allocated value-type record.
- **বাংলা:** `record` হলো C# ৯-এর একটি চমৎকার ফিচার যা মান-ভিত্তিক সমতা (value-based equality) এবং `with` দিয়ে অবজেক্ট ক্লোন করার সুবিধা দেয়। `record struct` স্ট্যাকে থাকা ভ্যালু টাইপ রেকর্ড।

```csharp
public record Person(string Name, int Age);
var p1 = new Person("Karim", 30);
var p2 = p1 with { Age = 31 }; // Non-destructive copy
```

### Q64: What is an `init`-only property?
- **English:** An `init` accessor allows a property to be assigned only during object construction or via object initializers (`new Person { Name = "X" }`). Once construction completes, the property becomes completely immutable.
- **বাংলা:** `init` অ্যাক্সেসর অবজেক্ট তৈরির সময় মান নির্ধারণ করতে দেয়, তবে একবার তৈরি হয়ে গেলে এটি অপরিবর্তনশীল (read-only) হয়ে যায়।

### Q65: What are `required` members in C# 11?
- **English:** The `required` modifier forces callers to initialize the property during object creation via object initializers or constructor contracts, causing a compile-time error if omitted.
- **বাংলা:** `required` মডিফায়ার কোনো প্রোপার্টি অবজেক্ট তৈরির সময় ইনিশিয়ালাইজ করা বাধ্যতামূলক করে; না দিলে কম্পাইল এরর দেয়।

### Q66: What are Primary Constructors in C# 12?
- **English:** Primary constructors allow parameters to be declared directly in the class or struct header. These parameters are accessible throughout the entire body of the class for field initialization or method execution.
- **বাংলা:** প্রাইমারি কনস্ট্রাক্টর ক্লাসের নামের পাশেই প্যারামিটার ঘোষণা করতে দেয়, যা ক্লাসের যেকোনো মেথডে বা ফিল্ডে সরাসরি ব্যবহার করা যায়।

```csharp
public class UserService(IUserRepository repo, ILogger logger) {
    public void Delete(int id) => repo.Delete(id);
}
```

### Q67: What are Collection Expressions in C# 12?
- **English:** Collection expressions provide a unified bracket syntax `[1, 2, 3]` to create arrays, `Span<T>`, `List<T>`, or immutable sets with identical clean syntax, supporting the spread operator (`..`) to concatenate collections.
- **বাংলা:** কালেকশন এক্সপ্রেশন `[1, 2, 3]` ব্র্যাকেট দিয়ে অ্যারে, লিস্ট বা স্প্যান তৈরির একীভূত সিনট্যাক্স দেয় এবং স্প্রেড অপারেটর (`..`) দিয়ে একাধিক কালেকশন সহজে যোগ করতে পারে।

```csharp
int[] a = [1, 2];
int[] b = [3, 4];
int[] combined = [..a, ..b, 5]; // [1, 2, 3, 4, 5]
```

### Q68: What are Raw String Literals (`"""`) in C# 11?
- **English:** Raw string literals use three or more quotes `"""` to represent multiline strings containing JSON, SQL, or XML without requiring any escape sequences (`\n`, `\"`), respecting whitespace and indentation cleanly.
- **বাংলা:** ট্রিপল কোট `"""` দিয়ে মাল্টিলাইন স্ট্রিং, JSON বা SQL কোনো ব্যাকস্ল্যাশ এস্কেপ ছাড়াই সরাসরি নিখুঁতভাবে লেখা যায়।

---

# Part 4: High-Performance Memory Engineering (Span, Memory & Pools)

### Q69: What is `Span<T>` and `ReadOnlySpan<T>`?
- **English:** `Span<T>` is a stack-only `ref struct` representing a contiguous region of arbitrary memory (managed arrays, native memory, or stack buffers). It enables zero-allocation slicing and high-speed data parsing without creating intermediate string or array copies.
- **বাংলা:** `Span<T>` হলো একটি স্ট্যাক-অনলি `ref struct` যা কোনো নতুন মেমরি কপি বা হিপ অ্যালোকেশন ছাড়াই মেমরির যেকোনো অংশের ওপর পয়েন্টার হিসেবে কাজ করে অতি দ্রুত ডেটা প্রসেস করতে দেয়।

### Q70: Why can't `Span<T>` be used across `await` and what is `Memory<T>`?
- **English:** Because `Span<T>` is a `ref struct`, it can only live on the execution stack and cannot be boxed onto the heap. When an `async` method hits `await`, its state machine is pushed to the heap, which is illegal for `Span<T>`. `Memory<T>` is a heap-compatible abstraction that wraps contiguous memory safely across asynchronous yields.
- **বাংলা:** `Span<T>` কেবল স্ট্যাকে থাকতে পারে এবং হিপে যাওয়া নিষেধ। `async` মেথডে `await` হলে স্টেট হিপে জমা হয়, তাই `Span<T>` অ্যাসিনক্রোনাস কলের মাঝে ব্যবহার করা যায় না। এর পরিবর্তে হিপ-উপযোগী `Memory<T>` ব্যবহার করতে হয়।

### Q71: How does `ArrayPool<T>` improve performance in high-throughput applications?
- **English:** `ArrayPool<T>.Shared` rents reusable managed arrays from a shared pool and returns them after use, drastically reducing the rate of Gen 0 and LOH memory allocations and preventing GC pauses in network servers and streaming pipelines.
- **বাংলা:** `ArrayPool<T>` মেমরিতে অ্যারে বারবার তৈরি না করে একটি শেয়ার্ড পুল থেকে ভাড়া নেয় এবং কাজ শেষে ফেরত নেয়, ফলে গারবেজ কালেকশনের ওপর চাপ সম্পূর্ণ দূর হয়।

```csharp
byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);
try {
    // Process buffer...
} finally {
    ArrayPool<byte>.Shared.Return(buffer);
}
```

---

# Part 5: Production Asynchronous & Memory Challenges

## Challenge 1: Bounded-Concurrency Async Map

### The Problem
Implement an asynchronous mapping function that runs over a collection with bounded concurrency, preserving original input order and propagating cancellations safely.

### Production Solution
```csharp
public static async Task<TResult[]> SelectAsync<T, TResult>(
    IReadOnlyList<T> source,
    int maxConcurrency,
    Func<T, CancellationToken, Task<TResult>> selector,
    CancellationToken cancellationToken = default)
{
    ArgumentNullException.ThrowIfNull(source);
    ArgumentNullException.ThrowIfNull(selector);
    ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxConcurrency);

    var results = new TResult[source.Count];
    using var gate = new SemaphoreSlim(maxConcurrency);

    var tasks = new Task[source.Count];
    for (int i = 0; i < source.Count; i++) {
        int index = i;
        tasks[index] = RunOneAsync(index);
    }

    await Task.WhenAll(tasks).ConfigureAwait(false);
    return results;

    async Task RunOneAsync(int index) {
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try {
            results[index] = await selector(source[index], cancellationToken).ConfigureAwait(false);
        } finally {
            gate.Release();
        }
    }
}
```

---

## Challenge 2: Timeout Without Losing Caller Cancellation

### The Problem
Execute an asynchronous operation with a timeout while distinguishing whether a cancellation was triggered by the timeout or the caller, preventing orphan tasks.

### Production Solution
```csharp
public static async Task<T> WithTimeoutAsync<T>(
    Func<CancellationToken, Task<T>> operation,
    TimeSpan timeout,
    CancellationToken callerToken = default)
{
    ArgumentNullException.ThrowIfNull(operation);
    using var timeoutCts = new CancellationTokenSource(timeout);
    using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(callerToken, timeoutCts.Token);

    try {
        return await operation(linkedCts.Token).ConfigureAwait(false);
    } catch (OperationCanceledException) when (callerToken.IsCancellationRequested) {
        throw; // Preserves caller cancellation
    } catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested) {
        throw new TimeoutException($"Operation timed out after {timeout}.");
    }
}
```

---

## Challenge 3: Bounded Producer/Consumer Pipeline with `Channel<T>`

### The Problem
Read an unbounded stream of items, transform them concurrently with worker tasks, and consume results with strict backpressure and graceful failure cancellation.

### Production Solution
```csharp
using System.Threading.Channels;

public static async Task ProcessPipelineAsync<T, TResult>(
    IAsyncEnumerable<T> source,
    int workerCount,
    int bufferCapacity,
    Func<T, CancellationToken, Task<TResult>> transform,
    Func<TResult, CancellationToken, ValueTask> consume,
    CancellationToken cancellationToken = default)
{
    var channel = Channel.CreateBounded<T>(new BoundedChannelOptions(bufferCapacity) {
        FullMode = BoundedChannelFullMode.Wait,
        SingleWriter = true
    });
    using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
    var token = linkedCts.Token;

    async Task ProduceAsync() {
        Exception? error = null;
        try {
            await foreach (var item in source.WithCancellation(token).ConfigureAwait(false))
                await channel.Writer.WriteAsync(item, token).ConfigureAwait(false);
        } catch (Exception ex) { error = ex; throw; }
        finally { channel.Writer.TryComplete(error); }
    }

    async Task WorkerAsync() {
        await foreach (var item in channel.Reader.ReadAllAsync(token).ConfigureAwait(false)) {
            var result = await transform(item, token).ConfigureAwait(false);
            await consume(result, token).ConfigureAwait(false);
        }
    }

    var workers = Enumerable.Range(0, workerCount).Select(_ => WorkerAsync());
    await Task.WhenAll(workers.Append(ProduceAsync())).ConfigureAwait(false);
}
```

---

## Challenge 4: Single-Flight Concurrent Request Deduplication

### The Problem
When hundreds of concurrent requests arrive for the exact same cache key, execute the backend fetch operation only once and share the result among all awaiting callers.

### Production Solution
```csharp
using System.Collections.Concurrent;

public sealed class AsyncSingleFlight<TKey, TValue> where TKey : notnull {
    private readonly ConcurrentDictionary<TKey, Lazy<Task<TValue>>> _inFlight = new();

    public async Task<TValue> GetAsync(TKey key, Func<TKey, Task<TValue>> factory) {
        var lazy = _inFlight.GetOrAdd(key, k =>
            new Lazy<Task<TValue>>(() => factory(k), LazyThreadSafetyMode.ExecutionAndPublication));

        try {
            return await lazy.Value.ConfigureAwait(false);
        } finally {
            ((ICollection<KeyValuePair<TKey, Lazy<Task<TValue>>>>)_inFlight)
                .Remove(new KeyValuePair<TKey, Lazy<Task<TValue>>>(key, lazy));
        }
    }
}
```

---

## Challenge 5: Stream Large File Using Pooled Memory

### The Problem
Stream or copy massive binary payloads between streams without triggering GC Gen 0 or LOH memory pressure by utilizing rented array buffers.

### Production Solution
```csharp
using System.Buffers;

public static async Task CopyPooledAsync(
    Stream source, Stream destination,
    int bufferSize = 64 * 1024,
    CancellationToken cancellationToken = default)
{
    byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
    try {
        int bytesRead;
        while ((bytesRead = await source.ReadAsync(buffer.AsMemory(0, bufferSize), cancellationToken).ConfigureAwait(false)) > 0) {
            await destination.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken).ConfigureAwait(false);
        }
    } finally {
        ArrayPool<byte>.Shared.Return(buffer);
    }
}
```

---

## Challenge 6: Zero-Allocation Delimited String Parser

### The Problem
Parse delimited integer tokens (e.g., `"120,450"`) without allocating substring copies using `ReadOnlySpan<char>`.

### Production Solution
```csharp
public static bool TryParsePair(ReadOnlySpan<char> line, out int first, out int second) {
    first = 0; second = 0;
    int comma = line.IndexOf(',');
    if (comma < 0) return false;

    return int.TryParse(line[..comma], out first) &&
           int.TryParse(line[(comma + 1)..], out second);
}
```

---

## Challenge 7: Async Enumeration with Cancellation & Cleanup

### The Problem
Stream items asynchronously while guaranteeing proper disposal of resources when enumeration is halted early.

### Production Solution
```csharp
using System.Runtime.CompilerServices;

public static async IAsyncEnumerable<T> TakeUntilAsync<T>(
    IAsyncEnumerable<T> source,
    Func<T, bool> predicate,
    [EnumeratorCancellation] CancellationToken cancellationToken = default)
{
    await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)) {
        yield return item;
        if (predicate(item)) yield break;
    }
}
```

---

## Challenge 8: Hot-Path Allocation Optimization & `stackalloc`

### The Problem
Format diagnostic telemetry without heap allocations in time-critical loops.

### Production Solution
```csharp
public static void LogFast(int id, int code) {
    Span<char> buffer = stackalloc char[32];
    buffer[0] = 'I'; buffer[1] = 'D'; buffer[2] = ':';
    id.TryFormat(buffer[3..], out int written1);
    // Fast zero-allocation formatting directly to stack
}
```

---

## Challenge 9: Diagnosing & Fixing Event Subscription Leaks

### The Problem
A short-lived view model subscribes to a singleton domain service event and is never collected by the GC.

### The Fix
```csharp
public sealed class OrderViewModel : IDisposable {
    private readonly EventBroker _broker;
    public OrderViewModel(EventBroker broker) {
        _broker = broker;
        _broker.OrderUpdated += OnOrderUpdated;
    }
    private void OnOrderUpdated(object? sender, EventArgs e) { /* Update state */ }
    public void Dispose() {
        _broker.OrderUpdated -= OnOrderUpdated; // Mandatory unhook to prevent memory leaks
    }
}
```

---

# Part 6: Common Coding & Algorithm Interview Problems

## Problem 1: Reverse a String
```csharp
static string ReverseString(string text) {
    if (string.IsNullOrEmpty(text)) return text;
    char[] chars = text.ToCharArray();
    Array.Reverse(chars);
    return new string(chars);
}
```
*Complexity:* $O(n)$ time, $O(n)$ space. *(For Unicode emoji grapheme clusters, use `StringInfo.GetTextElementEnumerator`).*

---

## Problem 2: Palindrome Validation
```csharp
static bool IsPalindrome(string text) {
    if (string.IsNullOrEmpty(text)) return true;
    int left = 0, right = text.Length - 1;
    while (left < right) {
        if (text[left] != text[right]) return false;
        left++; right--;
    }
    return true;
}
```
*Complexity:* $O(n)$ time, $O(1)$ auxiliary space.

---

## Problem 3: Find Maximum in an Array
```csharp
static int FindMax(int[] values) {
    if (values == null || values.Length == 0)
        throw new ArgumentException("Array must not be empty.");
    int max = values[0];
    for (int i = 1; i < values.Length; i++)
        if (values[i] > max) max = values[i];
    return max;
}
```
*Complexity:* $O(n)$ time, $O(1)$ space.

---

## Problem 4: Binary Search
```csharp
static int BinarySearch(int[] values, int target) {
    int low = 0, high = values.Length - 1;
    while (low <= high) {
        int mid = low + (high - low) / 2; // Prevents integer overflow
        if (values[mid] == target) return mid;
        if (values[mid] < target) low = mid + 1;
        else high = mid - 1;
    }
    return -1;
}
```
*Complexity:* $O(\log n)$ time, $O(1)$ space.

---

## Problem 5: Remove Duplicates Preserving Order
```csharp
static List<T> DistinctPreservingOrder<T>(IEnumerable<T> items) {
    var seen = new HashSet<T>();
    var result = new List<T>();
    foreach (var item in items)
        if (seen.Add(item)) result.Add(item);
    return result;
}
```
*Complexity:* $O(n)$ time, $O(n)$ space.

---

# Part 7: Code Review Checklist & Senior Interview Strategy

### Production Engineering Review Checklist
- **Concurrency & Backpressure:** Is concurrency bounded with a semaphore or bounded channel? Are large inputs protected against unbounded memory buffering?
- **Cancellation & Timeout:** Is cancellation cooperative? Are timeouts properly linked with `CreateLinkedTokenSource`?
- **Resource Ownership:** Does every rented `ArrayPool<T>` buffer get returned in a `finally` block? Are event handlers unsubscribed in `Dispose()`?
- **Async Hygiene:** Are all asynchronous operations awaited? Is `ConfigureAwait(false)` applied in class libraries? Are deadlocks from `.Result` or `.Wait()` eliminated?
- **Allocation Profile:** Are high-frequency hot paths using `Span<T>`, `ValueTask<T>`, or `ArrayPool<T>` instead of allocating temporary heap garbage?

### Strategic Interview Advice
1. **Lead with Trade-Offs:** Senior candidates don't just quote definitions—they explain the trade-offs (e.g., *"ValueTask eliminates heap allocations for cached hits, but adds struct copying overhead and usage restrictions if awaited multiple times"*).
2. **Clarify Constraints First:** In coding rounds, always clarify edge cases (null inputs, empty sets, thread safety, memory boundaries) before writing the first line of code.
3. **Connect Theory to CLR Mechanics:** When discussing concepts like strings, boxing, or asynchronous workflows, always mention what happens under the hood (Heap vs. Stack, vtable dynamic dispatch, IL compilation, state machines).

---
*End of Master C# / .NET Core Interview Guide*
