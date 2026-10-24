# Top Senior .NET Interview Questions & Answers: Memory Management & Garbage Collection (GC)

This guide contains the most critical interview questions on .NET Memory Management, Stack vs. Heap, Garbage Collection, and CLR internals. Each answer is written in **simple, conversational English** and paired with **বাংলা ব্যাখ্যা** and practical C# code examples.

---

## Quick Navigation

- [Top Senior .NET Interview Questions \& Answers: Memory Management \& Garbage Collection (GC)](#top-senior-net-interview-questions--answers-memory-management--garbage-collection-gc)
  - [Quick Navigation](#quick-navigation)
    - [Q1: Stack vs. Heap: Are value types always stored on the Stack?](#q1-stack-vs-heap-are-value-types-always-stored-on-the-stack)
      - [🎯 Quick Answer (English):](#-quick-answer-english)
      - [🇧🇩 বাংলায় ব্যাখ্যা:](#-বাংলায়-ব্যাখ্যা)
      - [💻 Code Demonstration:](#-code-demonstration)
    - [Q2: How does the Garbage Collector work under the hood?](#q2-how-does-the-garbage-collector-work-under-the-hood)
      - [🎯 Quick Answer (English):](#-quick-answer-english-1)
      - [🇧🇩 বাংলায় ব্যাখ্যা:](#-বাংলায়-ব্যাখ্যা-1)
      - [💻 Visual Phases:](#-visual-phases)
    - [Q3: What is the Large Object Heap (LOH) and how to avoid fragmentation?](#q3-what-is-the-large-object-heap-loh-and-how-to-avoid-fragmentation)
      - [🎯 Quick Answer (English):](#-quick-answer-english-2)
      - [🇧🇩 বাংলায় ব্যাখ্যা:](#-বাংলায়-ব্যাখ্যা-2)
      - [💻 Code Demonstration:](#-code-demonstration-1)
    - [Q4: What is the Pinned Object Heap (POH) in .NET 5+?](#q4-what-is-the-pinned-object-heap-poh-in-net-5)
      - [🎯 Quick Answer (English):](#-quick-answer-english-3)
      - [🇧🇩 বাংলায় ব্যাখ্যা:](#-বাংলায়-ব্যাখ্যা-3)
      - [💻 Code Demonstration:](#-code-demonstration-2)
    - [Q5: How do you implement the Standard Dispose Pattern?](#q5-how-do-you-implement-the-standard-dispose-pattern)
      - [🎯 Quick Answer (English):](#-quick-answer-english-4)
      - [🇧🇩 বাংলায় ব্যাখ্যা:](#-বাংলায়-ব্যাখ্যা-4)
      - [💻 Standard Implementation:](#-standard-implementation)
    - [Q6: What is the difference between Workstation GC and Server GC?](#q6-what-is-the-difference-between-workstation-gc-and-server-gc)
      - [🎯 Quick Answer (English):](#-quick-answer-english-5)
      - [🇧🇩 বাংলায় ব্যাখ্যা:](#-বাংলায়-ব্যাখ্যা-5)
      - [📊 Quick Comparison Matrix:](#-quick-comparison-matrix)
    - [Q7: How do memory leaks happen in C# despite having a Garbage Collector?](#q7-how-do-memory-leaks-happen-in-c-despite-having-a-garbage-collector)
      - [🎯 Quick Answer (English):](#-quick-answer-english-6)
      - [🇧🇩 বাংলায় ব্যাখ্যা:](#-বাংলায়-ব্যাখ্যা-6)
      - [💻 Event Leak Example \& Fix:](#-event-leak-example--fix)
      - [🐰 Real-Life Production Case: RabbitMQ `EventingBasicConsumer` Memory Leak](#-real-life-production-case-rabbitmq-eventingbasicconsumer-memory-leak)
  - [📂 Related Study Guides](#-related-study-guides)

---

### Q1: Stack vs. Heap: Are value types always stored on the Stack?

#### 🎯 Quick Answer (English):
> **"No, this is a common myth.** Value types are stored inline wherever they are declared. If a value type (like an `int` or `struct`) is a field inside a `class`, it lives on the **Managed Heap** inside that class. Also, value types captured inside lambda expressions (closures) or `async` state machines are lifted to the Heap. Reference type instances always live on the Heap, while their pointer variables live on the Stack."

#### 🇧🇩 বাংলায় ব্যাখ্যা:
> **না, এটি ভুল ধারণা।** ভ্যালু টাইপ যেখানে ডিক্লেয়ার করা হয় ঠিক সেখানেই সংরক্ষিত হয়। যদি কোনো `int` বা `struct` কোনো ক্লাসের ভেতরের ফিল্ড হয়, তবে তা অবজেক্টের অংশ হিসেবে **Managed Heap** এ থাকে। এছাড়া `async/await` স্টেট মেশিন বা ল্যাম্বডার ভেতর ব্যবহৃত লোকাল ভ্যারিয়েবল হিপে স্থানান্তরিত (Lifted to Heap) হয়। রেফারেন্স টাইপের মূল অবজেক্ট সবসময় Heap এ থাকে, আর তার অ্যাড্রেস বা পয়েন্টার Stack এ থাকে।

#### 💻 Code Demonstration:
```csharp
public class Order
{
    // Stored on the Managed Heap inside the Order instance!
    public int OrderId; 
    public decimal TotalAmount;
}

public class Program
{
    public static void Demo()
    {
        // 'orderNumber' lives on the Thread Stack
        int orderNumber = 42; 

        // 'order' pointer (8 bytes on 64-bit) lives on Stack.
        // The actual Order object (with OrderId & TotalAmount) lives on Heap.
        Order order = new Order { OrderId = 1, TotalAmount = 99.99m };
    }
}
```

> [!TIP]
> **Interview Pro-Tip**: State clearly: *"Value types live inline with their container. If the container is on the heap, the value type is on the heap."* Interviewers love this answer.

---

### Q2: How does the Garbage Collector work under the hood?

#### 🎯 Quick Answer (English):
> "The .NET Garbage Collector uses a **generational mark-and-compact** algorithm based on the generational hypothesis: *most objects die young*. Memory is split into 3 generations:
> - **Gen 0**: Newly created objects. Collected very fast (sub-millisecond).
> - **Gen 1**: Buffer generation for objects that survive Gen 0.
> - **Gen 2**: Long-lived objects (singletons, static data). Cleaned during a **Full GC**.
>
> During collection, the GC runs 3 phases:
> 1. **Mark**: Finds all live objects starting from roots (stack, registers, static references).
> 2. **Sweep**: Finds the memory blocks taken by dead objects.
> 3. **Compact**: Moves surviving objects together to eliminate empty gaps and updates stack pointers."

#### 🇧🇩 বাংলায় ব্যাখ্যা:
> .NET GC কাজ করে **Generational Hypothesis** নীতির ওপর: *অধিকাংশ নতুন অবজেক্ট খুব দ্রুত মারা যায়*। হিপ ৩টি জেনারেশনে বিভক্ত:
> - **Gen 0**: নতুন সব অবজেক্ট এখানে তৈরি হয় এবং ১ মিলিসেকেন্ডের কম সময়ে ক্লিন হয়।
> - **Gen 1**: Gen 0 তে যারা বেঁচে যায় তারা Gen 1 এ প্রমোশন পায়।
> - **Gen 2**: দীর্ঘজীবী অবজেক্ট (যেমন Singleton, Caching)। Gen 2 ক্লিন করাকে **Full GC** বলে যা বেশি CPU সময় নেয়।
>
> GC মূলত ৩টি ধাপে ক্লিন করে:
> 1. **Mark**: রুট (স্ট্যাক, সিপিইউ রেজিস্টার, স্ট্যাটিক ভ্যারিয়েবল) থেকে চেক করে জীবিত অবজেক্টগুলোকে "Alive" চিহ্নিত করে।
> 2. **Sweep**: মৃত অবজেক্টগুলোর মেমোরি খালি হিসেবে চিহ্নিত করে।
> 3. **Compact**: জীবিত অবজেক্টগুলোকে একপাশে চেপে নিয়ে এসে এক জায়গায় সাজায় এবং স্ট্যাকের অ্যাড্রেসগুলো আপডেট করে দেয়।

#### 💻 Visual Phases:
```
[Mark Phase]    -> Root -> Reachable (Alive) vs Unreachable (Dead)
[Sweep Phase]   -> Dead object memory marked as free
[Compact Phase] -> Live objects shifted together -> Single contiguous free block
```

---

### Q3: What is the Large Object Heap (LOH) and how to avoid fragmentation?

#### 🎯 Quick Answer (English):
> "The Large Object Heap (LOH) is a dedicated memory space for objects **85,000 bytes or larger** (primarily large arrays and strings). LOH objects start directly in Generation 2.
>
> **The Problem**: Moving large blocks of memory is very expensive for the CPU. Therefore, by default, the GC does not compact the LOH. Repeatedly allocating and discarding large arrays causes **heap fragmentation**, leading to `OutOfMemoryException` even when there is enough total free RAM.
>
> **The Solution**: Use **`ArrayPool<T>.Shared`** to rent and return buffers instead of allocating `new byte[]` on every operation."

#### 🇧🇩 বাংলায় ব্যাখ্যা:
> যেসব অবজেক্টের সাইজ **৮৫,০০০ বাইটস বা তার বেশি** (যেমন বড় ফাইল বাফার বা বড় স্ট্রিং), সেগুলো সরাসরি Large Object Heap (LOH) এ যায় এবং Gen 2 হিসেবে গণ্য হয়।
>
> **সমস্যা**: বড় অবজেক্ট মেমোরিতে সরানো অত্যন্ত ব্যয়বহুল। তাই ডিফল্টভাবে LOH মেমোরি কম্প্যাক্ট হয় না। বারবার বড় অ্যারে তৈরি ও ডিলিট করলে মেমোরিতে ফ্র্যাগমেন্টেশন ঘটে এবং পর্যাপ্ত RAM থাকা সত্ত্বেও একটানা জায়গা না থাকায় `OutOfMemoryException` হতে পারে।
>
> **সমাধান**: প্রতিবার `new byte[]` তৈরি না করে **`ArrayPool<T>.Shared`** ব্যবহার করে অ্যারে ভাড়া (Rent) নেওয়া এবং কাজ শেষে ফেরত (Return) দেওয়া।

#### 💻 Code Demonstration:
```csharp
using System.Buffers;

// ❌ BAD: Creates 100 KB array on LOH every call -> Causes LOH fragmentation!
public void BadProcess()
{
    byte[] buffer = new byte[100_000]; 
    // Do work...
}

// ✅ GOOD: Rents existing memory block from ArrayPool -> Zero LOH allocation!
public void GoodProcess()
{
    byte[] buffer = ArrayPool<byte>.Shared.Rent(100_000);
    try
    {
        // Do work with buffer...
    }
    finally
    {
        // Return buffer back to the pool for other requests
        ArrayPool<byte>.Shared.Return(buffer);
    }
}
```

---

### Q4: What is the Pinned Object Heap (POH) in .NET 5+?

#### 🎯 Quick Answer (English):
> "The **Pinned Object Heap (POH)** was introduced in .NET 5 to solve heap fragmentation caused by pinned memory.
>
> When C# code talks to native APIs (like sockets or OS file I/O), buffers must be pinned in memory so the GC cannot move them. Previously, pinning an object inside the Small Object Heap (Gen 0/1/2) prevented the GC from compacting surrounding memory, causing fragmentation.
>
> With POH, all pinned allocations are placed in a separate dedicated heap, allowing normal SOH generations to compact freely."

#### 🇧🇩 বাংলায় ব্যাখ্যা:
> .NET 5 এ **Pinned Object Heap (POH)** যুক্ত করা হয়েছে পিন করা অবজেক্টের কারণে ঘটা মেমোরি ফ্র্যাগমেন্টেশন দূর করতে।
>
> যখন C# কোড আনম্যানেজড বা নেটিভ কোডের সাথে যোগাযোগ করে (যেমন সকেট বা ফাইল I/O), তখন সেই মেমোরি বাফারকে পিন করে রাখা হয় যাতে GC তাকে নাড়াতে না পারে। আগে এই পিন করা অবজেক্ট সাধারণ SOH হিপে থাকত, ফলে GC এর আশেপাশের ফাঁকা জায়গাগুলো কম্প্যাক্ট করতে পারত না। 
>
> POH আসার পর সব পিন করা অবজেক্ট একটি আলাদা হিপে রাখা হয়, ফলে সাধারণ Gen 0/1/2 স্বাভাবিকভাবে নির্বিঘ্নে কম্প্যাক্ট হতে পারে।

#### 💻 Code Demonstration:
```csharp
// Allocating directly on the Pinned Object Heap (POH) in .NET 5+
int[] pinnedArray = GC.AllocateArray<int>(length: 1000, pinned: true);
```

---

### Q5: How do you implement the Standard Dispose Pattern?

#### 🎯 Quick Answer (English):
> "The Standard Dispose Pattern provides deterministic cleanup of unmanaged resources (like file handles or database connections). It involves:
> 1. Implementing `IDisposable` with `public void Dispose()`.
> 2. Calling `GC.SuppressFinalize(this)` to tell the GC not to run the finalizer, saving an extra GC cycle.
> 3. A protected virtual `Dispose(bool disposing)` method to handle both managed and unmanaged cleanup safely.
> 4. A defensive finalizer (`~ClassName()`) only if directly managing raw unmanaged handles."

#### 🇧🇩 বাংলায় ব্যাখ্যা:
> আনম্যানেজড রিসোর্স (যেমন ফাইল হ্যান্ডেল, ডাটাবেজ কানেকশন) তৎক্ষণাৎ মুক্ত করার জন্য স্ট্যান্ডার্ড ডিসপোজ প্যাটার্ন ব্যবহার করা হয়:
> 1. `IDisposable` ইন্টারফেসের `public void Dispose()` মেথড ইমপ্লিমেন্ট করা।
> 2. `GC.SuppressFinalize(this)` কল করা যাতে GC অবজেক্টটিকে ফাইনলাইজেশন কিউ থেকে সরিয়ে দেয় এবং অতিরিক্ত এক সাইকেল বাঁচিয়ে রাখে।
> 3. `Dispose(bool disposing)` মেথডে ম্যানেজড ও আনম্যানেজড রিসোর্স নিরাপদভাবে খালি করা।

#### 💻 Standard Implementation:
```csharp
using System;
using System.IO;

public class ResourceHolder : IDisposable
{
    private FileStream? _fileStream; // Managed resource
    private IntPtr _unmanagedHandle; // Unmanaged resource
    private bool _disposed = false;

    public ResourceHolder(string path)
    {
        _fileStream = new FileStream(path, FileMode.OpenOrCreate);
    }

    public void Dispose()
    {
        Dispose(true);
        // Instruct GC to skip the finalizer
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Free managed resources
                _fileStream?.Dispose();
                _fileStream = null;
            }

            // Free unmanaged resources here (if any)
            _disposed = true;
        }
    }

    // Defensive Finalizer: Only called by GC if user forgot to call Dispose()
    ~ResourceHolder()
    {
        Dispose(false);
    }
}
```

---

### Q6: What is the difference between Workstation GC and Server GC?

#### 🎯 Quick Answer (English):
> - **Workstation GC**: Default for client applications (WPF, WinForms, Console). It uses a single GC thread on CPU core 0. Its main goal is **low UI latency** so desktop applications do not freeze.
> - **Server GC**: Default for ASP.NET Core server applications. The CLR creates a **separate managed heap and dedicated GC thread for every CPU core**. It processes garbage collection in parallel across all cores. Its main goal is **high throughput** and scalability.

#### 🇧🇩 বাংলায় ব্যাখ্যা:
> - **Workstation GC**: ডেস্কটপ অ্যাপের জন্য ডিফল্ট। এটি একটি মাত্র থ্রেডে কাজ করে যাতে ইউজারের মাউস বা UI আটকে না যায় (Low Latency)।
> - **Server GC**: ASP.NET Core এর জন্য ডিফল্ট। এটি প্রতিটি CPU কোরের জন্য আলাদা আলাদা Managed Heap এবং আলাদা ডেডিকেটেড থ্রেড তৈরি করে সমান্তরালে কাজ করে যাতে ক্লাউড বা সার্ভারে প্রতি সেকেন্ডে হাজার হাজার রিকোয়েস্ট হ্যান্ডেল করা যায় (High Throughput)।

#### 📊 Quick Comparison Matrix:

| Feature | Workstation GC | Server GC |
| :--- | :--- | :--- |
| **Default In** | Desktop / Console Apps | ASP.NET Core Web APIs |
| **Heaps Created** | 1 Managed Heap | 1 Managed Heap **per CPU core** |
| **GC Threads** | 1 GC Thread | 1 Dedicated GC Thread **per CPU core** |
| **Primary Goal** | Smooth UI responsiveness (Low Latency) | Maximum Request Processing (High Throughput) |
| **RAM Consumption** | Lower baseline RAM | Higher baseline RAM (multiple heaps) |

---

### Q7: How do memory leaks happen in C# despite having a Garbage Collector?

#### 🎯 Quick Answer (English):
> "C# has a Garbage Collector (GC), but it cannot collect objects that are still reachable through references, even when the application no longer needs them. Memory leaks happen when objects remain reachable unintentionally, causing memory usage to grow over time.
>
> Common causes:
> 1. **Unregistered Event Handlers**: A short-lived subscriber listens to a long-lived publisher without unsubscribing (`-=`).
> 2. **Static Collections**: Adding objects to `static List<T>` or `static Dictionary` without clearing them.
> 3. **Capturing variables in closures**: Lambda expressions accidentally capturing large parent objects.
> 4. **Neglecting `Dispose()`**: Forgetting to dispose database connections, streams, or timers."

#### 🇧🇩 বাংলায় ব্যাখ্যা:
> .NET এ মেমোরি লিক মানে মেমোরি হারিয়ে যাওয়া নয়, বরং **অপ্রয়োজনীয় রেফারেন্স আটকে থাকা**। যে অবজেক্টটি আপনার আর প্রয়োজন নেই, কোনো একটি স্ট্যাটিক বা লং-লিভড অবজেক্ট এখনো তার রেফারেন্স ধরে রেখেছে। ফলে GC তাকে "Alive" ভেবে মেমোরিতে রেখে দেয়।
>
> প্রধান কারণসমূহ:
> 1. **ইভেন্ট হ্যান্ডলার লিক**: কোনো লং-লিভড অবজেক্টের ইভেন্টে সাবস্ক্রাইব করার পর `-=` দিয়ে আনসাবস্ক্রাইব না করা।
> 2. **স্ট্যাটিক কালেকশন**: `static List` বা ডিকশনারিতে ডাটা যোগ করতে থাকা কিন্তু কখনো ডিলিট না করা।
> 3. **ডিসপোজ না করা**: ফাইল স্ট্রিম বা টাইমার ঠিকমতো `Dispose()` না করা।

#### 💻 Event Leak Example & Fix:
```csharp
public class Publisher
{
    public event Action? OnDataReceived;
}

public class Subscriber
{
    public Subscriber(Publisher publisher)
    {
        // Subscribing creates a strong link: Publisher -> Subscriber
        publisher.OnDataReceived += HandleData;
    }

    private void HandleData() { }

    // ✅ FIX: Provide cleanup method to break the strong reference
    public void Unregister(Publisher publisher)
    {
        publisher.OnDataReceived -= HandleData;
    }
}
```

#### 🐰 Real-Life Production Case: RabbitMQ `EventingBasicConsumer` Memory Leak

In microservices, RabbitMQ channels (`IModel`) and connection managers are typically registered as **Singletons**. A very common production memory leak occurs when scoped message handlers or dynamic worker classes subscribe to `consumer.Received` without unsubscribing:

```csharp
// ❌ REAL-WORLD BUG: Scoped or dynamic worker leaks in a Singleton RabbitMQ listener
public class OrderNotificationWorker
{
    private readonly EventingBasicConsumer _singletonConsumer;
    private readonly byte[] _largeBuffer = new byte[50_000]; // 50 KB per worker

    public OrderNotificationWorker(EventingBasicConsumer singletonConsumer)
    {
        _singletonConsumer = singletonConsumer;
        // BUG: Delegate target '_target' points to this worker instance!
        // Because singletonConsumer lives forever, every worker instance stays alive in Gen 2!
        _singletonConsumer.Received += HandleOrderNotification;
    }

    private void HandleOrderNotification(object? model, BasicDeliverEventArgs ea)
    {
        // Process message...
    }
}

// ✅ PRODUCTION FIX: Implement IDisposable to detach the event handler
public class OrderNotificationWorkerFixed : IDisposable
{
    private readonly EventingBasicConsumer _singletonConsumer;
    private bool _disposed;

    public OrderNotificationWorkerFixed(EventingBasicConsumer singletonConsumer)
    {
        _singletonConsumer = singletonConsumer;
        _singletonConsumer.Received += HandleOrderNotification;
    }

    private void HandleOrderNotification(object? model, BasicDeliverEventArgs ea)
    {
        // Process message...
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            // CRITICAL: Unsubscribe delegate to remove strong reference from Singleton GC root!
            _singletonConsumer.Received -= HandleOrderNotification;
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
```

> [!IMPORTANT]
> **Why this happens under the hood**: In C#, a delegate is an object with two internal fields: `_target` (pointer to the class instance) and `_methodPtr` (pointer to the function). As long as the long-lived publisher (`EventingBasicConsumer`) is alive, its internal event invocation list holds a strong pointer to `_target`. The Garbage Collector sees an unbroken chain from the GC Root to the worker, so it can **never** reclaim it.

---

## 📂 Related Study Guides
- Deep-Dive Concept Guide: [memory_management.md](file:///Users/limon/interview/c-sharp-prep/docs/memory_management.md)
- CLR Runtime Internals: [CLR.md](file:///Users/limon/interview/c-sharp-prep/docs/CLR.md)
- Multithreading & Synchronization: [Multithreading.md](file:///Users/limon/interview/c-sharp-prep/docs/Multithreading.md)

