namespace scanOpener
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtConnected = new System.Windows.Forms.Label();
            this.btnConnection = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.txtSelected = new System.Windows.Forms.TextBox();
            this.Sélection = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fichierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nouvelleConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importerLaConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exporterLaConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.configuationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.quitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guiTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtConnected
            // 
            this.txtConnected.Location = new System.Drawing.Point(252, 54);
            this.txtConnected.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtConnected.Name = "txtConnected";
            this.txtConnected.Size = new System.Drawing.Size(156, 20);
            this.txtConnected.TabIndex = 12;
            this.txtConnected.Text = "Non connecté";
            // 
            // btnConnection
            // 
            this.btnConnection.Location = new System.Drawing.Point(512, 44);
            this.btnConnection.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(112, 44);
            this.btnConnection.TabIndex = 9;
            this.btnConnection.TabStop = false;
            this.btnConnection.Text = "Connecter";
            this.btnConnection.UseVisualStyleBackColor = true;
            this.btnConnection.Click += new System.EventHandler(this.btnConnection_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOpen.Location = new System.Drawing.Point(512, 105);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(112, 44);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "Ouvrir";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // txtSelected
            // 
            this.txtSelected.Location = new System.Drawing.Point(148, 112);
            this.txtSelected.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSelected.Name = "txtSelected";
            this.txtSelected.Size = new System.Drawing.Size(331, 26);
            this.txtSelected.TabIndex = 0;
            // 
            // Sélection
            // 
            this.Sélection.AutoSize = true;
            this.Sélection.Location = new System.Drawing.Point(14, 116);
            this.Sélection.Name = "Sélection";
            this.Sélection.Size = new System.Drawing.Size(121, 20);
            this.Sélection.TabIndex = 7;
            this.Sélection.Text = "Code-barres lu :";
            // 
            // txtMessage
            // 
            this.txtMessage.AcceptsReturn = true;
            this.txtMessage.AcceptsTab = true;
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.Location = new System.Drawing.Point(14, 195);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMessage.Size = new System.Drawing.Size(610, 326);
            this.txtMessage.TabIndex = 12;
            this.txtMessage.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 170);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(167, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Opérations effectuées";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fichierToolStripMenuItem,
            this.aideToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(638, 33);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fichierToolStripMenuItem
            // 
            this.fichierToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nouvelleConfigurationToolStripMenuItem,
            this.importerLaConfigurationToolStripMenuItem,
            this.exporterLaConfigurationToolStripMenuItem,
            this.toolStripSeparator1,
            this.configuationToolStripMenuItem,
            this.toolStripSeparator2,
            this.quitterToolStripMenuItem});
            this.fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
            this.fichierToolStripMenuItem.Size = new System.Drawing.Size(74, 29);
            this.fichierToolStripMenuItem.Text = "Fichier";
            // 
            // nouvelleConfigurationToolStripMenuItem
            // 
            this.nouvelleConfigurationToolStripMenuItem.Name = "nouvelleConfigurationToolStripMenuItem";
            this.nouvelleConfigurationToolStripMenuItem.Size = new System.Drawing.Size(312, 30);
            this.nouvelleConfigurationToolStripMenuItem.Text = "Nouvelle configuration";
            this.nouvelleConfigurationToolStripMenuItem.Click += new System.EventHandler(this.nouvelleConfigurationToolStripMenuItem_Click);
            // 
            // importerLaConfigurationToolStripMenuItem
            // 
            this.importerLaConfigurationToolStripMenuItem.Name = "importerLaConfigurationToolStripMenuItem";
            this.importerLaConfigurationToolStripMenuItem.Size = new System.Drawing.Size(312, 30);
            this.importerLaConfigurationToolStripMenuItem.Text = "Importer une configuration";
            this.importerLaConfigurationToolStripMenuItem.Click += new System.EventHandler(this.importerLaConfigurationToolStripMenuItem_Click);
            // 
            // exporterLaConfigurationToolStripMenuItem
            // 
            this.exporterLaConfigurationToolStripMenuItem.Name = "exporterLaConfigurationToolStripMenuItem";
            this.exporterLaConfigurationToolStripMenuItem.Size = new System.Drawing.Size(312, 30);
            this.exporterLaConfigurationToolStripMenuItem.Text = "Exporter la configuration";
            this.exporterLaConfigurationToolStripMenuItem.Click += new System.EventHandler(this.exporterLaConfigurationToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(309, 6);
            // 
            // configuationToolStripMenuItem
            // 
            this.configuationToolStripMenuItem.Name = "configuationToolStripMenuItem";
            this.configuationToolStripMenuItem.Size = new System.Drawing.Size(312, 30);
            this.configuationToolStripMenuItem.Text = "Configuation...";
            this.configuationToolStripMenuItem.Click += new System.EventHandler(this.configuationToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(309, 6);
            // 
            // quitterToolStripMenuItem
            // 
            this.quitterToolStripMenuItem.Name = "quitterToolStripMenuItem";
            this.quitterToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.quitterToolStripMenuItem.Size = new System.Drawing.Size(312, 30);
            this.quitterToolStripMenuItem.Text = "Quitter";
            this.quitterToolStripMenuItem.Click += new System.EventHandler(this.quitterToolStripMenuItem_Click);
            // 
            // aideToolStripMenuItem
            // 
            this.aideToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutToolStripMenuItem});
            this.aideToolStripMenuItem.Name = "aideToolStripMenuItem";
            this.aideToolStripMenuItem.Size = new System.Drawing.Size(60, 29);
            this.aideToolStripMenuItem.Text = "Aide";
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(350, 30);
            this.AboutToolStripMenuItem.Text = "À propos de MiSuperLauncher...";
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // guiTimer
            // 
            this.guiTimer.Enabled = true;
            this.guiTimer.Interval = 1000;
            this.guiTimer.Tick += new System.EventHandler(this.guiTimer_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "État du lecteur de code-barres :";
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnOpen;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 538);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtConnected);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnConnection);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.txtSelected);
            this.Controls.Add(this.Sélection);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(492, 448);
            this.Name = "MainForm";
            this.Text = "config.xml [modifié] - scanOpener";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.TextBox txtSelected;
        private System.Windows.Forms.Label Sélection;
        private System.Windows.Forms.Button btnConnection;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label txtConnected;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fichierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configuationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem aideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importerLaConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exporterLaConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Timer guiTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem nouvelleConfigurationToolStripMenuItem;
    }
}

