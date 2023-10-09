using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes01
{
    public class NotesEntity
    {

        public byte[]? image { get; set; } //поле: картинка
        [Key]
        public string text { get; set; } //текст являющийся первичным ключом

        public NotesEntity(string text, byte[]? image) //конструктор класса
        {
            this.text = text;
            if (image != null) { this.image = image; }
            
        }
    }
}
