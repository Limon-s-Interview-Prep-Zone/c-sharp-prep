### Task Based Async
Task-based async programming can use the thread pool, but whether it does depends on the type of work the task performs. Asynchronous programming with `Task` and `Task<T> `enhances `responsiveness, resource efficiency, and scalability`.

1. **`CPU-bound Operations`**: Tasks handling CPU-bound work will use the thread pool to manage threads.
   1. `Task.Run` or `Task.Factory.StartNew` will execute the work on a thread pool thread.
2. **`I/O-Bound Operations:`** Tasks handling I/O-bound work do not keep a thread occupied while waiting, but once the operation completes, the continuation is often executed on a thread pool thread.

**Common Terminology:**
1. **`Task.Run`**: It starts the task in a separate thread, allowing the main thread to continue without waiting for it to finish.
2. **`Task.Delay(1000)`**: It introduces a delay to simulate a time-consuming task.
3. **`task.Wait()`**: It waits for the task to complete.
4. **`Task.WhenAll`**: It waits for multiple tasks to complete before proceeding.
5. **`ContinueWith`**: Use ContinueWith to start a task after another task finishes. `initialTask.ContinueWith` starts continuation only after `initialTask` completes
6. **`Task.WhenAny:`** Waits for the first task to complete and returns that task.
7. **`Parallel.Invoke`** It accepts an array of Action delegates (or lambdas), each representing a task to run concurrently.
   1. `Parallel.Invoke` takes advantage of available cores on the machine to execute tasks concurrently, improving performance for CPU-bound operations.
8. **`Parallel.For() or Parallel.ForEach()`**: executes the loop body for each iteration in parallel across multiple threads. Each iteration might run on a different thread, managed by the thread pool.

9. **`PLINQ`**: Ideal for parallel processing of large data sets using LINQ syntax.
   1.  AsParallel converts the LINQ query into a parallel query.
   2.  Each Select operation runs on a separate thread, making the query execute concurrently.

### Task.WhenAll vs Parallel.Invoke()
**`Task.WhenAll()`: `Task.WhenAll` is designed for asynchronous workflows, especially for I/O-bound tasks that don’t require blocking the main thread.
   1. You’re working with asynchronous or I/O-bound tasks.
   2. You want a non-blocking, awaitable structure.
   3. Tasks are naturally asynchronous (e.g., fetching data, I/O operations, or CPU-bound tasks wrapped in Task.Run).

**`Parallel.Invoke()`: `Parallel.Invoke` is ideal for synchronous, CPU-bound tasks that need to run in parallel and block until completion.
   1. You have CPU-bound synchronous actions that can benefit from parallel execution.
   2. You don’t need asynchronous tasks or awaitable results.
   3. Blocking the calling thread until all tasks complete is acceptable.