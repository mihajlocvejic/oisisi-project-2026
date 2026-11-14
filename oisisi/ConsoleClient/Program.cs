using Core.Controllers;
using ConsoleClient.Views;

char separator = Path.DirectorySeparatorChar;

string fileName = Path.GetFullPath(
    $"..{separator}..{separator}..{separator}Data{separator}posetioci.csv",
    AppContext.BaseDirectory);

PosetilacController controller = new PosetilacController(fileName);
PosetilacConsoleView view = new PosetilacConsoleView(controller);

view.ShowMenu();