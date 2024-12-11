public enum InteractableType
{
    None = 0,
    Item,
    Document,
    Object
}

public enum ItemType
{
    Key,
    EquipItem,
    CnsItem,
    CcItem,
    Document
}

public enum ObjectType
{
    Door,
    Drawer,
    Cabinet,
}

public enum ConsumableType
{
    Health,
    Stamina,
    Mental,
    Gage//혹시 모르니 값만
}

public enum UseKeyType
{
    Driver
}

public enum UseLockType//UseKey와 상관관계
{
    Bolt
}