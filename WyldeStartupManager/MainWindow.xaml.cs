using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;

namespace WyldeStartupManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            PopulateStartupProgramsList();
        }

        private void PopulateStartupProgramsList()
        {
            if (startupPrograms.Items.Count > 0)
            {
                startupPrograms.Items.Clear();
            }
            RegistryKey key;
            key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", false);
            foreach (string name in key.GetValueNames())
            {
                try
                {
                    LVI lvi = new LVI
                    {
                        Enabled = "Yes",
                        Name = name,
                        Publisher = "publisher",
                        Key = "HKLM::Run",
                        FilePath = key.GetValue(name).ToString()[0] == '"' ? key.GetValue(name).ToString().Split('"')[1] : key.GetValue(name).ToString()
                    };
                    try
                    {
                        RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();
                        Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
                        runspace.Open();

                        Pipeline pipeline = runspace.CreatePipeline();
                        pipeline.Commands.AddScript("Get-AuthenticodeSignature \"" + lvi.FilePath + "\"");

                        Collection<PSObject> results = pipeline.Invoke();
                        runspace.Close();
                        Signature signature = results[0].BaseObject as Signature;
                        string[] temp = signature.SignerCertificate.Subject.Split('=');
                        for (int i = 1; i < temp.Length; i++)
                        {
                            if (temp[i - 1].Substring(temp[i - 1].Length - 1) == "O")
                            {
                                lvi.Publisher = temp[i].ToCharArray()[0] == '"' ? temp[i].Split('"')[1] : temp[i];
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error when trying to check if file is signed:" + lvi.FilePath + " --> " + e.Message);
                    }
                    startupPrograms.Items.Add(lvi);
                }
                catch (Exception) { }
            }
            key.Close();
            key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce", false);
            foreach (string name in key.GetValueNames())
            {
                try
                {
                    LVI lvi = new LVI
                    {
                        Enabled = "Yes",
                        Name = name,
                        Publisher = "",
                        Key = "HKLM::RunOnce",
                        FilePath = key.GetValue(name).ToString()[0] == '"' ? key.GetValue(name).ToString().Split('"')[1] : key.GetValue(name).ToString()
                    };
                    try
                    {
                        RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();
                        Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
                        runspace.Open();

                        Pipeline pipeline = runspace.CreatePipeline();
                        pipeline.Commands.AddScript("Get-AuthenticodeSignature \"" + lvi.FilePath + "\"");

                        Collection<PSObject> results = pipeline.Invoke();
                        runspace.Close();
                        Signature signature = results[0].BaseObject as Signature;
                        string[] temp = signature.SignerCertificate.Subject.Split('=');
                        for (int i = 1; i < temp.Length; i++)
                        {
                            if (temp[i - 1].Substring(temp[i - 1].Length - 1) == "O")
                            {
                                lvi.Publisher = temp[i].ToCharArray()[0] == '"' ? temp[i].Split('"')[1] : temp[i];
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error when trying to check if file is signed:" + lvi.FilePath + " --> " + e.Message);
                    }
                    startupPrograms.Items.Add(lvi);
                }
                catch (Exception) { }
            }
            key.Close();
            key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", false);
            foreach (string name in key.GetValueNames())
            {
                try
                {
                    LVI lvi = new LVI
                    {
                        Enabled = "Yes",
                        Name = name,
                        Publisher = "",
                        Key = "HKCU::Run",
                        FilePath = key.GetValue(name).ToString()[0] == '"' ? key.GetValue(name).ToString().Split('"')[1] : key.GetValue(name).ToString()
                    };
                    try
                    {
                        RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();
                        Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
                        runspace.Open();

                        Pipeline pipeline = runspace.CreatePipeline();
                        pipeline.Commands.AddScript("Get-AuthenticodeSignature \"" + lvi.FilePath + "\"");

                        Collection<PSObject> results = pipeline.Invoke();
                        runspace.Close();
                        Signature signature = results[0].BaseObject as Signature;
                        string[] temp = signature.SignerCertificate.Subject.Split('=');
                        for (int i = 1; i < temp.Length; i++)
                        {
                            if (temp[i - 1].Substring(temp[i - 1].Length - 1) == "O")
                            {
                                lvi.Publisher = temp[i].ToCharArray()[0] == '"' ? temp[i].Split('"')[1] : temp[i];
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error when trying to check if file is signed:" + lvi.FilePath + " --> " + e.Message);
                    }
                    startupPrograms.Items.Add(lvi);
                }
                catch (Exception) { }
            }
            key.Close();
        }

        private void dragBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButtons_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void maximizeButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Maximized)
            {
                WindowState = System.Windows.WindowState.Normal;
                maximizeButton.Text = "1";
            }
            else
            {
                WindowState = System.Windows.WindowState.Maximized;
                maximizeButton.Text = "2";
            }
        }

        private void minimizeButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = System.Windows.WindowState.Minimized;
        }

        private void closeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender == closeButton)
            {
                closeColorBox.Opacity = .2;
            }
            else if (sender == maximizeButton)
            {
                maximizeColorBox.Opacity = .2;
            }
            else if (sender == minimizeButton)
            {
                minimizeColorBox.Opacity = .2;
            }
        }

        private void closeColorBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender == closeButton)
            {
                closeColorBox.Opacity = 0;
            }
            else if (sender == maximizeButton)
            {
                maximizeColorBox.Opacity = 0;
            }
            else if (sender == minimizeButton)
            {
                minimizeColorBox.Opacity = 0;
            }
        }
    }

    public class LVI
    {
        public string Enabled { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public string Key { get; set; }
        public string FilePath { get; set; }
    }
}
