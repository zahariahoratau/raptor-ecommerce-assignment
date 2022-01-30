using EcommerceCompany.Application.Models;
using EcommerceCompany.Application.Services;
using EcommerceCompany.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace EcommerceCompany.Receiver
{
    public class OrderCreateReceiver : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OrderCreateReceiver> _logger;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;

        public OrderCreateReceiver(
            ILogger<OrderCreateReceiver> logger,
            IServiceProvider serviceProvider)
        {
            _hostname = RabbitMqOptions.Hostname;
            _queueName = RabbitMqOptions.QueueName;
            _username = RabbitMqOptions.UserName;
            _password = RabbitMqOptions.Password;

            _logger = logger;
            _serviceProvider = serviceProvider;

            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(RabbitMqOptions.ExchangeName, ExchangeType.Topic);
            _channel.QueueDeclare(RabbitMqOptions.QueueName, false, false, false, null);
            _channel.QueueBind(RabbitMqOptions.QueueName, RabbitMqOptions.ExchangeName, RabbitMqOptions.RoutingKey, null);
            _channel.BasicQos(0, 1, false);
        }

        private void HandleOrderCreateMessage(DtoOrder dtoOrder, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _logger.LogInformation($"Received information: {dtoOrder}");
            
            GetScopedOrderServiceAndCreateOrder(dtoOrder);
        }

        private void GetScopedOrderServiceAndCreateOrder(DtoOrder dtoOrder)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

                orderService.CalculatePriceAndCreateOrderFromOrderMessage(dtoOrder);
            }
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _logger.LogInformation("Hosted Service running.");
            
            ConsumeRabbitMqOrderWhenReceived(cancellationToken);

            return Task.CompletedTask;
        }

        private void ConsumeRabbitMqOrderWhenReceived(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ch, ea) => AcknowledgeAndHandleOrderCreateMessage(ea, cancellationToken);

            _channel.BasicConsume("Order.Queue.Create", false, consumer);
        }

        private void AcknowledgeAndHandleOrderCreateMessage(BasicDeliverEventArgs ea, CancellationToken cancellationToken)
        {
            try
            {
                _channel.BasicAck(ea.DeliveryTag, false);

                DtoOrder? dtoOrder = DeserializeOrderMessageContent(ea);

                if (dtoOrder is null)
                    throw new InvalidDataException();

                HandleOrderCreateMessage(dtoOrder, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Order object not valid.\n" + ex.Message);
            }
        }

        private static DtoOrder? DeserializeOrderMessageContent(BasicDeliverEventArgs ea)
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            DtoOrder? dtoOrder = JsonConvert.DeserializeObject<DtoOrder>(content);
            return dtoOrder;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service is stopping.");

            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
