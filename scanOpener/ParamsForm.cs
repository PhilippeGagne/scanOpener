using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace scanOpener
{
    public partial class ParamsForm : Form
    {
        private ParameterStruct parameters;

        private MainForm parentForm;  // Nécessaire pour tester le Comport.

        public ParamsForm(MainForm parent)
        {
            InitializeComponent();
            parentForm = parent;
        }
   
        public ParameterStruct Parameters
        {
            get
            {
                DialogToParameters();
                return parameters;
            }
            set
            {
                parameters = value;
                ParametersToDialog();
            }
        }

        private void DialogToParameters()
        {
            parameters.BaseDir = txtBaseDir.Text;
            parameters.ConversionFilePath = txtConvertFile.Text;
            parameters.CodeReaderComPort = txtComPort.Text;
            parameters.PdfReaderProcessName = txtPdfProcess.Text;
            parameters.AutoConnectMode = chkAutoConnect.Checked;
            parameters.OpenFileExpression = txtOpenExpression.Text;

            parameters.UseGlobalFilePattern = chkUseGlobalPattern.Checked;
            parameters.AskUserValidation = chkAskUserValidation.Checked;

            parameters.CloseExplorerWindows = chkCloseExplorerWindows.Checked;
            parameters.CloseViewers = chkCloseViewers.Checked;
            parameters.MinimizeApplication = chkMinimizeApplication.Checked;
            parameters.OpenBaseDirExplorerWindow = chkOpenBaseDir.Checked;

            parameters.AppendCodeToBasePath = chkAppendCodeToBasePath.Checked;

            parameters.ClipboardCopy = chkClipboardCopy.Checked;
        }

        private void ParametersToDialog()
        {
            txtBaseDir.Text = parameters.BaseDir;
            txtConvertFile.Text = parameters.ConversionFilePath;
            txtComPort.Text = parameters.CodeReaderComPort;
            txtPdfProcess.Text = parameters.PdfReaderProcessName;
            chkAutoConnect.Checked = parameters.AutoConnectMode;
            txtOpenExpression.Text = parameters.OpenFileExpression;

            chkUseGlobalPattern.Checked = parameters.UseGlobalFilePattern;
            chkAskUserValidation.Checked = parameters.AskUserValidation;

            chkCloseExplorerWindows.Checked = parameters.CloseExplorerWindows;
            chkCloseViewers.Checked = parameters.CloseViewers;
            chkMinimizeApplication.Checked = parameters.MinimizeApplication;
            chkOpenBaseDir.Checked = parameters.OpenBaseDirExplorerWindow;

            chkAppendCodeToBasePath.Checked = parameters.AppendCodeToBasePath;

            chkClipboardCopy.Checked = parameters.ClipboardCopy;
        }

        private void btnSelectBaseDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (Directory.Exists(txtBaseDir.Text))
                fbd.SelectedPath = txtBaseDir.Text;
            fbd.ShowNewFolderButton = false;

            var result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtBaseDir.Text = fbd.SelectedPath;
            }
        }

        private void btnSelectConvert_Click(object sender, EventArgs e)
        {
            OpenFileDialog fbd = new OpenFileDialog();
            if (File.Exists(txtConvertFile.Text))
            {
                fbd.FileName = Path.GetFileName(txtConvertFile.Text);
                fbd.InitialDirectory = Path.GetDirectoryName(txtConvertFile.Text);
            }

            fbd.CheckFileExists = true;

            var result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtConvertFile.Text = fbd.FileName;
            }
        }

        private void btnConnection_Click(object sender, EventArgs e)
        {
            if (txtComPort.Text != "")
            {
                bool connected = parentForm.OpenSerialPort(txtComPort.Text);

                if (connected)
                    MessageBox.Show(
                        string.Format("L'application peut utiliser le port {0}.", 
                        txtComPort.Text), "Test de la communication", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);

                parentForm.CloseSerialPort();
            }
        }

        private void btnOpenBaseDir_Click(object sender, EventArgs e)
        {
            if (txtBaseDir.Text != "")
            {
                Process.Start(txtBaseDir.Text);
            }
        }

        private void btnOpenConversionFile_Click(object sender, EventArgs e)
        {
            if (txtConvertFile.Text!="")
            {
                Process.Start(txtConvertFile.Text);
            }
        }

        private void ParamsForm_Paint(object sender, PaintEventArgs e)
        {
//            UpdateGUI();
        }

        private void UpdateGUI()
        {
            // Il faut qu'un port soit afficher pour qu'on puisse le tester
            btnConnection.Enabled = txtComPort.TextLength > 0;
            chkAutoConnect.Enabled = txtComPort.TextLength > 0;

            // Les boutons d'ouverture de fichier/répertoire ne sont en
            // fonction que s'il y a quelque chose à afficher.
            btnOpenBaseDir.Enabled = txtBaseDir.TextLength > 0;
            btnOpenConversionFile.Enabled = txtConvertFile.TextLength > 0;

            // Le patron global n'a de sens que si on veut l'utiliser.
            txtOpenExpression.Enabled = chkUseGlobalPattern.Checked;

            // Les processus à fermer n'ont de sens que si on veut les utiliser.
            txtPdfProcess.Enabled = chkCloseViewers.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateGUI();
        }

        private void btnOpenDeviceManagementWindow_Click(object sender, EventArgs e)
        {
            string windowsSystemDir = Environment.SystemDirectory;
            string deviceManagerCmd = Path.Combine(windowsSystemDir, "devmgmt.msc");
            Process.Start(deviceManagerCmd);
        }
    }
}
