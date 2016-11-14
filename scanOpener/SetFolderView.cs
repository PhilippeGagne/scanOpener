// source: https://bitbucket.org/hintdesk/dotnet-set-folder-view-programmatically/overview

using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Set_folder_view_2
{
    public class Program
    {
        //private static void Main(string[] args)
        //{
        //    string path = @"C:\temp\ThưMục%C9É";
        //    SetFolderView(path, Set_folder_view_2.WinAPI.FOLDERVIEWMODE.FVM_TILE);
        //    Console.WriteLine("Set folder view successfully");
        //    Console.ReadLine();
        //}

        public static void SetFolderView(string path, Set_folder_view_2.WinAPI.FOLDERVIEWMODE folderViewMode)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    int tries = 0;

                    WinAPI.ShellExecute(IntPtr.Zero, "Open", path, "", "", WinAPI.ShowCommands.SW_NORMAL);

                    IntPtr hwndFolder = IntPtr.Zero;

                    while (hwndFolder == IntPtr.Zero && tries < 100)
                    {
                        hwndFolder = WinAPI.FindWindow("CabinetWClass", Path.GetFileName(path));
                        tries++;
                        Thread.Sleep(100);
                    }

                    if (hwndFolder != IntPtr.Zero)
                    {
                        SHDocVw.ShellWindows shellWindows = null;
                        try
                        {
                            shellWindows = new SHDocVw.ShellWindows();
                            foreach (Set_folder_view_2.WinAPI._IServiceProvider serviceProvider in shellWindows)
                            {
                                SHDocVw.InternetExplorer ie = (SHDocVw.InternetExplorer)serviceProvider;
                                string locationUrl = ParseUnicode(ie.LocationURL);
                                if (Path.GetFileNameWithoutExtension(ie.FullName).Equals("explorer", StringComparison.OrdinalIgnoreCase) && locationUrl.Equals("file:///" + path.Replace("\\", "/"), StringComparison.OrdinalIgnoreCase))
                                {
                                    object oShellBrowser;
                                    serviceProvider.QueryService(Set_folder_view_2.WinAPI.ExplorerGUIDs.IID_IShellBrowser, Set_folder_view_2.WinAPI.ExplorerGUIDs.IID_IUnknown, out oShellBrowser);
                                    Set_folder_view_2.WinAPI.IShellBrowser shellBrowser = oShellBrowser as Set_folder_view_2.WinAPI.IShellBrowser;
                                    Set_folder_view_2.WinAPI.IShellView shellView = null;
                                    try
                                    {
                                        if (0 == shellBrowser.QueryActiveShellView(out shellView))
                                        {
                                            Set_folder_view_2.WinAPI.IFolderView folderView = shellView as Set_folder_view_2.WinAPI.IFolderView;

                                            if (folderView != null)
                                            {
                                                Set_folder_view_2.WinAPI.FOLDERVIEWMODE currentMode = 0;
                                                folderView.GetCurrentViewMode(ref currentMode);
                                                Console.WriteLine(currentMode.ToString());
                                                folderView.SetCurrentViewMode(folderViewMode);
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    finally
                                    {
                                        if (shellView != null)
                                            Marshal.ReleaseComObject(shellView);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        finally
                        {
                            if (shellWindows != null)
                            {
                                Marshal.ReleaseComObject(shellWindows);
                            }
                        }
                    }

                    else
                    {
                        Console.WriteLine("Can not get handle of opened folder");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static string ParseUnicode(string mixUnicodeString)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < mixUnicodeString.Length; i++)
            {
                if (mixUnicodeString[i] != '%')
                    result.Append(mixUnicodeString[i]);
                else
                {
                    string hexCode = mixUnicodeString.Substring(i + 1, 2);
                    short charCode = 0;
                    if (Int16.TryParse(hexCode, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out charCode))
                    {
                        result.Append((char)charCode);
                        i += 2;
                    }
                }
            }
            return result.ToString();
        }
    }
}