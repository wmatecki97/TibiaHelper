using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.Modules.WalkerModule
{
    public class Action : WalkerStatement, ICloneable
    {
        public int defaultAction;

        public object[] args;

        public Action(int defaultActionType, params object[] arguments)
        {
            defaultAction = defaultActionType;
            args = arguments;
            type = (int)StatementType.getType["action"];
        }

        public void DoAction()
        {
            DefaultActions.DoAction(defaultAction, args);
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
