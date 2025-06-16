using Backend.Services.DiceServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqnrollBackend.TestInfrastructure.DiceFakes
{
	public class QueueBasedDiceService : IDiceService
	{
		private Queue<int> _rollQueue = new();
		private IDiceService _strategyIfEmpty;

		public QueueBasedDiceService(IDiceService? strategyIfEmpty = null)
		{
			if (strategyIfEmpty == null)
			{
				_strategyIfEmpty = new RandomDiceService();
			}
			else
			{
				_strategyIfEmpty = strategyIfEmpty;
			}
		}

		public void EnqueueRolls(IEnumerable<int> rolls)
		{
			foreach (var r in rolls) _rollQueue.Enqueue(r);
		}

		public int Roll()
		{
			if (_rollQueue.Count == 0)
				return _strategyIfEmpty.Roll();

			return _rollQueue.Dequeue();
		}
	}
}
