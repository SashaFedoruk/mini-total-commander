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


        static void Main()
        {
            bool Input = true;
            bool []isCopy = new bool[]{false, false};
            bool[] isMove = new bool[] { false, false };
            bool pLogicalDrives = true;
            bool isFind = false;


            String[] LogicalDrives = Directory.GetLogicalDrives();  
            FileInfo[] files = new FileInfo[1];
            DirectoryInfo[] folders = new DirectoryInfo[1];
            DirectoryInfo dir = new DirectoryInfo("/");

            DirectoryInfo dirCopy = new DirectoryInfo("/");
            FileInfo fileCopy = new FileInfo("/");

            DirectoryInfo dirMove = new DirectoryInfo("/");
            FileInfo fileMove = new FileInfo("/");

            String[] createMenu = new String[2]{
                        "Create folder",
                        "Create file"
                        };
            int indxActElHelpMenu = 0;
            bool inputHelpMenu = true;

            String[] FindMenu = new String[3]{
                        "Find by name",
                        "Find by size",
                        "Find by Created date",
                        };

            int idxAct = 0;
            ConsoleKeyInfo cki;
            Console.TreatControlCAsInput = true;

            while (true)
            {
                if (Input && pLogicalDrives)
                {
                    Console.Clear();
                    PrintHeaderFolder();
                    for (int i = 0; i < LogicalDrives.Length; i++)
                    {
                        PrintFolders(idxAct, i, (i + 1).ToString(), LogicalDrives[i]);
                    }
                    Console.Write("---------------------------------------------------------");
                    Console.WriteLine("-------------------------------------");
                    Input = false;
                }
                else if (Input)
                {
                    Console.Clear();
                    PrintHeaderFolder();
                    int i = 0;
                    PrintFolders(idxAct, i++, i.ToString(), "[..]");
                    foreach (var el in folders)
                    {
                        PrintFolders(idxAct, i++, i.ToString(), el.Name);
                    }
                    foreach (var el in files)
                    {
                        if (!isFind)
                        {
                            PrintFiles(idxAct, i++, i.ToString(), el);
                        }
                        else
                        {
                            PrintFiles(idxAct, i++, i.ToString(), el, true);
                            
                        }
                    }
                    if (isFind)
                    {
                        isFind = false;
                        idxAct = 0;
                    }
                    Console.Write("---------------------------------------------------------");
                    Console.WriteLine("-------------------------------------");
                    Input = false;
                }
                cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.UpArrow && idxAct > 0)
                {
                    idxAct--;
                    Input = true;
                }
                else if (cki.Key == ConsoleKey.DownArrow)
                {
                    if (pLogicalDrives && idxAct < LogicalDrives.Length - 1)
                    {
                        idxAct++;
                        Input = true;
                    }
                    else if (!pLogicalDrives && idxAct < files.Length + folders.Length)
                    {
                        idxAct++;
                        Input = true;
                    }

                }
                else if ((cki.Key == ConsoleKey.Enter))
                {
                    if (pLogicalDrives)
                    {
                        pLogicalDrives = false;
                        UpdateInfo(ref dir, ref folders, ref files, LogicalDrives[idxAct]);
                        idxAct = 0;
                    }
                    else
                    {
                        if (idxAct == 0)
                        {
                            if (dir.Parent != null)
                            {
                                UpdateInfo(ref dir, ref folders, ref files, dir.Parent.FullName);
                                idxAct = 0;
                            }
                            else
                            {
                                pLogicalDrives = true;
                                idxAct = 0;
                            }
                        }
                        else if (idxAct <= folders.Length)
                        {
                            UpdateInfo(ref dir, ref folders, ref files, folders[idxAct - 1].FullName);
                            idxAct = 0;
                        }

                    }
                    Input = true;
                }
                else if (cki.Key == ConsoleKey.Backspace && !pLogicalDrives)
                {
                    if (dir.Parent != null)
                    {
                        UpdateInfo(ref dir, ref folders, ref files, dir.Parent.FullName);
                        idxAct = 0;
                    }
                    else
                    {
                        pLogicalDrives = true;
                        idxAct = 0;
                    }
                    Input = true;
                }
                else if (cki.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    break;
                }
                else if ((cki.Modifiers & ConsoleModifiers.Control) != 0 && cki.Key == ConsoleKey.C)
                {
                    if (!pLogicalDrives)
                    {
                        if (idxAct <= folders.Length)
                        {
                            dirCopy = folders[idxAct - 1];
                            isCopy[0] = true;
                            isCopy[1] = false;
                            isMove[0] = false;
                            isMove[1] = false;
                            Input = true;
                        }
                        else
                        {
                            fileCopy = files[idxAct - folders.Length - 1];
                            isCopy[1] = true;
                            isCopy[0] = false;
                            isMove[0] = false;
                            isMove[1] = false;
                            Input = true;
                        }
                    }
                }
                else if ((cki.Modifiers & ConsoleModifiers.Control) != 0 && cki.Key == ConsoleKey.X)
                {
                    if (!pLogicalDrives)
                    {
                        if (idxAct <= folders.Length)
                        {
                            dirMove = folders[idxAct - 1];
                            Input = true;
                            isMove[0] = true;
                            isMove[1] = false;
                            isCopy[0] = false;
                            isCopy[1] = false;
                        }
                        else
                        {
                            fileMove = files[idxAct - folders.Length - 1];
                            Input = true;
                            isMove[1] = true;
                            isMove[0] = false;
                            isCopy[0] = false;
                            isCopy[1] = false;
                        }
                    }
                }
                else if ((cki.Modifiers & ConsoleModifiers.Control) != 0 && cki.Key == ConsoleKey.V)
                {
                    if (isMove[0] || isMove[1])
                    {
                        if (dirMove.Parent != null && isMove[0])
                        {
                            dirMove.MoveTo(dir.FullName + "\\\\" + dirMove.Name);
                            Input = true;
                        }
                        else if (fileMove.FullName != "/" && isMove[1])
                        {
                            fileMove.MoveTo(dir.FullName + "\\\\" + fileMove.Name);
                            Input = true;
                        }
                        UpdateInfo(ref dir, ref folders, ref files, dir.FullName);
                        idxAct = 0;
                    }
                    if (isCopy[0] || isCopy[1])
                    {
                        if (dirCopy.Parent != null && isCopy[0])
                        {
                            CopyFolder(dirCopy.FullName, dir.FullName, dirCopy.Name);

                            Input = true;
                        }
                        else if (fileCopy.FullName != "/" && isCopy[1])
                        {
                            fileCopy.CopyTo(Path.Combine(dir.FullName, fileCopy.Name), true);
                            Input = true;
                        }
                        UpdateInfo(ref dir, ref folders, ref files, dir.FullName);
                        idxAct = 0;
                    }
                }
                else if ((cki.Modifiers & ConsoleModifiers.Shift) != 0 && cki.Key == ConsoleKey.Delete)
                {
                    if (idxAct <= folders.Length && idxAct != 0)
                    {
                        folders[idxAct - 1].Delete(true);
                        Input = true;
                    }
                    else if (idxAct != 0)
                    {
                        files[idxAct - folders.Length - 1].Delete();
                        Input = true;
                    }
                    UpdateInfo(ref dir, ref folders, ref files, dir.FullName);
                    idxAct = 0;
                }
                else if ((cki.Modifiers & ConsoleModifiers.Control) != 0 && cki.Key == ConsoleKey.N 
                                                      && !pLogicalDrives)
                {
                    cki = Console.ReadKey();
                    indxActElHelpMenu = 0;
                    while (cki.Key != ConsoleKey.Escape)
                    {
                        if (inputHelpMenu)
                        {
                            Console.SetCursorPosition(40, 10);
                            Console.Write("--------------------------------");
                            for (int i = 0; i < createMenu.Length; i++)
                            {
                                Console.SetCursorPosition(40, 11 + i);
                                if (indxActElHelpMenu == i)
                                {
                                    Console.Write("|");
                                    WriteActLine(createMenu[i]);
                                    Console.SetCursorPosition(71, 11 + i);
                                    Console.Write("|");
                                }
                                else
                                {
                                    Console.Write("|{0}|", createMenu[i].PadRight(30));
                                }
                                
                            }
                            Console.SetCursorPosition(40, 13);
                            Console.Write("--------------------------------");
                            inputHelpMenu = false;
                        }
                        cki = Console.ReadKey();
                        if (cki.Key == ConsoleKey.UpArrow && indxActElHelpMenu > 0)
                        {
                            indxActElHelpMenu--;
                            inputHelpMenu = true;
                        }
                        else if (cki.Key == ConsoleKey.DownArrow && indxActElHelpMenu < createMenu.Length - 1)
                        {
                            indxActElHelpMenu++;
                            inputHelpMenu = true;
                        }
                        else if (cki.Key == ConsoleKey.Enter)
                        {
                            Console.SetCursorPosition(40, 14);
                            Console.Write("|{0}|", "Enter name file:".PadRight(30));
                            Console.SetCursorPosition(40, 15);
                            Console.Write("|{0}|", "".PadRight(30));
                            Console.SetCursorPosition(40, 16);
                            Console.Write("--------------------------------");
                            Console.SetCursorPosition(41, 15);
                            String ent = Console.ReadLine();
                            if (indxActElHelpMenu == 0)
                            {
                                DirectoryInfo tmp = new DirectoryInfo(Path.Combine(dir.FullName, ent));
                                tmp.Create();
                            }
                            else if (indxActElHelpMenu == 1)
                            {
                                if (!Directory.Exists(Path.Combine(dir.FullName, ent)))
                                {
                                    File.Create(Path.Combine(dir.FullName, ent));
                                }
                            }
                            UpdateInfo(ref dir, ref folders, ref files, dir.FullName);
                            break;
                        }
                    }
                    indxActElHelpMenu = 0;
                    Input = true;
                    inputHelpMenu = true;
                }

                else if ((cki.Modifiers & ConsoleModifiers.Control) != 0 && cki.Key == ConsoleKey.F
                                                      && !pLogicalDrives)
                {
                    cki = Console.ReadKey();
                    while (cki.Key != ConsoleKey.Escape)
                    {
                        if (inputHelpMenu)
                        {
                            Console.SetCursorPosition(40, 10);
                            Console.Write("--------------------------------");
                            for (int i = 0; i < FindMenu.Length; i++)
                            {
                                Console.SetCursorPosition(40, 11 + i);
                                if (indxActElHelpMenu == i)
                                {
                                    Console.Write("|");
                                    WriteActLine(FindMenu[i]);
                                    Console.SetCursorPosition(71, 11 + i);
                                    Console.Write("|");
                                }
                                else
                                {
                                    Console.Write("|{0}|", FindMenu[i].PadRight(30));
                                }

                            }
                            Console.SetCursorPosition(40, 14);
                            Console.Write("--------------------------------");
                            inputHelpMenu = false;
                        }
                        cki = Console.ReadKey();
                        if (cki.Key == ConsoleKey.UpArrow && indxActElHelpMenu > 0)
                        {
                            indxActElHelpMenu--;
                            inputHelpMenu = true;
                        }
                        else if (cki.Key == ConsoleKey.DownArrow && indxActElHelpMenu < FindMenu.Length - 1)
                        {
                            indxActElHelpMenu++;
                            inputHelpMenu = true;
                        }
                        else if (cki.Key == ConsoleKey.Enter)
                        {
                            Console.SetCursorPosition(40, 15);
                            Console.Write("|{0}|", "Enter info file:".PadRight(30));
                            Console.SetCursorPosition(40, 16);
                            Console.Write("|{0}|", "".PadRight(30));
                            Console.SetCursorPosition(40, 17);
                            Console.Write("--------------------------------");
                            Console.SetCursorPosition(41, 16);
                            String ent = Console.ReadLine();
                            
                            if (indxActElHelpMenu == 0)
                            {
                                    files = dir.GetFiles(ent, SearchOption.AllDirectories);
                                    folders = new DirectoryInfo[0];
                                    break;
                            }
                            else if (indxActElHelpMenu == 1)
                            {
                                Console.WriteLine(files[0].Length);
                                files = dir.GetFiles("*", SearchOption.AllDirectories).Where(x => x.Length == int.Parse(ent)).ToArray();
                                folders = new DirectoryInfo[0];
                                break;
                            }
                            else if (indxActElHelpMenu == 2)
                            {
                                String[] tmp = ent.Split('.');
                                if (tmp.Length == 3)
                                {
                                    files = dir.GetFiles("*", SearchOption.AllDirectories).Where(x => x.CreationTime.Date.GetDateTimeFormats()[0] == ent).ToArray();
                                    folders = new DirectoryInfo[0];
                                }
                                break;
                            }
                        }
                    }
                    isFind = true;
                    indxActElHelpMenu = 0;
                    Input = true;
                    inputHelpMenu = true;
                    idxAct = 1000;
                }
            }
        }


    }
}