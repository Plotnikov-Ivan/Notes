using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notes01
{
    public partial class Form2 : Form
    {
        public string dateString { get; set; } //поле для даты с типом данных string
        public string TextBoxValue // поле с содержимым textBox
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }
        public byte[] imageBytes { get; set; } // массив для сохранения картинки
        DateTime selectedDate; // переменная для хранения выбранной даты



        public Form2()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();

        }

        private void AcceptButtton_Click(object sender, EventArgs e)
        {
            
            selectedDate = dateTimePicker1.Value;
            dateString = selectedDate.ToString("dd-MM-yyyy");
            TextBoxValue = dateString + " | " + textBox1.Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png, *.jpg, *.jpeg, )|*.png;*.jpg;*.jpeg;";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {

                string filePath = openFileDialog.FileName;

                Image image = Image.FromFile(filePath);


                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, image.RawFormat);
                    imageBytes = ms.ToArray();
                }
                label1.Text = "Картинка загружена!";
                image.Dispose();
            }
        }

    }
}
