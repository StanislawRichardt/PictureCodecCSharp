using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lista5
{
    public partial class Form1 : Form
    {
        byte[] byteTablePredicted;
        byte[] byteTableRaw;
        int width = 640;
        int height = 500;
        public Form1()
        {
            InitializeComponent();
        }

        //Open File
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenDialog = new OpenFileDialog();
            if (OpenDialog.ShowDialog() == DialogResult.OK)
            {
                byteTableRaw = File.ReadAllBytes(OpenDialog.FileName);
            }
        }

        //Draw raw picture
        private void button2_Click(object sender, EventArgs e)
        {
            drawingPicture(byteTableRaw);
        }

        //Predicting action
        private void button3_Click(object sender, EventArgs e)
        {
            byteTablePredicted = predictionCoding(byteTableRaw);
            drawingPicture(byteTablePredicted);
        }

        //Prediction algorithm
        private byte[] predictionCoding(byte[] byteTable)
        {
            Byte[] predictionData = new byte[byteTable.Length - 1];
            int temp;
            predictionData[0] = byteTable[0];
            for (int i = 1; i < byteTable.Length - 1; i++)
            {
                temp = (int)(byteTable[i]) - (int)(byteTable[i - 1]);
                if (temp < 0)
                {
                    predictionData[i] = (byte)(temp + 256);
                }
                else
                {
                    predictionData[i] = (byte)temp;
                }
            }
            return predictionData;
        }

        //Save file
        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog SaveDialog = new SaveFileDialog();
            if (SaveDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(SaveDialog.FileName, byteTablePredicted);
            }
        }

        //Decoding algorithm
        private byte[] decoding(byte[] byteTable)
        {
            byte[] byteTableAfterPrediction = new byte[byteTable.Length];
            byteTableAfterPrediction[0] = byteTable[0];
            int temp;
            for (int i = 1; i < byteTable.Length; i++)
            {
                temp = Convert.ToInt32(byteTableAfterPrediction[i - 1]) + Convert.ToInt32(byteTable[i]);
                if (temp < 0)
                {
                    byteTableAfterPrediction[i] = (byte)(temp + 256);
                }
                else if (temp > 256)
                {
                    byteTableAfterPrediction[i] = (byte)(temp - 256);
                }
                else
                {
                    byteTableAfterPrediction[i] = (byte)(temp);
                }
            }
            return byteTableAfterPrediction;
        }

        //Decoding display
        private void button5_Click(object sender, EventArgs e)
        {
            drawingPicture(decoding(byteTablePredicted));
        }

        //Drawing pictures
        private void drawingPicture(byte[] byteTable)
        {
            int position = 0;
            Bitmap picture = new Bitmap(width, height);
            pictureBox1.Image = picture;

            for (int i = 0; i < width - 1; i++)
            {
                for (int j = 0; j < height - 1; j++)
                {
                    position = byteTable[j * 640 + i];
                    picture.SetPixel(i, j, Color.FromArgb(position, position, position));
                }
            }
        }
    }
}

