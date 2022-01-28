using Microsoft.Win32;
using System;
using System.IO;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Skapar en DriveInfo och hämtar alla aktiva diskar
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            //För varje disk i arrayen med diskar
            foreach (DriveInfo d in allDrives)
            {
                //Om aktuell disk är aktiv (redo), och typen av disk är removable
                //OBS! Följande kod sparar ner licensnyckeln på den första aktiva removable devicen som den hittar.
                //För mer specifik plats så får man skriva om ifsatsen, eller skippa sökningen efter drives helt
                //och ange sökvägen manuellt i konsolfönstret.
                if (d.IsReady && d.DriveType == DriveType.Removable)
                {
                    //Skapa upp en ny registernyckel och sök reda på värdet i angiven nyckel.
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform");

                    //Om nyckeln hittades
                    if (key != null)
                    {
                        //Skriv värdet i BackupProductKeyDefault till USB minnet
                        var folder = d.Name + "/Windows keys";
                        Directory.CreateDirectory(folder);
                        File.WriteAllText(folder + "/key.txt",
                            System.Environment.MachineName +
                            $"\n" +
                            key.GetValue("BackupProductKeyDefault").ToString());

                        File.AppendAllText(
                            folder + "/allkeys.txt",
                            System.Environment.MachineName +
                            $"\n" +
                            $"Datum: {DateTime.Now.ToShortDateString()}\n" +
                            key.GetValue("BackupProductKeyDefault").ToString()
                            + "\n\n-------------------------------------------\n\n");

                        Console.WriteLine($"Din licensnyckel har blivit sparad till USB-minnet:  {d.Name}");
                        Console.WriteLine("Tryck valfri tangent för att avsluta");
                        key.Close();
                    }
                }
            }
            Console.ReadKey();
        }
    }
}