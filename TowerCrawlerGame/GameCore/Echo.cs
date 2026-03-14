using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class Echo : Entity
    {
        public EchoData Data { get; set; }
        public Echo(EchoData data) : base("Echo", 1)
        {
            Data = data;
        }
    }
}
