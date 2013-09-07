﻿namespace AS.ToolKit.Core.Entities
{
    public class ShoppingContribution
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }

        public virtual ShoppingGroup ShoppingGroup { get; set; }
        public virtual ShoppingPerson ShoppingPerson { get; set; }

        public ShoppingContribution()
        {
            
        }
    }
}