using SmsAnalizer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsAnalizer.Model
{
    /// <summary>
    /// Типы транзакций
    /// </summary>
    public enum TransactionTypeEnum
    {
        /// <summary>
        /// Не определена
        /// </summary>
        None,

        /// <summary>
        /// Покупка
        /// </summary>
        [TransactionSign('-')]
        Purchase,

        /// <summary>
        /// Покупка, USD
        /// </summary>
        [TransactionSign('-')]
        PurchaseUSD,

        /// <summary>
        /// Зачисление
        /// </summary>
        [TransactionSign('+')]
        Enrollment,

        /// <summary>
        /// Зачисление, USD
        /// </summary>
        [TransactionSign('+')]
        EnrollmentUSD,

        /// <summary>
        /// Списание
        /// </summary>
        [TransactionSign('-')]
        WriteOff,

        /// <summary>
        /// Оплата
        /// </summary>
        [TransactionSign('-')]
        Payment,

        /// <summary>
        /// Выдача наличных (Обналичить)
        /// </summary>
        [TransactionSign('-')]
        CashOut,
    }
}
