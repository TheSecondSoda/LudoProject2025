using Backend.Services.DiceServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqnrollBackend.TestInfrastructure.DiceFakes
{
	public class SpecificValueDiceService : IDiceService
	{
		private int _rollValue;

		public SpecificValueDiceService(int roll = 1)
		{
			_rollValue = roll;
		}

		public void SetNextRoll(int value)
		{
			_rollValue = value;
		}

		public int Roll()
		{
			return _rollValue;
		}
	}
}
