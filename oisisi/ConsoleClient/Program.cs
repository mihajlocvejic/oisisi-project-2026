using ConsoleClient.Views;
using Core.Controllers;
using Core.DAO;
using Core.Services;
using Core.Storage;
using Core.Utils;
using System.Linq;

AdresaDAO adresaDAO = new AdresaDAO(
    DataPaths.GetPath("adrese.csv"));

AutorDAO autorDAO = new AutorDAO(
    DataPaths.GetPath("autori.csv"), adresaDAO);

PosetilacDAO posetilacDAO = new PosetilacDAO(
    DataPaths.GetPath("posetioci.csv"), adresaDAO);

IzdavacDAO izdavacDAO = new IzdavacDAO(
    DataPaths.GetPath("izdavaci.csv"), autorDAO);

KnjigaDAO knjigaDAO = new KnjigaDAO(
    DataPaths.GetPath("knjige.csv"));

KupovinaDAO kupovinaDAO = new KupovinaDAO(
    DataPaths.GetPath("kupovine.csv"));

AutorKnjigaDAO autorKnjigaDAO = new AutorKnjigaDAO(
    DataPaths.GetPath("autor_knjiga.csv"));

foreach (var posetilac in posetilacDAO.GetAll())
{
    posetilac.Kupovine = kupovinaDAO.GetByPosetilac(
        posetilac.BrojClanskeKarte);

    posetilac.IzracunajProsecnuOcenu();
}

foreach (var izdavac in izdavacDAO.GetAll())
{
    izdavac.Knjige = knjigaDAO.GetAll()
        .Where(k => EntityComparison.SameIdentifier(
            k.SifraIzdavaca, izdavac.Sifra))
        .ToList();
}

foreach (var knjiga in knjigaDAO.GetAll())
{
    knjiga.KupciClanskeKarte = kupovinaDAO.GetByISBN(knjiga.ISBN)
        .Select(k => k.BrojClanskeKarte)
        .Distinct()
        .ToList();

    knjiga.ListaZeljaClanskeKarte = posetilacDAO.GetAll()
        .Where(p => p.ListaZeljaISBN.Any(isbn =>
            EntityComparison.SameISBN(isbn, knjiga.ISBN)))
        .Select(p => p.BrojClanskeKarte)
        .ToList();
}

AutorKnjigaService autorKnjigaService = new AutorKnjigaService(
    autorDAO, knjigaDAO, autorKnjigaDAO);

PosetilacController posetilacController = new PosetilacController(
    posetilacDAO, kupovinaDAO);

KupovinaController kupovinaController = new KupovinaController(
    kupovinaDAO, posetilacDAO, knjigaDAO);

IzdavacController izdavacController = new IzdavacController(
    izdavacDAO, autorDAO, knjigaDAO);

AutorController autorController = new AutorController(
    autorDAO, autorKnjigaService);

KnjigaController knjigaController = new KnjigaController(
    knjigaDAO, autorKnjigaService);

AdresaController adresaController = new AdresaController(
    adresaDAO, autorDAO, posetilacController);

AdresaConsoleView adresaView = new AdresaConsoleView(
    adresaController);

PosetilacConsoleView posetilacView = new PosetilacConsoleView(
    posetilacController, adresaView);

AutorConsoleView autorView = new AutorConsoleView(
    autorController, adresaView);

KnjigaConsoleView knjigaView = new KnjigaConsoleView(
    knjigaController);

KupovinaConsoleView kupovinaView = new KupovinaConsoleView(
    kupovinaController);

IzdavacConsoleView izdavacView = new IzdavacConsoleView(
    izdavacController);

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