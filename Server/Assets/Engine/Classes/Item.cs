public class Item {

    public string ItemName { get; set; }
    public int ItemLevel { get; set; }
    public int Amount { get; set; }
    public bool IsStackable { get; set; } = false;

    //Empty Item
    public Item()
    {
        ItemName = "";
        ItemLevel = -1;
        Amount = -1;
    }

    //Base item with defined name and default level and amount
    public Item(string a_name)
    {
        ItemName = a_name;
        ItemLevel = 1;
        Amount = 1;
    }
    
    //Constructor with items level and default amount
    public Item(string a_name, int a_level)
    {
        ItemName = a_name;
        ItemLevel = a_level;
        Amount = 1;
    }


    //Constructor with items level and amount
    public Item(string a_name, int a_level, int a_amount)
    {
        ItemName = a_name;
        ItemLevel = a_level;
        Amount = a_amount;
    }

    public void Clear()
    {
        ItemName = "";
        ItemLevel = -1;
        Amount = -1;
    }
    
}
