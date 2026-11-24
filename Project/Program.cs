// See https://aka.ms/new-console-template for more information

/*
RUN PROJECT:
dotnet run --project .\Project\Project.csproj

ADMIN:
admin@admin.com
adm123

USER:
test@test.com
test123

VLUCHT BOEKEN:
Turkije, Istanbul -> Nederland, Amsterdam

DEBUGS:
- Keuzemenus nummers weghalen
*/

// Stoelen automatisch aanmaken voor de drie vliegtuigen
StoelLogic stoelLogic = new();
stoelLogic.SlaStoelenOp(1, "Airbus A330-200");
stoelLogic.SlaStoelenOp(2, "Boeing 737");
stoelLogic.SlaStoelenOp(3, "Boeing 787");

//AccountsLogic accountsLogic = new AccountsLogic();
//accountsLogic.CheckLogin("test@test.com", "test123");

Console.WriteLine("Welkom bij Project B Airlines. Als u dit ziet, is er iets fout gegaan met de applicatie.");
Menu.Start();