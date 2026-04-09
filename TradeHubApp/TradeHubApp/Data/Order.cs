using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubApp.Data
{
    internal class Order
    {
        public int Id { get; set; }

        public int BuyerId { get; set; }
        public User Buyer { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public bool? IsPaid { get; set; }
    }
}
