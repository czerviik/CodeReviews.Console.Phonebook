using System.Reflection;
using Spectre.Console;

namespace PhoneBook;

public static class UserInterface
{
    internal static MenuOptions OptionChoice { get; private set; }
    public static void MainMenu()
    {
        Header("phone book");

        MenuOptions[] options = {
                MenuOptions.ShowAllContacts,
                MenuOptions.Exit};

        ChooseOptions(options);
    }


    private static void Header(string headerText)
    {
        Console.Clear();
        Console.WriteLine($"----- {headerText.ToUpper()} -----");
        Console.WriteLine();
    }

    private static void ChooseOptions(MenuOptions[] options)
    {
        OptionChoice = AnsiConsole.Prompt(
            new SelectionPrompt<MenuOptions>()
            .HighlightStyle("yellow")
            .AddChoices(options)
            );
    }

    // private static void DisplayTable(DrinkDetail drinkDetail)
    // {


    //     TableHeader(drinkDetail);

    //     var table = new Table()
    //     .AddColumns("details:", "")
    //     .Border(TableBorder.Rounded);

    //     FormatTableData(table, drinkDetail);

    //     table.Columns[0].RightAligned();
    //     AnsiConsole.Write(table);
    // }

    // private static void FormatTableData(Table table, DrinkDetail drinkDetail)
    // {
    //     var ingredientNumber = 0;
    //     var drinkType = drinkDetail.GetType();
    //     PropertyInfo[] properties = drinkType.GetProperties();

    //     foreach (var property in properties)
    //     {
    //         var modifiedPropertyName = Operations.ModifyPropertyName(property).ToString();

    //         if (property.GetValue(drinkDetail) != null &&
    //         property.GetValue(drinkDetail) != "" &&
    //         modifiedPropertyName != "Name" &&
    //         modifiedPropertyName != "IsFavorite" &&
    //         modifiedPropertyName != "Id" &&
    //         !modifiedPropertyName.StartsWith("Measure"))
    //         {
    //             if (modifiedPropertyName.StartsWith("Ingredient"))
    //             {
    //                 ingredientNumber++;

    //                 var measureProperty = properties.FirstOrDefault(p => p.Name == ("strMeasure" + ingredientNumber.ToString()));
    //                 var measurePropertyValue = measureProperty.GetValue(drinkDetail) ?? "";

    //                 if (measurePropertyValue != "")
    //                 {
    //                     table.AddRow(modifiedPropertyName, $"{measurePropertyValue} of {property.GetValue(drinkDetail)}");
    //                     continue;
    //                 }
    //             }
    //             table.AddRow(modifiedPropertyName, property.GetValue(drinkDetail).ToString());
    //         }
    //     }
    // }

    // private static void TableHeader(DrinkDetail drinkDetail)
    // {
    //     if (drinkDetail.IsFavorite)
    //     {
    //         Console.Write($"DRINK: {drinkDetail.Name} ");
    //         Console.BackgroundColor = ConsoleColor.Blue;
    //         Console.ForegroundColor = ConsoleColor.White;
    //         Console.WriteLine("*FAVORITE*");
    //         Console.ResetColor();
    //     }
    //     else
    //         Console.WriteLine($"DRINK: {drinkDetail.Name}");
    // }

    public static void DisplayMessage(string message = "", string actionMessage = "continue", bool consoleClear = false)
    {
        if (consoleClear) Console.Clear();

        if (message == "")
            Console.WriteLine($"\nPress any key to {actionMessage}...");
        else
            Console.WriteLine($"\n{message} Press any key to {actionMessage}...");

        Console.ReadKey();
    }
}