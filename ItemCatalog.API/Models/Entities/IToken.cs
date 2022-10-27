namespace ItemCatalog.API.Models.Entities;

public interface IToken : IEntity
{
    public string Token { get; set; }
    public DateTime AddedDate { get; set; }
    public DateTime ExpiryDate { get; set; }
}