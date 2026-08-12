public enum OpponentPackMode
{
    SameAsPlayer,
    RandomPack,
    AllPacks
}

public static class GameSettings
{
    public static PackDefinition SelectedPack;
    public static CustomPackData SelectedCustomPack;

    public static OpponentPackMode OpponentPackMode =
        OpponentPackMode.SameAsPlayer;
}