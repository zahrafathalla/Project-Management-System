using MediatR;
using ProjectManagementSystem.Abstractions;
using RabbitMQ.Client;
using System.Text;

namespace ProjectManagementSystem.CQRS.Tasks.Command
{
    public record PublishRabbitMqMessageCommand(string Message, string RoutingKey) : IRequest<Result>;

    public class PublishRabbitMqMessageCommandHandler : IRequestHandler<PublishRabbitMqMessageCommand, Result>
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public PublishRabbitMqMessageCommandHandler()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("TaskExchange", ExchangeType.Direct, true, false);
            _channel.QueueDeclare("task_assignment_queue", durable: true, exclusive: false, autoDelete: false);
            _channel.QueueDeclare("task_create_queue", durable: true, exclusive: false, autoDelete: false);
            _channel.QueueDeclare("task_modify_queue", durable: true, exclusive: false, autoDelete: false);

            _channel.QueueBind("task_assignment_queue", "TaskExchange", "key1");
            _channel.QueueBind("task_create_queue", "TaskExchange", "key2");
            _channel.QueueBind("task_modify_queue", "TaskExchange", "key3");
        }

        public Task<Result> Handle(PublishRabbitMqMessageCommand request, CancellationToken cancellationToken)
        {
            var body = Encoding.UTF8.GetBytes(request.Message);

            _channel.BasicPublish(exchange: "TaskExchange", routingKey: request.RoutingKey, body: body);


            return Task.FromResult(Result.Success());
        }
    }

}
