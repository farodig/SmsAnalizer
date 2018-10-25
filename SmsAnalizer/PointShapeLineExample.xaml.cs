using LiveCharts;
using LiveCharts.Wpf;
using SmsAnalizer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace SmsAnalizer
{
    /// <summary>
    /// Логика взаимодействия для PointShapeLineExample.xaml
    /// </summary>
    public partial class PointShapeLineExample : UserControl
    {

        // Графики
        public SeriesCollection SeriesCollection { get; set; }

        // Ось x
        public string[] Labels { get; set; }

        /// <summary>
        /// Инициализация графиков
        /// </summary>
        public PointShapeLineExample()
        {
            InitializeComponent();
            
            // Файл экспорта смс с номера 900 из программы SMS Backup & Restore
            var path = @"sms-2018-10-19 12-19-02.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(Original_SmsRoot));

            ChartValues<decimal> data;
            ChartValues<decimal> balance;
            using (StreamReader streamReader = new StreamReader(path))
            {
                var xmlData = (Original_SmsRoot)serializer.Deserialize(streamReader);
                //var firstRange = new DateTime(2018, 1, 1);
                //var lastRange = new DateTime(2019, 1, 1);

                var smsArray = xmlData.SmsList.Where(a => a.Type == 1).ToList()
                    .Where(SmsItem.IsValid)

                    .Select(a => new SmsItem(a))
                    //.Where(a => a.DateTime >= firstRange && a.DateTime < lastRange)
                    .Where(a => a.TransactionType != TransactionTypeEnum.None)
                    .ToList();
                
                // Заполняем данные графика транзакций
                data = new ChartValues<decimal>(smsArray.Select(a => a.TransactionValue).ToList());

                // Заполняем данные графика баланса
                balance = new ChartValues<decimal>(smsArray.Select(a => a.Balance).ToList());

                // Заполняем ось х даты
                Labels = smsArray.Select(a => a.DateTime.ToShortDateString()).ToArray();
            }

            // Создать графики
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Транзакции",
                    Values = data,
                    PointGeometry = DefaultGeometries.Circle,
                },
                new LineSeries
                {
                    Title = "Баланс",
                    Values = balance,
                    PointGeometry = DefaultGeometries.Circle,
                },
            };

            DataContext = this;
        }
    }
}
