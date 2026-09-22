namespace CBS.Core.Domain.Transfers;

public class Transfer
{
  private const decimal CommissionRatio = 0.02m;

  public TransferSide SenderSide { get; }
  public TransferSide RecipientSide { get; }
  public decimal SenderAmount { get; private set; }
  public decimal RecipientAmount { get; private set; }
  public Guid Id { get; } = Guid.NewGuid();
  public decimal Commission { get; private set; }
  public decimal Rate { get; private set; }

  // for EF
  private Transfer() {}

  public Transfer(TransferSide senderSide, TransferSide recipientSide, decimal amount, decimal rate)
  {
    if (senderSide.AccountId == recipientSide.AccountId)
      throw new InvalidOperationException("Transfers between the same account are prohibited");

    if (rate <= 0)
      throw new ArgumentException("Rate is less than 0 or equals 0");

    if (amount <= 0)
      throw new ArgumentException("Amount for send is less than 0 or equals 0");

    SenderAmount = amount;

    var recipientAmount = rate * amount;
    Commission = senderSide.ClientId == recipientSide.ClientId ? 0 : Math.Round(recipientAmount * CommissionRatio, recipientSide.Currency.Scale);

    SenderSide = senderSide;
    RecipientSide = recipientSide;

    RecipientAmount = Math.Round(recipientAmount - Commission,
      recipientSide.Currency.Scale);
    Rate = rate;
  }
}
