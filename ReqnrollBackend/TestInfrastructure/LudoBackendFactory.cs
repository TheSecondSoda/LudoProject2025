using Backend.Services.DiceServices.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqnrollBackend.TestInfrastructure
{
	public class LudoBackendFactory : WebApplicationFactory<Program>
	{
		private readonly IDiceService? _customDiceService;

		public LudoBackendFactory(IDiceService? diceServiceOverride = null)
		{
			_customDiceService = diceServiceOverride;
		}

		protected override IHost CreateHost(IHostBuilder builder)
		{
			builder.ConfigureServices(services =>
			{
				if (_customDiceService != null)
				{
					var existing = services.FirstOrDefault(d => d.ServiceType == typeof(IDiceService));
					if (existing != null)
						services.Remove(existing);

					services.AddSingleton(typeof(IDiceService), _customDiceService);
				}
			});

			return base.CreateHost(builder);
		}
	}
}
