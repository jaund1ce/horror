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
    Gage//Ȥ�� �𸣴� ����
}

public enum UseKeyType
{
    Driver
}

public enum UseLockType//UseKey�� �������
{
    Bolt
}

public enum PlayerState
{
    Normal = -1,
    Chased = 0,
    Danger,
    Chasing,
    Hide
}

public enum GroundType
{
    Cement = 0,
    Wood,
    Dirt,
    Grass
}
