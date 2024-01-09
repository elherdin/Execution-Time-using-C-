using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerformanceExe
{
    public partial class Form1 : Form
    {
        private string selectedFilePath;

        public Form1()
        {
            InitializeComponent();
        }

        List<Tuple<string, string>> executionTimes = new List<Tuple<string, string>>();

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "C# Files (*.cs)|*.cs|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFilePath = openFileDialog.FileName;
                    textBox1.Text = selectedFilePath;

                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    string fileContent = File.ReadAllText(selectedFilePath);
                    textBox2.Text = fileContent;

                    stopwatch.Stop();

                    label2.Text = $"Time (ms) = {stopwatch.Elapsed.ToString()}";

                    executionTimes.Add(new Tuple<string, string>(stopwatch.Elapsed.ToString(), selectedFilePath));
                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFolderPath = folderBrowserDialog.SelectedPath;
                    textBox1.Text = selectedFolderPath;

                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    string[] csFiles = Directory.GetFiles(selectedFolderPath, "*.cs");
                    StringBuilder allFileContent = new StringBuilder();

                    foreach (string csFile in csFiles)
                    {
                        string fileContent = File.ReadAllText(csFile);
                        allFileContent.AppendLine(fileContent);
                    }

                    textBox2.Text = allFileContent.ToString();

                    stopwatch.Stop();

                    label2.Text = $"Time (ms) = {stopwatch.Elapsed.ToString()}";

                    executionTimes.Add(new Tuple<string, string>(stopwatch.Elapsed.ToString(), selectedFolderPath));
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void button5_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        private void ShowHelp()
        {
            string help = OpenHelp();
            MessageBox.Show(help, "User Guide", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private string OpenHelp()
        {
            StringBuilder help = new StringBuilder();

            help.AppendLine("User Guide for Execution Time Application");
            help.AppendLine();
            help.AppendLine("1. Press the \"upload file\" option if you want to insert an individual file with the .cs format only.");
            help.AppendLine("2. Choose the \"upload folder\" option to include a folder containing .cs files.");
            help.AppendLine("3. The application will display the successfully executed time on the \"time (ms)\" bar.");
            help.AppendLine("4. The recorded time is presented in milliseconds (ms).");
            help.AppendLine("5. Press the export.csv button to download the executed time");
           

            return help.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowInfo();
        }

        private void ShowInfo()
        {
            string info = OpenInfo();
            MessageBox.Show(info, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string OpenInfo()
        {
            StringBuilder info = new StringBuilder();

            info.AppendLine("Execution Time: ");
            info.AppendLine();
            info.AppendLine("\"The execution time  is the total amount of time that the process executes; that time is generally independent of the initiation time but often depends on the input data.\"[1]");
            info.AppendLine("[1] https://www.sciencedirect.com/topics/computer-science/total-execution-time");
            info.AppendLine();
            info.AppendLine("Matrics: ");
            info.AppendLine();
            info.AppendLine("1. CPU Usage:");
            info.AppendLine("    a. Description:");
            info.AppendLine("        The CPU usage metric represents the percentage of the computer's processing power utilized during the execution time testing.");
            info.AppendLine("    b. Formula:");
            info.AppendLine("        CPU Usage = Current CPU Usage");
            info.AppendLine("2. RAM Usage:");
            info.AppendLine("    a. Description:");
            info.AppendLine("        The RAM usage metric indicates the amount of computer memory used during the execution time testing.");
            info.AppendLine("    b. Formula:");
            info.AppendLine("        RAM Usage = Current RAM Usage");
            info.AppendLine("3. Successful Request:");
            info.AppendLine("    a. Description:");
            info.AppendLine("        Successful Request is the time it takes for the application to read and display the contents of the selected file or folder.");
            info.AppendLine("    b. Formula:");
            info.AppendLine("        Execution Time = Time Required to Read and Display the Contents of a File or Folder");

            return info.ToString();
        }

        private void txtOutput_Click(object sender, EventArgs e)
        {
            if (executionTimes.Count > 0)
            {
                try
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
                    saveFileDialog.Title = "Save Metrics to CSV";
                    saveFileDialog.DefaultExt = "csv";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string csvFilePath = Path.Combine(saveFileDialog.InitialDirectory, saveFileDialog.FileName);

                        // Write the header to the CSV file
                        StringBuilder csvContent = new StringBuilder("Execution Time (ms),Processed File\n");

                        // Write each execution time and processed file name to the CSV file
                        foreach (var tuple in executionTimes)
                        {
                            csvContent.AppendLine($"{tuple.Item1},{tuple.Item2}");
                        }

                        // Write the content of txtOutput to the selected CSV file
                        File.WriteAllText(csvFilePath, csvContent.ToString());

                        MessageBox.Show("Metrics saved to CSV file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No metrics to export", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


    }
}
