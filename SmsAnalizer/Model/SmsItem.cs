using SmsAnalizer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmsAnalizer.Model
{
    /// <summary>
    /// Смс обработанная
    /// </summary>
    public class SmsItem
    {
        /// <summary>
        /// Дата и время
        /// </summary>
        public DateTime DateTime { get; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Сумма транзакции
        /// </summary>
        public decimal TransactionValue { get; }

        /// <summary>
        /// Текущий баланс
        /// </summary>
        public decimal Balance { get; }

        /// <summary>
        /// Тип транзакции
        /// </summary>
        public TransactionTypeEnum TransactionType { get; }

        /// <summary>
        /// Преобразование смс в удобный формат
        /// </summary>
        /// <param name="item">Смс</param>
        public SmsItem(Original_SmsItem item)
        {
            // Дата и время
            DateTime = UnixDate.AddMilliseconds(item.DateInt).ToLocalTime();

            // Текст сообщения
            Text = item.Text;

            // Вычисляем сумму и тип транзакции
            foreach (var nextSearch in transactionDict)
            {
                Regex regex = new Regex(nextSearch.Key, RegexOptions.IgnoreCase);
                var founded = regex.Match(Text);

                if (founded.Groups.Count > 1)
                {
                    // Сумма транзакции
                    TransactionValue = decimal.Parse(founded.Groups[1].Value.Replace(".", ","));

                    // Тип транзакции
                    TransactionType = nextSearch.Value;
                    
                    if (TransactionType == TransactionTypeEnum.PurchaseUSD || TransactionType == TransactionTypeEnum.EnrollmentUSD)
                    {
                        // умножить на текущий курс доллара 19.10.2018
                        TransactionValue *= 65.50m;
                    }

                    if (typeof(TransactionTypeEnum).GetMember(TransactionType.ToString())[0].GetCustomAttributes(typeof(TransactionSignAttribute), false).FirstOrDefault() is TransactionSignAttribute transactionAttr)
                    {
                        // Сумма транзакции
                        TransactionValue *= transactionAttr.Sign;
                    }
                    break;
                }
            }

            // Вычисляем баланс
            {
                Regex regex = new Regex(@"Баланс: (\d+(.{1}\d{2})?)р", RegexOptions.IgnoreCase);
                var founded = regex.Match(Text);

                if (founded.Groups.Count > 1)
                {
                    // Текущий баланс
                    Balance = decimal.Parse(founded.Groups[1].Value.Replace(".", ","));
                }
            }

        }

        // время для преобразования
        private static readonly DateTime UnixDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        // регулярки транзакций
        private static Dictionary<string, TransactionTypeEnum> transactionDict = new Dictionary<string, TransactionTypeEnum>()
        {
            { @"покупка (\d+(.{1}\d{2})?)р", TransactionTypeEnum.Purchase },
            { @"покупка (\d+(.{1}\d{2})?)USD", TransactionTypeEnum.PurchaseUSD },
            { @"выдача наличных (\d+(.{1}\d{2})?)р", TransactionTypeEnum.CashOut },
            { @"выдача (\d+(.{1}\d{2})?)р", TransactionTypeEnum.CashOut },
            { @"оплата (\d+(.{1}\d{2})?)р", TransactionTypeEnum.Payment },
            { @"оплата услуг (\d+(.{1}\d{2})?)р", TransactionTypeEnum.Payment },
            { @"оплата Мобильного банка за \d{2}/\d{2}/\d{4}-\d{2}/\d{2}/\d{4} (\d+(.{1}\d{2})?)р", TransactionTypeEnum.Payment },
            { @"оплата годового обслуживания карты (\d+(.{1}\d{2})?)р", TransactionTypeEnum.Payment },
            //{ @"перевод (\d+(.{1}\d{2})?)р", TransactionTypeEnum.CashOut },
            { @"списание (\d+(.{1}\d{2})?)р", TransactionTypeEnum.WriteOff },
            { @"зачисление (\d+(.{1}\d{2})?)р", TransactionTypeEnum.Enrollment },
            { @"зачисление зарплаты (\d+(.{1}\d{2})?)р", TransactionTypeEnum.Enrollment },
            { @"зачисление аванса (\d+(.{1}\d{2})?)р", TransactionTypeEnum.Enrollment },
            { @"отмена покупки (\d+(.{1}\d{2})?)р", TransactionTypeEnum.Enrollment },
            { @"отмена покупки (\d+(.{1}\d{2})?)USD", TransactionTypeEnum.EnrollmentUSD },
            { @"возврат покупки (\d+(.{1}\d{2})?)р", TransactionTypeEnum.Enrollment },
        };

        /// <summary>
        /// Проверка сообщения на осуществление транзакции
        /// </summary>
        /// <param name="sms">Сообщение</param>
        /// <returns>true|false</returns>
        internal static bool IsValid(Original_SmsItem sms) =>
                    !sms.Text.ToLower().Contains("отказ покупка")
                    && !sms.Text.ToLower().Contains("кредит")
                    && !sms.Text.ToLower().Contains("карта")
                    && !sms.Text.ToLower().Contains("карту")
                    && !sms.Text.ToLower().Contains("картой")
                    && !sms.Text.ToLower().Contains("карте")
                    && !sms.Text.ToLower().Contains("перевод")
                    && !sms.Text.ToLower().Contains("откройте")
                    && !sms.Text.ToLower().Contains("оформите")
                    && !sms.Text.ToLower().Contains("отправьте")
                    && !sms.Text.ToLower().Contains("обратитесь")
                    && !sms.Text.ToLower().Contains("не сообщайте")
                    && !sms.Text.ToLower().Contains("встречайте")
                    && !sms.Text.ToLower().Contains("можете")
                    && !sms.Text.ToLower().Contains("с днем рождения")
                    && !sms.Text.ToLower().Contains("запрос на регистрацию")
                    && !sms.Text.ToLower().Contains("вход");
    }
}
