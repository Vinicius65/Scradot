﻿using Scradot.Core;
using Scradot.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Item : Generator
    {
        public string Url { get; set; }
        public string Autor { get; set; }
        public string Descricao { get; set; }
    }
}