using Core.Controllers;
using Core.DAO;
using Core.Services;
using Core.Storage;
using Core.Utils;
using System.Linq;

namespace Core
{
    public class ApplicationContext
    {
        public AdresaController AdresaController { get; }
        public AutorController AutorController { get; }
        public PosetilacController PosetilacController { get; }
        public IzdavacController IzdavacController { get; }
        public KnjigaController KnjigaController { get; }
        public KupovinaController KupovinaController { get; }

        public ApplicationContext()
        {
            AdresaDAO adresaDAO = new(
                DataPaths.GetPath("adrese.csv"));

            AutorDAO autorDAO = new(
                DataPaths.GetPath("autori.csv"), adresaDAO);

            PosetilacDAO posetilacDAO = new(
                DataPaths.GetPath("posetioci.csv"), adresaDAO);

            IzdavacDAO izdavacDAO = new(
                DataPaths.GetPath("izdavaci.csv"), autorDAO);

            KnjigaDAO knjigaDAO = new(
                DataPaths.GetPath("knjige.csv"));

            KupovinaDAO kupovinaDAO = new(
                DataPaths.GetPath("kupovine.csv"));

            AutorKnjigaDAO autorKnjigaDAO = new(
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
                knjiga.KupciClanskeKarte = kupovinaDAO
                    .GetByISBN(knjiga.ISBN)
                    .Select(k => k.BrojClanskeKarte)
                    .Distinct()
                    .ToList();

                knjiga.ListaZeljaClanskeKarte = posetilacDAO.GetAll()
                    .Where(p => p.ListaZeljaISBN.Any(isbn =>
                        EntityComparison.SameISBN(isbn, knjiga.ISBN)))
                    .Select(p => p.BrojClanskeKarte)
                    .ToList();
            }

            AutorKnjigaService autorKnjigaService = new(
                autorDAO, knjigaDAO, autorKnjigaDAO);

            PosetilacController = new(
                posetilacDAO, kupovinaDAO, knjigaDAO);

            KupovinaController = new(
                kupovinaDAO, posetilacDAO, knjigaDAO);

            IzdavacController = new(
                izdavacDAO, autorDAO, knjigaDAO);

            AutorController = new(
                autorDAO, autorKnjigaService, izdavacDAO);

            KnjigaController = new(
                knjigaDAO, autorKnjigaService,
                kupovinaDAO, posetilacDAO, izdavacDAO);

            AdresaController = new(
                adresaDAO, autorDAO, PosetilacController);
        }
    }
}