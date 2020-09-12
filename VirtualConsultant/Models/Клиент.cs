using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VirtualConsultant.Models
{
    public class Клиент
    {
		[Key]
		public Guid primaryKey { get; set; }
		public string Имя { get; set; }
		public string ДатаРождения { get; set; }
		public string Пол { get; set; }
		public long TelegramGuid { get; set; }
		public string CurrentNodeId { get; set; }
	}
}
