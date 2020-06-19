using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace kAfkawebsample.Services
{
	public class ConsumerServices : BackgroundService
	{
		private readonly string topicName;
		private readonly IOptions<ConsumerConfig> _consumerConfig;
		public ConsumerServices(IOptions<ConsumerConfig> consumerConfig)
		{
			Console.WriteLine("Service Started");
			this.topicName = "foo";
			this._consumerConfig = consumerConfig;
		}
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				var _consumer = new ConsumerBuilder<string, string>(this._consumerConfig.Value).Build();
				_consumer.Subscribe(this.topicName);
				await Task.Delay(200, stoppingToken);
				var consumeResult = _consumer.Consume();
				Console.WriteLine(consumeResult.Message.Value);
			}
		}
	}
}
