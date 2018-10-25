using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsAnalizer.Attributes
{
    /// <summary>
    /// Аттрибут транзакции
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum|AttributeTargets.Field)]
    public class TransactionSignAttribute : Attribute
    {
        /// <summary>
        /// Знак транзакции
        /// </summary>
        public int Sign { get; }

        /// <summary>
        /// Аттрибут транзакции
        /// </summary>
        /// <param name="sign">Знак операции -/+</param>
        public TransactionSignAttribute(string sign)
        {
            if (sign == "-")
                Sign = -1;
            else if (sign == "+")
                Sign = 1;
            else
                throw new Exception("Допустимые значения + или -");
        }

        /// <summary>
        /// Аттрибут транзакции
        /// </summary>
        /// <param name="sign">Знак операции -/+</param>
        public TransactionSignAttribute(char sign)
        {
            if (sign == '-')
                Sign = -1;
            else if (sign == '+')
                Sign = 1;
            else
                throw new Exception("Допустимые значения + или -");
        }

        /// <summary>
        /// Аттрибут транзакции
        /// </summary>
        /// <param name="sign">Знак операции -/+</param>
        public TransactionSignAttribute(int sign)
        {
            if (sign == -1)
                Sign = -1;
            else if (sign == 1)
                Sign = 1;
            else
                throw new Exception("Допустимые значения +1 или -1");
        }
    }
}
