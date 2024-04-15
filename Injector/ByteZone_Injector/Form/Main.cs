using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
namespace ByteZone_Injector
{




    public partial class ByteZone_Injector : Form
    {

        /// <summary>
        /// Gets or sets the name of the target process for injection.
        /// </summary>
        public string S_ProcessName { get; private set; }

        /// <summary>
        /// Gets or sets the file path of the DLL to be injected.
        /// </summary>
        private string dllPath;

        /// <summary>
        /// Instance of the InjectionMethods class used for performing injections.
        /// </summary>
        private InjectionMethods I_Method;

        public ByteZone_Injector()
        {
            InitializeComponent();
            I_Method = new InjectionMethods();
        }


        /// <summary>
        /// Retrieves a list of currently running processes and populates the process list.
        /// </summary>
        private void GetProcesses()
        {
            Process[] Processes = Process.GetProcesses(); // Get a list of all running processes
            foreach (Process process in Processes) // Add each process name and ID to the list box
            {
                string processInfo = $"{process.ProcessName} (ID: {process.Id})";
                listBox1.Items.Add(processInfo);
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            dllNameLbl.Visible = false;
            GetProcesses(); // Load the processes when injector launched
            string sValue = "Native injection";
            comboBox1.SelectedItem = sValue;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(S_ProcessName) && !string.IsNullOrEmpty(dllPath)) 
            {
                I_Method.NtNativeInjection(S_ProcessName, dllPath); // Calling NativeInjection from classes.cs
            }
            else
            {
                MessageBox.Show("Please select a process and a DLL file first.");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the button click event to select a DLL file using an OpenFileDialog.
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        { 
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.InitialDirectory = Environment.CurrentDirectory; // Set the initial directory to the current directory

            ofd.Filter = "dll files (*.dll) | *.dll"; // Filter to display only DLL files

            // If the user selects a file and clicks OK
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                dllPath = ofd.FileName; // Get the selected file path

                
                if (!string.IsNullOrEmpty(dllPath)) // If the file path is not empty
                {
                    
                    string fileName = Path.GetFileName(dllPath); // Get the file name without the path
                    dllNameLbl.Visible = true;
                    dllNameLbl.Text = fileName;
                }
            }
        }

        /// <summary>
        /// Handles the selection change event of the list box to update the selected process name.
        /// </summary>
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected item from the list box and split it to extract the process name
            S_ProcessName = listBox1.SelectedItem?.ToString().Split(new[] { " (ID: " }, StringSplitOptions.None)[0];
        }

    }
}


