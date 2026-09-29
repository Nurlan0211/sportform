using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json; // Подключаем библиотеку для работы с JSON

namespace ConsoleApp1
{
    // Класс, который будем сериализовать в JSON
    public class Student
    {
        public string Name { get; set; }      // Имя студента
        public int Age { get; set; }          // Возраст студента
        public string Group { get; set; }     // Группа студента
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 1. СОЗДАЕМ ОБЪЕКТ ДЛЯ СЕРИАЛИЗАЦИИ
            // Создаем экземпляр класса Student и заполняем его данными
            Student student = new Student();
            student.Name = "Алексей Иванов";   // Присваиваем имя
            student.Age = 20;                  // Присваиваем возраст
            student.Group = "ИС-21";           // Присваиваем группу

            // 2. СЕРИАЛИЗУЕМ ОБЪЕКТ В JSON СТРОКУ
            // JsonConvert.SerializeObject - метод, который превращает объект в JSON строку
            string json = JsonConvert.SerializeObject(student);

            // 3. ВЫВОДИМ РЕЗУЛЬТАТ В КОНСОЛЬ
            Console.WriteLine("Объект сериализован в JSON:");
            Console.WriteLine(json);  // Вывод JSON строки

            // 4. ЗАДЕРЖИВАЕМ КОНСОЛЬ ЧТОБЫ УВИДЕТЬ РЕЗУЛЬТАТ
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();  // Ждем нажатия клавиши
        }
    }
}