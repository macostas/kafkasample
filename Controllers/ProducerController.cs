using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace kAfkawebsample.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Produces("application/json")]
	public class ProducerController : ControllerBase
	{
		private readonly IOptions<ProducerConfig> _config;
		public ProducerController(IOptions<ProducerConfig> config)
		{
			this._config = config;
		}

		private static readonly Random rand = new Random();

		[Route("{message}")]
		[HttpGet]
		public async Task<ActionResult> Producer(string message)
		{

			var _topicName = "foo";
			var _producer = new ProducerBuilder<string, string>(this._config.Value).Build();
			await _producer.ProduceAsync(_topicName, new Message<string, string>()
			{
				Key = rand.Next(5).ToString(),
				Value = message
			});
			return Ok();
		}
	}
}
