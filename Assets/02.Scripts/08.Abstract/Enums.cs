public enum InteractableType
{
    None = 0,
    Item,
    Document,
    Object
}

public enum ItemType
{
    Equipment,
    Useable,
    Consumable,
    Key
}

public enum ObjectType
{
    Door,
    Drawer,
    Cabinet
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