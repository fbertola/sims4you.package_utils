﻿using s4pi.Interfaces;
using s4pi.Package;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TS4SimRipper;
using System.Text.Json;
using System.Xml;
using MorphTool;
using System.Reflection;
using System.Text;

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

        private Dictionary<ulong, string> ParseTextResourcesList(string filename)
        {
            string executingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string resourcePath = Path.Combine(executingPath, filename);
            if (!File.Exists(resourcePath))
            {
                Console.Error.WriteLine(string.Format("'{0}' not found in CAS Tools directory '{1}'; TGI and resource name list cannot be loaded.", filename, executingPath));
                return null;
            }

            StreamReader file = new System.IO.StreamReader(resourcePath);
            Dictionary<ulong, string> dictOut = new Dictionary<ulong, string>();
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] s = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                dictOut.Add((new TGI(s[0])).Instance, s[1]);
            }
            file.Close();

            return dictOut;
        }

        private Dictionary<ulong, Sculpt> FetchGameSculpts()
        {
            Dictionary<ulong, Sculpt> sculpts = new Dictionary<ulong, Sculpt>();
            static bool pred(IResourceIndexEntry r) => r.ResourceType == (uint)ResourceTypes.Sculpt;
            static bool predNMap(IResourceIndexEntry r) => r.ResourceType == (uint)ResourceTypes.NameMap;
            NameMap nameMap = null;


            for (int i = 0; i < gamePackages.Length; i++)
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irieNMap = p.Find(pred);

                if (irieNMap != null)
                {
                    using (BinaryReader br = new BinaryReader(p.GetResource(irieNMap), Encoding.ASCII))
                    {
                        nameMap = new NameMap(br);
                    }
                }
            }

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

                            if (nameMap != null)
                            {
                                sculpt.sculptName = nameMap.getName(irie.Instance);
                            }

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

        private Dictionary<ulong, CASP> FetchGameCASP()
        {
            Dictionary<ulong, CASP> casps = new Dictionary<ulong, CASP>();
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

        private Dictionary<ulong, SMOD> FetchGameSimModifiers()
        {
            Dictionary<ulong, string> smodNames = ParseTextResourcesList("ModifierList.txt");
            Dictionary<ulong, SMOD> smods = new Dictionary<ulong, SMOD>();
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
                            smodNames.TryGetValue(irie.Instance, out smod.smodName);

                            if (smod.smodName != null && IsFacialModifier(smod))
                            {
                                smods.Add(irie.Instance, smod);
                            }
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

        private static bool IsFacialModifier(SMOD smod)
        {
            return (
                smod.region == SimRegion.EYES ||
                smod.region == SimRegion.NOSE ||
                smod.region == SimRegion.MOUTH ||
                smod.region == SimRegion.CHEEKS ||
                smod.region == SimRegion.CHIN ||
                smod.region == SimRegion.JAW ||
                smod.region == SimRegion.FOREHEAD ||
                smod.region == SimRegion.BROWS ||
                smod.region == SimRegion.EARS ||
                smod.region == SimRegion.HEAD ||
                smod.region == SimRegion.FULLFACE
                );
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Fetching game Sculpts and CASPs...");
            Program program = new Program("D:\\Games\\The Sims 4\\Data");
            bool wasAbleToReadGamePacks = program.SetupGamePacks();

            if (wasAbleToReadGamePacks)
            {
                Dictionary<ulong, SMOD> modifiers = program.FetchGameSimModifiers();
                Dictionary<ulong, Sculpt> sculpts = program.FetchGameSculpts();
                Dictionary<ulong, CASP> casps = program.FetchGameCASP();

                Console.WriteLine("---------MODIFIERS---------");
                foreach (KeyValuePair<ulong, SMOD> de in modifiers)
                {
                    SMOD smod = de.Value;
                    string region = smod.region.ToString().ToLower();
                    string ages = JsonSerializer.Serialize(smod.age.ToString().Split(", "));
                    string genders = JsonSerializer.Serialize(smod.gender.ToString().Split(", "));
                    Console.WriteLine("\"" + de.Key + "\": {\"name\": \"" + smod.smodName + "\", \"region\": \"" + region + "\", \"age\": " + ages + ", \"gender\": " + genders + "},");
                }

                Console.WriteLine("---------SCULPTS---------");
                string sculptOutputPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                using StreamWriter sculptOutputFile = new StreamWriter(Path.Combine(sculptOutputPath, "sculpts.txt"));
                foreach (KeyValuePair<ulong, Sculpt> de in sculpts)
                {
                    Sculpt sculpt = de.Value;
                    string region = sculpt.region.ToString().ToLower();
                    string ages = JsonSerializer.Serialize(sculpt.age.ToString().Split(", "));
                    string genders = JsonSerializer.Serialize(sculpt.gender.ToString().Split(", "));
                    var bgeoKeys = JsonSerializer.Serialize(sculpt.BGEOKey);
                    sculptOutputFile.WriteLine("\"" + de.Key + "\": {\"name\": \"" + sculpt.sculptName + "\", \"region\": \"" + region + "\", \"age\": " + ages + ", \"gender\": " + genders + ", \"bgeo\": " + bgeoKeys + "},");
                }

                Console.WriteLine("---------CASP---------");
                string caspsOutputPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                using StreamWriter caspsOutputFile = new StreamWriter(Path.Combine(caspsOutputPath, "casps.txt"));
                foreach (KeyValuePair<ulong, CASP> de in casps)
                {
                    CASP casp = de.Value;
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
