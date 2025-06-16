using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ControllerPattern
{
    public interface IButtonAction
    {
        Button ButtonObject { get; set; }

        public void DoAction();

    }
}
