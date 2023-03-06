using LuzanTest;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuzanTest.Model
{
    static public class MainModel
    {
        static MainModel()
        {
            _dataBase= new DataBase("Server=.\\SQLEXPRESS;Database=LuzanTestDB;Trusted_Connection=True;");
        }
        static private readonly DataBase _dataBase;
        static public DataBase GetDataBase()
        {
            return _dataBase;
        }
    }

}
