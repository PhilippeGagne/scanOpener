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
using System.Windows.Forms;

// Pour le singleton
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices; //required for APIs
using NLog;

namespace scanOpener
{
    static class Program
    {
        // Support de log
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //Import the SetForeground API to activate it
        //[DllImportAttribute("User32.dll")]
        //private static extern IntPtr SetForegroundWindow(int hWnd);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // Pour supporter le mode instance unique de l'application.
            // ref: http://stackoverflow.com/questions/19147/what-is-the-correct-way-to-create-a-single-instance-application

            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "B8DA8ADA-E42C-4C26-BDF3-ABE7D79DF3E9", out createdNew))
            {
                if (createdNew)
                {
                    logger.Info("Start");
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());
                    mutex.ReleaseMutex();
                }
                else
                {
                    logger.Info("Already running");
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            int hwnd = (int)process.MainWindowHandle;
                            // V1:
                            //SetForegroundWindow(hwnd);
                            
                            // V2:
                            // send our Win32 message to make the currently running instance
                            // jump on top of all the other windows
                            NativeMethods.PostMessage(
                                (IntPtr)NativeMethods.HWND_BROADCAST,
                                NativeMethods.WM_SHOWME,
                                IntPtr.Zero,
                                IntPtr.Zero);


                            break;
                        }
                    }
                }
            }

            logger.Info("Stop");
        }

        // Pour supporter le mode instance unique de l'application.
        // ref: http://stackoverflow.com/questions/19147/what-is-the-correct-way-to-create-a-single-instance-application
        // this class just wraps some Win32 stuff that we're going to use
        internal class NativeMethods
        {
            public const int HWND_BROADCAST = 0xffff;
            public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME_B8DA8ADA-E42C-4C26-BDF3-ABE7D79DF3E9");
            [DllImport("user32")]
            public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
            [DllImport("user32", CharSet = CharSet.Unicode)]
            public static extern int RegisterWindowMessage(string message);
        }
    }
}
