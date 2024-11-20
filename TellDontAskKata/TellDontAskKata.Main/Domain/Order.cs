using System;
using System.Collections.Generic;
using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Main.Domain;

public class Order
{
    public decimal Total { get; set; }
    public string Currency { get; set; }
    public IList<OrderItem> Items { get; set; }
    public decimal Tax { get; set; }
    public OrderStatus Status { get; set; }
    public int Id { get; set; }

    public void Ship()
    {
        if (Status is OrderStatus.Created or OrderStatus.Rejected)
        {
            throw new OrderCannotBeShippedException();
        }

        if (Status is OrderStatus.Shipped)
        {
            throw new OrderCannotBeShippedTwiceException();
        }

        Status = OrderStatus.Shipped;
    }

    public void ChangeApprove(bool approvedState)
    {
        if (Status is OrderStatus.Shipped)
        {
            throw new ShippedOrdersCannotBeChangedException();
        }

        if (approvedState && Status is OrderStatus.Rejected)
        {
            throw new RejectedOrderCannotBeApprovedException();
        }

        if (!approvedState && Status is OrderStatus.Approved)
        {
            throw new ApprovedOrderCannotBeRejectedException();
        }

        Status = approvedState
            ? OrderStatus.Approved
            : OrderStatus.Rejected;
    }

    public void CreateItemFromProduct(Product? product, int quantity)
    {
        if (product is null)
        {
            throw new UnknownProductException();
        }

        var unitaryTax = Round(product.Price / 100m * product.Category.TaxPercentage);
        var unitaryTaxedAmount = Round(product.Price + unitaryTax);
        var taxedAmount = Round(unitaryTaxedAmount * quantity);
        var taxAmount = Round(unitaryTax * quantity);

        var orderItem = new OrderItem
        {
            Product = product,
            Quantity = quantity,
            Tax = taxAmount,
            TaxedAmount = taxedAmount
        };
        Items.Add(orderItem);
        Total += taxedAmount;
        Tax += taxAmount;
    }

    private static decimal Round(decimal amount)
    {
        return decimal.Round(amount, 2, MidpointRounding.ToPositiveInfinity);
    }
}