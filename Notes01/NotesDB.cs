using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes01
{
    public class NotesDB
    {

        public bool AddNote(NotesEntity item) //метод позволяющий добавлять замтеку в таблицу с проверкой на уникальность текста
        {
            using (NotesInitDB db = new NotesInitDB())
            {
                bool exists = db.Notes.Any(n => n.text == item.text);
                if (exists)
                {
                    return false;
                }
                else
                {
                    db.Notes.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
        }

        public void DeleteNote(string text) //метод удаления выбраннной заметки из таблицы
        {
            List<NotesEntity> NtEntities = GetAllNotes();
            using (NotesInitDB db = new NotesInitDB())
            {
                foreach (var curr in NtEntities)
                {
                    if (text == curr.text)
                    {
                        db.Notes.Remove(curr);
                        db.SaveChanges();
                    }
                }
            }
        }

        public List<NotesEntity> GetAllNotes() //метод полученния всех заметок в коллекции list
        {
            using (NotesInitDB db = new NotesInitDB())
            {
                return db.Notes.ToList();
            }
        }

        public bool ChangeNote(string text, byte[]? image, string ntext) //метод изменения заметки text - старый текст заметки
        {                                                               //ntext - новый текст заметки
            using (NotesInitDB db = new NotesInitDB())                  // метод также имеет проверку на уникальность
            {
                bool exists = db.Notes.Any(n => n.text == ntext);
                if (exists)
                {
                    return false;
                }
                else
                {
                    NotesEntity NtEntity = db.Notes.Find(text);
                    if (NtEntity != null)
                    {
                        db.Notes.Remove(NtEntity); // Удаляем старую сущность
                        db.SaveChanges();

                        // Создаем новую сущность с новыми значениями
                        NotesEntity newNtEntity = new NotesEntity(ntext, image);
                        db.Notes.Add(newNtEntity);
                        db.SaveChanges();
                    }
                }
                return true;
            }
        }

        public byte[] GetNoteImage(string text)  // метод ищет среди строк таблицы строку с ключом text
        {                                       //  после чего возвращает из этой строки картинку как массив byte[]
            using (NotesInitDB db = new NotesInitDB())
            {
                NotesEntity note = db.Notes.Find(text);
                if (note != null)
                {
                    return note.image;
                }
                else
                {
                    return null;
                }
            }
        }


    }
}
