using System;
using System.Collections.Generic;
using System.IO;

namespace sims4you.package_utils
{
    class Program
    {
        Package[] gamePackages = new Package[0];
        string[] gamePackageNames = new string[0];

        private bool SetupGamePacks()
        {
            string TS4FilesPath = Properties.Settings.Default.TS4Path;
            List<Package> gamePacks = new List<Package>();
            List<string> paths = new List<string>();
            try
            {
                List<string> pathsSim = new List<string>(Directory.GetFiles(TS4FilesPath, "Simulation*Build*.package", SearchOption.AllDirectories));
                List<string> pathsClient = new List<string>(Directory.GetFiles(TS4FilesPath, "Client*Build*.package", SearchOption.AllDirectories));
                pathsSim.Sort();
                pathsClient.Sort();
                paths.AddRange(pathsClient);
                paths.AddRange(pathsSim);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.Error.WriteLine("Your game packages path is invalid! Please go to File / Change Settings and correct it or make it blank to reset, then restart."
                    + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }
            catch (IOException e)
            {
                Console.Error.WriteLine("Your game packages path is invalid or a network error has occurred! Please go to File / Change Settings and correct it or make it blank to reset, then restart."
                    + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }
            catch (ArgumentException e)
            {
                Console.Error.WriteLine("Your game packages path is not specified correctly! Please go to File / Change Settings and correct it or make it blank to reset, then restart."
                    + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.Error.WriteLine("You do not have the required permissions to access the game packages folder! Please restart with admin privileges."
                    + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }
            if (paths.Count == 0)
            {
                Console.Error.WriteLine("Can't find game packages! Please go to File / Change Settings and correct the game packages path or make it blank to reset, then restart.");
                return false;
            }
            try
            {
                for (int i = 0; i < paths.Count; i++)
                {
                    Package p = OpenPackage(paths[i], false);
                    if (p == null)
                    {
                        Console.Error.WriteLine("Can't read game packages!");
                        return false;
                    }
                    gamePacks.Add(p);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.Error.WriteLine("You do not have the required permissions to open the game packages! Please restart with admin privileges."
                    + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }

            gamePackages = gamePacks.ToArray();
            gamePackageNames = paths.ToArray();

            return true;
        }

        private Sculpt FetchGameSculpt(TGI tgi, ref string errorMsg)
        {
            if (tgi.Instance == 0ul) return null;
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.Instance == tgi.Instance;
            for (int i = 0; i < gamePackages.Length; i++)
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.FindAll(pred);
                if (irie != null)
                {
                    using (BinaryReader br = new BinaryReader(p.GetResource(irie)))
                    {
                        try
                        {
                            Sculpt sculpt = new Sculpt(br);
                            return sculpt;
                        }
                        catch
                        {
                            errorMsg += "Can't read Sculpt " + tgi.ToString() + ", Package: " + gamePackageNames[i] + Environment.NewLine;
                            return null;
                        }
                    }
                }
            }
            errorMsg += "Can't find Sculpt " + tgi.ToString() + Environment.NewLine;
            return null;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
