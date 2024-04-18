namespace ByteZone_Injector
{
    partial class ByteZone_Injector
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ByteZone_Injector));
            label1 = new Label();
            comboBox1 = new ComboBox();
            button1 = new Button();
            label2 = new Label();
            button2 = new Button();
            dllNameLbl = new Label();
            label3 = new Label();
            listView1 = new ListView();
            imageList1 = new ImageList(components);
            sProcessLbl = new Label();
            panel1 = new Panel();
            label4 = new Label();
            panel2 = new Panel();
            wLbl = new Label();
            closeAfterInjectCheckBox = new CheckBox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.ForeColor = Color.White;
            label1.Name = "label1";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { resources.GetString("comboBox1.Items"), resources.GetString("comboBox1.Items1"), resources.GetString("comboBox1.Items2"), resources.GetString("comboBox1.Items3"), resources.GetString("comboBox1.Items4") });
            resources.ApplyResources(comboBox1, "comboBox1");
            comboBox1.Name = "comboBox1";
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(20, 106, 94);
            button1.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(button1, "button1");
            button1.ForeColor = Color.White;
            button1.Name = "button1";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.ForeColor = Color.White;
            label2.Name = "label2";
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(20, 106, 94);
            button2.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(button2, "button2");
            button2.ForeColor = Color.White;
            button2.Name = "button2";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // dllNameLbl
            // 
            resources.ApplyResources(dllNameLbl, "dllNameLbl");
            dllNameLbl.ForeColor = Color.White;
            dllNameLbl.Name = "dllNameLbl";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.ForeColor = Color.White;
            label3.Name = "label3";
            // 
            // listView1
            // 
            listView1.BackColor = Color.FromArgb(35, 44, 51);
            listView1.ForeColor = Color.White;
            resources.ApplyResources(listView1, "listView1");
            listView1.Name = "listView1";
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.SmallIcon;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            resources.ApplyResources(imageList1, "imageList1");
            imageList1.TransparentColor = Color.Transparent;
            // 
            // sProcessLbl
            // 
            resources.ApplyResources(sProcessLbl, "sProcessLbl");
            sProcessLbl.ForeColor = Color.White;
            sProcessLbl.Name = "sProcessLbl";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(35, 44, 51);
            panel1.Controls.Add(dllNameLbl);
            panel1.Controls.Add(sProcessLbl);
            resources.ApplyResources(panel1, "panel1");
            panel1.Name = "panel1";
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.ForeColor = Color.White;
            label4.Name = "label4";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(35, 44, 51);
            panel2.Controls.Add(wLbl);
            panel2.Controls.Add(closeAfterInjectCheckBox);
            panel2.Controls.Add(comboBox1);
            panel2.Controls.Add(label1);
            resources.ApplyResources(panel2, "panel2");
            panel2.Name = "panel2";
            panel2.Paint += panel2_Paint;
            // 
            // wLbl
            // 
            resources.ApplyResources(wLbl, "wLbl");
            wLbl.ForeColor = Color.Red;
            wLbl.Name = "wLbl";
            // 
            // closeAfterInjectCheckBox
            // 
            resources.ApplyResources(closeAfterInjectCheckBox, "closeAfterInjectCheckBox");
            closeAfterInjectCheckBox.ForeColor = Color.White;
            closeAfterInjectCheckBox.Name = "closeAfterInjectCheckBox";
            closeAfterInjectCheckBox.UseVisualStyleBackColor = true;
            // 
            // ByteZone_Injector
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(34, 41, 49);
            Controls.Add(panel2);
            Controls.Add(label4);
            Controls.Add(panel1);
            Controls.Add(listView1);
            Controls.Add(label3);
            Controls.Add(button2);
            Controls.Add(label2);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "ByteZone_Injector";
            Opacity = 0.88D;
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox comboBox1;
        private Button button1;
        private Label label2;
        private Button button2;
        private Label dllNameLbl;
        private Label label3;
        private ListView listView1;
        private ImageList imageList1;
        private Label sProcessLbl;
        private Panel panel1;
        private Label label4;
        private Panel panel2;
        private CheckBox closeAfterInjectCheckBox;
        private Label wLbl;
    }
}
