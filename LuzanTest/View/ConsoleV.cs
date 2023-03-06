using LuzanTest.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuzanTest
{
    internal class ConsoleV
    {
        
        static void Main(string[] args)
        {
            ConsoleVM _dataContext = new ConsoleVM(); // Задание контекста
            while (true)
            {
                _dataContext.Command =  Console.ReadLine();
                if (_dataContext.IsOut) // Привязка к свойству
                    break;                
                if (_dataContext.IsClear) // Привязка к свойству
                    Console.Clear();
                Console.WriteLine(_dataContext.Result); // Привязка к свойству


            }
        }
    }
}
