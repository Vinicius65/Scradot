using Scradot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Core
{
    public class SpiderDepthLog : ISpiderLogging
    {
        public Dictionary<int, int> DepthRequests { get; private set; }
        private bool startLog = false;
        public SpiderDepthLog(){
            DepthRequests = new Dictionary<int, int>();
        }

        public void AddDepthRequest(int keyDepth)
        {
            if (DepthRequests.TryGetValue(keyDepth, out int value))
            {
                DepthRequests[keyDepth]++;
            }
            else{
                DepthRequests.Add(keyDepth, 1);
            }
            if (!startLog)
            {
                startLog = true;
                StartLog();
            }
        }
        public async void StartLog()
        {
            while (true)
            {
                await Task.Delay(3000);

                int cursorTop = Console.CursorTop;

                Console.Write($"DepthRequests: [1][{DepthRequests[1]}] ");
                foreach (var depth in DepthRequests.Keys.ToList().Skip(1))
                {
                    Console.Write($"---> [{depth}][{DepthRequests[depth]}] ");
                }
                Console.SetCursorPosition(0, cursorTop);
                Console.ResetColor();
            }
        }
    }
}
