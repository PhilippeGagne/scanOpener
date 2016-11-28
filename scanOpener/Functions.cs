//////////////////////////////////////////////////////////////////////////////
//BSD 2-Clause License
//
//Copyright(c) 2016, Philippe Gagné
//All rights reserved.
//
//Redistribution and use in source and binary forms, with or without
//modification, are permitted provided that the following conditions are met:
//
//* Redistributions of source code must retain the above copyright notice, this
//  list of conditions and the following disclaimer.
//
//* Redistributions in binary form must reproduce the above copyright notice,
//  this list of conditions and the following disclaimer in the documentation
//  and/or other materials provided with the distribution.
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
//AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
//IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//DISCLAIMED.IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
//FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
//DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
//CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
//OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
//OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace scanOpener
{
    class Functions
    {
        public static string[] TranslateUserSelection(string selected, string convert_file)
        {
            // On refiltre la chaine d'entrée contre les caractères bizarres.
            selected = CleanupString(selected);

            var lines = File.ReadAllLines(convert_file);
            foreach(string line in lines)
            {
                var cols = line.Split('\t');

                if (cols[0] == selected)
                {
                    return cols;
                } 
            }

            // Si on se rend ici, c'est qu'on a pas trouvé de correspondance.
            return null;
        }

        // Filtrage contre les caractères bizarres. On ne garde que les caractères texte et chiffres.
        public static string CleanupString(string input)
        {
            string output = Regex.Replace(input, @"[^\w]", "");
            return output;
        }

        public static void OpenExplorerWindow(string path)
        {
            Application.DoEvents();
            Set_folder_view_2.Program.SetFolderView(path, Set_folder_view_2.WinAPI.FOLDERVIEWMODE.FVM_ICON);

            // Work aroud pour se donner une chance que windows ait terminé d'ouvrir l'explorateur avant
            // d'ouvrir les autres fenêtres.
            Application.DoEvents();
            Thread.Sleep(200);
            Application.DoEvents();
        }

        public static void CloseAllExplorerWindows()
        {
            SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindows();

            foreach (SHDocVw.InternetExplorer ie in shellWindows)
            {
                string filename = Path.GetFileNameWithoutExtension(ie.FullName).ToLower();

                if (filename.Equals("explorer"))
                {
                    ie.Quit();
                }
            }

            // Work aroud pour se donner une chance que windows ait terminé d'ouvrir l'explorateur avant
            // d'ouvrir les autres fenêtres. Semble important, sinon il arrive que la fenêtre suivante
            // n'apparaisse pas.
            Application.DoEvents();
            Thread.Sleep(200);
        }

        public static void CloseViewerWindows(string viewerProcessName)
        {
            if (viewerProcessName != "")
            {
                string[] processNames = viewerProcessName.Split(new[] { ';', ':', ',' });
                for (int i = 0; i < processNames.Length; i++)
                    processNames[i] = processNames[i].Trim();

                string errorsProcesses = "";
                bool error = false;

                foreach (Process p in Process.GetProcesses(System.Environment.MachineName))
                {
                    if (p.MainWindowHandle != IntPtr.Zero)
                    {
                        foreach (string t in processNames)
                        {
                            if (string.Equals(p.ProcessName, t, StringComparison.OrdinalIgnoreCase))
                            {
                                bool result = p.CloseMainWindow();
                                if (!result)
                                {
                                    error = true;
                                    if (errorsProcesses.Length > 0)
                                        errorsProcesses += ", ";
                                    errorsProcesses += p.MainWindowTitle;
                                }

                                break; // Terminé pour cette fenêtre.
                            }
                        }
                    }
                }

                if (error)
                {
                    string message = string.Format("Impossible de fermer les fenêtres {0}.", errorsProcesses);
                    MessageBox.Show(message, "Fermeture des fenêtres", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                Application.DoEvents();
                Thread.Sleep(200);
            }
        }

        /// <summary>
        /// Méthode traditionnelle d'ouverture des fichiers. 
        /// </summary>
        /// <param name="base_dir">Répertoire de base</param>
        /// <param name="selected_files">Ligne complète de la table de converion {code_barre, code, [it1], [it2], ...}. </param>
        /// <param name="messages">TextBox pour afficher les messages.</param>
        /// <returns></returns>
        public static bool OpenDirectoryAndFiles(string base_dir, string[] selected_files, TextBox messages)
        {
            bool retval = true;

            string full_path = System.IO.Path.Combine(base_dir, selected_files[1]);

            // On donne une chance à Windows d'ouvrir les fichiers.
            Application.DoEvents();

            if (Directory.Exists(full_path))
            {
                string message = string.Format("Ouverture du répertoire:{0}{1}", full_path, Environment.NewLine);
                messages.Text += message;

                OpenExplorerWindow(full_path);

                // Ouverture des fichiers optionnels
                for (int i=2; i<selected_files.Length; i++)
                {
                    string doc_path = System.IO.Path.Combine(full_path, selected_files[i]);
                    if (File.Exists(doc_path))
                    {
                        string doc_message = string.Format("Ouverture de : {0}{1}", selected_files[i], Environment.NewLine);
                        messages.Text += doc_message;
                        Process.Start(doc_path);
                    }
                    else
                    {
                        string doc_message = string.Format("{1}Impossible de trouver : {0}{1}", selected_files[i], Environment.NewLine);
                        messages.Text += doc_message;
                        retval = false;
                    }
                    Application.DoEvents();
                }
             }
            else
            {
                string message = string.Format("Impossible de trouver :{1}{0}{1}", full_path, Environment.NewLine);
                messages.Text = message;
                //if (debug_mode)
                //{
                //    MessageBox.Show(message, dialog_caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}

                retval = false;
            }

            return retval;
        }

        /// <summary>
        /// Ouverture des répertoires et fichiers utilisant le patron général.
        /// </summary>
        /// <param name="paths">Liste de fichiers et répertoires.</param>
        /// <param name="messages">TextBox pour afficher les messages</param>
        /// <returns></returns>
        public static bool OpenDirectoryAndFiles(string[] paths, TextBox messages)
        {
            bool retval = true;

            // On donne une chance à Windows d'ouvrir les fichiers.
            Application.DoEvents();

            foreach (string path in paths)
            {
                if (Directory.Exists(path))
                {
                    string message = string.Format("Ouverture du répertoire : {0}{1}", path, Environment.NewLine);
                    messages.Text += message;
                    OpenExplorerWindow(path);
                }
                else if (File.Exists(path))
                {
                    string doc_message = string.Format("Ouverture de : {0}{1}", path, Environment.NewLine);
                    messages.Text += doc_message;
                    Process.Start(path);
                   // Thread.Sleep(200);
                   // Application.DoEvents();
                }
                else
                {
                    string doc_message = string.Format("{1}Impossible d'ouvrir : {0}{1}", path, Environment.NewLine);
                    messages.Text += doc_message;
                    retval = false;
                }

                // On donne une chance à Windows d'ouvrir les fichiers.
                Thread.Sleep(200);
                Application.DoEvents();
            }

            return retval;
        }

        /// <summary>
        /// Ouverture des répertoires et fichiers selon la liste de conversion avec en plus le support des * et ?.
        /// </summary>
        /// <param name="paths">Liste de fichiers et répertoires.</param>
        /// <param name="messages">TextBox pour afficher les messages</param>
        /// <returns></returns>
        public static bool OpenDirectoryAndFilesInterpolatif(string base_dir, string[] selected_files, bool OpenBaseDirExplorerWindow, TextBox messages)
        {
            bool retval = true;

            string full_path = System.IO.Path.Combine(base_dir, selected_files[1]);

            // On coupe les deux premières colonnes de la ligne de la table de conversion
            // pour ne garder que les patterns de nom de fichier.
            List<string> tempStr = new List<string>();
            for (int i = 2; i < selected_files.Length; i++)
                tempStr.Add(selected_files[i]);
            string[] filePatterns = tempStr.ToArray();

            // On donne une chance à Windows d'ouvrir les fichiers.
            Application.DoEvents();

            if (Directory.Exists(full_path))
            {
                if (OpenBaseDirExplorerWindow)
                {
                    string message = string.Format("Ouverture du répertoire:{0}{1}", full_path, Environment.NewLine);
                    messages.Text += message;
                    OpenExplorerWindow(full_path);
                }
                else
                {
                    string message = string.Format("Le répertoire de base ne sera pas affiché : {0}{1}", full_path, Environment.NewLine);
                    messages.Text += message;
                }

                // Liste des fichiers à ouvrir.
                string[] paths = FilePathInterpolator(full_path, selected_files[1], filePatterns);

                foreach (string path in paths)
                {
                    if (Directory.Exists(path))
                    {
                        string doc_message = string.Format("Ouverture du répertoire : {0}{1}", path, Environment.NewLine);
                        messages.Text = doc_message;
                        OpenExplorerWindow(path);
                    }
                    else if (File.Exists(path))
                    {
                        string doc_message = string.Format("Ouverture de : {0}{1}", path, Environment.NewLine);
                        messages.Text += doc_message;
                        Process.Start(path);
                     //   Thread.Sleep(200);
                     //   Application.DoEvents();
                    }
                    else
                    {
                        string doc_message = string.Format("{1}Impossible d'ouvrir : {0}{1}", path, Environment.NewLine);
                        messages.Text += doc_message;
                        retval = false;
                    }

                    // On donne une chance à Windows d'ouvrir les fichiers.
                    Thread.Sleep(200);
                    Application.DoEvents();
                }
            }

            return retval;
        }

        /// <summary>
        /// Crée la liste des fichiers à ouvrir.
        /// </summary>
        /// <param name="basepath">Répertoire de base (ex: C:\a\b).</param>
        /// <param name="code">Code correspondant au barcode lu. Il est représenté par @ dans les chaînes.</param>
        /// <param name="patterns">quoi rechercher. Peut contenir * ou %. Peut contenir des sous-répertoires.</param>
        public static string[] FilePathInterpolator(string basepath, string code, string patterns)
        {
            string[] separated_pattern = patterns.Split(new[] { ';', ':', ',' });
            for (int i = 0; i < separated_pattern.Length; i++)
                separated_pattern[i] = separated_pattern[i].Trim();

            return FilePathInterpolator(basepath, code, separated_pattern);
        }

        /// <summary>
        /// Crée la liste des fichiers à ouvrir.
        /// </summary>
        /// <param name="basepath">Répertoire de base (ex: C:\a\b).</param>
        /// <param name="code">Code correspondant au barcode lu. Il est représenté par @ dans les chaînes.</param>
        /// <param name="separated_pattern">quoi rechercher. Chaque string peut contenir * ou %. Peut contenir des sous-répertoires.</param>
        public static string[] FilePathInterpolator(string basepath, string code, string[] separated_pattern)
        {
            List<string> files = new List<string>();

            foreach (string pattern in separated_pattern)
            {
                // On combine puis sépare pour regrouper les éventuels sous-répertoires dans le
                // pattern et les renvoyer dans le basepath. Surtout important si on a mis des «..».
                string raw_path = Path.Combine(basepath, pattern);

                // Place le code au bon endroit.
                raw_path = raw_path.Replace("@", code);

                if (raw_path.Contains("*") || raw_path.Contains("?"))
                {
                    // Contient des caractères de recherche génériques. On doit
                    // rechercher des fichiers.

                    // Répertoire et expression de base réels.
                    string base_path = Path.GetDirectoryName(raw_path);
                    string search_exp = Path.GetFileName(raw_path);

                    // Recherche les fichiers.
                    try
                    {
                        string[] file_list = Directory.GetFiles(base_path, search_exp);
  
                        // Ajout à la liste
                        files.AddRange(file_list);
                    }
                    catch (Exception /*ex*/)
                    {
                        // TODO: Message d'erreur ?
                    }
                }
                else
                {
                    // Nom complet. On ajoute.
                    string path = Path.GetFullPath(raw_path);
                    files.Add(path);
                }
            }

            return files.ToArray();
        }
    }
}
