using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Media;

namespace LiveDots
{
    static class JawsSettings
    {
        private static string applicationData = "C:\\Program Files";
        private static string applicationName = "LiveDots";
        private static string Currentpath = Directory.GetCurrentDirectory();
        public static string JawsPath = Path.Combine(Currentpath, "JAWS");
        //public static string JawsPath = @"C:\Users\Óscar\Documents\GitHub\TFG-Infor\JAWS";
        private static string aux1 = "Freedom Scientific\\JAWS";
        private static string aux2 = "Settings\\esn";


        // Set a registry value.
        public static void SetRegistryValue(RegistryKey hive,
            string subkey_name, string value_name, object value)
        {
            //accessing the CurrentUser root element  

            //and adding "OurSettings" subkey to the "SOFTWARE" subkey  
            RegistrySecurity rs = new RegistrySecurity();
            rs = hive.GetAccessControl();
            string currentUserStr = Environment.UserDomainName + "\\" + Environment.UserName;
            rs.AddAccessRule(new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), RegistryRights.WriteKey | RegistryRights.ReadKey | RegistryRights.Delete | RegistryRights.FullControl | RegistryRights.CreateSubKey, AccessControlType.Allow));

            Registry.SetValue(hive.Name, value_name, value, RegistryValueKind.String);

            RegistryKey key = hive.CreateSubKey(subkey_name);

            //storing the values  
            key.SetValue(value_name, value);
            //key.SetValue("Setting2", "This is our setting 2");

            key.Close();
            LiveDotsCOMObj.Register();
        }

        public static bool CheckJawsInstalled()
        {
            // Instalar la fuente de edico en el ordenador
           

            //ESTO HAY QUE QUITARLO PORQUE NUESTRO EJECUTABLE YA TENDRÁ GUAY LOS ARCHIVOS
            //System.IO.File.Copy(@"C:\Users\Óscar\AppData\Roaming\Freedom Scientific\JAWS\2020\Settings\enu\LiveDots.JSS",Path.Combine(JawsPath,"LiveDots.JSS"), true);
            //System.IO.File.Copy(@"C:\Users\Óscar\AppData\Roaming\Freedom Scientific\JAWS\2020\Settings\enu\LiveDots.jsb", Path.Combine(JawsPath, "LiveDots.jsb"), true);

            try
            {
                // Busca las versiones de Jaws instaladas
                StringDictionary jawsPaths = new StringDictionary();
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths");
                if (key != null)
                {
                    string[] apps = key.GetSubKeyNames();
                    foreach (string app in apps)
                        if (app.Contains("JAWS"))
                        {
                            try
                            {
                                RegistryKey subkey = key.OpenSubKey(app);
                                string jawsPath = Path.GetDirectoryName(subkey.GetValue(null).ToString());
                                string version = Path.GetFileName(jawsPath);
                                if (!jawsPaths.ContainsKey(version))
                                    jawsPaths.Add(version, jawsPath);

                                var filedata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), aux1);
                                filedata = Path.Combine(filedata, version);
                                filedata = Path.Combine(filedata, aux2);
                                System.IO.File.Copy(Path.Combine(JawsPath, "LiveDots.JSS"), Path.Combine(filedata, "LiveDots.JSS"));
                                System.IO.File.Copy(Path.Combine(JawsPath, "LiveDots.jsb"), Path.Combine(filedata, "LiveDots.jsb"));
                            }
                            catch { }
                        }
                    key.Close();
                }

                // Se hacen todos los cambios necesarios para que LiveDots funcione correctamente con cada versión (jbt, jcf, jss y jsb)
                string srcPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, JawsPath);
                foreach (string version in jawsPaths.Keys)
                {
                    try
                    {
                        // Comprueba el JBT
                        System.IO.File.Copy(Path.Combine(srcPath, "livedotsEsp.jbt"), Path.Combine(jawsPaths[version], "livedotsEsp.jbt"), true);
                        // Busca las carpetas de tres letras donde Jaws guarda la configuración para los distintos idiomas
                        string settingsPath = Path.Combine(applicationData, @"Freedom Scientific\JAWS", version, "Settings");
                        Directory.CreateDirectory(settingsPath); // asegura que exista la carpeta
                        string[] directories = Directory.GetDirectories(settingsPath);
                        List<string> jawsSettingsPaths = new List<string>();
                        foreach (string directory in directories)
                        {
                            string name = Path.GetFileName(directory);
                            if (name.Length == 3)
                                jawsSettingsPaths.Add(name);
                        }
                        // En el directorio en español (esn) debe instalarse siempre, por lo que se añade si no lo ha encontrado
                        if (!jawsSettingsPaths.Contains("esn"))
                        {
                            jawsSettingsPaths.Add("esn");
                            Directory.CreateDirectory(Path.Combine(settingsPath, "esn")); // asegura que exista la carpeta
                        }
                        // Hace la instalación para cada una de las carpetas encontradas
                        foreach (string jawsSettingsPath in jawsSettingsPaths)
                        {
                            // Comprueba el JSS
                            string settingsPathFile = Path.Combine(settingsPath, jawsSettingsPath, applicationName);
                            System.IO.File.Copy(Path.Combine(srcPath, "LiveDots.JSS"), settingsPathFile + ".jss", true);
                            // Comprueba el JSB
                            System.IO.File.Copy(Path.Combine(srcPath, "LiveDots.jsb"), settingsPathFile + ".jsb", true);
                            // Comprueba el JCF
                            string dstFileName = settingsPathFile + ".jcf";
                            if (File.Exists(dstFileName))
                            {
                                // Si existe asegura que la tabla braille sea correcta
                                string[] jcf_content = File.ReadAllLines(dstFileName);
                                bool updated = false;
                                for (int i = 0; i < jcf_content.Length; i++)
                                {
                                    if (jcf_content[i].StartsWith("BrailleTranslationTable=") && jcf_content[i] != "BrailleTranslationTable=livedotsEsp")
                                    {
                                        jcf_content[i] = "BrailleTranslationTable=livedotsEsp";
                                        updated = true;
                                    }
                                }
                                if (updated)
                                    File.WriteAllLines(dstFileName, jcf_content);
                            }
                            else
                            {
                                // Si no existe se copia el de la instalación
                                System.IO.File.Copy(Path.Combine(srcPath, "livedots.jcf"), dstFileName, true);
                            }
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }

                // Registra el objeto COM utilizado en los scripts
                SetRegistryValue(Registry.ClassesRoot, @"LiveDots.LiveDotsCOMObj\CLSID", "{64550ee5-5255-4c18-98f0-71ec898fe97c}", RegistryValueKind.String);
                SetRegistryValue(Registry.LocalMachine, @"SOFTWARE\Classes\LiveDots.LiveDotsCOMObj\CLSID", "{64550ee5-5255-4c18-98f0-71ec898fe97c}", RegistryValueKind.String);
                //Registry.SetValue(Registry.ClassesRoot, @"LiveDots.LiveDotsCOMObj\CLSID", "{64550ee5-5255-4c18-98f0-71ec898fe97c}", RegistryValueKind.String);
                //Registry.SetValue(Registry.LocalMachine.Name, @"SOFTWARE\Classes\LiveDots.LiveDotsCOMObj\CLSID", "{64550ee5-5255-4c18-98f0-71ec898fe97c}", RegistryValueKind.String);
                //SetRegistryValue(Registry.ClassesRoot, @"LiveDots.LiveDotsComObj\CLSID", "", "{64550ee5-5255-4c18-98f0-71ec898fe97c}", RegistryValueKind.String);
                //SetRegistryValue(Registry.LocalMachine, @"SOFTWARE\Classes\LiveDots.LiveDotsComObj\CLSID", "", "{64550ee5-5255-4c18-98f0-71ec898fe97c}", RegistryValueKind.String);

                // Elimina la entrada creada por error por la versión 1.0
                try
                {
                    Registry.CurrentUser.DeleteSubKeyTree("LiveDots.LiveDotsCOMObj");
                }
                catch { }

                // Ha ido todo bien
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
