using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    enum TEXT_TYPE
    {
        Talk,
        Qustion
    }

    internal class TextWriter
    {
        public void OneByOne(string text, int interval) 
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(interval);
            }
        }

        public void WaitSymbol(int repeat, int interval, int wait)
        {
            for (int i = 0; i < repeat; ++i)
            {
                foreach(char c in "......")
                {
                    Console.Write(c);
                    Thread.Sleep(interval);
                }
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop);
            }
            Thread.Sleep(wait);
        }
    }
}
