namespace EcommerceCompany.Options
{
    public record RabbitMqOptions
    {
        public static string Hostname = "localhost";

        public static string QueueName = "Order.Queue.Create";
        public static string ExchangeName = "Order.Exchange";
        public static string RoutingKey = "Order.Queue.*";

        public static string UserName = "guest";
        public static string Password = "guest";
    }
}
