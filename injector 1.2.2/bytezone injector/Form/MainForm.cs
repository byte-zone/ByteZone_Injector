using System.Diagnostics;
using System.Drawing;
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
        private string dllBuffer;
        /// <summary>
        /// Instance of the InjectionMethods class used for performing injections.
        /// </summary>
        private InjectionMethods I_Method;

        private ImageList imageList = new ImageList();
        public ByteZone_Injector()
        {
            InitializeComponent();
            wLbl.Visible = false;
            I_Method = new InjectionMethods();
        }


        /// <summary>
        /// Retrieves a list of currently running processes and populates the process list.
        /// </summary>
        private void GetProcesses()
        {
            Process[] processes = Process.GetProcesses(); // Get a list of all running processes
            foreach (Process process in processes)
            {
                try
                {
                    if (process.MainModule != null)
                    {
                        Icon processIcon = Icon.ExtractAssociatedIcon(process.MainModule.FileName);
                        imageList.Images.Add(process.MainModule.FileName, processIcon); // Access the Images property of the imageList object
                        ListViewItem item = new ListViewItem(process.ProcessName, process.MainModule.FileName);
                        listView1.Items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    // Some of windows processes does not have icon this will through exception foreach process he can't get the icon so leave it
                    //MessageBox.Show($"Error extracting icon for process {process.ProcessName}: {ex.Message}");
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.SmallImageList = imageList;
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            Thread thread = new Thread(GetProcesses); // Call GetProcesses in new thread to improve performance 
            thread.Start(); // Start the thread 


            //GetProcesses(); // Old method take a while until load the injector 
            comboBox1.SelectedIndex = 0;
        }



        private void button1_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(S_ProcessName) && !string.IsNullOrEmpty(dllPath))
            {
                // Check the ComboBox selection and call the corresponding injection method
                if (comboBox1.SelectedIndex == 0)
                {
                    I_Method.NtNativeInjection(S_ProcessName, dllPath);
                    if (closeAfterInjectCheckBox.Checked)
                    {
                        Environment.Exit(0); // If Close after injection checked Call env exit
                    }
                }

                else if (comboBox1.SelectedIndex == 1)
                {
                    I_Method.NtManualMapInjection(S_ProcessName, dllPath);
                    if (closeAfterInjectCheckBox.Checked)
                    {
                        Environment.Exit(0); // If Close after injection checked Call env exit
                    }
                }

                else if (comboBox1.SelectedIndex == 2)
                {
                    // Kernel 
                }

                else if (comboBox1.SelectedIndex == 3)
                {
                    I_Method.NtThreadHijacking(S_ProcessName, dllPath);

                    if (closeAfterInjectCheckBox.Checked) // If Close after injection checked Call env exit
                    {
                        Environment.Exit(0);
                    }
                }

                else if (comboBox1.SelectedIndex == 4)
                {
                    I_Method.NtLdrpLoadDll(S_ProcessName, dllPath);

                    if (closeAfterInjectCheckBox.Checked) // If CLose after injection checked Call env exit
                    {
                        Environment.Exit(0);
                    }
                }

            }
            else
            {
                MessageBox.Show("Please select a process and a DLL file first.");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 4)
            {
                wLbl.Visible = true;
            }
            else
            {
                wLbl.Visible = false;
            }
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
                    dllNameLbl.Text = "Selected module: " + fileName;
                }
            }
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Handles the selection change event of the list box to update the selected process name.
        /// </summary>
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];
                S_ProcessName = selectedItem.SubItems[0].Text;
                label9.Text = "Selected Process: " + selectedItem.SubItems[0].Text;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2GroupBox3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];
                S_ProcessName = selectedItem.SubItems[0].Text;
                label9.Text = "Selected Process: " + selectedItem.SubItems[0].Text;
            }
        }

        private void guna2GradientTileButton1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(S_ProcessName) && !string.IsNullOrEmpty(dllPath))
            {
                // Check the ComboBox selection and call the corresponding injection method
                if (comboBox1.SelectedIndex == 0)
                {
                    I_Method.NtNativeInjection(S_ProcessName, dllPath);
                    if (closeAfterInjectCheckBox.Checked)
                    {
                        Environment.Exit(0); // If Close after injection checked Call env exit
                    }
                }

                else if (comboBox1.SelectedIndex == 1)
                {
                    I_Method.NtManualMapInjection(S_ProcessName, dllPath);
                    if (closeAfterInjectCheckBox.Checked)
                    {
                        Environment.Exit(0); // If Close after injection checked Call env exit
                    }
                }

                else if (comboBox1.SelectedIndex == 2)
                {
                    // Kernel 
                }

                else if (comboBox1.SelectedIndex == 3)
                {
                    I_Method.NtThreadHijacking(S_ProcessName, dllPath);

                    if (closeAfterInjectCheckBox.Checked) // If Close after injection checked Call env exit
                    {
                        Environment.Exit(0);
                    }
                }

                else if (comboBox1.SelectedIndex == 4)
                {
                    I_Method.NtLdrpLoadDll(S_ProcessName, dllPath);

                    if (closeAfterInjectCheckBox.Checked) // If CLose after injection checked Call env exit
                    {
                        Environment.Exit(0);
                    }
                }

            }
            else
            {
                MessageBox.Show("Please select a process and a DLL file first.");
            }
        }

        private void guna2GradientTileButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.InitialDirectory = Environment.CurrentDirectory; // Set the initial directory to the current director

            ofd.Filter = "dll files (*.dll) | *.dll"; // Filter to display only DLL files

            // If the user selects a file and clicks OK
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                dllPath = ofd.FileName; // Get the selected file path


                if (!string.IsNullOrEmpty(dllPath)) // If the file path is not empty
                {

                    string fileName = Path.GetFileName(dllPath); // Get the file name without the path
                    dllNameLbl.Text = "Selected module: " + fileName;
                }
            }
        }

        private void sProcessLbl_Click(object sender, EventArgs e)
        {

        }

        private void wLbl_Click(object sender, EventArgs e)
        {

        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex == 4)
            {
                wLbl.Visible = true;
            }
            else
            {
                wLbl.Visible = false;
            }
        }

        private void guna2GradientTileButton3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2GradientTileButton4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void closeAfterInjectCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void guna2GradientTileButton6_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.InitialDirectory = Environment.CurrentDirectory; // Set the initial directory to the current director

            ofd.Filter = "dll files (*.dll) | *.dll"; // Filter to display only DLL files

            // If the user selects a file and clicks OK
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                dllPath = ofd.FileName; // Get the selected file path


                if (!string.IsNullOrEmpty(dllPath)) // If the file path is not empty
                {

                    string fileName = Path.GetFileName(dllPath); // Get the file name without the path
                    dllNameLbl.Text = "Selected module: " + fileName;
                }
            }
        }
    }
}



