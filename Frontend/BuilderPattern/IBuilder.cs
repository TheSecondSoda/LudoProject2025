using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.BuilderPattern
{
    internal interface IBuilder
    {
        public GameObject BuildGameObject();
    }
}
