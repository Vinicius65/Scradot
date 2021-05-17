using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Core.Models
{
    public class Meta
    {
        public int Depth { get; private set; }
        public TimeSpan DownloadDelay { get; private set; }
        public int NumberOfRetrys { get; private set; }

        public Meta(int depth, TimeSpan timeDownloadDelay, int numberOfRetrys)
        {
            Depth = depth;
            DownloadDelay = timeDownloadDelay;
            NumberOfRetrys = numberOfRetrys;
        }
    }
}
