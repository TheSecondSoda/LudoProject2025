using Backend.Services.DiceServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqnrollBackend.TestInfrastructure.DiceFakes
{
	public class RandomDiceService : IDiceService
	{
		private readonly Random _random = new();
		public int Roll() => _random.Next(1, 7);
	}
}
