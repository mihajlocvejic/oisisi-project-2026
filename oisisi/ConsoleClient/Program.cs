using Core.Controllers;
using ConsoleClient.Views;

char separator = Path.DirectorySeparatorChar;

string fileName = Path.GetFullPath(
    $"..{separator}..{separator}..{separator}Data{separator}posetioci.csv",
    AppContext.BaseDirectory);

string kupovinaFileName = Path.GetFullPath(
    $"..{separator}..{separator}..{separator}Data{separator}kupovine.csv",
    AppContext.BaseDirectory);

string izdavacFileName = Path.GetFullPath(
    $"..{separator}..{separator}..{separator}Data{separator}izdavaci.csv",
    AppContext.BaseDirectory);

PosetilacController controller = new PosetilacController(fileName);
PosetilacConsoleView view = new PosetilacConsoleView(controller);

KupovinaController kupovinaController = new KupovinaController(kupovinaFileName);
KupovinaConsoleView kupovinaView = new KupovinaConsoleView(kupovinaController);

IzdavacController izdavacController = new IzdavacController(izdavacFileName);
IzdavacConsoleView izdavacView = new IzdavacConsoleView(izdavacController);

izdavacView.ShowMenu();