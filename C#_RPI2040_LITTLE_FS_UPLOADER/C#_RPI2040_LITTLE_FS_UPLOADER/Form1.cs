using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C__RPI2040_LITTLE_FS_UPLOADER
{
    public partial class Form1 : Form
    {
        string file_path = "";
        byte count_delete_button = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            richTextBox1.BackColor = Color.LightBlue;
            textBox1.BackColor = Color.LightBlue;
            file_path = Application.ExecutablePath; // get *.exe path from start
            button2.Enabled = false;
            button3.Enabled = false;
            button11.Enabled = false;
            button10.Enabled = false;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //richTextBox1.BackColor = SystemColors.Window;
            if (serialPort1.IsOpen)
            { richTextBox1.BackColor = Color.LimeGreen;
                textBox1.BackColor = Color.LimeGreen;
            }
            else
            { richTextBox1.BackColor = Color.LightBlue;
                textBox1.BackColor = Color.LightBlue;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {           // BUTTON CHECK SERIAL PORTS
            string[] ports = SerialPort.GetPortNames();
            richTextBox1.AppendText("rs232 ports found\n\n");
            comboBox1.Items.Clear();

            foreach (string port in ports)
            {
                richTextBox1.AppendText(port + " is active\n");
                comboBox1.Items.Add(port);
                comboBox1.SelectedIndex = 0;
            }
            var count = comboBox1.Items.Count;
             //richTextBox1.AppendText(count + " serial port are active\n");

            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                richTextBox1.AppendText("NO ACTIVE SERIAL PORT\n");
            }
            else
            {
                //button1.Enabled = false; // button connect
                button2.Enabled = true; // button connect
                button3.Enabled = false;// button disconnect
                richTextBox1.AppendText(count + " serial port are active\n");
                //MessageBox.Show("Item Selected is:" + comboBox1.Text);
            }
                richTextBox1.ScrollToCaret();
        }

        private void button2_Click(object sender, EventArgs e)
        {           // BUTTON CONNECT TO SERIAL
            serialPort1.PortName = comboBox1.SelectedItem.ToString();
            try
            {
                if (!(serialPort1.IsOpen))
                {
                    //serialPort1.ReadTimeout = 1000;
                    //serialPort1.WriteTimeout = 1000;
                  // IMPORTANT RECEIVE FROM USB ARDUINO/PICO IN C# NEED THIS FLAGS LIKE TRUE !!!!
                    serialPort1.RtsEnable = true; // IMPORTANT RECEIVE FROM USB ARDUINO/PICO !!!!
                    serialPort1.DtrEnable = true; // IMPORTANT RECEIVE FROM USB ARDUINO/PICO !!!!
                    serialPort1.Open();
                   
                    button1.Enabled = false; // button check serial
                    button2.Enabled = false; // button connect
                    button3.Enabled = true;// button disconnect

             richTextBox1.AppendText(serialPort1.PortName.ToString() + " is CONNECTED" + "\n");
             richTextBox1.AppendText("Speed = 9600, N, 1 standart setting \n");
                }
            }
            catch (Exception ex)
            {
               richTextBox1.AppendText("\nserial port already open >> " + ex.Message + "\n\n");
            }
                  richTextBox1.ScrollToCaret();
        }
        //======================================================================
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {       // SERIAL DATA RECEIVED EVENT IF SERIAL RECEIVE DATA FOM PICO
           // string[] serial_read = new string[4096];
            string serial_read = "";
            //uint receive_count = 0;
         
            serial_read = serialPort1.ReadExisting();
            richTextBox1.AppendText(serial_read);
            //while (receive_count < serial_read.Length)
            //{
            if (serial_read == "\n")
            { serialPort1.DiscardInBuffer(); }

            //richTextBox1.AppendText("\n");
            serial_read = "";

            //receive_count++; 
            //}

            //if (serial_read == "\0")
            // { serialPort1.DiscardInBuffer(); }

            // richTextBox1.AppendText("\n");
            // richTextBox1.AppendText(">>> " + serial_read);
            //richTextBox1.AppendText(serial_read);

            richTextBox1.ScrollToCaret();
           


        }
        //======================================================================
        private void button3_Click(object sender, EventArgs e)
        {        // BUTTON DISCONNECT FROM SERIAL
            try
            {    //serialPort1.DtrEnable = false;
                //serialPort1.RtsEnable = false;
                serialPort1.DiscardInBuffer();
                serialPort1.DiscardOutBuffer();
                serialPort1.Close(); serialPort1.Dispose();
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(serialPort1.PortName.ToString() + " is CLOSED" + "\n");
                richTextBox1.ScrollToCaret();
            }

            var items = comboBox1.Items.Count;
            while (items > 0)
            { comboBox1.Items.Clear(); items--; }

            button1.Enabled = true; // button check serial
            button2.Enabled = false; // button connect
            button3.Enabled = true;// button disconnect
            button1.PerformClick();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string read_textbox = textBox1.Text;
            if (read_textbox != "")
            { richTextBox1.AppendText(read_textbox + "\n"); }
            else
            { richTextBox1.AppendText("text box is empty\n"); }

            if (serialPort1.IsOpen)
            {
                richTextBox1.AppendText("<<<" + read_textbox + "\n");
                richTextBox1.ScrollToCaret();
                serialPort1.WriteLine(read_textbox);
            }
            else
            {
                richTextBox1.AppendText("serial is not open\n");
                richTextBox1.ScrollToCaret();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {       // BUTTON CLEAR MEMO
            richTextBox1.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {       // BUTTON OPEN FILE
            int currentMessage = 0;
            int lines_plus = 0;
            uint lines_count = 0;
            uint file_size = 0;
            bool isfile_open = false;
            // Create an OpenFileDialog to request a file to open.
            OpenFileDialog openFile1 = new OpenFileDialog();

            // Initialize the OpenFileDialog to look for RTF files.
            openFile1.DefaultExt = "*.txt";
            openFile1.Filter = "TXT Files|*.txt";

            // Determine whether the user selected a file from the OpenFileDialog.
            if (openFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
               openFile1.FileName.Length > 0)
            {
                file_path = openFile1.FileName;
                // Load the contents of the file into the RichTextBox.
                richTextBox1.LoadFile(openFile1.FileName, RichTextBoxStreamType.PlainText);


                 lines_count = (uint)File.ReadLines(file_path).Count();
                 file_size = (uint)File.OpenText(file_path).ReadToEnd().Count();

                richTextBox1.AppendText("\n\nlines = " + lines_count.ToString() + "\n");
                richTextBox1.AppendText("file_size = " + file_size.ToString() + " in bytes\n");

                richTextBox1.AppendText("\n\nFILE IS LOADED\n");
                isfile_open = true;
            }
            else
            {
                isfile_open = false;
            }

            if (isfile_open == true)
            {   // check file size for more then 128kb
                if (file_size > 0 && file_size <= 131072)
                {
                    char[] file_read_buffer = new char[file_size];
                    richTextBox1.AppendText("file_read_buffer loaded with files size\n");
                    isfile_open = false;
                    button11.Enabled = true;
                }
                else
                {
                    richTextBox1.AppendText("file is too big from 128kb\n");
                    richTextBox1.AppendText("reduce size of file and load again\n");
                    richTextBox1.AppendText("char buffer is empty\n");
                    isfile_open = false;
                }
            }
            else
            {
                richTextBox1.AppendText("\nFILE NOT LOADED\n");
            }
                richTextBox1.ScrollToCaret();
        }

        private void button9_Click(object sender, EventArgs e)
        {               // BUTON SAVE FILE AS
            
            richTextBox1.AppendText("\n");
            richTextBox1.ScrollToCaret();
            SaveFileDialog saveFile1 = new SaveFileDialog();

            saveFile1.Filter = "Text Files (*.txt)|*.txt";
            saveFile1.AddExtension = true;

            if (saveFile1.ShowDialog() == DialogResult.OK)
            {
                file_path = saveFile1.FileName;
                richTextBox1.SaveFile(file_path, RichTextBoxStreamType.PlainText);
                richTextBox1.AppendText("\nFILE IS SAVE\n");
                button10.Enabled = true;
            }
            else
            {
                richTextBox1.AppendText("\nFILE IS NOT SAVE\n");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {       //SAVE FILE
            richTextBox1.AppendText("\n");
            richTextBox1.ScrollToCaret();

            try
            {
                richTextBox1.ScrollToCaret();
                richTextBox1.SaveFile(file_path, RichTextBoxStreamType.PlainText);
                richTextBox1.AppendText("\n\nFILE IS SAVE\n");
                richTextBox1.ScrollToCaret();
            }
            catch
            {
                richTextBox1.AppendText("\nFILE IS NOT SAVE\n");
                richTextBox1.ScrollToCaret();
            }
           // richTextBox1.ScrollToCaret();
        }

        private void button8_Click(object sender, EventArgs e)
        {         // BUTTON BUILDLEDON for test communication with pico
            if (serialPort1.IsOpen)
            {
                richTextBox1.AppendText("<<< BUILDLEDON\n");
                richTextBox1.ScrollToCaret();
                serialPort1.WriteLine("BUILDLEDON");
            }
            else
            {
                richTextBox1.AppendText("serial is not open\n");
                richTextBox1.ScrollToCaret();
            }
            richTextBox1.ScrollToCaret();
        }

        private void button7_Click(object sender, EventArgs e)
        {// BUTTON BUILDLEDOFF for test communication with pico

            if (serialPort1.IsOpen)
            {
                richTextBox1.AppendText("<<< BUILDLEDOFF\n");
                richTextBox1.ScrollToCaret();
                serialPort1.WriteLine("BUILDLEDOFF");
            }
            else
            {
                richTextBox1.AppendText("serial is not open\n");
                richTextBox1.ScrollToCaret();
            }
            richTextBox1.ScrollToCaret();
        }
       
        private void button12_Click(object sender, EventArgs e)
        {
            // BUTTON LIST DIRECTORY send command READFILE to pico FS
            if (serialPort1.IsOpen)
            {
                richTextBox1.AppendText("<<< LISTDIR\n");
                richTextBox1.ScrollToCaret();
                serialPort1.WriteLine("LISTDIR");
            }
            else
            {
                richTextBox1.AppendText("serial is not open\n");
                richTextBox1.ScrollToCaret();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {// BUTTON READFILE send command READFILE to pico FS
            string send_read_file;
            string command_read_file;
            string command_read_send;

            if (textBox2.Text == "")
            {
                richTextBox1.AppendText("\nFILE FIELD IS EMPTY\n");
                richTextBox1.AppendText("TYPEE A NAME OF FILE\n");
                richTextBox1.AppendText("EXAMPLE : data.txt\n");
                richTextBox1.AppendText("OR USE LIST DIR BUTTON\n");
            }
            else
            {
                 send_read_file = textBox2.Text;
                 command_read_file = "READFILE";
                command_read_send = command_read_file + send_read_file;
               
                if (serialPort1.IsOpen)
                {
                    richTextBox1.AppendText("\n<<< READFILE + " + send_read_file + "\n");
                    richTextBox1.ScrollToCaret();
                    serialPort1.WriteLine(command_read_send);
                }
                else
                {
                    richTextBox1.AppendText("serial is not open\n");
                    richTextBox1.ScrollToCaret();
                }
            }
            richTextBox1.ScrollToCaret();
            send_read_file = ""; command_read_send = "";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // BUTTON WRITEFILE send command READFILE to pico FS
            string send_write_file;
            string command_write_file;
            string command_write_send;
           
            int lines_count = File.ReadLines(file_path).Count();
            int file_size = File.OpenText(file_path).ReadToEnd().Count();
            
            string[] writeText = new string[lines_count];
            writeText = File.ReadAllLines(file_path);

            if (textBox2.Text == "")
            {
                richTextBox1.AppendText("\nUSE LIST DIR TO SEE\n");
                richTextBox1.AppendText("EXISTING FILES\n");
                richTextBox1.AppendText("FILE FIELD IS EMPTY\n");
                richTextBox1.AppendText("TYPE A NAME OF FILE\n");
                richTextBox1.AppendText("EXAMPLE : data.txt\n");
                richTextBox1.AppendText("DO NOT GIVE EXISTING NAME\n");
                richTextBox1.AppendText("OR OLD DATA IN FILE WILL BE LOST\n");
            }
            else
            {
                send_write_file = textBox2.Text;
                command_write_file = "WRITEFILE";
                command_write_send = command_write_file + send_write_file;

                if (serialPort1.IsOpen)
                {
                    serialPort1.WriteLine(command_write_send);
                    // readText = File.ReadAllLines(Init.file_path);
                    richTextBox1.AppendText("SEND FILE STARTED\n");
                    richTextBox1.AppendText("lines = " + lines_count.ToString() + "\n");
                    richTextBox1.AppendText("file_size = " + file_size.ToString() + "\n");

                    serialPort1.ReadLine();
                    foreach (string data_send in writeText)
                    {
                        richTextBox1.AppendText(data_send + "\n");
                        serialPort1.WriteLine(data_send);
                    }
                    richTextBox1.ScrollToCaret();
                }
                else
                {
                    richTextBox1.AppendText("serial is not open\n");
                    richTextBox1.ScrollToCaret();
                }
            }
            richTextBox1.ScrollToCaret();
            send_write_file = ""; command_write_send = "";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            // BUTTON DELETEFILE send command READFILE to pico FS
            string send_read_file;
            string command_read_file;
            string command_read_send;
            
            richTextBox1.AppendText("\nFILE WILL BE DELETED !!!\n");
            richTextBox1.AppendText("IF YOU SURE PRESS ONCE AGIAN !!!\n");
            richTextBox1.AppendText("NAME OF FILE WILL BE DELETED !!!\n");
            count_delete_button++;

            if (textBox2.Text == "")
            {
                richTextBox1.AppendText("\nUSE LIST DIR TO SEE\n");
                richTextBox1.AppendText("EXISTING FILES\n");
                richTextBox1.AppendText("FILE FIELD IS EMPTY\n");
                richTextBox1.AppendText("TYPE A NAME OF FILE\n");
                richTextBox1.AppendText("EXAMPLE : data.txt\n");
                richTextBox1.AppendText("ONCE YOU PRESS DELETE\n");
                richTextBox1.AppendText("FILE WILL BE DELETED !!!\n");
            }
            else if(count_delete_button == 2 && textBox2.Text != "")
            {
                send_read_file = textBox2.Text;
                command_read_file = "DELETEFILE";
                command_read_send = command_read_file + send_read_file;

                if (serialPort1.IsOpen)
                {
                    richTextBox1.AppendText("<<< DELETEFILE + " + send_read_file + "\n");
                    richTextBox1.ScrollToCaret();
                    serialPort1.WriteLine(command_read_send);
                }
                else
                {
                    richTextBox1.AppendText("serial is not open\n");
                    richTextBox1.ScrollToCaret();
                }
            }
            richTextBox1.ScrollToCaret();
            send_read_file = ""; command_read_send = "";
            count_delete_button = 0;
        }

    }
}
