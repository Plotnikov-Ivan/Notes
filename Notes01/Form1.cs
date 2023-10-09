using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Net.WebSockets;
using System.Windows.Forms;
using System.Xml;
using System.Drawing;
using System.IO;


namespace Notes01
{
    public partial class Form1 : Form
    {
        int index, font; // index - ���������, ������������ ��� ����������� ������� ������� ������ � ������ Edit, font - ������ ����������� ������
        private List<NotesEntity> noteslist; //��������� List ��������� �� �������� ������ NotesEntity ������������ ��� ������ � listbox ������
                                             //�� ������� ��� ������� ���������
        string noteText,chtext;  // ���������� noteText � ������� ������� � ���������� chtext � ���������� �������
        byte[] noteimage; //������ ��� �������� ��������
        private NotesDB ndb = new NotesDB(); //������ ������ NotesDB ��� ���������� ������� �������������� � ����� ������


        public Form1()
        {
            InitializeComponent();
            noteslist = new List<NotesEntity>();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            if (form2.ShowDialog() == DialogResult.OK)
            {
                noteText = form2.TextBoxValue;               
                noteimage = form2.imageBytes;

                NotesEntity ne = new NotesEntity(noteText, noteimage); //������������� ������� ��� ������� ������ NotesEntity
                if (ndb.AddNote(ne))
                {
                  listBox1.Items.Add(noteText); 
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string deltext = listBox1.SelectedItem.ToString();
                if(deltext != null) { ndb.DeleteNote(deltext); }              
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                chtext = listBox1.SelectedItem.ToString(); 

                Form2 form2 = new Form2();

                if (form2.ShowDialog() == DialogResult.OK)
                {
                    if (form2.TextBoxValue != null)
                    {
                        noteText = form2.TextBoxValue;
                        noteimage = form2.imageBytes;
                    }
                    if (chtext != null) { ndb.ChangeNote(chtext, noteimage, noteText); }
                    index = listBox1.SelectedIndex;
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                    listBox1.Items.Insert(index, form2.TextBoxValue);
                }
            }
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("    ����������\n ��� �������� ������ ��������� ������������ ������������ ������ ��� �������. ������ � ��������� �����������" +
                    "����������� 4 ������: �������� ������� ������������� ��������. \n ������ �������� ��������� ���� � ����� ��� ������, � ������� ������������" +
                    "������������ �������� ����� �������, ���� � �� ������� �������� \n ������ ������� ������� ��������� ������������� �������\n" +
                    " ������ ������������� ��������� ���� � ����� ��� ������, ������� ������� ���� � ��������� ������������ �������� ��������� �� �������\n " +
                    "������ �������� ��������� ������� ��������, ����������� �������������� � ������� � \"��������� ���������� Windows\" (���� ��� ���������� ����������� �� ����� ����������)." +
                    "\n ����� ������������ ����� �������� ������ ������ � �������� ����� ������ \"������ ������ ������\", ���� ������ �����");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            noteslist = ndb.GetAllNotes();
            foreach (var curr in noteslist) 
            {
                listBox1.Items.Add(curr.text);
            }
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                noteimage = ndb.GetNoteImage(listBox1.SelectedItem.ToString());
                if (noteimage != null)
                {
                    using (MemoryStream memoryStream = new MemoryStream(noteimage))
                    {
                        Bitmap bitmap = new Bitmap(memoryStream);
                        bitmap.Save("note_image.jpg", ImageFormat.Jpeg); // ��������� �������� � ����
                        Process.Start("explorer.exe", "note_image.jpg"); // ��������� ���� � ������� ������������ ���������� ��� ��������� ��������
                    }
                }
                else { MessageBox.Show("����������� ��������!"); }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Font = new Font(listBox1.Font, FontStyle.Bold);
            checkBox3.Checked = false;
            checkBox2.Checked = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Font = new Font(listBox1.Font, FontStyle.Italic);
            checkBox3.Checked = false;
            checkBox1.Checked = false;
        }

        private void checkBox3_CheckedChanged_1(object sender, EventArgs e)
        {
            checkBox2.Checked = false;
            checkBox1.Checked = false;
            listBox1.Font = new Font(listBox1.Font, FontStyle.Regular);
        }

        private void FontButton_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out font))
            {
                if (font >= 1 && font <= 40)
                {
                    listBox1.Font = new Font(listBox1.Font.FontFamily, font);
                }
            }
        }
    }
} 