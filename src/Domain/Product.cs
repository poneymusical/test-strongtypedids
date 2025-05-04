namespace Domain;


public class Product
{
    public ProductId Id { get; private set; }
    public string Name { get; set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public Product(string name)
    {
        Id = ProductId.New();
        Name = name;
        CreatedAt = DateTimeOffset.UtcNow;
    }
}

[StronglyTypedId]
public partial struct ProductId;