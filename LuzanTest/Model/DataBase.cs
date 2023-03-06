using LuzanTest.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LuzanTest.Model
{   
    public class DataBase
    {
        private string _connectionString;
        private SqlConnection _connection;
        public DataBase(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(connectionString);
        }
        private string _status; //Информация о последней операции
        public string GetStatus()
        {
            return _status;
        }
        public bool CreatePeople() // Задание 1
        {
            bool status;
            _connection.Open();
            try
            {
                int isCreate = 0;
                string isPeopleCreate =
                    "IF OBJECT_ID (N'People', N'U') IS NOT NULL " +
                    "SELECT 1 AS res ELSE SELECT 0 AS res; ";
                SqlCommand sqlCommand = new SqlCommand(isPeopleCreate, _connection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while(reader.Read())
                {
                    isCreate = int.Parse(reader["res"].ToString());
                }
                reader.Close();
                if(isCreate == 1)
                {
                    _status = "Таблица уже создана";
                    _connection.Close();
                    return true;
                }
                string createPeopleCommand = 
                    "CREATE TABLE People (" +
                    "Id int Primary key not null identity(1, 1)," +
                    "Name nvarchar(MAX) not null," +
                    "Surname nvarchar(MAX) not null," +
                    "Patronymic nvarchar(MAX) not null," +
                    "BirthDate DateTime not null," +
                    "Gender nvarchar(1)" +
                    "); ";
                sqlCommand = new SqlCommand(createPeopleCommand, _connection);
                sqlCommand.ExecuteNonQuery();
                _status = "Таблица создана";
                status = true;
            } 
            catch(Exception ex)
            {
                _status = ex.Message;
                status = false;
            }
            _connection.Close();
            return status;
        }
        public int AddPeople(string name, string surname, string patronymic, DateTime birthDate, string gender) // Задание 2
        {
            _connection.Open();
            int RowsChanged = 0;
            try
            {
                string insertCommand = string.Format("INSERT INTO People (Name, Surname, Patronymic, BirthDate, Gender) VALUES ('{0}','{1}','{2}','{3}','{4}');", name,surname,patronymic,birthDate,gender);
                SqlCommand sqlCommand = new SqlCommand(insertCommand, _connection);
                RowsChanged = sqlCommand.ExecuteNonQuery();
                _status = "Успешное добавление";
            }
            catch(Exception ex)
            {
                _status = ex.Message;
            }
            _connection.Close();
            return RowsChanged;
        }
        public int DeletePeople(string name, string surname, string patronymic, DateTime birthDate, string gender)
        {
            _connection.Open();
            int RowsChanged = 0;
            try
            {
                string insertCommand = string.Format("DELETE FROM People " +
                    "WHERE Name = '{0}' and " +
                    "Surname = '{1}' and " +
                    "Patronymic = '{2}' and " +
                    "BirthDate = '{3}' and " +
                    "Gender = '{4}';", 
                    name, surname, patronymic, birthDate, gender);
                SqlCommand sqlCommand = new SqlCommand(insertCommand, _connection);
                RowsChanged = sqlCommand.ExecuteNonQuery();
                _status = "Успешное удаление";
            }
            catch (Exception ex)
            {
                _status = ex.Message;
            }
            _connection.Close();
            return RowsChanged;
        }
        public List<People> SelectUniquePeople() // Задание 3
        {
            
            string distinctCommand = "Select DISTINCT Name, Surname, Patronymic, BirthDate, Gender from People";
            SqlCommand sqlCommand = new SqlCommand(distinctCommand, _connection);
            _connection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();

            List<People> people = new List<People>();
            while (reader.Read())
            {
                People one = new People()
                {
                    Name = reader["Name"].ToString(),
                    Surname = reader["Surname"].ToString(),
                    Patronymic = reader["Patronymic"].ToString(),
                    
                    Gender = reader["Gender"].ToString(),
                };
                DateTime temp;
                DateTime.TryParse(reader["BirthDate"].ToString(), out temp);
                one.BirthDate = temp;
                people.Add(one);
            }
            reader.Close();
            _connection.Close();
            return people;
        }
        public int AddMillione() // Задание 4
        {
            int RowsChanged = 0;
            _connection.Open();
            try
            {
                string dropDommand = "TRUNCATE TABLE people"; // очищаю таблицу
                SqlCommand sqlCommand = new SqlCommand(dropDommand, _connection);
                sqlCommand.ExecuteNonQuery();

                List<string> Surnames = new List<string>() { "Abramson", "Babcock", "Calhoun", "Daniels", "Eddington", "Faber", "Galbraith", "Haig", "Jacobson", "Keat", "Laird", "MacAdam", "Nash", "Oakman", "Packer", "Quincy", "Raleigh", "Salisbury", "Taft", "Vance", "Wainwright", "Youmans" };
                List<string> MalesName = new List<string>() { "Abbott", "Bailey", "Cade", "Dale", "Earl", "Fabian", "Garfield", "Hadden", "Ike", "Jack", "Keith", "Lacey", "Magnus", "Nathaniel", "Obadiah", "Paddy", "Quentin", "Rafe", "Samson", "Tad", "Ulric", "Valentine", "Waally", "Zach" };
                List<string> FemalesName = new List<string>() { "Abbey", "Babs", "Camilla", "Daisy", "Ebony", "Faith", "Gail", "Haley", "Ida", "ackie", "Jacqueline", "Kaley", "Lacey", "Mabel", "Nadia", "Octavia", "Pamela", "Queenie", "Rachel", "Sabella", "Tammy", "Unity", "Val", "Wanda", "Xenia", "Yasmin", "Zanna" };
                //string names = "Abbey Abbie Abigail Ada Adelaide Adele Adrienne Agatha Agnes Aileen Alana Alex Alexandra Alice Alina Alison Alma Amanda Amber Amelia Amy Andrea Angela Ann Annabelle Anne Anthea April Arlene Ashley Audrey Ava Avril Babs Barbara Bea Beatrix Becky Belinda Bella Bernice Berry Bertha Beryl Bess Bet Beth Bethany Betsy Betty Beverly Blanche Bobbie Bonnie Brenda Brianne Bridget Britney Brittany Brooke Camilla Candice Cara Carissa Carla Carly Carmel Carol Caroline Carrie Cass Catherine Cathy Charis Charity Charlene Charlie Charlotte Chelsea Cherida Cherish Cheryl Chloe Christina Christine Clara Clare Clarissa Claudia Connie Courtney Cynthia Daisy Darlene Davida Dawn Deborah Dee Deirdre Delia Dena Diana Dina Dolly Donna Dora Doreen Doris Dorothy Ebony Edith Edna Edwina Effie Elaine Eleanor Elena Elinor Eliza Elizabeth Ella Ellen Ellie Elsa Emily Emma Emmy Enola Erin Esta Estelle Ethel Eudora Eugenia Eunice Eve Evelyn Evette Evie Evonne Faith Fanny Fay Faye Felicity Fern Flo Flora Florence Florrie Fran Frances Freda Gail Gale Gayle Gaynor Georgia Georgiana Geraldine Germaine Gertrude Gill Gillian Gloria Glynis Grace Gracie Gretta Guinevere Haley Harriet Hattie Hayley Hazel Heather Helen Henrietta Hettie Hilary Hilda Hollie Holly Honey Hope Hyacinth Ida Imogen Iole Irene Iris Isabel Isabella Isabelle Ivy ackie Jacqueline Jade Jane Janet Janette Janice Jasmine Jay Jean Jeane Jennet Jennifer Jenny Jess Jessica Jessie Jewel Jill Joan Jodie Josephine Joy Joyce Judy Juliet June Justine Kaley Kate Katherine Katie Katy Kay Kayla Kayley Keeley Kelly Kendra Kerena Kerry Kim Kimberley Kitty Kyla Kyle Kylie Lacey Lana Laura Lauren Leanne Lee Lena Leona Lesley Lexi Lexy Libby Lilian Lilly Lily Linda Linda Lindsay Lindy Linette Liona Lisa Livia Liz Liza Lizzie Lola Loretta Lorraine Lottie Louise Lucy Lynette Lynn Mabel Madeline Madge Madonna Maggie Maggie Malvina Mandy Mara Marcia Margaret Marice Marilyn Mary Maud Maura Maureen Mavis Maxine May Maya Meg Melanie Melinda Melissa Melody Mercy Merle Mildred Millicent Millie Minnie Mirabelle Miranda Misty Molly Mona Monica Muriel Myra Myrtle Nadia Nancy Nell Nerissa Nessa Nicola Nita Nora Noreen Norma Octavia Olive Olivia Opal Pamela Pandora Pansy Patience Patricia Patsy Paula Pearl Peggy Penelope Petra Petula Philippa Philomena Phoebe Phyllis Pippa Polly Poppy Primrose Prudence Prunella Queenie Rachel Raine Reanna Reenie Regina Rhoda Rikki Rina Rita Robin Ronnie Rosalind Roseanne Rose Rosemary Rosie Rowena Roxanne Ruby Sabella Sabrina Sadie Sally Samantha Sandra Sandy Sapphire Scarlett Selena Shannah Shannon Sharon Sheila Shirley Silver Sissy Skye Sue Summer Susan Sybil Sylvia Tammy Tamsin Tansy Tara Tasha Tawny Teri Tessa Thelma Tiffany Tilda Tori Tracy Trina Trisha Trixie Trudy Unity Ursa Ursula Val Valda Valene Valerie Vanessa Velma Vera Verity Verona Vicky Victoria Viola Violet Virginia Vita Vivian Wanda Wendy Whitney Willa Willow Wilona Winifred Wynne Xenia Xenthe Yasmin Yolanda Zanna Zelda Zelene Zera Zoe ";
                //List<string> namee = names.Split(' ').Where(Ns => Ns != "").ToList();
                //var res = namee.GroupBy(Ns => Ns[0]).Select(Ns => Ns.Where(Nss => Nss.Length >1).First()).ToList();
                //string namesddw = "new List<string>() {\"" + string.Join("\",\"", res) + "\"}";
                Random rnd = new Random();
                string command = "";
                string name;
                string surname;
                string patronymic;
                string gender;
                for (int i = 0; i < 1000; i++)
                {
                    
                    if (i % 2 == 0)
                    {
                        name = MalesName[rnd.Next(0, MalesName.Count)];
                        surname = Surnames[rnd.Next(0, Surnames.Count)];
                        patronymic = MalesName[rnd.Next(0, MalesName.Count)];
                        gender = "M";
                    }
                    else
                    {
                        name = FemalesName[rnd.Next(0, FemalesName.Count)];
                        surname = Surnames[rnd.Next(0, Surnames.Count)];
                        patronymic = MalesName[rnd.Next(0, MalesName.Count)];
                        gender = "F";
                    }

                    string year = rnd.Next(1950, 2023).ToString();
                    string month = rnd.Next(1, 12).ToString();
                    string day = rnd.Next(1, 28).ToString();
                    DateTime birthDate = DateTime.Parse(string.Join(".", day, month, year));
                    command += "('" + name + "','" + surname + "','" + patronymic + "','" + birthDate.ToString("dd.MM.yyyy") + "','" + gender + "')";
                    if (i < 999)
                    {
                        command += ",";
                    }
                    else
                    {
                        command += ";";
                    }
                }
                for (int i = 0; i < 1000; i++)
                {
                    sqlCommand = new SqlCommand("Insert Into People Values " + command, _connection); ;
                    RowsChanged += sqlCommand.ExecuteNonQuery();

                }
                command = "";
                string fname = MalesName.Where(Ns => Ns[0] == 'F').First();
                for (int i = 0; i < 100; i++)
                {

                    name = fname;
                    surname = Surnames[rnd.Next(0, Surnames.Count)];
                    patronymic = MalesName[rnd.Next(0, MalesName.Count)];
                    gender = "M";
                    string year = rnd.Next(1950, 2023).ToString();
                    string month = rnd.Next(1, 12).ToString();
                    string day = rnd.Next(1, 28).ToString();
                    DateTime birthDate = DateTime.Parse(string.Join(".", day, month, year));
                    command += "('" + name + "','" + surname + "','" + patronymic + "','" + birthDate.ToString("dd.MM.yyyy") + "','" + gender + "')";
                    if (i < 99)
                    {
                        command += ",";
                    }
                    else
                    {
                        command += ";";
                    }
                }
                sqlCommand = new SqlCommand("Insert Into People Values " + command, _connection);
                RowsChanged += sqlCommand.ExecuteNonQuery();
                _status = "Успешное выполнение";
            }
            catch(Exception ex)
            {
                _status = ex.Message;
            }
            _connection.Close();
            return RowsChanged;
        }
        private string _count;
        public string GetCount()
        {
            return _count.ToString();
        }
        public List<People> SelectFormFMale() // Задание 5
        {
            string selectFromFMale = "Select Name, Surname, Patronymic, BirthDate, Gender from People " +
                "Where SUBSTRING(Name, 1, 1) = 'F' and " +
                "Gender = 'M'";
            SqlCommand sqlCommand = new SqlCommand(selectFromFMale, _connection);
            List<People> people = new List<People>();
            _connection.Open();
            try
            {
                DateTime start = DateTime.Now;
                SqlDataReader reader = sqlCommand.ExecuteReader();
                DateTime end = DateTime.Now;
                
                while (reader.Read())
                {
                    People one = new People()
                    {
                        Name = reader["Name"].ToString(),
                        Surname = reader["Surname"].ToString(),
                        Patronymic = reader["Patronymic"].ToString(),

                        Gender = reader["Gender"].ToString(),
                    };
                    DateTime temp;
                    DateTime.TryParse(reader["BirthDate"].ToString(), out temp);
                    one.BirthDate = temp;
                    people.Add(one);

                }
                _count = (end - start).TotalSeconds.ToString();
                reader.Close();
                _status = "Успешное выполнение";
            }
            catch (Exception ex)
            {
                _status = ex.Message;
            }
            _connection.Close();
            
            return people;
        }

    }
}
