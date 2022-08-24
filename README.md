# AppClientServerTest
Решение состоит из 3х проектов:
- ClientRectangle - Клиент отображающий прямоугольники
- GrpcServer - Grpc сервер
- GrpcServiceProvider - структура Protobuf и экземпляр клиента gRPC
Для запуска необходимо собрать все решения. К ClientRectangle а зависимости подключить GrpcServiceProvider. Для работы необходимы дополнительные библиотеки из диспетчера пакетов NuGet:
- Google.Protobuf
- Grpc.AspNetCore
- Grpc.Net.Client
- Grpc.Tools
