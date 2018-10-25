using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmsAnalizer.Model
{
    /// <summary>
    /// СМС оригинальная
    /// </summary>
    [Serializable]
    [XmlRoot("sms")]
    public class Original_SmsItem
    {
        /// <summary>
        /// Тип смс
        /// 1 - принято
        /// 2 - отправлено
        /// </summary>
        [XmlAttribute("type")]
        public int Type { get; set; }

        /// <summary>
        /// Дата и время (unix postfix)
        /// </summary>
        [XmlAttribute("date")]
        public long DateInt { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        [XmlAttribute("body")]
        public string Text { get; set; }
    }
}
