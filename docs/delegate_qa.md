# Top .NET Interview Questions & Answers: Delegates & Events (Simple English + বাংলা)

---

## Quick Navigation
1. [Q1: What is a delegate?](#q1-what-is-a-delegate)
2. [Q2: How does a multicast delegate work?](#q2-how-does-a-multicast-delegate-work)
3. [Q3: What is the difference between a raw delegate field and an `event`?](#q3-what-is-the-difference-between-a-raw-delegate-field-and-an-event)
4. [Q4: Why do we need built‑in generic delegates like `Action` and `Func`?](#q4-why-do-we-need-built‑in-generic-delegates)
5. [Q5: How can delegates cause memory leaks and how to prevent them?](#q5-how-can-delegates-cause-memory-leaks-and-how-to-prevent-them)
6. [Q6: When should you use `Action` vs `Func`?](#q6-when-should-you-use-action-vs-func)
7. [Q7: What is the purpose of `Predicate<T>`?](#q7-what-is-the-purpose-of-predicatet)

---

### Q1: What is a delegate?
**English:** A delegate is a **type‑safe object** that stores a reference to a method. You can assign a method to a delegate variable and invoke it later.
**বাংলা:** ডেলিগেট হল **টাইপ‑সেফ অবজেক্ট** যা কোনো মেথডের রেফারেন্স (পয়েন্টার) রাখে। ডেলিগেট ভেরিয়েবলে মেথড অ্যাসাইন করে এরপর কল করা যায়।

---

### Q2: How does a multicast delegate work?
**English:** A multicast delegate holds an **invocation list** (`_invocationList`). When you use `+=` to add another method, the delegate stores another entry. Calling `.Invoke()` runs each method in order.
**বাংলা:** মাল্টিকাস্ট ডেলিগেটের ভিতরে একটি **ইনভোকেশন লিস্ট** থাকে। `+=` দিয়ে একটি নতুন মেথড যোগ করলে লিস্টে আরেকটি এন্ট্রি যোগ হয়। `.Invoke()` কল করলে লিস্টের সব মেথড ক্রমানুসারে চালিত হয়।

---

### Q3: What is the difference between a raw delegate field and an `event`?
**English:** A public delegate field can be **overwritten** (`=`) or invoked by any code, breaking encapsulation. An `event` hides the backing delegate; external code can only **subscribe (`+=`) or unsubscribe (`-=`)**, and only the declaring class can raise it.
**বাংলা:** পাবলিক ডেলিগেট ফিল্ড যেকোনো কোডে `=` দিয়ে ওভাররাইট করা যায় অথবা সরাসরি কল করা যায়, যা এনক্যাপসুলেশন ভেঙ্গে দেয়। `event` ব্যাকিং ডেলিগেটকে গোপন রাখে; বাহিরের কোড কেবল `+=`/`-=` করতে পারে, আর ইভেন্ট তোলার অধিকার শুধুই ক্লাসের ভিতরে থাকে।

---

### Q4: Why do we need built‑in generic delegates like `Action` and `Func`?
**English:** They cover **90 % of everyday scenarios** without writing a custom `delegate` type. `Action<T…>` returns `void`; `Func<T…, TResult>` returns a value. They make code concise and IDE‑friendly.
**বাংলা:** অধিকাংশ ব্যবহার কেসে কাস্টম `delegate` লিখতে হয় না। `Action<T…>` `void` রিটার্ন করে, `Func<T…, TResult>` মান রিটার্ন করে। এগুলি কোডকে সংক্ষিপ্ত এবং ইডিই‑তে ভাল সাপোর্ট দেয়।

---

### Q5: How can delegates cause memory leaks and how to prevent them?
**English:** When a **long‑lived publisher** (e.g., a singleton service or RabbitMQ consumer) holds a delegate reference to a **short‑lived subscriber**, the subscriber stays reachable from a GC root and never gets collected. **Fix:** Unsubscribe (`-=`) in `Dispose` or when the subscriber’s lifetime ends.
**বাংলা:** দীর্ঘ‑জীবী পাবলিশার (সিঙ্গলটন, RabbitMQ কনজিউমার) যদি ছোট‑জীবী সাবস্ক্রাইবারের ডেলিগেট বজায় রাখে, তবে সাবস্ক্রাইবার GC‑রুট থেকে যুক্ত থাকে এবং কখনও গারবেজ‑কলেক্ট হয় না। **সমাধান:** `Dispose`‑এ অথবা লাইফসাইকেল শেষ হলে `-=` দিয়ে আনসাবস্ক্রাইব করুন।

---

### Q6: When should you use `Action` vs `Func`?
**English:** Use **`Action`** when the method **does not return a value** (e.g., logging, event callbacks). Use **`Func`** when you need a **result** (e.g., calculations, data transformations).
**বাংলা:** যদি মেথড **কোনো মান রিটার্ন না করে**, `Action` ব্যবহার করুন (লগিং, কলব্যাক)। যদি মেথড **ফলাফল দেয়**, `Func` ব্যবহার করুন (গণনা, ট্রান্সফরমেশন)।

---

### Q7: What is the purpose of `Predicate<T>`?
**English:** `Predicate<T>` is a **shortcut for `Func<T,bool>`**. It expresses a **true/false test** on a single item, typically used in collection methods like `FindAll`.
**বাংলা:** `Predicate<T>` হল **`Func<T,bool>`‑এর সংক্ষিপ্ত রূপ**। এটি একক আইটেমের জন্য সত্য/মিথ্যা পরীক্ষা প্রকাশ করে, সাধারণত `List<T>.FindAll`‑এ ব্যবহৃত হয়।

---

## 📂 Related Docs
- Full delegate guide: [delegate.md](file:///Users/limon/interview/c-sharp-prep/docs/delegate.md)
- Memory‑management guide (event‑leak example): [memory_management_qa.md](file:///Users/limon/interview/c-sharp-prep/docs/memory_management_qa.md)

