using Scradot.Core;
using Scradot.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Scradot.ConsoleApp
{
    public class Item
    {
        public string Url { get; set; }
        public string Autor { get; set; }
        public string Descricao { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
