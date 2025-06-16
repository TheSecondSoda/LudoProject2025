using Frontend.BuilderPattern;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ControllerPattern
{
    internal class RollForPlayerOrderButton : IButtonAction
    {
        public Button ButtonObject { get; set; }

        public void DoAction()
        {
            // request backend to roll for playerorder
            GameWorld.Instance.StartGame();



            





        }
    }
}
