using Core.Controllers;
using Core.Storage;
using ConsoleClient.Views;

string fileName = DataPaths.GetPath("posetioci.csv");
string kupovinaFileName = DataPaths.GetPath("kupovine.csv");
string izdavacFileName = DataPaths.GetPath("izdavaci.csv");

PosetilacController controller = new PosetilacController(fileName);
PosetilacConsoleView view = new PosetilacConsoleView(controller);

KupovinaController kupovinaController = new KupovinaController(kupovinaFileName);
KupovinaConsoleView kupovinaView = new KupovinaConsoleView(kupovinaController);

IzdavacController izdavacController = new IzdavacController(izdavacFileName);
IzdavacConsoleView izdavacView = new IzdavacConsoleView(izdavacController);

izdavacView.ShowMenu();