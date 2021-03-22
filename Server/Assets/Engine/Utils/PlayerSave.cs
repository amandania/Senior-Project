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