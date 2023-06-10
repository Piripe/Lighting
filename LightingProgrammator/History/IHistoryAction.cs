using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightingProgrammator.History
{
    internal interface IHistoryAction
    {
        public void Undo();
        public void Redo();
    }
}
