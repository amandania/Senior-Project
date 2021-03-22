/// <summary>
/// This structure is used for serialization deserialization purposes. Json is the form of serialization we use to save a player file.
/// We dont need any gameobject logic thats defined in Player class so we just have the struct represent everything we need.
/// </summary>
public struct PlayerSave
{
    public string Username { get; set; }
    public string Password { get; set; }
    public int PlayerLevel { get; set; }
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }
    public SlotItem[] HotkeyItems { get; set; }
    public Quest[] PlayerQuests { get; set; }

    public int CurrentSlotEquipped { get; set; }

}