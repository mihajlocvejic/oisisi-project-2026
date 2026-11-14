using Core.Controllers;
using ConsoleClient.Views;

string fileName = "posetioci.csv";

PosetilacController controller = new PosetilacController(fileName);
PosetilacConsoleView view = new PosetilacConsoleView(controller);

view.UpdatePosetilac();