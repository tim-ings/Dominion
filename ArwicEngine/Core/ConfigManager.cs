﻿// Dominion - Copyright (C) Timothy Ings
// ConfigManager.cs
// This file defines classes that manage config variables

using System;
using System.Collections.Generic;
using System.IO;
using static ArwicEngine.Constants;

namespace ArwicEngine.Core
{
    /// <summary>
    /// Manages a list of variables
    /// </summary>
    public class ConfigManager
    {
        /// <summary>
        /// Reference to the engine
        /// </summary>
        public Engine Engine { get; set; }

        private List<string> vars = new List<string>();
        private List<string> vals = new List<string>();

        /// <summary>
        /// Creates a new config manager
        /// </summary>
        /// <param name="engine"></param>
        public ConfigManager(Engine engine)
        {
            Engine = engine;
        }

        /// <summary>
        /// Returns a variable's value
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        public string GetVar(string var)
        {
            bool exists = false;
            int i;
            // loop through all variables
            for (i = 0; i < vars.Count; i++)
            {
                if (vars[i] == var)
                {
                    // break when we have found the var
                    exists = true;
                    break;
                }
            }
            // if we have found the var, return its value
            if (exists)
                return vals[i];
            // else return null
            return "NULL";
        }

        /// <summary>
        /// Sets a variable's value
        /// </summary>
        /// <param name="var"></param>
        /// <param name="val"></param>
        public void SetVar(string var, string val)
        {
            // check if the var exists
            bool exists = false;
            int i;
            for (i = 0; i < vars.Count; i++)
            {
                if (vars[i] == var)
                {
                    exists = true;
                    break;
                }
            }
            // if it does, set the var to val
            if (exists)
            {
                vals[i] = val;
                return;
            }
            // else, add var = val as a new variable
            else
            {
                vars.Add(var);
                vals.Add(val);
            }
        }

        /// <summary>
        /// Reset the config to its defaults
        /// </summary>
        public void SetDefaults()
        {
            Engine?.Console?.WriteLine("Setting defaults", MsgType.Info);

            // clear/reset vars and vals
            vars = new List<string>();
            vals = new List<string>();

            // set default variables
            SetVar(CONFIG_RESOLUTION, "1920x1080");
            SetVar(CONFIG_VSYNC, "0");
            SetVar(CONFIG_DISPLAYMODE, "0");
            SetVar(CONFIG_SOUNDVOLUME, "1");
            SetVar(CONFIG_MUSICVOLUME, "1");
            SetVar(CONFIG_NET_SERVER_PORT, "7894");
            SetVar(CONFIG_NET_SERVER_TIMEOUT, "2000");
            SetVar(CONFIG_NET_CLIENT_PORT, "7894");
            SetVar(CONFIG_NET_CLIENT_TIMEOUT, "2000");
            SetVar(CONFIG_NET_CLIENT_ADDRESS, "localhost");
        }

        /// <summary>
        /// Writes the config to a text file
        /// </summary>
        /// <param name="path"></param>
        public void Write(string path)
        {
            if (vars.Count != vals.Count)
                throw new Exception("Config parity lost");

            List<string> file = new List<string>();
            if (File.Exists(path))
            {
                string[] fileArray = File.ReadAllLines(CONFIG_PATH);
                for (int i = 0; i < fileArray.Length; i++)
                    file.Add(fileArray[i]);
            }

            for (int vi = 0; vi < vars.Count; vi++)
            {
                bool found = false;
                for (int fi = 0; fi < file.Count; fi++)
                {
                    string fileVar = file[fi].Split(' ')?[1];
                    if (string.Compare(fileVar, vars[vi]) == 0)
                    {
                        file[fi] = $"set {vars[vi]} {vals[vi]}";
                        found = true;
                        break;
                    }
                }
                if (!found)
                    file.Add($"set {vars[vi]} {vals[vi]}");
            }

            if (File.Exists(path))
                File.Delete(path);
            File.WriteAllLines(path, file);
            Engine?.Console?.WriteLine($"Saved config vars to '{path}'");
        }
    }
}
