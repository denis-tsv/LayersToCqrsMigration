using System;

namespace Domain
{
    public class Order : Entity
    {
        public DateTime CreateDate { get; set; }

        public string UserEmail { get; set; }
    }
}
