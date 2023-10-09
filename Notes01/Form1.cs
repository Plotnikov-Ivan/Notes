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
        int index, font; // index - пременная, использующая для обозначения индекса текущей строки в методе Edit, font - размер задаваемого шрифта
        private List<NotesEntity> noteslist; //коллекция List состоящая из объектов класса NotesEntity используется для вывода в listbox данных
                                             //из таблицы при запуске программы
        string noteText,chtext;  // переменная noteText с текстом заметки и переменная chtext с измененным текстом
        byte[] noteimage; //массив для хранения картинки
        private NotesDB ndb = new NotesDB(); //объект класса NotesDB для реализации методов взаимодействия с базой данных


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

                NotesEntity ne = new NotesEntity(noteText, noteimage); //инициализация заметки как объекта класса NotesEntity
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
            MessageBox.Show("    ИНСТРУКЦИЯ\n При открытии данной программы пользователю показывается список его заметок. Работа с заметками реализована" +
                    "посредством 4 кнопок: Добавить Удалить Редактировать Просмотр. \n Кнопка Добавить открывает окно с полем для текста, в котором пользователю" +
                    "предлагается добавить текст заметки, дату и по желанию картинку \n Кнопка Удалить удаляет выбранную пользователем заметку\n" +
                    " Кнопка Редактировать открывает окно с полем для текста, которое описано выше и позволяет пользователю изменить выбранную им заметку\n " +
                    "Кнопка просмотр позволяет увидеть картинку, загруженную пользоваьтелем в заметку в \"Просмотре фотографий Windows\" (если это приложение установлено на вашем компьютере)." +
                    "\n Также пользователь может изменять размер текста в заметках через кнопку \"Задать размер текста\", если укажет число");
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
                        bitmap.Save("note_image.jpg", ImageFormat.Jpeg); // сохраняем картинку в файл
                        Process.Start("explorer.exe", "note_image.jpg"); // открываем файл с помощью стандартного приложения для просмотра картинок
                    }
                }
                else { MessageBox.Show("Отсутствует картинка!"); }
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