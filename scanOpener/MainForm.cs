﻿using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Xml.Serialization;
using static scanOpener.FileListerFunctions;
using System.Collections.Generic;
using System.Configuration;
using NLog;
using static scanOpener.Program;

namespace scanOpener
{
    public struct ParameterStruct
    {
        public string BaseDir;

        // Fichier de conversion, première colonne est le code à barre,
        // la deuxième est le répertoire pertinent et les colonnes suivantes sont
        // des fichiers à ouvrir (dans le répertoire pertinent).
        public string ConversionFilePath;
        public string PdfReaderProcessName;
        public string OpenFileExpression;
        public string CodeReaderComPort;

        public bool AutoConnectMode;
        public bool AppendCodeToBasePath;

        public bool UseGlobalFilePattern;
        public bool AskUserValidation;

        public bool CloseExplorerWindows;
        public bool CloseViewers;
        public bool MinimizeApplication;
        public bool OpenBaseDirExplorerWindow;
        public bool ClipboardCopy;
        public bool ExplorerIconMode;
    }

    public partial class MainForm : Form
    {
        private System.IO.Ports.SerialPort mySerialPort;

        // Paramètres de fonctionnement principaux, visibles par l'utilisateur
        // et sauvegardés.
        private ParameterStruct Parameters;

        // Support de log
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public MainForm()
        {
            InitializeComponent();

            LoadSettings();

            if (Parameters.AutoConnectMode && Parameters.CodeReaderComPort.Length > 0)
                  OpenSerialPort(Parameters.CodeReaderComPort);

            // On prépare le GUI à recevoir un nouveau code
            txtSelected.Text = "";
            txtSelected.Focus();

            UpdateGUI();
        }

        // Pour supporter le mode instance unique de l'application.
        // ref: http://stackoverflow.com/questions/19147/what-is-the-correct-way-to-create-a-single-instance-application
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_SHOWME)
            {
                //ShowMe();
                BringWindowToFront();
            }
            base.WndProc(ref m);
        }

        // Pour supporter le mode instance unique de l'application.
        // ref: http://stackoverflow.com/questions/19147/what-is-the-correct-way-to-create-a-single-instance-application
        private void ShowMe()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
            // get our current "TopMost" value (ours will always be false though)
            bool top = TopMost;
            // make our form jump to the top of everything
            TopMost = true;
            // set it back to whatever it was
            TopMost = top;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
             ProcessOpenDirRequest();

            // On prépare le GUI à recevoir un nouveau code
            txtSelected.Text = "";
            txtSelected.Focus();
        }

        /// <summary>
        /// Fonction principale de l'application. Elle traite le code affiché dans le GUI.
        /// </summary>
        private void ProcessOpenDirRequest()
        {
            // Code à barre.
            string selected = Functions.CleanupString(txtSelected.Text);

            // Note l'événement
            logger.Trace("ProcessOpenDirRequest: {0}", selected);

            // Liste des fichiers à ouvrir. La première colonne est
            // le répertoire où se trouvent les documents pertinents
            string[] selected_files = null;     

            const string dialog_caption = "Ouverture d'un répertoire";

            // Initialise les messages d'ouverture.
            txtMessage.Text = "";

            // On ne veut qu'une seule fenêtre explorer en mode Touch Screen.
            if (Parameters.CloseExplorerWindows)
            {
                txtMessage.Text += "Ménage des fenêtres explorateur... ";
                Functions.CloseAllExplorerWindows();
                Application.DoEvents();
                txtMessage.Text += "Fait." + Environment.NewLine;
            }

            if (Parameters.CloseViewers && Parameters.PdfReaderProcessName.Length>0)
            {
                txtMessage.Text += "Ménage des fenêtres d'affichage... ";
                Functions.CloseViewerWindows(Parameters.PdfReaderProcessName);
                Application.DoEvents();
                txtMessage.Text += "Fait." + Environment.NewLine;
            }

            if (Parameters.CloseExplorerWindows || (Parameters.CloseViewers && Parameters.PdfReaderProcessName.Length > 0))
                txtMessage.Text += Environment.NewLine;

            // On a rien à faire ici si la sélection est vide.
            // TODO: filtrage contre les gremelins (espaces).
            if (selected.Length == 0)
            {
                string message = "Rien de sélectionné.";
                txtMessage.Text += message + Environment.NewLine;
                return; // Rien de sélectionné.
            }

            // Tentative de conversion du nom du fichier, si la table existe.
            if (Parameters.ConversionFilePath == "")
            {
                // Pas de fichier de conversion, on se fait une ligne de conversion simple.
                selected_files = new string[] { selected, selected };

                string message = "Pas de fichier de conversion spécifié.";
                txtMessage.Text += message + Environment.NewLine;
            }
            if (Parameters.ConversionFilePath.Length > 0 && !File.Exists(Parameters.ConversionFilePath))
            {
                string message = string.Format("Impossible de trouver le fichier de conversion {0}", Parameters.ConversionFilePath);
                txtMessage.Text += message + Environment.NewLine;
                MessageBox.Show(message, dialog_caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (Parameters.ConversionFilePath.Length > 0 && File.Exists(Parameters.ConversionFilePath))
            {
                selected_files = Functions.TranslateUserSelection(selected, Parameters.ConversionFilePath);

                if (selected_files == null || selected_files.Length<2 || selected_files[1]=="")
                {
                    string message = string.Format("Impossible de trouver une conversion pour le code-barre {0}", selected);
                    txtMessage.Text += message + Environment.NewLine;
                    MessageBox.Show(message, dialog_caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Rendu ici, selected_files a au moins 2 colonnes: la première est le code à barre lu, la deuxième
            // le numéro de pièce correspondant, ensuite il est possible qu'on ait d'autres colonnes de fichier
            // à ouvrir.
            Debug.Assert(selected_files.Length >= 2);

            logger.Info("ProcessOpenDirRequest: {0} -> {1}", selected_files[0], selected_files[1]);

            // On peut vouloir copier le nom de la pièce.
            if (Parameters.ClipboardCopy)
            {
                Clipboard.SetText(selected_files[1]);
            }

            bool open_ok;
  
            FileListerInfos[] files;

            string code = selected_files[1];

            if (Parameters.UseGlobalFilePattern && Parameters.OpenFileExpression.Length > 0)
            {
                // La liste des fichiers à ouvrir vient du champ des parametres.
                string[] patterns = Parameters.OpenFileExpression.Split(new[] { ';', ':', ',' });
                for (int i = 0; i < patterns.Length; i++)
                    patterns[i] = patterns[i].Trim();

                files = FileListerFunctions.FileLister(Parameters.BaseDir, 
                                                        code, 
                                                        patterns, 
                                                        Parameters.OpenBaseDirExplorerWindow, 
                                                        Parameters.AppendCodeToBasePath);
            }
            else
            {
                // La liste des fichiers à ouvrir vient de la table de conversion.

                // On coupe les deux premières colonnes de la ligne de la table de conversion
                // pour ne garder que les patterns de nom de fichier.
                List<string> patterns = new List<string>();
                for (int i = 2; i < selected_files.Length; i++)
                    patterns.Add(selected_files[i]);

                files = FileListerFunctions.FileLister(Parameters.BaseDir,
                                                        code, 
                                                        patterns.ToArray(), 
                                                        Parameters.OpenBaseDirExplorerWindow, 
                                                        Parameters.AppendCodeToBasePath);
            }

            // Vérifie si on a des erreurs, ce qui ne devrait pas arriver.
            string fileListString = FileListerInfosAsText(files, true, true);

            if (MustStopFileListError(files))
            {
                logger.Warn("User stopped, some files missing{1}{0}", fileListString, Environment.NewLine);
                return;
            }

            // Validation optionnelle des opérations, avant de faire quoi que ce soit.
            if (Parameters.AskUserValidation)
            {
                string operations = "";
                foreach (var file in files)
                {
                    operations += file.path + Environment.NewLine;
                }

                // TODO: s'organiser pour avoir la liste des fichiers à ouvrir.
                bool valid = UserValidation(selected_files[0], selected_files[1], operations);
                if (!valid)
                {
                    logger.Warn("Validation refused by user{1}{0}", fileListString, Environment.NewLine);
                    return;
                }
            }

            // On peut logger les opérations à faire.
            logger.Info("Files to open:{1}{0}", fileListString, Environment.NewLine);

            // Rendu ici on peut ouvrir les fichiers.
            open_ok = FileListerFunctions.FileOpener(files, true, true, txtMessage, Parameters.ExplorerIconMode);

            if (!open_ok)
            {
                // TODO: log plus explicite.
                logger.Warn("Opening file errors");

                // On a eu des problèmes lors de l'ouverture, on s'assure que la fenêtre est au premier plan.
                BringWindowToFront();
            }
            else if (Parameters.MinimizeApplication)
            {
                // En mode touchScreen on minimise la fenêtre après une ouverture correcte.
                WindowState = FormWindowState.Minimized;
                Show();
            }
        }

        /// <summary>
        /// Ouverture du port COM donné en argument.
        /// 
        /// On peut passer un port vide ("") pour fermer le port.
        /// </summary>
        /// <param name="port"></param>
        /// <returns>true si on est connecté en sortant.</returns>
        public bool OpenSerialPort(string port)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            try
            {
                CloseSerialPort();

                if (port != "")
                {
                    mySerialPort = new System.IO.Ports.SerialPort();
                    mySerialPort.PortName = port;
                    mySerialPort.BaudRate = 9600;
                    mySerialPort.Open();
                    mySerialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(DataReceived);
                }
            }
            catch (System.IO.IOException ex)
            {
                Activate();
                string message = string.Format(
                    "Impossible de communiquer avec le lecteur de code à barre.\n\n{0}", 
                    ex.Message.ToString());
                MessageBox.Show(message);

                CloseSerialPort();
            }

            UpdateGUI();

            Cursor.Current = Cursors.Default;
            Application.DoEvents();

            return mySerialPort != null && mySerialPort.IsOpen;
        }

        public void CloseSerialPort()
        {
            if (mySerialPort != null)
            {
                if (mySerialPort.IsOpen)
                {
                    mySerialPort.Close();
                    Application.DoEvents();
                    Thread.Sleep(200);
                }

                mySerialPort.Dispose();
                Application.DoEvents();
                Thread.Sleep(200);

                mySerialPort = null;
            }
        }

        /// <summary>
        /// Mise à jour de l'état d'affichage du GUI.
        /// 
        /// On conseille de mettre timerTrigger à true lorsqu'on l'appele
        /// à répétition par le timer: on ne fait alors que les ajustements
        /// nécéssaires pour être plus rapide.
        /// </summary>
        public void UpdateGUI(bool timerTrigger = false)
        {
            // Vérifie si le port COM est ouvert et ajuste le GUI pour afficher l'état.
            bool connected = mySerialPort != null && mySerialPort.IsOpen;

            if (connected)
                txtConnected.Text = "Connecté";
            else
                txtConnected.Text = "Déconnecté";

            if (!timerTrigger)
            {
                // Titre de la fenêtre
                string appName = AboutBox.AssemblyProduct;
                string windowTitle;
                if (Properties.Settings.Default.DocumentFilename.Length > 0)
                {
                    string modifiedText;
                    if (Properties.Settings.Default.DocumentDirty)
                        modifiedText = " [Modifié]";
                    else
                        modifiedText = "";

                    windowTitle = string.Format(
                        "{0} {1} - {2}",
                        Path.GetFileName(Properties.Settings.Default.DocumentFilename),
                        modifiedText,
                        appName);
                }
                else
                    windowTitle = appName;

                this.Text = windowTitle;
            }
        }

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(string text);

        public void SetText(string text)
        {
            // Un callback est sur un autre thread...
            if (this.txtSelected.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                // On veut être la fenêtre sur le devant. 
                // Comme on a été appelé par un thread on ne l'ai peut-être pas.
                BringWindowToFront();

                txtSelected.Text = Functions.CleanupString(text);
                ProcessOpenDirRequest();
            }
        }

        void BringWindowToFront()
        {
            Activate();
            txtSelected.Focus();
            WindowState = FormWindowState.Minimized;
            Show();
            WindowState = FormWindowState.Normal;
            Application.DoEvents();
        }

        public void DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(100);
            string barcode_data = mySerialPort.ReadExisting();
            System.Threading.Thread.Sleep(100);
            SetText(barcode_data);
        }

        private void btnConnection_Click(object sender, EventArgs e)
        {
            if (Parameters.CodeReaderComPort.Length>0)
            {
                OpenSerialPort(Parameters.CodeReaderComPort);              
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseSerialPort();
            SaveSettings();
        }

        /// <summary>
        /// Lecture des paramètres visibles à partir du stockage usager.
        /// </summary>
        private void LoadSettings()
        {
            Parameters.BaseDir = Properties.Settings.Default.BaseDir;
            Parameters.ConversionFilePath = Properties.Settings.Default.TranslationFile;
            Parameters.CodeReaderComPort = Properties.Settings.Default.ComPort;
            Parameters.AutoConnectMode = Properties.Settings.Default.AutoConnect;
            Parameters.PdfReaderProcessName = Properties.Settings.Default.PdfProcess;
            Parameters.OpenFileExpression = Properties.Settings.Default.OpenFileExpression;

            Parameters.UseGlobalFilePattern = Properties.Settings.Default.UseGlobalFilePattern;
            Parameters.AskUserValidation = Properties.Settings.Default.AskUserValidation;

            Parameters.CloseExplorerWindows = Properties.Settings.Default.CloseExplorerWindows;
            Parameters.CloseViewers = Properties.Settings.Default.CloseViewers;
            Parameters.MinimizeApplication = Properties.Settings.Default.MinimizeApplication;
            Parameters.OpenBaseDirExplorerWindow = Properties.Settings.Default.OpenBaseDirExplorerWindow;

            Parameters.AppendCodeToBasePath = Properties.Settings.Default.AppendCodeToBasePath;

            Parameters.ClipboardCopy = Properties.Settings.Default.ClipboardCopy;

            Parameters.ExplorerIconMode = Properties.Settings.Default.ExplorerIconMode;

            Width = Properties.Settings.Default.FormWidth;
            Height = Properties.Settings.Default.FormHeigth;
        }

        /// <summary>
        /// Sauvegarde des paramètres à partir du stockage usager.
        /// </summary>
        private void SaveSettings()
        {
            Properties.Settings.Default.BaseDir = Parameters.BaseDir;
            Properties.Settings.Default.TranslationFile = Parameters.ConversionFilePath;
            Properties.Settings.Default.ComPort = Parameters.CodeReaderComPort;
            Properties.Settings.Default.AutoConnect = Parameters.AutoConnectMode;
            Properties.Settings.Default.PdfProcess = Parameters.PdfReaderProcessName;
            Properties.Settings.Default.OpenFileExpression = Parameters.OpenFileExpression;

            Properties.Settings.Default.UseGlobalFilePattern = Parameters.UseGlobalFilePattern;
            Properties.Settings.Default.AskUserValidation = Parameters.AskUserValidation;

            Properties.Settings.Default.CloseExplorerWindows = Parameters.CloseExplorerWindows;
            Properties.Settings.Default.CloseViewers = Parameters.CloseViewers;
            Properties.Settings.Default.MinimizeApplication = Parameters.MinimizeApplication;
            Properties.Settings.Default.OpenBaseDirExplorerWindow = Parameters.OpenBaseDirExplorerWindow;

            Properties.Settings.Default.AppendCodeToBasePath = Parameters.AppendCodeToBasePath;

            Properties.Settings.Default.ClipboardCopy = Parameters.ClipboardCopy;

            Properties.Settings.Default.ExplorerIconMode = Parameters.ExplorerIconMode;

            Properties.Settings.Default.FormWidth = Width;
            Properties.Settings.Default.FormHeigth = Height;

            Properties.Settings.Default.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Le test ne fonctionne pas. 
            //System.Windows.Forms.Timer MyTimer = new System.Windows.Forms.Timer();
            //MyTimer.Interval = (1 * 1000);
            //MyTimer.Tick += new EventHandler(MyTimer_Tick);
            //MyTimer.Start();
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            // Le test ne fonctionne pas. 
            //CheckComPortOpen();
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            Configurer();
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void configuationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Configurer();
        }

        private void Configurer()
        {
            ParamsForm dlg = new ParamsForm(this);
            dlg.Parameters = Parameters;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Parameters = dlg.Parameters;

                Properties.Settings.Default.DocumentDirty = true;
            }

            // On ne sait pas ce qui s'est passé dans le menu de configuration, 
            // peut-être qu'on a déconnecté le port série. Il est important
            // de remettre à jour l'affichage.
            UpdateGUI();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox dlg = new AboutBox();
            dlg.ShowDialog();
        }

        private bool UserValidation(string barCode, string itemCode, string operations)
        {
            bool valid;

            string message = string.Format("Vous avez scanné : {0}", barCode) + Environment.NewLine;
            message += string.Format("Ce qui correspond à : {0}", itemCode) + Environment.NewLine;

            if (operations.Length > 0)
            {
                message += Environment.NewLine
                    + " Les fichiers suivants seront affichés: " + Environment.NewLine
                    + operations;
            }

            message += Environment.NewLine + "Est-ce correct ?";

            DialogResult res = MessageBox.Show(message, "Validation des opérations", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res==DialogResult.No)
            {
                // Quelque chose n'est pas correct
                MessageBox.Show("Veuillez communiquer le problème au responsable de l'application.", 
                                "Validation des opérations", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Warning);
                valid = false;

                // TODO: On pourrait logger.
            }
            else
            {
                valid = true;
            }

            return valid;
        }

        /// <summary>
        /// Verifie s'il y a des fichiers introuvables dans la liste à ouvrir.
        /// Si oui, affiche un message, fait confirmer à l'usager si on continue. 
        /// </summary>
        /// <param name="files">Liste des fichiers</param>
        /// <returns>vrai s'il faut arrêter les traitements suite à une erreur.</returns>
        private bool MustStopFileListError(FileListerInfos[] files)
        {
            bool retval = false;

            // Y a-t'il des erreurs ?
            bool errorFound = false;
            string message = "";
            foreach (var file in files)
            {
                if (file.error)
                {
                    errorFound = true;
                    message += file.path + Environment.NewLine;
                }
            }

            if (errorFound)
            {
                message = "Impossible de trouver certains fichiers ou répertoires demandés : "
                            + Environment.NewLine
                            + message
                            + Environment.NewLine
                            + "Vous devriez en aviser le responsable de l'application (pour vérifier la table de conversion)."
                            + Environment.NewLine
                            + Environment.NewLine
                            + "On continue quand même ?";

                var userChoice = MessageBox.Show(message, 
                                                 "Fichiers à ouvrir", 
                                                 MessageBoxButtons.YesNo, 
                                                 MessageBoxIcon.Warning);
                if (userChoice == DialogResult.No)
                    retval = true;
            }

            return retval;
        }

        private void guiTimer_Tick(object sender, EventArgs e)
        {
            UpdateGUI(true);
        }

        /// <summary>
        /// Remise aux paramètres d'origine de la configuration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nouvelleConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logger.Info("ConfigNew");

            Properties.Settings.Default.Reset();
            LoadSettings();
            UpdateGUI();
        }

        private void importerLaConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fbd = new OpenFileDialog();
            fbd.Filter = "Fichier XML|*.xml|Tous les fichiers|*.*";
            fbd.Title = "Importer une configuration";
            fbd.CheckFileExists = true;

            var result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                logger.Info("ConfigImport: {0}", fbd.FileName);

                XmlSerializer reader = new XmlSerializer(Parameters.GetType());

                StreamReader file = new StreamReader(fbd.FileName);
                Parameters = (ParameterStruct)reader.Deserialize(file);
                file.Close();

                Properties.Settings.Default.DocumentFilename = fbd.FileName;
                Properties.Settings.Default.DocumentDirty = false;

                UpdateGUI();
            }
        }

        private void exporterLaConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fbd = new SaveFileDialog();

            fbd.Filter = "Fichier XML|*.xml|Tous les fichiers|*.*";
            fbd.Title = "Exporter la configuration";

            // Nom de la configuration précédente.
            if (Properties.Settings.Default.DocumentFilename.Length > 0)
            {
                fbd.FileName = Path.GetFileName(Properties.Settings.Default.DocumentFilename);

                string initialDirectory = Path.GetDirectoryName(Properties.Settings.Default.DocumentFilename);
                if (Directory.Exists(initialDirectory))
                    fbd.InitialDirectory = initialDirectory;
            }

            var result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                logger.Info("ConfigExport: {0}", fbd.FileName);

                XmlSerializer writer = new XmlSerializer(Parameters.GetType());
                StreamWriter file = new StreamWriter(fbd.FileName);
                writer.Serialize(file, Parameters);
                file.Close();

                Properties.Settings.Default.DocumentFilename = fbd.FileName;
                Properties.Settings.Default.DocumentDirty = false;

                UpdateGUI();
            }
        }
    }
}
