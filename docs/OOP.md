## C# Access Specifiers

Access specifiers control the visibility and accessibility of classes, methods, properties, and other members in C#. Understanding these specifiers is crucial for encapsulation and maintaining clean, manageable code.

| Access Specifier      | Accessibility                                                               |
|-----------------------|---------------------------------------------------------------------------|
| **public**            | Accessible from anywhere.                                                  |
| **private**           | Accessible only within the declaring class.                               |
| **protected**         | Accessible within the declaring class and derived classes.                |
| **internal**          | Accessible only within the same assembly.                                 |
| **protected internal** | Accessible within the same assembly or from derived classes in other assemblies. |
| **private protected**  | Accessible only within the declaring class and derived classes in the same assembly. |

## Different Types of construtor:
#### Default Constructor
1. Purpose: A default constructor initializes an object without any parameters.
2. Usage: This constructor is automatically provided if no other constructor is defined. It is often used when there are no special setup requirements for the object.

#### Parameterized Constructor
1. Purpose: This constructor takes parameters, allowing you to initialize an object with specific values.
2. Usage: It’s used when certain properties need to be set at the time of object creation.

#### Copy Constructor
1. Purpose: A copy constructor creates a new object as a copy of an existing object.
2. Usage: This is useful when you want to create a new instance with the same values as an existing one.
3. Copy constructors are useful when working with complex objects where shallow copies (using assignment) could lead to shared references, which is undesirable. By using a copy constructor, you create a new object with the same properties but independent of the original object.
    ```c#
    public Person(Person existingPerson)
    {
        Name = existingPerson.Name;
        Age = existingPerson.Age;
    }
    ```
#### Static Constructor
1. Purpose: A static constructor initializes static members of the class. It is called only once, automatically, before any instance is created or any static member is accessed.
2. Usage: Used for one-time setup logic that applies to the class itself rather than to instances.
    ```c#
    public class Logger
    {
        public static string LogPath;

        // Static constructor
        static Logger()
        {
            LogPath = "/var/log/app.log";
        }
    }
    ```
#### Private Constructor
1. Purpose: A private constructor prevents the class from being instantiated externally.
2. Usage: Often used in singleton patterns or for classes containing only static members.
    ```c#
    public class Singleton
    {
        private static Singleton _instance;

        // Private constructor
        private Singleton() { }

        public static Singleton GetInstance()
        {
            return _instance ??= new Singleton();
        }
    }
    ```
#### Constructor Chaining
1. Purpose: Constructor chaining allows one constructor to call another constructor within the same class or a base class.
2. Usage: Used to avoid code duplication when multiple constructors have overlapping initialization logic.
    ```c#
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        // Constructor chaining
        public Person() : this("Unknown", 0) { }

        public Person(string name) : this(name, 0) { }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
    ```

## Types of Inheritance in C#

| **Type of Inheritance**      | **Description**                                                         | **Example**                    |
| ---------------------------- | ----------------------------------------------------------------------- | ------------------------------ |
| **Single Inheritance**       | One derived class inherits from a single base class                     | `Dog : Animal`                 |
| **Hierarchical Inheritance** | Multiple derived classes inherit from the same base class               | `Dog : Animal`, `Cat : Animal` |
| **Multilevel Inheritance**   | A chain of inheritance, with each class inheriting from the one above   | `Dog : Mammal : Animal`        |
| **Multiple Inheritance**     | Simulated with interfaces, where a class implements multiple interfaces | `Dog : IAnimal, IPet`          |

---

### Descriptions

- **`Single Inheritance`**: In this inheritance model, a class can inherit only from one base class. 
- **`Hierarchical Inheritance`**: One base class is inherited by multiple derived classes, providing a common structure while allowing specialized functionality.
- **`Multilevel Inheritance`**: Allows creating a chain of inheritance, where each class derives from the one above it.
- **`Multiple Inheritance`**: C# does not support multiple inheritance with classes directly. Instead, it uses interfaces to allow a class to implement multiple behaviors.

Each type of inheritance serves specific needs and promotes code reuse, flexibility, and a clear organizational structure.

## Polimorphism
Polymorphism is a core concept in object-oriented programming (OOP) that allows `objects of different classes` to be treated as objects of a `common superclass`. It provides a way to perform a single action in different forms, promoting flexibility, code reuse, and ease of maintenance.
1. **`Compile-Time Polymorphism (Static Polymorphism)`**: Method Overloading refers multiple methods in the same class have the same name but different parameters.
2. **`Run-Time Polymorphism (Dynamic Polymorphism)`**: Run-time polymorphism is achieved through method `overriding`. The base class method is declared as `virtual`, and the derived class uses the `override` keyword to implement its specific behavior.

## Abstraction
An abstract class in C# serves as a base class that cannot be instantiated on its own. It is designed to provide a common structure and behavior that derived classes can inherit and build upon.
- can have `constractor`, `concreate method`, and `abstract method`.
- It is best practice to make a `concreate method sealed` which will prevent `overriding` in derieved class. 

Using abstract classes in C# is beneficial because it:
1. Establishes a clear base for inheritance.
2. Encourages code reuse and reduces duplication.
3. `Enforces` a contract with `abstract methods`.
4. Facilitates polymorphism for flexible code design.
5. Supports the open/closed principle, enhancing maintainability.
6. Improves code organization through clear hierarchies.
7. Allows shared implementation of common functionality.

### Comparison of Abstract Class, Static Class, and Interface in C#

This document provides a concise comparison of the features of Abstract Classes, Static Classes, and Interfaces in C#.

| Feature                       | Abstract Class                            | Static Class                   | Interface                                                 |
| ----------------------------- | ----------------------------------------- | ------------------------------ | --------------------------------------------------------- |
| Instantiation                 | Cannot be instantiated                    | Cannot be instantiated         | Cannot be instantiated                                    |
| Inheritance                   | Can inherit and be inherited              | Cannot inherit or be inherited | Can be implemented by classes                             |
| State (fields and properties) | Can have fields and properties            | Cannot have instance members   | Cannot have fields                                        |
| Implementation                | Can have implemented and abstract methods | All methods must be static     | Methods are unimplemented, but can have defaults in C# 8+ |
| Constructors                  | Can have constructors                     | No constructors                | No constructors                                           |
| Multiple Inheritance          | Single inheritance                        | Not applicable                 | Can be implemented alongside others                       |

#### Summary

- **Abstract Class**: Use when you want to provide shared code with inheritance and require derived classes to implement certain methods.
- **Static Class**: Use for utility or helper functions that don’t depend on instance data or require inheritance.
- **Interface**: Use when defining a set of behaviors or capabilities that different classes can share, especially when multiple inheritance is desired.

## Class vs Record
**Class**: Classes are reference types, meaning they are stored in the heap, and accessing them involves a level of indirection, which can have performance implications.
1. You need an object with complex behaviors or methods.
2. You require mutable state that changes over the object's lifecycle.
3. Reference equality is essential (you want to check if two variables point to the exact same object).
4. You plan to implement inheritance hierarchies or polymorphism.

**Record**: Records are also reference types by default, but you can create record structs (introduced in C# 10) for value-type semantics, which makes them more efficient for small data-only objects.
1. You need an immutable data structure.
2. Value-based equality is more meaningful than reference equality.
3. You are working with data-centric types where the focus is on data consistency rather than behavior.
4. You want concise syntax for creating simple data containers.
5. `var person2 = person1 with { Age = 31 };` create a new person with existing person1 value but `Age=31`

## C# `new` Keyword Uses
The `new` keyword in C# has multiple uses across different contexts. Below is a summary of the most important scenarios where `new` is used and their examples.

### Summary of `new` Keyword Uses

| Context                         | Description                                                                                      | Example                                   |
|---------------------------------|--------------------------------------------------------------------------------------------------|-------------------------------------------|
| **Object Creation**             | Creates instances of classes, structs, arrays, or delegates.                                     | `new Person("Alice");`                    |
| **Member Hiding in Inheritance**| Hides a base class member with a new member in a derived class.                                  | `public new void Speak() { }`             |
| **Anonymous Type Creation**     | Creates an instance of an anonymous type.                                                        | `var person = new { Name = "Alice" };`    |
| **Value Type Initialization**   | Initializes structs, calling their default constructors (if defined) and setting default values. | `Point p1 = new Point();`                 |
| **Generic Constraints**         | Specifies that a generic type parameter must have a parameterless constructor.                   | `where T : new()`                         |

```bash
    public class Factory<T> where T : new()
    {
        public T CreateInstance()
        {
            return new T();
        }
    }

    var factory = new Factory<Person>();
    var person = factory.CreateInstance(); // Creates a new instance of Person using the default constructor

```

## C# Important Keywords
1. **`dynamic`**: Enables dynamic typing, allowing types to be determined at runtime.
2. **`readonly`**: Prevents modification of a field after it has been initialized.
3. **`const`**: Declares a constant value that cannot be changed.
4. **`yield`**: Used with iterators to return each element one at a time.
5. **`in, out, ref`**: Pass parameters by reference or explicitly define variance in interfaces and delegates.
6. **`typeof`**: Obtains the System.Type instance for a type.
7. **`sizeof`**: Returns the size in bytes of a value type.
8. **`init`**: keyword is a property modifier introduced in C# 9 that allows you to set properties during object initialization, but not modify them afterward. It provides a middle ground between fully mutable properties (set accessors) and fully immutable properties. ` public string Make { get; init; }`. Use-case
   1. `Data Transfer Objects (DTOs)`: For passing data that should not change after being set.
   2. `Configuration Settings`: To define settings that are set once and remain constant.
   3. `Record Types`: For immutable data models where changes should create new instances rather than modify existing ones.


