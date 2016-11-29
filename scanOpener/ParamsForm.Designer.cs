namespace scanOpener
{
    partial class ParamsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParamsForm));
            this.btnExit = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSelectConvert = new System.Windows.Forms.Button();
            this.txtConvertFile = new System.Windows.Forms.TextBox();
            this.txtBaseDir = new System.Windows.Forms.TextBox();
            this.btnSelectBaseDir = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtComPort = new System.Windows.Forms.TextBox();
            this.btnConnection = new System.Windows.Forms.Button();
            this.chkAutoConnect = new System.Windows.Forms.CheckBox();
            this.txtPdfProcess = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOpenExpression = new System.Windows.Forms.TextBox();
            this.chkUseGlobalPattern = new System.Windows.Forms.CheckBox();
            this.chkAskUserValidation = new System.Windows.Forms.CheckBox();
            this.btnOpenConversionFile = new System.Windows.Forms.Button();
            this.btnOpenBaseDir = new System.Windows.Forms.Button();
            this.chkCloseExplorerWindows = new System.Windows.Forms.CheckBox();
            this.chkCloseViewers = new System.Windows.Forms.CheckBox();
            this.chkMinimizeApplication = new System.Windows.Forms.CheckBox();
            this.chkOpenBaseDir = new System.Windows.Forms.CheckBox();
            this.chkAppendCodeToBasePath = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnOpenDeviceManagementWindow = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.guiTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.chkClipboardCopy = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnExit.Location = new System.Drawing.Point(517, 581);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 35);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Ok";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(623, 581);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 35);
            this.button1.TabIndex = 4;
            this.button1.Text = "Annuler";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnSelectConvert
            // 
            this.btnSelectConvert.Location = new System.Drawing.Point(546, 19);
            this.btnSelectConvert.Name = "btnSelectConvert";
            this.btnSelectConvert.Size = new System.Drawing.Size(75, 35);
            this.btnSelectConvert.TabIndex = 1;
            this.btnSelectConvert.TabStop = false;
            this.btnSelectConvert.Text = "...";
            this.btnSelectConvert.UseVisualStyleBackColor = true;
            this.btnSelectConvert.Click += new System.EventHandler(this.btnSelectConvert_Click);
            // 
            // txtConvertFile
            // 
            this.txtConvertFile.Location = new System.Drawing.Point(9, 25);
            this.txtConvertFile.Name = "txtConvertFile";
            this.txtConvertFile.Size = new System.Drawing.Size(531, 22);
            this.txtConvertFile.TabIndex = 0;
            this.txtConvertFile.TabStop = false;
            // 
            // txtBaseDir
            // 
            this.txtBaseDir.Location = new System.Drawing.Point(9, 21);
            this.txtBaseDir.Name = "txtBaseDir";
            this.txtBaseDir.Size = new System.Drawing.Size(531, 22);
            this.txtBaseDir.TabIndex = 0;
            this.txtBaseDir.TabStop = false;
            // 
            // btnSelectBaseDir
            // 
            this.btnSelectBaseDir.Location = new System.Drawing.Point(546, 15);
            this.btnSelectBaseDir.Name = "btnSelectBaseDir";
            this.btnSelectBaseDir.Size = new System.Drawing.Size(75, 35);
            this.btnSelectBaseDir.TabIndex = 1;
            this.btnSelectBaseDir.TabStop = false;
            this.btnSelectBaseDir.Text = "...";
            this.btnSelectBaseDir.UseVisualStyleBackColor = true;
            this.btnSelectBaseDir.Click += new System.EventHandler(this.btnSelectBaseDir_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Port COM du lecteur : ";
            // 
            // txtComPort
            // 
            this.txtComPort.Location = new System.Drawing.Point(158, 30);
            this.txtComPort.Name = "txtComPort";
            this.txtComPort.Size = new System.Drawing.Size(108, 22);
            this.txtComPort.TabIndex = 0;
            this.txtComPort.TabStop = false;
            // 
            // btnConnection
            // 
            this.btnConnection.Location = new System.Drawing.Point(272, 24);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(94, 35);
            this.btnConnection.TabIndex = 1;
            this.btnConnection.TabStop = false;
            this.btnConnection.Text = "Tester";
            this.btnConnection.UseVisualStyleBackColor = true;
            this.btnConnection.Click += new System.EventHandler(this.btnConnection_Click);
            // 
            // chkAutoConnect
            // 
            this.chkAutoConnect.AutoSize = true;
            this.chkAutoConnect.Location = new System.Drawing.Point(7, 58);
            this.chkAutoConnect.Name = "chkAutoConnect";
            this.chkAutoConnect.Size = new System.Drawing.Size(374, 21);
            this.chkAutoConnect.TabIndex = 11;
            this.chkAutoConnect.TabStop = false;
            this.chkAutoConnect.Text = "Connection automatique au démarrage de l\'application";
            this.chkAutoConnect.UseVisualStyleBackColor = true;
            // 
            // txtPdfProcess
            // 
            this.txtPdfProcess.Location = new System.Drawing.Point(195, 19);
            this.txtPdfProcess.Name = "txtPdfProcess";
            this.txtPdfProcess.Size = new System.Drawing.Size(321, 22);
            this.txtPdfProcess.TabIndex = 0;
            this.txtPdfProcess.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 17);
            this.label4.TabIndex = 15;
            this.label4.Text = "Nom du processus :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(182, 17);
            this.label5.TabIndex = 17;
            this.label5.Text = "Patron des fichiers à ouvrir:";
            // 
            // txtOpenExpression
            // 
            this.txtOpenExpression.Location = new System.Drawing.Point(195, 76);
            this.txtOpenExpression.Name = "txtOpenExpression";
            this.txtOpenExpression.Size = new System.Drawing.Size(346, 22);
            this.txtOpenExpression.TabIndex = 0;
            this.txtOpenExpression.TabStop = false;
            // 
            // chkUseGlobalPattern
            // 
            this.chkUseGlobalPattern.AutoSize = true;
            this.chkUseGlobalPattern.Location = new System.Drawing.Point(568, 77);
            this.chkUseGlobalPattern.Name = "chkUseGlobalPattern";
            this.chkUseGlobalPattern.Size = new System.Drawing.Size(133, 21);
            this.chkUseGlobalPattern.TabIndex = 1;
            this.chkUseGlobalPattern.Text = "Utiliser le patron";
            this.chkUseGlobalPattern.UseVisualStyleBackColor = true;
            // 
            // chkAskUserValidation
            // 
            this.chkAskUserValidation.AutoSize = true;
            this.chkAskUserValidation.Location = new System.Drawing.Point(10, 24);
            this.chkAskUserValidation.Name = "chkAskUserValidation";
            this.chkAskUserValidation.Size = new System.Drawing.Size(240, 21);
            this.chkAskUserValidation.TabIndex = 0;
            this.chkAskUserValidation.Text = "Afficher un dialogue de validation";
            this.chkAskUserValidation.UseVisualStyleBackColor = true;
            // 
            // btnOpenConversionFile
            // 
            this.btnOpenConversionFile.Location = new System.Drawing.Point(626, 19);
            this.btnOpenConversionFile.Name = "btnOpenConversionFile";
            this.btnOpenConversionFile.Size = new System.Drawing.Size(75, 35);
            this.btnOpenConversionFile.TabIndex = 2;
            this.btnOpenConversionFile.TabStop = false;
            this.btnOpenConversionFile.Text = "Ouvrir";
            this.btnOpenConversionFile.UseVisualStyleBackColor = true;
            this.btnOpenConversionFile.Click += new System.EventHandler(this.btnOpenConversionFile_Click);
            // 
            // btnOpenBaseDir
            // 
            this.btnOpenBaseDir.Location = new System.Drawing.Point(626, 15);
            this.btnOpenBaseDir.Name = "btnOpenBaseDir";
            this.btnOpenBaseDir.Size = new System.Drawing.Size(75, 35);
            this.btnOpenBaseDir.TabIndex = 2;
            this.btnOpenBaseDir.TabStop = false;
            this.btnOpenBaseDir.Text = "Ouvrir";
            this.btnOpenBaseDir.UseVisualStyleBackColor = true;
            this.btnOpenBaseDir.Click += new System.EventHandler(this.btnOpenBaseDir_Click);
            // 
            // chkCloseExplorerWindows
            // 
            this.chkCloseExplorerWindows.AutoSize = true;
            this.chkCloseExplorerWindows.Location = new System.Drawing.Point(466, 47);
            this.chkCloseExplorerWindows.Name = "chkCloseExplorerWindows";
            this.chkCloseExplorerWindows.Size = new System.Drawing.Size(241, 21);
            this.chkCloseExplorerWindows.TabIndex = 2;
            this.chkCloseExplorerWindows.Text = "Fermer les explorateurs de fichier";
            this.chkCloseExplorerWindows.UseVisualStyleBackColor = true;
            // 
            // chkCloseViewers
            // 
            this.chkCloseViewers.AutoSize = true;
            this.chkCloseViewers.Location = new System.Drawing.Point(538, 21);
            this.chkCloseViewers.Name = "chkCloseViewers";
            this.chkCloseViewers.Size = new System.Drawing.Size(163, 21);
            this.chkCloseViewers.TabIndex = 1;
            this.chkCloseViewers.Text = "Fermer les afficheurs";
            this.chkCloseViewers.UseVisualStyleBackColor = true;
            // 
            // chkMinimizeApplication
            // 
            this.chkMinimizeApplication.AutoSize = true;
            this.chkMinimizeApplication.Location = new System.Drawing.Point(281, 24);
            this.chkMinimizeApplication.Name = "chkMinimizeApplication";
            this.chkMinimizeApplication.Size = new System.Drawing.Size(302, 21);
            this.chkMinimizeApplication.TabIndex = 1;
            this.chkMinimizeApplication.Text = "Minimiser l\'application durant son utilisation";
            this.chkMinimizeApplication.UseVisualStyleBackColor = true;
            // 
            // chkOpenBaseDir
            // 
            this.chkOpenBaseDir.AutoSize = true;
            this.chkOpenBaseDir.Location = new System.Drawing.Point(10, 48);
            this.chkOpenBaseDir.Name = "chkOpenBaseDir";
            this.chkOpenBaseDir.Size = new System.Drawing.Size(321, 21);
            this.chkOpenBaseDir.TabIndex = 3;
            this.chkOpenBaseDir.Text = "Ouvrir le répertoire de base dans l\'explorateur";
            this.chkOpenBaseDir.UseVisualStyleBackColor = true;
            // 
            // chkAppendCodeToBasePath
            // 
            this.chkAppendCodeToBasePath.AutoSize = true;
            this.chkAppendCodeToBasePath.Location = new System.Drawing.Point(10, 21);
            this.chkAppendCodeToBasePath.Name = "chkAppendCodeToBasePath";
            this.chkAppendCodeToBasePath.Size = new System.Drawing.Size(505, 21);
            this.chkAppendCodeToBasePath.TabIndex = 3;
            this.chkAppendCodeToBasePath.Text = "Ajouter le code de l\'objet  (@) au répertoire commun (i.e. z:\\commun\\code\\)";
            this.chkAppendCodeToBasePath.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtBaseDir);
            this.groupBox1.Controls.Add(this.btnSelectBaseDir);
            this.groupBox1.Controls.Add(this.btnOpenBaseDir);
            this.groupBox1.Location = new System.Drawing.Point(12, 181);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(713, 65);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Répertoire commun où se situent les documents";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtConvertFile);
            this.groupBox2.Controls.Add(this.btnSelectConvert);
            this.groupBox2.Controls.Add(this.btnOpenConversionFile);
            this.groupBox2.Location = new System.Drawing.Point(12, 106);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(713, 69);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Table de conversion";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnOpenDeviceManagementWindow);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.txtComPort);
            this.groupBox3.Controls.Add(this.btnConnection);
            this.groupBox3.Controls.Add(this.chkAutoConnect);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(711, 88);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Lecteur de code-barres";
            // 
            // btnOpenDeviceManagementWindow
            // 
            this.btnOpenDeviceManagementWindow.Location = new System.Drawing.Point(552, 30);
            this.btnOpenDeviceManagementWindow.Name = "btnOpenDeviceManagementWindow";
            this.btnOpenDeviceManagementWindow.Size = new System.Drawing.Size(149, 35);
            this.btnOpenDeviceManagementWindow.TabIndex = 12;
            this.btnOpenDeviceManagementWindow.TabStop = false;
            this.btnOpenDeviceManagementWindow.Text = "Afficher les ports";
            this.btnOpenDeviceManagementWindow.UseVisualStyleBackColor = true;
            this.btnOpenDeviceManagementWindow.Click += new System.EventHandler(this.btnOpenDeviceManagementWindow_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkAppendCodeToBasePath);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.txtOpenExpression);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.chkUseGlobalPattern);
            this.groupBox4.Controls.Add(this.chkOpenBaseDir);
            this.groupBox4.Location = new System.Drawing.Point(12, 252);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(713, 135);
            this.groupBox4.TabIndex = 31;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Ouverture automatique";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(434, 17);
            this.label6.TabIndex = 2;
            this.label6.Text = "NB: @ représente le code de l\'objet. Séparer chaque item par ; ou :";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.chkCloseViewers);
            this.groupBox5.Controls.Add(this.chkCloseExplorerWindows);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.txtPdfProcess);
            this.groupBox5.Location = new System.Drawing.Point(12, 393);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(713, 77);
            this.groupBox5.TabIndex = 32;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Fermeture automatique des fenêtres";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(225, 17);
            this.label1.TabIndex = 16;
            this.label1.Text = "Ex: AcroRd32 pour acrobat reader";
            // 
            // guiTimer
            // 
            this.guiTimer.Enabled = true;
            this.guiTimer.Interval = 200;
            this.guiTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.chkClipboardCopy);
            this.groupBox6.Controls.Add(this.chkAskUserValidation);
            this.groupBox6.Controls.Add(this.chkMinimizeApplication);
            this.groupBox6.Location = new System.Drawing.Point(12, 476);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(711, 99);
            this.groupBox6.TabIndex = 33;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Autres options";
            // 
            // chkClipboardCopy
            // 
            this.chkClipboardCopy.AutoSize = true;
            this.chkClipboardCopy.Location = new System.Drawing.Point(10, 51);
            this.chkClipboardCopy.Name = "chkClipboardCopy";
            this.chkClipboardCopy.Size = new System.Drawing.Size(183, 21);
            this.chkClipboardCopy.TabIndex = 2;
            this.chkClipboardCopy.Text = "Copier dans le clipboard";
            this.chkClipboardCopy.UseVisualStyleBackColor = true;
            // 
            // ParamsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 628);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox6);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ParamsForm";
            this.Text = "Configuration";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ParamsForm_Paint);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSelectConvert;
        private System.Windows.Forms.TextBox txtConvertFile;
        private System.Windows.Forms.TextBox txtBaseDir;
        private System.Windows.Forms.Button btnSelectBaseDir;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtComPort;
        private System.Windows.Forms.Button btnConnection;
        private System.Windows.Forms.CheckBox chkAutoConnect;
        private System.Windows.Forms.TextBox txtPdfProcess;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtOpenExpression;
        private System.Windows.Forms.CheckBox chkUseGlobalPattern;
        private System.Windows.Forms.CheckBox chkAskUserValidation;
        private System.Windows.Forms.Button btnOpenConversionFile;
        private System.Windows.Forms.Button btnOpenBaseDir;
        private System.Windows.Forms.CheckBox chkCloseExplorerWindows;
        private System.Windows.Forms.CheckBox chkCloseViewers;
        private System.Windows.Forms.CheckBox chkMinimizeApplication;
        private System.Windows.Forms.CheckBox chkOpenBaseDir;
        private System.Windows.Forms.CheckBox chkAppendCodeToBasePath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Timer guiTimer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnOpenDeviceManagementWindow;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkClipboardCopy;
    }
}