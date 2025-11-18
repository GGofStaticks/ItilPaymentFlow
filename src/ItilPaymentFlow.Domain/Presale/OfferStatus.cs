using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItilPaymentFlow.Domain.Presale
{
    public enum OfferStatus
    {
        OnReview = 1,   // на рассмотрении по умолчанию
        Approved = 2,   // согласовано
        Rejected = 3    // отклонено
    }
}