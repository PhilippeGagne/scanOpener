using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Xml.Serialization;
using static scanOpener.FileListerFunctions;
using System.Collections.Generic;

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
        public bool OldConvertInterpretation;
        public bool AskUserValidation;

        public bool CloseExplorerWindows;
        public bool CloseViewers;
        public bool MinimizeApplication;
        public bool OpenBaseDirExplorerWindow;
    }

    public partial class MainForm : Form
    {
        private System.IO.Ports.SerialPort mySerialPort;

        /* Paramètres de fonctionnement principaux, visibles par l'utilisateur
         * et sauvegardés.
         */
        private ParameterStruct Parameters;


        public MainForm()
        {
            InitializeComponent();

            LoadSettings();

            if (Parameters.AutoConnectMode && Parameters.CodeReaderComPort.Length > 0)
                  OpenSerialPort(Parameters.CodeReaderComPort);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            ProcessOpenDirRequest();
        }
        
        private void ProcessOpenDirRequest()
        {
            // Code à barre.
            string selected = Functions.CleanupString(txtSelected.Text);
//            // Répertoire de base où sont tous les sous répertoires.
//            string base_dir = txtBaseDir.Text;

            // Liste des fichiers à ouvrir. La première colonne est
            // le répertoire où se trouvent les documents pertinents
            string[] selected_files = null;     

//            // Fichier de conversion, première colonne est le code à barre,
//            // la deuxième est le répertoire pertinent et les colonnes suivantes sont
//            // des fichiers à ouvrir (dans le répertoire pertinent).
//            string convert_file = txtConvertFile.Text;  

//            bool debug_mode = chk_DebugMode.Checked;
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
                Functions.CloseAllPdfWindows(Parameters.PdfReaderProcessName);
                Application.DoEvents();
                txtMessage.Text += "Fait." + Environment.NewLine;
            }


            // On a rien à faire ici si la sélection est vide.
            // TODO: filtrage contre les gremelins (espaces).
            if (selected.Length == 0)
            {
                string message = "Rien de sélectionné.";
                txtMessage.Text += message + Environment.NewLine;
//                if (Parameters.DebugMode)
//                {
//                    MessageBox.Show(message, dialog_caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
//                }
                return; // Rien de sélectionné.
            }

            // Tentative de conversion du nom du fichier, si la table existe.
            if (Parameters.ConversionFilePath == "")
            {
                // Pas de fichier de conversion, on se fait une ligne de conversion simple.
                selected_files = new string[] { selected, selected };

                string message = "Pas de fichier de conversion spécifié.";
                txtMessage.Text += message + Environment.NewLine;
//                if (Parameters.DebugMode)
//                {
//                    MessageBox.Show(message, dialog_caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
//                }
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

            bool open_ok;
            if (Parameters.OldConvertInterpretation)
            {
                // Ancienne façon de fonctionner.
                // TODO: ne plus utiliser quand on sera certain que la nouvelle manière de faire est bonne.

                // Validation optionnelle des opérations, avant de faire quoi que ce soit.
                if (Parameters.AskUserValidation)
                {
                    // TODO: s'organiser pour avoir la liste des fichiers à ouvrir.
                    bool valid = UserValidation(selected_files[0], selected_files[1], "");

                    if (!valid)
                        return;
                }

                // Mode ancien d'ouverture des fichiers selon la table de conversion. Pas de caractères génériques.
                open_ok = Functions.OpenDirectoryAndFiles(Parameters.BaseDir, selected_files, txtMessage);
            }
            else
            {
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
                if (MustStopFileListError(files))
                {
                    // TODO: log
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
                        return;
                }

                // Rendu ici on peut ouvrir les fichiers.
                open_ok = FileListerFunctions.FileOpener(files, true, true, txtMessage);
            }


            if (!open_ok)
            {
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
        /// </summary>
        public void UpdateGUI()
        {
            // Vérifie si le port COM est ouvert et ajuste le GUI pour afficher l'état.
            bool connected = mySerialPort != null && mySerialPort.IsOpen;
            chkConnected.Checked = connected;
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

        private void LoadSettings()
        {
            Parameters.BaseDir = Properties.Settings.Default.BaseDir;
            Parameters.ConversionFilePath = Properties.Settings.Default.TranslationFile;
            Parameters.CodeReaderComPort = Properties.Settings.Default.ComPort;
            Parameters.AutoConnectMode = Properties.Settings.Default.AutoConnect;
            Parameters.PdfReaderProcessName = Properties.Settings.Default.PdfProcess;
            Parameters.OpenFileExpression = Properties.Settings.Default.OpenFileExpression;

            Parameters.UseGlobalFilePattern = Properties.Settings.Default.UseGlobalFilePattern;
            Parameters.OldConvertInterpretation = Properties.Settings.Default.OldConvertInterpretation;
            Parameters.AskUserValidation = Properties.Settings.Default.AskUserValidation;

            Parameters.CloseExplorerWindows = Properties.Settings.Default.CloseExplorerWindows;
            Parameters.CloseViewers = Properties.Settings.Default.CloseViewers;
            Parameters.MinimizeApplication = Properties.Settings.Default.MinimizeApplication;
            Parameters.OpenBaseDirExplorerWindow = Properties.Settings.Default.OpenBaseDirExplorerWindow;

            Parameters.AppendCodeToBasePath = Properties.Settings.Default.AppendCodeToBasePath;

            Width = Properties.Settings.Default.FormWidth;
            Height = Properties.Settings.Default.FormHeigth;
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.BaseDir = Parameters.BaseDir;
            Properties.Settings.Default.TranslationFile = Parameters.ConversionFilePath;
            Properties.Settings.Default.ComPort = Parameters.CodeReaderComPort;
            Properties.Settings.Default.AutoConnect = Parameters.AutoConnectMode;
            Properties.Settings.Default.PdfProcess = Parameters.PdfReaderProcessName;
            Properties.Settings.Default.OpenFileExpression = Parameters.OpenFileExpression;

            Properties.Settings.Default.UseGlobalFilePattern = Parameters.UseGlobalFilePattern;
            Properties.Settings.Default.OldConvertInterpretation = Parameters.OldConvertInterpretation;
            Properties.Settings.Default.AskUserValidation = Parameters.AskUserValidation;

            Properties.Settings.Default.CloseExplorerWindows = Parameters.CloseExplorerWindows;
            Properties.Settings.Default.CloseViewers = Parameters.CloseViewers;
            Properties.Settings.Default.MinimizeApplication = Parameters.MinimizeApplication;
            Properties.Settings.Default.OpenBaseDirExplorerWindow = Parameters.OpenBaseDirExplorerWindow;

            Properties.Settings.Default.AppendCodeToBasePath = Parameters.AppendCodeToBasePath;

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
            }

            // On ne sait pas ce qui s'est passé dans le menu de configuration, 
            // peut-être qu'on a déconnecté le port série. Il est important
            // de remettre à jour l'affichage.
            UpdateGUI();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 dlg = new AboutBox1();
            dlg.ShowDialog();
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
                XmlSerializer reader = new XmlSerializer(Parameters.GetType());

                StreamReader file = new StreamReader(fbd.FileName);
                Parameters = (ParameterStruct) reader.Deserialize(file);
                file.Close();
            }
        }

        private void exporterLaConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fbd = new SaveFileDialog();

            fbd.Filter = "Fichier XML|*.xml|Tous les fichiers|*.*";
            fbd.Title = "Exporter la configuration";
            var result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                XmlSerializer writer = new XmlSerializer(Parameters.GetType());
                StreamWriter file = new StreamWriter(fbd.FileName);
                writer.Serialize(file, Parameters);
                file.Close();
            }
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
            UpdateGUI();
        }
    }
}
