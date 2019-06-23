using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nMiniTotalComander
{
    public partial class MiniTotalComander
    {
        static public void UpdateInfo(ref DirectoryInfo dir, ref DirectoryInfo[] folders, ref  FileInfo[] files, String NewPath){
            dir = new DirectoryInfo(NewPath);
            files = dir.GetFiles();
            folders = dir.GetDirectories();
        }

        static void WriteActLine(String value)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("{0}", value.PadRight(30));
            Console.ResetColor();
            Console.WriteLine();
        }

        static void PrintHeaderFolder()
        {
            Console.Write("---------------------------------------------------------");
            Console.WriteLine("-------------------------------------");
            Console.Write("|#   |Name                                              |");
            Console.WriteLine("Size                |Last Modify    |");
            Console.Write("---------------------------------------------------------");
            Console.WriteLine("-------------------------------------");
        }

        public static void CopyFolder(String SourcePath, String DestinationPath, String NameFolder)
        {
            Directory.CreateDirectory(DestinationPath + "\\\\" + NameFolder);
            DestinationPath += "\\\\" + NameFolder;
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));
            }

            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
            }
        }

        static void PrintLogicalDrivers(int idxAct, int i, String idx, String name)
        {
            if (idxAct == i)
            {

                String tmp = String.Format("|{0}|{1}|{2, 20}|{2, 15}|",
                   idx.PadRight(4), name.PadRight(50), "");
                WriteActLine(tmp);
            }
            else
            {
                Console.WriteLine("|{0}|{1}|{2, 20}|{2, 15}|",
                   idx.PadRight(4), name.PadRight(50), "");
            }
        }

        static void PrintFolders(int idxAct, int i, String idx, String name)
        {
            if (idxAct == i)
            {
                String tmp = String.Format("|{0}|{1}|{2, 20}|{2, 15}|",
                   idx.PadRight(4), name.PadRight(50), "");
                WriteActLine(tmp);
            }
            else
            {
                Console.WriteLine("|{0}|{1}|{2, 20}|{2, 15}|",
                   idx.PadRight(4), name.PadRight(50), "");
            }
        }

        static void PrintFiles(int idxAct, int i, String idx, FileInfo file, bool pFullName = false)
        {
            String size = file.Length.ToString();

            String dateModify = file.LastWriteTime.GetDateTimeFormats()[0];
            if (idxAct == i)
            {
                String tmp;
                if (!pFullName)
                {
                    tmp = String.Format("|{0}|{1}|{2}|{3}|",
                       idx.PadRight(4), file.Name.PadRight(50), size.PadRight(20), dateModify.PadRight(15));
                }
                else
                {
                    tmp = String.Format("|{0}|{1}|{2}|{3}|",
                       idx.PadRight(4), file.FullName.PadRight(50), size.PadRight(20), dateModify.PadRight(15));
                }
                WriteActLine(tmp);
            }
            else
            {
                if (!pFullName)
                {
                    Console.WriteLine("|{0}|{1}|{2}|{3}|",
                       idx.PadRight(4), file.Name.PadRight(50), size.PadRight(20), dateModify.PadRight(15));
                }
                else
                {
                    Console.WriteLine("|{0}|{1}|{2}|{3}|",
                       idx.PadRight(4), file.FullName.PadRight(50), size.PadRight(20), dateModify.PadRight(15));
                }
            }
        }
    }
}
