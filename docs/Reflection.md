## Reflection
Reflection in C# is a powerful feature that allows you to inspect metadata about types, methods, properties, and events at runtime. It can be useful for a variety of tasks, such as dynamically creating instances of types, invoking methods, accessing properties, and even modifying them.

### Use Cases
Key Use Cases of Reflection
1. Dynamic Type Inspection: It helps in identifying the types within an assembly, including their methods, properties, constructors, and fields. For instance, you can retrieve the names and attributes of all methods in a class at runtime.

2. Late Binding: With reflection, you can create instances and invoke methods on types that are unknown at compile time. This is particularly useful in scenarios like plugin architectures where you load types from external libraries dynamically.

3. Attribute Inspection: Reflection can check custom attributes applied to classes, methods, or properties, allowing behaviors to change based on metadata (e.g., validating model properties marked with specific attributes).

4. Serialization and Deserialization: Reflection helps in serializing objects to formats like JSON or XML by accessing their properties and fields, even if their structure is unknown beforehand.

5. Automated Testing and Mocking: Reflection is commonly used in unit tests to mock objects, inspect private members, or test internal functionalities without exposing them publicly.

---

### Common Terms:
#### `Assembly`
Assemblies are compiled code libraries (.DLL or .EXE files). Reflection allows us to load assemblies at runtime, inspect their contents (types, resources), or dynamically execute code.
   1. Assembly.LoadFrom("Plugin.dll"): Loads an assembly given its name.
   2. Assembly.GetExecutingAssembly(): Gets the assembly that contains the code that is currently executing.
   3. Assembly.GetEntryAssembly(): Gets the assembly that is the entry point of the process (the assembly that contains the Main method).
   4. typeof(MyDbContext).Assembly: This method retrieves the assembly where the MyDbContext class is defined.

---

#### `Type`
In reflection, the Type class in .NET represents the metadata of a type `(class, interface, struct, enum, or delegate)`, and using the Type class allows you to inspect its `structure at runtime` (like its `methods, properties, constructors, fields, events, and attributes`).

   1. `typeof()` is used when the type is known at compile time.
   2. `object.GetType()` is called on an instance to get its runtime type.
   3. `Type.GetType()` retrieves a type using a string representation, and it requires the fully qualified name of the type, including the assembly if it's not in the `executing assembly or same assembly`.
   4. 

---

#### Dynamic Invocation and Late Binding:
Dynamic invocation and late binding in C# allow invoking members of a type at runtime without compile-time checks. This can be achieved through `dynamic types` or `reflection (MethodInfo.Invoke)`.
   1. `Activator.CreateInstance` dynamically creates an instance of the Person class.
   2. `GetMethod()` retrieves a method by name. BindingFlags.NonPublic | BindingFlags.Instance allows accessing private methods.
   3. `MethodInfo.Invoke` executes the method at runtime.

----

#### Attribute
In C#, attributes are a way to add metadata to your code elements (such as classes, methods, properties, etc.), providing additional information that can be accessed at runtime using reflection.

**Common Use Cases for Attributes**
   1. Metadata: Store extra information about your code elements (like versioning, authorship).
   2. Validation: Define validation rules for properties (e.g., data annotations in ASP.NET).
   3. Conditional Logic: Control behavior, such as specifying whether a method should be exposed via an API.
   4. Serialization: Customize how data is serialized/deserialized in JSON, XML, or other formats.

**Attribute Targets**<br>
The AttributeTargets enumeration specifies where an attribute can be applied, such as:
   1. AttributeTargets.Class
   2. AttributeTargets.Method
   3. AttributeTargets.Property
   4. AttributeTargets.Field

You specify the valid targets using `[AttributeUsage]` in the custom attribute definition.

---

#### Important Terms:
   1. **`Scan()`**: Scans the assembly for all classes that implement INotificationServiceFactory.
   2. **`AddClasses()`**: Adds classes that match the provided condition.
   3. **`AsImplementedInterfaces()`**: Registers them under the interfaces they implement.
   4. **`WithTransientLifetime()`**: Sets the lifetime of these services as Transient.
   

| **Method/Property** | **Purpose**                                    | **Example Use Case**                    |
|---------------------|-------------------------------------------------|-----------------------------------------|
| **TryGetValue**     | Safely retrieve a value from a dictionary       | `dict.TryGetValue(key, out value)`      |
| **GetType**         | Get the runtime type of an object               | `obj.GetType()`                         |
| **GetTypes**        | Get all types in an assembly                    | `assembly.GetTypes()`                   |
| **GetValues**       | Get all values from an enum                     | `Enum.GetValues(typeof(EnumType))`      |
| **GetValue**        | Get value of a field or property via reflection | `propertyInfo.GetValue(object)`         |
| **GetProperty**     | Get metadata for a property by name             | `person.GetType().GetProperty("Name")`  |


---
## Real Life Examples:
### A abstract factory method for invoking object dynamically:
For example it will dynamically add other features like `Email, Push, or other type of notifications` without alter object creation logics.
```c#
public interface INotificationFactory
{
    INotificationServiceFactory GetFactory(string notificationType);
}

public class NotificationFactory : INotificationFactory
{
    private readonly IEnumerable<INotificationServiceFactory> _factories;

    // Constructor injection using ASP.NET Core DI
    public NotificationFactory(IEnumerable<INotificationServiceFactory> factories)
    {
        _factories = factories;
    }

    // Returns the appropriate factory based on the notification type
    // this is also problematic as we used hard-coded strings
    public INotificationServiceFactory GetFactory(string notificationType)
    {
        return _factories.FirstOrDefault(f =>
            f.GetType().Name.Contains(notificationType, StringComparison.OrdinalIgnoreCase))
            ?? throw new ArgumentException("Invalid notification type.");
    }
}

//DI section
// Register individual notification factories, this is problematic
builder.Services.AddTransient<INotificationServiceFactory, EmailNotificationServiceFactory>();
builder.Services.AddTransient<INotificationServiceFactory, SMSNotificationServiceFactory>();
builder.Services.AddTransient<INotificationServiceFactory, PushNotificationServiceFactory>();

// Register the NotificationFactory itself
builder.Services.AddSingleton<INotificationFactory, NotificationFactory>();


// Create objects
var factory = scope.ServiceProvider.GetRequiredService<INotificationFactory>();
var emailFactory = factory.GetFactory("Email");
var emailService = emailFactory.CreateNotificationService();
emailService.Send("Hello from Email!");

```
However, the above code is not efficient in a case where `INotificationServiceFactory` will be inheritated many class and also .

Solution:
```c#
// Register all INotificationServiceFactory implementations with Scoped lifetime
builder.Services.Scan(scan => scan
    .FromAssemblyOf<INotificationServiceFactory>() // Scan the current assembly
    .AddClasses(classes => classes.AssignableTo<INotificationServiceFactory>())
    .AsImplementedInterfaces()
    .WithScopedLifetime() // Use Scoped lifetime instead of Transient
);
```

Remove the hard-coded string:
```c#
public class NotificationFactory : INotificationFactory
{
    private readonly IEnumerable<INotificationServiceFactory> _factories;

    public NotificationFactory(IEnumerable<INotificationServiceFactory> factories)
    {
        _factories = factories;
    }

//generic type
    public INotificationServiceFactory GetFactory<T>() where T : INotificationServiceFactory
    {
        return _factories.OfType<T>().FirstOrDefault()
            ?? throw new ArgumentException("Invalid notification factory type.");
    }
}

var factory = scope.ServiceProvider.GetRequiredService<INotificationFactory>();

var emailFactory = factory.GetFactory<EmailNotificationServiceFactory>();
var emailService = emailFactory.CreateNotificationService();
emailService.Send("Hello from Email!");
```

### ***`In the DB context Configuration:`***
```c#
builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
services.AddAutoMapper(Assembly.GetExecutingAssembly());
services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
services.AddMediatR(Assembly.GetExecutingAssembly());
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
```