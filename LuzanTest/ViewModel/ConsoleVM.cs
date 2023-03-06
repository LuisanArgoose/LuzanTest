using LuzanTest.Model;
using LuzanTest.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LuzanTest.ViewModel
{
    public class ConsoleVM
    {
        public ConsoleVM()
        {
            IsOut = false;
        }
        public string Command
        {
            set => CommandHandler(value);
        }
        private void CommandHandler(string command) // Метод обработки команд
        {
            List<string> splitCommand = command.Split(' ').ToList();
            if (command == "out")
            {
                IsOut = true;
                Result = "Выход";
                return;
            }
            if (command == "clear")
            {
                IsClear = true;
                Result = "Очищено";
                return;
            }

            if (splitCommand.Count < 2)
            {
                Result = "Неизвестная команда";
                return;
            }
            string commandId = string.Join(" ", splitCommand[0], splitCommand[1]); // Разделение строки что бы считать команду
            switch (commandId)
            {
                case "myApp 1":
                    MainModel.GetDataBase().CreatePeople();
                    Result = MainModel.GetDataBase().GetStatus();                    
                    break;
                case "myApp 2":
                    if (splitCommand.Count != 7)
                    {
                        Result = "Нехватает данных";
                        return;
                    }
                    DateTime birthDate;
                    if(DateTime.TryParse(splitCommand[5], out birthDate) == false)
                    {
                        Result = "Неверный формат даты рождения";
                        return;
                    }
                    if (splitCommand[6] != "M" && splitCommand[6] != "F")
                    {
                        Result = "Неверный формат пола";
                        return;
                    }
                    MainModel.GetDataBase().AddPeople(
                          splitCommand[2]
                        , splitCommand[3]
                        , splitCommand[4]
                        , birthDate
                        , splitCommand[6]
                        );
                    Result = MainModel.GetDataBase().GetStatus();
                    break;
                case "myApp 3":
                    List<People> people = MainModel.GetDataBase().SelectUniquePeople();
                    Result = "";
                    foreach (var p in people)
                    {

                        Result += p.ToString() + "\n\r";
                    }
                    break;
                case "myApp 4":
                    MainModel.GetDataBase().AddMillione();
                    Result = MainModel.GetDataBase().GetStatus();
                    break;
                case "myApp 5":
                    List<People> people1 = MainModel.GetDataBase().SelectFormFMale();
                    Result = "";
                    foreach (var p in people1)
                    {

                        Result += p.ToString() + "\n\r";
                    }
                    Result += MainModel.GetDataBase().GetCount() + " секунд";
                    break;
                default:
                    Result = "Неизвестная команда";
                    break;
            }
        }
        private string _result;
        public string Result
        {
            get => _result;
            private set => _result = value;
        }
        private bool _isOut;
        public bool IsOut 
        { 
            get => _isOut;
            private set => _isOut = value;
        }
        private bool _isClear;
        public bool IsClear
        {
            get
            {
                bool res = _isClear;
                _isClear = false;
                return res;
            }
            private set => _isClear = value;
        }
    }
}
