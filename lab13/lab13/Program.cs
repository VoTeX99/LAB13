using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;


namespace lab13
{
    class Program
    {
        public static DateTime now = DateTime.Now;
        static void Main(string[] args)
        {
            string buf = "";
            BVGLog bvgLog = new BVGLog();
            bvgLog.EventIO += BVGLog.OnEvent;
            Console.WriteLine("Введите строку для записи");
            bvgLog.Write(
                new string[] {
                    "Береза",
                    "Ван",
                    "Лав"
                }
            );
            Console.WriteLine("\nПострочныый вывод из файла: " + bvgLog.path + "\n");
            bvgLog.Read(bvgLog.path);

            Console.WriteLine("Введите строку для поиска");
            buf = Console.ReadLine();

            Console.WriteLine("Поиск по строке");
            bvgLog.Search(buf);

            BVGDiskInfo info = new BVGDiskInfo();
            info.InfoOfDrivers();

            BVGFileInfo fileInfo = new BVGFileInfo();
            fileInfo.InfoOfFiles(bvgLog.path);

            BVGFileManager manager = new BVGFileManager();
            manager.ListOfFiles();
            manager.Task2();

            manager.Task3(@"E:\", "txt");
            manager.Task4();
            Console.ReadLine();
        }

        class BVGLog
        {
            public delegate void del(string m, string path);
            public event del EventIO;
            public string path = @"BVGlogfile.txt";

            public void Write(string[] s)
            {
                string buf = "";
                using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
                {
                    foreach (string str in s) { sw.WriteLine(str); }
                    sw.Close();
                }

                Type type = typeof(BVGLog);

                foreach (MethodInfo m in type.GetMethods())
                {
                    if (m.Name == "Write") { buf = m.Name; }
                }

                EventIO(buf, path);
            }


            public void Read(string path)
            {
                string buf = "";

                Type type = typeof(BVGLog);

                foreach (MethodInfo m in type.GetMethods())
                {
                    if (m.Name == "Read")
                    {
                        buf = m.Name;
                    }
                }

                EventIO(buf, path);
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }


                    sr.Close();

                }
            }


            public void Search(string search)
            {

                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line == search)
                        {
                            Console.WriteLine("Найдено совпадение " + line);
                            Console.WriteLine(sr.ReadLine());
                        }
                    }
                    sr.Close();
                }
            }

            public static void OnEvent(string m, string path)
            {
                using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine("Пользователь вызвал метод: " + m);
                    sw.WriteLine("Время: " + now);
                }
            }
        }


        class BVGDiskInfo
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            public void InfoOfDrivers()
            {
                foreach (DriveInfo drive in drives)
                {
                    Console.WriteLine("Название: {0}", drive.Name);
                    Console.WriteLine("Тип: {0}", drive.DriveType);
                    if (drive.IsReady)
                    {
                        Console.WriteLine("Объем диска: {0}", drive.TotalSize);
                        Console.WriteLine("Свободное пространство: {0}", drive.TotalFreeSpace);
                        Console.WriteLine("Метка: {0}", drive.VolumeLabel);
                    }
                    Console.WriteLine();
                }
            }

        }


        class BVGFileInfo
        {
            public void InfoOfFiles(string path)
            {

                FileInfo fileInf = new FileInfo(path);
                if (!fileInf.Exists) return;

                Console.WriteLine("Имя файла: {0}", fileInf.Name);
                Console.WriteLine("Путь : {0}", fileInf.FullName);
                Console.WriteLine("Время создания: {0}", fileInf.CreationTime);
                Console.WriteLine("Размер: {0}", fileInf.Length);
            }
        }

        class BVGFileManager
        {
            public void ListOfFiles()
            {
                string dirName = @"C:\";
                string[] dirs = Directory.GetDirectories(dirName);
                string[] files = Directory.GetFiles(dirName);
                if (!Directory.Exists(dirName)) return;


                Console.WriteLine("Подкаталоги:");

                foreach (string s in dirs)
                {
                    Console.WriteLine(s);
                }
                Console.WriteLine();
                Console.WriteLine("Файлы:");

                foreach (string s in files)
                {
                    Console.WriteLine(s);
                }
            }

            public void Task2()
            {
                string dirName = @"C:\";
                string prefPath = @"";
                string[] dirs = Directory.GetDirectories(dirName);
                string[] files = Directory.GetFiles(dirName);
                DirectoryInfo directory = new DirectoryInfo(prefPath + @"BVGInspect");
                directory.Create();

                string path = prefPath + @"BVGInspect\BVGdirinfo.txt";

                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine("Подкаталоги:");

                    foreach (string s in dirs)
                    {
                        sw.WriteLine(s);
                    }
                    sw.WriteLine();
                    sw.WriteLine("Файлы:");

                    foreach (string s in files)
                    {
                        sw.WriteLine(s);
                    }

                    Console.WriteLine("Зaписано");

                }

                string newPath = prefPath + @"BVGInspect\NEWBVGdirinfo.txt"; ;
                FileInfo fileInf = new FileInfo(path);
                if (fileInf.Exists)
                {
                    fileInf.CopyTo(newPath, true);
                }
                File.Delete(prefPath + @"BVGInspect\BVGdirinfo.txt");


            }

            public void Task3(string dirName, string txt)
            {
                string prefPath = @"";
                DirectoryInfo directory = new DirectoryInfo(prefPath + "BVGFiles");
                directory.Create();

                string[] files = Directory.GetFiles(dirName, "*." + txt);

                foreach (var s in files)
                {
                    FileInfo fileInf = new FileInfo(s);
                    if (fileInf.ToString() == s)
                    {
                        Console.WriteLine(s);
                        fileInf.CopyTo(prefPath + @"BVGFiles\" + fileInf.Name, true);
                    }
                }
                DirectoryInfo dirInfo = new DirectoryInfo(prefPath + @"BVGInspect");
                dirInfo.Delete(true);
                Directory.Move(prefPath + @"BVGFiles", prefPath + @"BVGInspect\");
            }

            public void Task4()
            {
                string prefPath = @"";
                string startPath = prefPath + @"BVGInspect";
                string zipPath = prefPath + @"result.zip";
                string extractPath = prefPath + @"result";

                ZipFile.CreateFromDirectory(startPath, zipPath);

                //ZipFile.ExtractToDirectory(zipPath, extractPath);

            }
        }
    }
}
