using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chrio.Controls
{
    public enum CommandType
    {
        Move,
        Stop,
        Attack,
        Capture
    }

    public class Command
    {
        public CommandType Type;
        public string Data;

        public Command (CommandType CmdType, string CmdData)
        {
            Type = CmdType;
            Data = CmdData;
        }
    }
}
