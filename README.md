in my code:
Violation of SRP:
The DataManager class violates SRP because it handles both file I/O operations and JSON serialization/deserialization. Ideally, these responsibilities should be separated into different classes. For instance, file I/O operations could be handled by one class, and JSON serialization/deserialization could be handled by another.
Volation DIP:
Both DataManager and ReservationHandler classes depend on concrete implementations. This violates the Dependency Injection Principle because they are tightly coupled with specific implementations, making them harder to test and less flexible to change.

Why are these principles essential in web applications?
Maintainability:
By adhering to SRP, each class becomes easier to understand, maintain, and extend and reducing the risk of unintentional side effects.
Testability:
Following DIP allows for easier dependency injection, which improves testability. Classes that depend on abstractions can be easily mocked or stubbed during testing, enabling more effective unit testing.
Flexibility:
Separating concerns, It becomes easier to swap out implementations or introduce new features without affecting existing code.
