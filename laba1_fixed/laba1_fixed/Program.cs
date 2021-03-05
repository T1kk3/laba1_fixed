using System;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;


namespace laba1
{
  class Person
  {
    public string Name { get; set; }
    public int Age { get; set; }
  }

  class Program
  {
    static async Task Main(string[] args)
    {
      Console.WriteLine("Меню: \n");
      Console.WriteLine("1. Информация о дисках:");
      Console.WriteLine("2. Создание .txt файла:");
      Console.WriteLine("3. XML Файлы:");
      Console.WriteLine("4. JSON Файлы:");
      Console.WriteLine("5. ZIP - файлы:");
      Console.WriteLine("0. Выйти");
      int menubutton = Convert.ToInt32(Console.ReadLine());

      switch (menubutton)
      {
        case 1:
          {
            Console.WriteLine();
            Console.WriteLine("Добрый день! \nДанные диска(-ов): \n");
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
              Console.WriteLine($"Название: {drive.Name}");
              Console.WriteLine($"Тип: {drive.DriveType}");
              if (drive.IsReady)
              {
                Console.WriteLine($"Объем диска: {drive.TotalSize}");
                Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
                Console.WriteLine($"Метка: {drive.VolumeLabel}");
              }
              Console.WriteLine();


            }
            break;
          }
        case 2:
          {
            Console.WriteLine();
            // Каталог для файла
            string path = @"C:\Users\student\source\repos";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
              dirInfo.Create();
            }

            // Запись в файл
            Console.WriteLine("Введите строку для записи в файл:");
            string text = Console.ReadLine();
            using (FileStream fstream = new FileStream($"{path}/testfile.txt", FileMode.OpenOrCreate))
            {
              byte[] array = System.Text.Encoding.Default.GetBytes(text);
              fstream.Write(array, 0, array.Length);
              Console.WriteLine("Текст записан в файл");
            }

            // Чтение из файла
            using (FileStream fstream = File.OpenRead($"{path}/testfile.txt"))
            {
              byte[] array = new byte[fstream.Length];
              fstream.Read(array, 0, array.Length);
              string textFromFile = System.Text.Encoding.Default.GetString(array);
              Console.WriteLine($"Текст из файла: {textFromFile}");
            }

            // Удаление файла
            Console.WriteLine("Нажмите '1', чтобы удалить файл!");
            int delButton = int.Parse(Console.ReadLine());
            if (delButton == 1)
            {
              FileInfo fileInf = new FileInfo($"{path}/testfile.txt");
              if (fileInf.Exists)
              {
                File.Delete($"{path}/testfile.txt");
                Console.WriteLine("Готово.");
              }
            }
            else
            {
              Console.WriteLine("Ошибка! Завершаю работу...");
            }
            break;
          }
        case 3:
          {
            Console.WriteLine("Работа с XML:");
            XmlDocument xDoc = new XmlDocument();
            XDocument xdoc = new XDocument();
            Console.Write("Сколько пользователей нужно ввести: ");
            int count = Convert.ToInt32(Console.ReadLine());
            XElement list = new XElement("users");
            for (int i = 1; i <= count; i++)
            {
              XElement User = new XElement("user");
              Console.Write("Введите имя пользователя: ");
              XAttribute UserName = new XAttribute("name", Console.ReadLine());
              Console.WriteLine();
              Console.Write("Введите возраст пользователя: ");
              XElement UserAge = new XElement("age", Convert.ToInt32(Console.ReadLine()));
              Console.WriteLine();
              Console.Write("Введите название компании: ");
              XElement UserCompany = new XElement("company", Console.ReadLine());
              Console.WriteLine();
              User.Add(UserName);
              User.Add(UserAge);
              User.Add(UserCompany);
              list.Add(User);
            }
            xdoc.Add(list);
            xdoc.Save(@"C:\Users\student\source\repos\users.xml");

            Console.Write("Прочитать XML файл? (yes/no): ");
            switch (Console.ReadLine())
            {
              case "yes":
                Console.WriteLine();
                xDoc.Load(@"C:\Users\student\source\repos\users.xml");
                XmlElement xRoot = xDoc.DocumentElement;
                foreach (XmlNode xnode in xRoot)
                {
                  if (xnode.Attributes.Count > 0)
                  {
                    XmlNode attr = xnode.Attributes.GetNamedItem("name");
                    if (attr != null)
                      Console.WriteLine($"Имя: {attr.Value}");
                  }
                  foreach (XmlNode childnode in xnode.ChildNodes)
                  {
                    if (childnode.Name == "age")
                      Console.WriteLine($"Возраст: {childnode.InnerText}");
                    if (childnode.Name == "company")
                      Console.WriteLine($"Компания: {childnode.InnerText}");
                  }
                }
                Console.WriteLine();
                break;
              case "no":
                break;
              default:
                Console.WriteLine("Введены неправильные данные!");
                break;
            }
            Console.Write("Удалить созданный xml файл? (yes/no): ");
            switch (Console.ReadLine())
            {
              case "yes":
                FileInfo xmlfilecheck = new FileInfo(@"C:\Users\student\source\repos\users.xml");
                if (xmlfilecheck.Exists)
                {
                  xmlfilecheck.Delete();
                  Console.WriteLine("Файл удален!");
                }
                else Console.WriteLine("Файла не существует!");
                break;
              case "no":
                break;
              default:
                Console.WriteLine("Введено неверное зачение!");
                break;
            }
            Console.WriteLine();
            break;
          }
        case 4:
          {
            Console.WriteLine("Работа с JSON:");
            // сохранение данных
            using (FileStream fs = new FileStream(@"D:\Documents\ete\user.json", FileMode.OpenOrCreate))
            {
              Person Roma = new Person() { Name = "Roma", Age = 19 };
              await JsonSerializer.SerializeAsync<Person>(fs, Roma);
              Console.WriteLine("Данные были введены автоматически и они сохранены!");
            }

            // чтение данных
            using (FileStream fs = new FileStream(@"D:\Documents\ete\user.json", FileMode.OpenOrCreate))
            {
              Person restoredPerson = await JsonSerializer.DeserializeAsync<Person>(fs);
              Console.WriteLine($"Name: {restoredPerson.Name}  Age: {restoredPerson.Age}");
            }
            Console.Write("Удалить файл? (yes/no): ");
            switch (Console.ReadLine())
            {
              case "yes":
                File.Delete(@"D:\Documents\ete\user.json");
                Console.WriteLine("\nФайл удален!");
                break;
              case "no":
                break;
            }
            break;
          }

        case 5:
          {
            Console.WriteLine("Работа с ZIP:");
            string SourceFile = @"D:\Documents\lalb1\12.txt"; // исходный файл
            string CompressedFile = @"D:\Documents\lalb1\bin.gz"; // сжатый файл
            string TargetFile = @"D:\Documents\lalb1\123.txt"; // восстановленный файл
                                                               // создание сжатого файла
            Compress(SourceFile, CompressedFile);
            // чтение из сжатого файла
            Decompress(CompressedFile, TargetFile);
            Console.WriteLine("Удалить файлы? (yes/no): ");
            switch (Console.ReadLine())
            {
              case "yes":
                if ((File.Exists(SourceFile) &&
                     File.Exists(CompressedFile) && File.Exists(TargetFile)) == true)
                {
                  File.Delete(SourceFile);
                  File.Delete(CompressedFile);
                  File.Delete(TargetFile);
                  Console.WriteLine("Файлы удалены!");
                }
                else Console.WriteLine("Ошибка в удалении файлов!\n Проверьте их наличие!");
                break;
              case "no":
                break;
              default:
                Console.WriteLine("Введены неправильные данные!");
                break;
            }
            Console.WriteLine();
            break;
          }
        case 0:
          {
            Console.WriteLine("Закрываю.");
            break;
          }

        default:
          {
            Console.WriteLine("Перезапустите программу и введите действительное значение!");
            break;
          }
      }
    }

    public static void Compress(string sourceFile, string compressedFile)
    {
      // поток для чтения исходного файла
      using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
      {
        // поток для записи сжатого файла
        using (FileStream targetStream = File.Create(compressedFile))
        {
          // поток архивации
          using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
          {
            sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
            Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.",
                sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());
          }
        }
      }
    }
    public static void Decompress(string compressedFile, string targetFile)
    {
      // поток для чтения из сжатого файла
      using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
      {
        // поток для записи восстановленного файла
        using (FileStream targetStream = File.Create(targetFile))
        {
          // поток разархивации
          using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
          {
            decompressionStream.CopyTo(targetStream);
            Console.WriteLine("Восстановлен файл: {0}", targetFile);
          }
        }
      }


    }
  }
}
