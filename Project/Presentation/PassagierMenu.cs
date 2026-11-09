static class VluchtBoeken
{
    static private PassagierLogic passagierLogic = new PassagierLogic();
    public static void Start(int vluchtId)
    {
        Console.WriteLine($"U heeft vlucht nummer {vluchtId} geselecteerd.");
        Console.WriteLine("Vul uw passagiersgegevens in (* verplicht):");
    }
}