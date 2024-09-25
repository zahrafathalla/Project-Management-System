
using MediatR;
using ProjectManagementSystem.CQRS.Projects.Query;
using ProjectManagementSystem.Helper;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ProjectManagementSystem.RabbitMQ
{
    public class RabbitMQConsumer : IHostedService
    {
        IConnection _connection;
        IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMQConsumer(IServiceProvider serviceProvider)
        {

            _serviceProvider = serviceProvider;

            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += Consumer_Received;

            _channel.BasicConsume(queue: "task_assignment_queue", autoAck: false, consumer: consumer);
            _channel.BasicConsume(queue: "task_create_queue", autoAck: false, consumer: consumer);
            _channel.BasicConsume(queue: "task_modify_queue", autoAck: false, consumer: consumer);


            return Task.CompletedTask;
        }

        private async void Consumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            var routingKey = e.RoutingKey; 

            if (routingKey == "key1") 
            {
                await ProcessTaskAssignment(message);
            }
            else if (routingKey == "key2") 
            {
                await ProcessTaskCreation(message);
            }
            else if (routingKey == "key3") 
            {
                await ProcessTaskModification(message);
            }

            _channel.BasicAck(e.DeliveryTag, false);

        }


        private async Task ProcessTaskAssignment(string message)
        {
            var parts = message.Split(new[] { "has been assigned to user with email" }, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                var taskTitle = parts[0].Replace("Task '", "").Replace("'", "").Trim(); 
                var userEmail = parts[1].Trim();

                string subject = "Task Assigned: " + taskTitle;
                string body = $"You have been assigned to the task '{taskTitle}'.";

                await EmailSender.SendEmailAsync(userEmail, subject, body);
            }
        }

        private async Task ProcessTaskCreation(string message)
        {
            var projectId = ExtractProjectIdFromMessage(message); 
            var taskName = ExtractTaskNameFromMessage(message);
            var projectName = ExtractProjectNameFromMessage(message);


            using var scope = _serviceProvider.CreateScope();
            var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = await _mediator.Send(new GetProjectUsersByProjectIdQuery(projectId));

            if (result.IsSuccess)
            {
                foreach (var user in result.Data)
                {
                    string subject = "New Task Created";
                    string body = $"A new task '{taskName}' has been created in project '{projectName}'.";
                    await EmailSender.SendEmailAsync(user.Email, subject, body);
                }
            }
        }
        private async Task ProcessTaskModification(string message)
        {
            var projectId = ExtractProjectIdFromMessage(message); 
            var taskName = ExtractTaskNameFromMessage(message);
            var projectName = ExtractProjectNameFromMessage(message);

            using var scope = _serviceProvider.CreateScope();
            var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var result = await _mediator.Send(new GetProjectUsersByProjectIdQuery(projectId));

            if (result.IsSuccess)
            {
                foreach (var user in result.Data)
                {
                    string subject = "Task Modified";
                    string body = $"The task '{taskName}' has been modified in project '{projectName}'.";
                    await EmailSender.SendEmailAsync(user.Email, subject, body);
                }
            }
        }
        private int ExtractProjectIdFromMessage(string message)
        {
            
            var parts = message.Split(new[] { "ID " }, StringSplitOptions.None);
            if (parts.Length > 1)
            {
                var idPart = parts[1].Trim();
                if (int.TryParse(idPart, out int projectId))
                {
                    return projectId;
                }
            }
            return 0; 
        }
        private string ExtractTaskNameFromMessage(string message)
        {
            var parts = message.Split(new[] { "Task '" }, StringSplitOptions.None);
            if (parts.Length > 1)
            {
                var taskPart = parts[1].Split('\'')[0].Trim(); 
                return taskPart;
            }
            return string.Empty; 
        }
        private string ExtractProjectNameFromMessage(string message)
        {
            var parts = message.Split(new[] { "in project '" }, StringSplitOptions.None);
            if (parts.Length > 1)
            {
                var projectPart = parts[1].Split('\'')[0].Trim();
                return projectPart;
            }
            return string.Empty;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Close();
            _connection.Close();
            return Task.CompletedTask;
        }
    }
}
