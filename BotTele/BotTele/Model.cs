public class X_Acc
{
    public string ID { get; set; }
    public string TK { get; set; }
    public string MK { get; set; }
    public bool AccNew { get; set; }
    public DateTime Created_Date { get; set; }
}

public class Transaction
{
    public string ID { get; set; }
    public long Payment_Amount { get; set; }
    public DateTime Created_Date { get; set; }
}