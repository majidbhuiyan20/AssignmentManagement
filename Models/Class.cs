public class Class
{
    public int Id {get; set;} // here we are using Id as primary key for the class table(id is auto incremented by default in EF core) 
    public string Name {get; set;} = string.Empty;
    public string? Descrption {get; set;}
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
}