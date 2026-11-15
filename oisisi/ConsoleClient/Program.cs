using ConsoleClient.Views;
using Core;

ApplicationContext context = new ApplicationContext();

AdresaConsoleView adresaView = new(
    context.AdresaController);

PosetilacConsoleView posetilacView = new(
    context.PosetilacController, adresaView);

AutorConsoleView autorView = new(
    context.AutorController, adresaView);

KnjigaConsoleView knjigaView = new(
    context.KnjigaController);

KupovinaConsoleView kupovinaView = new(
    context.KupovinaController);

IzdavacConsoleView izdavacView = new(
    context.IzdavacController);

while (true)
{
    Console.WriteLine("\n--- Uprava sajma knjiga ---");
    Console.WriteLine("1. Posetioci");
    Console.WriteLine("2. Autori");
    Console.WriteLine("3. Knjige");
    Console.WriteLine("4. Kupovine");
    Console.WriteLine("5. Izdavaci");
    Console.WriteLine("6. Adrese");
    Console.WriteLine("0. Izlaz");
    Console.Write("Izaberite opciju: ");

    switch (Console.ReadLine())
    {
        case "1":
            posetilacView.ShowMenu();
            break;
        case "2":
            autorView.ShowMenu();
            break;
        case "3":
            knjigaView.ShowMenu();
            break;
        case "4":
            kupovinaView.ShowMenu();
            break;
        case "5":
            izdavacView.ShowMenu();
            break;
        case "6":
            adresaView.ShowMenu();
            break;
        case "0":
        case null:
            return;
        default:
            Console.WriteLine("Neispravna opcija.");
            break;
    }
}