using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    // Создаём класс "Студент" - это шаблон для объекта
    public class Student
    {
        public string FullName { get; set; }    // Свойство: ФИО студента
        public int Age { get; set; }            // Свойство: Возраст студента
        public string Group { get; set; }       // Свойство: Учебная группа
        public double AverageGrade { get; set; } // Свойство: Средний балл
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Устанавливаем кодировку для корректного отображения русского текста
            Console.OutputEncoding = Encoding.UTF8;

            // ===== ЧАСТЬ 1: РУЧНАЯ СЕРИАЛИЗАЦИЯ (Объект → JSON строка) =====
            Console.WriteLine("========== СЕРИАЛИЗАЦИЯ ОБЪЕКТА ==========");

            // Создаём объект студента и заполняем его данными
            Student student = new Student();
            student.FullName = "Иванов Алексей Петрович";  // Заполняем ФИО
            student.Age = 20;                               // Заполняем возраст
            student.Group = "ИС-21";                        // Заполняем группу
            student.AverageGrade = 4.7;                     // Заполняем средний балл

            // Ручная сериализация - создаём JSON строку сами через конкатенацию
            string jsonResult = "{\"FullName\":\"" + student.FullName + "\",";
            jsonResult = jsonResult + "\"Age\":" + student.Age + ",";
            jsonResult = jsonResult + "\"Group\":\"" + student.Group + "\",";
            jsonResult = jsonResult + "\"AverageGrade\":" + student.AverageGrade + "}";

            // Выводим результат сериализации в консоль
            Console.WriteLine("Результат сериализации (объект → JSON):");
            Console.WriteLine(jsonResult);

            // ===== ЧАСТЬ 2: ДЕСЕРИАЛИЗАЦИЯ (JSON строка → Объект) =====
            Console.WriteLine("\n========== ДЕСЕРИАЛИЗАЦИЯ JSON ==========");

            // Создаём JSON строку вручную (как будто получили от сервера)
            string jsonString = "{\"FullName\":\"Петрова Мария Сергеевна\",\"Age\":22,\"Group\":\"ПМ-31\",\"AverageGrade\":5.0}";

            Console.WriteLine("Исходная JSON строка:");
            Console.WriteLine(jsonString);

            // Создаём новый объект для заполнения
            Student newStudent = new Student();

            // Вспомогательный метод для извлечения строкового значения из JSON
            string FindStringValue(string json, string fieldName)
            {
                string searchPattern = "\"" + fieldName + "\":\"";  // Шаблон для поиска: "fieldName":"
                int start = json.IndexOf(searchPattern) + searchPattern.Length;  // Начало значения
                int end = json.IndexOf("\"", start);  // Конец значения (следующая кавычка)
                return json.Substring(start, end - start);  // Вырезаем значение между кавычками
            }

            // Вспомогательный метод для извлечения числового значения из JSON
            string FindNumberValue(string json, string fieldName)
            {
                string searchPattern = "\"" + fieldName + "\":";  // Шаблон для поиска: "fieldName":
                int start = json.IndexOf(searchPattern) + searchPattern.Length;  // Начало числа
                int end = start;
                // Идём по строке пока встречаем цифры или точку
                while (end < json.Length && ((json[end] >= '0' && json[end] <= '9') || json[end] == '.'))
                {
                    end++;
                }
                return json.Substring(start, end - start);  // Вырезаем число
            }

            // Извлекаем все поля из JSON строки
            newStudent.FullName = FindStringValue(jsonString, "FullName");      // Извлекаем ФИО
            newStudent.Group = FindStringValue(jsonString, "Group");           // Извлекаем группу

            string ageStr = FindNumberValue(jsonString, "Age");                // Извлекаем возраст как строку
            newStudent.Age = int.Parse(ageStr);                                 // Преобразуем в число

            string gradeStr = FindNumberValue(jsonString, "AverageGrade");     // Извлекаем балл как строку
            // Исправление ошибки: заменяем точку на запятую для русской Windows
            gradeStr = gradeStr.Replace(".", ",");
            newStudent.AverageGrade = double.Parse(gradeStr);                   // Преобразуем в дробное число

            // Выводим данные полученного объекта
            Console.WriteLine("\nДанные объекта после десериализации:");
            Console.WriteLine("ФИО студента: " + newStudent.FullName);        // Выводим ФИО
            Console.WriteLine("Возраст: " + newStudent.Age);                  // Выводим возраст
            Console.WriteLine("Группа: " + newStudent.Group);                 // Выводим группу
            Console.WriteLine("Средний балл: " + newStudent.AverageGrade);    // Выводим балл

            // Задержка консоли
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();  // Ждём нажатия клавиши
        }
    }
}