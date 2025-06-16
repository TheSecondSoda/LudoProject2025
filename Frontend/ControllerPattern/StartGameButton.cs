using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ControllerPattern
{
    internal class StartGameButton : IButtonAction
    {
        public Button ButtonObject { get; set; }

        public void DoAction()
        {
            GameWorld.Instance.stateManager.ChangeGameState(GameState.Playing);
        }
    }
}
