Hello! It's our first practice in writing a distributed system in C#.

The architecture of this system includes 3 main components.

The *client* sends requests with matrix expressions that need to be calculated.

The *server* ("manager") accepts requests from clients, decomposes expressions, distributes subtasks to existing performers, and returns the results to clients.

An *employee* ("executor", "worker") - directly performs the tasks sent to him by the manager.

This system performs simple matrix operations such as matrix addition and multiplication.
