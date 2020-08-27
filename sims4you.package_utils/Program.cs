using s4pi.Interfaces;
using s4pi.Package;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TS4SimRipper;
using System.Text.Json;


namespace sims4you.package_utils
{
    class Program
    {
        readonly string TS4FilesPath = "";
        Package[] gamePackages = new Package[0];
        string[] gamePackageNames = new string[0];

        Program(string TS4FilesPath)
        {
            this.TS4FilesPath = TS4FilesPath;
        }

        private bool SetupGamePacks()
        {
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
                    Package p = (Package)Package.OpenPackage(0, paths[i], false);
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

        private Hashtable FetchGameSculpts()
        {
            Hashtable sculpts = new Hashtable();
            static bool pred(IResourceIndexEntry r) => r.ResourceType == (uint)ResourceTypes.Sculpt;

            for (int i = 0; i < gamePackages.Length; i++)
            {
                Package p = gamePackages[i];
                List<IResourceIndexEntry> iries = p.FindAll(pred);
                if (iries != null)
                {
                    foreach (IResourceIndexEntry irie in iries)
                    {
                        using BinaryReader br = new BinaryReader(p.GetResource(irie));
                        try
                        {
                            Sculpt sculpt = new Sculpt(br);
                            if (!sculpts.ContainsKey(irie.Instance))
                            {
                                sculpts.Add(irie.Instance, sculpt);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.Error.WriteLine("[ERROR]" + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                        }
                    }
                }
            }


            return sculpts;
        }

        private Hashtable FetchGameCASP()
        {
            Hashtable casps = new Hashtable();
            static bool pred(IResourceIndexEntry r) => r.ResourceType == (uint)ResourceTypes.CASP;
            for (int i = 0; i < gamePackages.Length; i++)
            {
                Package p = gamePackages[i];
                List<IResourceIndexEntry> iries = p.FindAll(pred);
                if (iries != null)
                {
                    foreach (IResourceIndexEntry irie in iries)
                    {
                        using BinaryReader br = new BinaryReader(p.GetResource(irie));
                        try
                        {
                            CASP casp = new CASP(br);
                            if (!casps.ContainsKey(irie.Instance) && IsFacialCASP(casp))
                            {
                                casps.Add(irie.Instance, casp);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.Error.WriteLine("[ERROR]" + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                        }
                    }
                }
            }

            return casps;
        }

        private Hashtable FetchGameSimModifiers()
        {
            Hashtable smods = new Hashtable();
            static bool pred(IResourceIndexEntry r) => r.ResourceType == (uint)ResourceTypes.SimModifier;
            for (int i = 0; i < gamePackages.Length; i++)
            {
                Package p = gamePackages[i];
                List<IResourceIndexEntry> iries = p.FindAll(pred);
                if (iries != null)
                {
                    foreach (IResourceIndexEntry irie in iries)
                    {
                        using BinaryReader br = new BinaryReader(p.GetResource(irie));
                        try
                        {
                            SMOD smod = new SMOD(br);
 //                           if (!casps.ContainsKey(irie.Instance) && IsFacialCASP(casp))
 //                           {
                                smods.Add(irie.Instance, smod);
 //                           }
                        }
                        catch (Exception e)
                        {
                            Console.Error.WriteLine("[ERROR]" + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                        }
                    }
                }
            }

            return smods;
        }

        private static bool IsFacialCASP(CASP casp)
        {
            return (
                casp.bodyType == (uint)BodyType.Hair ||
                casp.bodyType == (uint)BodyType.Head ||
                casp.bodyType == (uint)BodyType.Face ||
                casp.bodyType == (uint)BodyType.FacialHair ||
                casp.bodyType == (uint)BodyType.Lipstick ||
                casp.bodyType == (uint)BodyType.Eyeshadow ||
                casp.bodyType == (uint)BodyType.Eyeliner ||
                casp.bodyType == (uint)BodyType.Blush ||
                casp.bodyType == (uint)BodyType.Eyebrows ||
                casp.bodyType == (uint)BodyType.Eyecolor ||
                casp.bodyType == (uint)BodyType.Mascara ||
                casp.bodyType == (uint)BodyType.ForeheadCrease ||
                casp.bodyType == (uint)BodyType.Freckles ||
                casp.bodyType == (uint)BodyType.MouthCrease ||
                casp.bodyType == (uint)BodyType.SkinDetailAcne
                );
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Fetching game Sculpts and CASPs...");
            Program program = new Program("D:\\Games\\The Sims 4\\Data");
            bool wasAbleToReadGamePacks = program.SetupGamePacks();

            if (wasAbleToReadGamePacks)
            {
                Hashtable modifiers = program.FetchGameSimModifiers();
                Hashtable sculpts = program.FetchGameSculpts();
                Hashtable casps = program.FetchGameCASP();

                Console.WriteLine("---------MODIFIERS---------");
                foreach (DictionaryEntry de in modifiers)
                {
                    // TODO: link modifier's BGEO to sculpt's BGEO
                    if (((SMOD)de.Value).BGEOKey.Length > 0)
                    {
                        Console.WriteLine(de.Key + " " + ((SMOD)de.Value).BGEOKey[0].Instance);
                    }
                }

                Console.WriteLine("---------SCULPTS---------");
                string sculptOutputPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                using StreamWriter sculptOutputFile = new StreamWriter(Path.Combine(sculptOutputPath, "sculpts.txt"));
                foreach (DictionaryEntry de in sculpts)
                {
                    Sculpt sculpt = (Sculpt)de.Value;
                    string region = sculpt.region.ToString().ToLower();
                    string ages = JsonSerializer.Serialize(sculpt.age.ToString().Split(", "));
                    string genders = JsonSerializer.Serialize(sculpt.gender.ToString().Split(", "));
                    var bgeoKeys = JsonSerializer.Serialize(sculpt.BGEOKey);
                    sculptOutputFile.WriteLine("\"" + de.Key + "\": {\"region\": \"" + region + "\", \"age\": " + ages + ", \"gender\": " + genders + ", \"bgeo\": " + bgeoKeys + "},");
                }

                Console.WriteLine("---------CASP---------");
                string caspsOutputPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                using StreamWriter caspsOutputFile = new StreamWriter(Path.Combine(caspsOutputPath, "casps.txt"));
                foreach (DictionaryEntry de in casps)
                {
                    CASP casp = (CASP)de.Value;
                    string bodyType = ((BodyType)casp.bodyType).ToString().ToLower();
                    string ages = JsonSerializer.Serialize(casp.age.ToString().Split(", "));
                    string genders = JsonSerializer.Serialize(casp.gender.ToString().Split(", "));

                    caspsOutputFile.WriteLine("\"" + de.Key + "\": {\"name\": \"" + casp.partname + "\", \"body_type\": \"" + bodyType + "\", \"age\": " + ages + ", \"gender\": " + genders + "},");
                }
            } 
            else
            {
                Console.Error.WriteLine("Wasn't able to read Game Packs!");
            }
        }
    }
}
