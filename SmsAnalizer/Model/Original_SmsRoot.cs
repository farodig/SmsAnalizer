using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmsAnalizer.Model
{
    /// <summary>
    /// Корень дерева смс
    /// </summary>
    [Serializable]
    [XmlRoot("smses")]
    public class Original_SmsRoot
    {
        /// <summary>
        /// Список смс
        /// </summary>
        [XmlElement("sms")]
        public List<Original_SmsItem> SmsList { get; set; }
    }
}
