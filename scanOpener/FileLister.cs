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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace scanOpener
{
    class FileListerFunctions
    {
        public struct FileListerInfos
        {
            public string path;
            public bool isDir;
            public bool error;
        }

        /// <summary>
        /// Ouverture des répertoire et fichiers spécifiés dans la liste obtenue par FileLister.
        /// </summary>
        /// <param name="files">Liste des fichiers. Toutes les interpolations ont été faites.</param>
        /// <param name="ignoreKnownError">Ne pas essayer de traiter les fichiers en erreur.</param>
        /// <param name="verbose">Afficher, en plus des erreurs, les opérations d'ouverture dans les messages.</param>
        /// <param name="messages">Messages de progression.</param>
        /// <returns></returns>
        public static bool FileOpener(FileListerInfos[] files, bool ignoreKnownError, bool verbose, TextBox messages)
        {
            // TODO: On pourrait utiliser une inversion de responsabilité pour éviter de dépendre d'un textbox, mais qu'on
            // puisse utiliser une string, par exemple.

            bool retval = true;

            // On donne une chance à Windows de faire le ménage de ses fichiers.
            Application.DoEvents();

            foreach (FileListerInfos file in files)
            {
                if (ignoreKnownError && file.error)
                {
                    // Peut être qu'on ne veut pas réafficher les fichiers en erreur.
                    continue;
                }

                if (file.isDir)
                {
                    if (Directory.Exists(file.path))
                    {
                        if (verbose)
                        {
                            messages.Text += string.Format("Ouverture du répertoire{1}   {0}{1}{1}", file.path, Environment.NewLine);
                        }

                        Functions.OpenExplorerWindow(file.path);

                        // TODO: On a pas de preuve qu'il a été posible de l'ouvrir.
                    }
                    else
                    {
                        // On ne peut trouver le répertoire, on affiche une erreur.
                        messages.Text += string.Format("Impossible d'ouvrir le répertoire{1}   {0}{1}{1}", file.path, Environment.NewLine);

                        // Signale l'erreur.
                        retval = false;
                    }
                }
                else
                {
                    // Il s'agit d'un fichier, pas d'un répertoire.
                    if (File.Exists(file.path))
                    {
                        if (verbose)
                        {
                            messages.Text += string.Format("Ouverture du fichier{1}   {0}{1}{1}", file.path, Environment.NewLine);
                        }

                        Process.Start(file.path);

                        // TODO: On a pas de preuve qu'il a été posible de l'ouvrir.
                    }
                    else
                    {
                        // On ne peut trouver le répertoire, on affiche une erreur.
                        messages.Text += string.Format("Impossible d'ouvrir le fichier{1}   {0}{1}{1}", file.path, Environment.NewLine);

                        // Signale l'erreur.
                        retval = false;
                    }
                }
                
                // Un peu de temps pour que windows se replace.
                Application.DoEvents();
            }

            return retval;
        }

        /// <summary>
        /// Ouverture des répertoires et fichiers selon la liste de conversion avec en plus le support des * et ?.
        /// </summary>
        /// <param name="base_dir">Répertoire de base.</param>
        /// <param name="code">Code de l'objet.</param>
        /// <param name="paths">Liste de fichiers et répertoires.</param>
        /// <param name="patterns">Liste des fichiers à ouvrir. Peut contenir des *, % ou @.</param>
        /// <param name="OpenBaseDirExplorerWindow">Ouvrir automatiquement le répertoire de base..</param>
        /// <param name="concatenateCodeToBase">Ajouter automatiquement code à base_dir</param>
        /// <returns></returns>
        public static FileListerInfos[] FileLister(string base_dir, string code, string[] patterns, bool openBaseDirExplorerWindow, bool concatenateCodeToBase)
        {
            // Liste des fichiers.
            var fileList = new List<FileListerInfos>();

            // Répertoire de base. Si on est en mode table de connection on concatène automatiquement le code.
            string full_path;
            if (concatenateCodeToBase)
                full_path = System.IO.Path.Combine(base_dir, code);
            else
                full_path = base_dir;

            if (openBaseDirExplorerWindow)
            {
                if (!Directory.Exists(full_path))
                {
                    // Ne pas trouver le répertoire de base est une erreur grave.
                    FileListerInfos info = new FileListerInfos { path = full_path,
                        isDir = true,
                        error = true };
                    fileList.Add(info);
                }
                else
                {
                    // On ajoute le répertoire à la liste d'ouverture.
                    FileListerInfos info = new FileListerInfos
                    {
                        path = full_path,
                        isDir = true,
                        error = false
                    };
                    fileList.Add(info);
                }
            }

            // On explore chaque pattern. Si on ne trouve rien c'est une erreur.              
            foreach (string pattern in patterns)
            {
                // Liste des fichiers à ouvrir.
                string[] paths = FilePathInterpolator(full_path, code, pattern);

                if (paths.Length == 0)
                {
                    // C'est une erreur de ne rien trouver pour un pattern.
                    if (!Directory.Exists(full_path))
                    {
                        // Ne pas trouver le répertoire de base est une erreur grave.
                        FileListerInfos info = new FileListerInfos
                        {
                            path = pattern,
                            isDir = false,
                            error = true
                        };
                        fileList.Add(info);
                    }
                }
                else
                {
                    // On vérifie maintenant l'existance de chaque fichier.
                    foreach (string path in paths)
                    {
                        if (Directory.Exists(path))
                        {
                            FileListerInfos info = new FileListerInfos
                            {
                                path = path,
                                isDir = true,
                                error = false
                            };
                            fileList.Add(info);
                        }
                        else if (File.Exists(path))
                        {
                            FileListerInfos info = new FileListerInfos
                            {
                                path = path,
                                isDir = false,
                                error = false
                            };
                            fileList.Add(info);
                        }
                        else
                        {
                            FileListerInfos info = new FileListerInfos
                            {
                                path = path,
                                isDir = false,
                                error = true
                            };
                            fileList.Add(info);
                        }
                    }
                }
            }
            return fileList.ToArray();
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
                    catch (Exception ex)
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
