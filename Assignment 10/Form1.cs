using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
/*=========================================================
* Resul Ucar
* rucar@purdue.edu
* CNIT155 Assignment10
* Lab Section: Fr. 03:30
* Program Description: 
*
* Academic Honesty: 
*	I attest that this is my original work.
*	I have not used unauthorized source code, either modified or unmodified.
*	I have not given other fellow student(s) access to my program.
*=========================================================== */
namespace Assignment_10
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private const int cSize = 100;
        private string[] mName = new string[cSize];
        private int[] mScore = new int[cSize];
        private bool[] mDean = new bool[cSize];
        private int mIndex = 0;
        private string mFileName = Path.Combine(Application.StartupPath, "Students.txt");
        
        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (ValidateInput() == false)
            {
                return;
            }
            string name = txtName.Text;
            int score = int.Parse(txtScore.Text);
            mName[mIndex] = name;
            mScore[mIndex] = score;
            if (chkDean.Checked == true)
            {
                mDean[mIndex] = true;
            }
            if (chkDean.Checked == false)
            {
                mDean[mIndex] = false;
            }
            mIndex++;
            if (mIndex == cSize)
            {
                DisplayMessage("The array is full!");
                btnEnter.Enabled = false;
            }
            ClearInput();
        }
        private void ClearInput()
        {
            txtName.Clear();
            txtScore.Clear();
            chkDean.Checked = false;
            txtName.Focus();
            int index = lstOut.SelectedIndex;
        }
        private void DisplayMessage(String msg)
        {
            MessageBox.Show(msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private bool ValidateInput()
        {
            string name = txtName.Text;
            int score;
            if (name == " ")
            {
                DisplayMessage("Please enter a name");
                txtName.Focus();
                return false;
            }
            if(int.TryParse(txtScore.Text, out score)==false)
            {
                DisplayMessage("Please enter a whole number");
                txtScore.Focus();
                return false;
            }
            if(score<0||score>100)
            {
                DisplayMessage("Please enter a score between 0 to 100");
                txtScore.Focus();
                return false;
            }

            return true;
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            if (mIndex == 0)
            {
                DisplayMessage("The array is empty!");
                return;
            }
            lstOut.Items.Clear();
            int ctr;
            lstOut.Items.Add("Name".PadRight(20) + "Score".PadRight(10) + "Dean's List");
            lstOut.Items.Add("=============================================================");
            for (ctr = 0; ctr < mIndex; ctr++)
            {
                lstOut.Items.Add(mName[ctr].PadRight(20) + mScore[ctr].ToString().PadRight(10)+mDean[ctr]);
            }
        }
       

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you wish to end the program?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            StreamWriter SW = null;
            try
            {
                SW = new StreamWriter(mFileName);
                int ctr;
                for (ctr = 0; ctr < mIndex; ctr++)
                {
                    SW.WriteLine((ctr + 1) + "\t" + mName[ctr] + "\t" + mScore[ctr] + "\t" + mDean[ctr]);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                {
                    if (SW != null)
                        SW.Close();
                }

            }


        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInput();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lstOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lstOut.SelectedIndex <2)
            {
                return;
            }
            int index = lstOut.SelectedIndex - 2;
            txtName.Text = mName[index];
            txtScore.Text = mScore[index].ToString();
            if (mDean[index] == false)
            {
                chkDean.Checked = false;
            }
            else
                chkDean.Checked = true;
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            if (mIndex == 0)
            {
                DisplayMessage("The arrays are empty.");
                return;
            }
            if (radAsc.Checked == false && radDesc.Checked == false)
            {
                DisplayMessage("Please select a sort option.");
                return;
            }

            lstOut.Items.Clear();

            int[] Score1 = new int[cSize];
            for(int count = 0; count <mIndex; count++)
            {
                Score1[count] = mScore[count];
            }
            Array.Sort(mScore, mName, 0, mIndex);
            Array.Sort(Score1, mDean, 0, mIndex);
            if (radDesc.Checked==true)
            {
                Array.Reverse(mName, 0, mIndex);
                Array.Reverse(mScore, 0, mIndex);
                Array.Reverse(mDean, 0, mIndex);
            }

            lstOut.Items.Add("Name".PadRight(10) + "Score".PadRight(10) + "Dean's List");
            lstOut.Items.Add("=================================================================");
            for (int count = 0; count < mIndex; count++)
            { 
                lstOut.Items.Add(mName[count].PadRight(10) + mScore[count].ToString().PadRight(10) + mDean[count].ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(mFileName) == false)

            {

                DisplayMessage("The file does not exist.");

                return;

            }

            StreamReader SR = null;

            try

            {

                SR = new StreamReader(mFileName);

                while (SR.Peek() != -1)

                {

                    string line = SR.ReadLine();

                    string[] parts = line.Split('\t');

                    mName[mIndex] = parts[1];

                    mScore[mIndex] = int.Parse(parts[2]);

                    if (parts[3] == "True")

                    {

                        mDean[mIndex] = true;

                    }

                    else

                    {

                        mDean[mIndex] = false;

                    }

                    mIndex++;

                }

            }

            catch (Exception ex)

            {

                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            finally

            {

                SR.Close();

            }
        }
    }
}
