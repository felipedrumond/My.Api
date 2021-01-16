using My.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace My.Api.Trolleys
{
    public class TrolleyCalculator
    {
        private Trolley trolley { get; set; }

        public TrolleyCalculator(Trolley trolley)
        {
            this.trolley = trolley;
        }

        public decimal CalculateTotal()
        {
            decimal total = 0;

            var specialBundleConfig = GetSpecialBundleConfiguration(trolley.Quantities, trolley.Specials);

            if (specialBundleConfig != null)
            {
                // adds the bundle price
                total += specialBundleConfig.Total;

                // then reduce the item quantity from the customer trolley
                specialBundleConfig.Quantities.ForEach(itemInBundleConfig =>
                {
                    var itemInTrolley = trolley.Quantities.Find(i => i.Name == itemInBundleConfig.Name);
                    itemInTrolley._Quantity -= itemInBundleConfig._Quantity;
                });
            }

            // for reminiscent items, add their regular price * quantity in trolley
            trolley.Quantities.ForEach(item =>
            {
                total += GetRegularPriceOfItem(item, trolley.Products) * item._Quantity;
            });

            return total;
        }

        private decimal GetRegularPriceOfItem(Quantity item, List<Product> products)
        {
            var price = products
                .Where(p => p.Name == item.Name)
                .Single()?.Price
                ?? throw new System.Exception("Not a single product was found.");

            return price;
        }

        private Special GetSpecialBundleConfiguration(List<Quantity> items, List<Special> specials)
        {
            Special bundle = null;

            specials.ForEach(specialConfig =>
            {
                var specialConfigIsApplicableToItems = specialConfig.Quantities.All(
                    q => items.Select(item => item.Name).Contains(q.Name)
                         && items.Select(y => y._Quantity).All(quantity => quantity >= q._Quantity));

                if (specialConfigIsApplicableToItems)
                {
                    bundle = specialConfig;
                }
            });

            return bundle;
        }
    }
}