using LuzanTest.Tables;
using LuzanTest.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LuzanTestUni
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMyApp1()
        {
            bool result = MainModel.GetDataBase().CreatePeople();
            string status = MainModel.GetDataBase().GetStatus();
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void TestMyApp2()
        {
            string name = "Foo";
            string surname = "Bar";
            string patronymic = "Don";
            DateTime birthDate = DateTime.Parse(DateTime.Now.ToString("dd.MM.yyyy"));
            string gender = "T";
            int result = MainModel.GetDataBase().AddPeople(name, surname, patronymic, birthDate, gender);
            MainModel.GetDataBase().DeletePeople(name, surname, patronymic, birthDate, gender);
            string status = MainModel.GetDataBase().GetStatus();
            Assert.AreEqual(1, result);    
        }
        [TestMethod]
        public void TestMyApp3()
        {
            List<People> list = MainModel.GetDataBase().SelectUniquePeople();
            var res =   (from dbo in list
                        select dbo).Distinct().OrderBy(name => name.ToString());
            Assert.AreEqual(true, res.Count() > 0 && res.Count() == list.Count());            
        }
        [TestMethod]
        public void TestMyApp4()
        {
            int result = MainModel.GetDataBase().AddMillione();
            string status = MainModel.GetDataBase().GetStatus();
            Assert.AreEqual(1000100, result);
        }
        [TestMethod]
        public void TestMyApp5()
        {
            List<People> list = MainModel.GetDataBase().SelectFormFMale();
            var res = from dbo in list
                      where dbo.Name[0] == 'F' &&
                      dbo.Gender == "M"
                      select dbo;
            Assert.AreEqual(true, res.Count() > 0 && res.Count() == list.Count());
        }
    }
}
