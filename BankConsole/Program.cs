using BankConsole;
using System.Text.RegularExpressions;

if (args.Length == 0)
    EmailService.SendMail();
else
    ShowMenu();

void ShowMenu()
{
    Console.Clear();
    Console.WriteLine("Selecciona una opción: ");
    Console.WriteLine("1.- Crear un usuario nuevo.");
    Console.WriteLine("2.- Eliminar un usuario existente.");
    Console.WriteLine("3.- Salir");

    int option = 0;
    do
    {
        string input = Console.ReadLine();

        if (!int.TryParse(input, out option))
            Console.WriteLine("Debes ingresar un número (1, 2 o 3).");
        else if (option > 3)
            Console.WriteLine("Debes ingresar un número válido (1, 2 o 3).");
    } while (option == 0 || option > 3);

    switch (option)
    {
        case 1:
            CreateUser();
            break;
        case 2:
            DeleteUser();
            break;
        case 3:
            Environment.Exit(0);
            break;
    }
}

void CreateUser()
{
    Console.Clear();
    Console.WriteLine("Ingresa la información del usuario: ");

    Console.Write("ID: ");
    int ID = IDValidado();

    Console.Write("Nombre: ");
    string name = Console.ReadLine();

    string email;
    bool emailValido;
    do
    {
        Console.Write("Email: ");
        email = Console.ReadLine();

        string emailPattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

        emailValido = Regex.IsMatch(email, emailPattern);

        if (!emailValido)
            Console.WriteLine("Ingresa un correo electrónico válido.");

    } while (!emailValido);

    Console.Write("Saldo: ");
    decimal balance = BalanceValidado();

    char userType;
    bool validInput = false;
    do
    {
        Console.Write("Escribe 'c' si el usuario es Cliente o 'e' si es Empleado: ");
        userType = char.Parse(Console.ReadLine());

        if (userType != 'c' && userType != 'e')
            Console.WriteLine("Ingresa un caracter válido. Solo se permiten los caracteres 'c' y 'e'.");
        else
            validInput = true;

    } while (!validInput);

    User newUser;

    if (userType.Equals('c'))
    {
        Console.Write("Regimen Fiscal: ");
        char taxRegime = char.Parse(Console.ReadLine());

        newUser = new Client(ID, name, email, balance, taxRegime);
    }
    else
    {
        Console.Write("Departamento: ");
        string department = Console.ReadLine();

        newUser = new Employee(ID, name, email, balance, department);
    }

    Storage.AddUser(newUser);

    Console.WriteLine("Usuario creado.");
    Thread.Sleep(2000);
    ShowMenu();
}

void DeleteUser()
{
    Console.Clear();

    Console.Write("Ingresa el ID del usuario a eliminar: ");
    int ID = IDValidado();

    string result = Storage.DeleteUser(ID);

    if (result.Equals("Sucess"))
    {
        Console.Write("Usuario eliminado.");
        Thread.Sleep(2000);
        ShowMenu();
    }
}

int IDValidado()
{
    int ID;
    bool salir = false;
    do
    {
        try
        {
            do
            {
                ID = int.Parse(Console.ReadLine());
                if (ID < 0)
                    Console.Write("Ingrese un valor numérico positivo.");
                salir = true;

            } while (ID < 0);
        }
        catch (FormatException)
        {
            throw new FormatException("Error de formato. Por favor, ingresa un valor numérico entero.");
        }
    } while (!salir);

    return ID;
}

decimal BalanceValidado()
{
    decimal balance;
    bool salir = false;
    do
    {
        try
        {
            do
            {
                balance = decimal.Parse(Console.ReadLine());
                if (balance < 0)
                    Console.Write("Ingrese un valor numérico positivo.");
                salir = true;

            } while (balance < 0);
        }
        catch (FormatException)
        {
            throw new FormatException("Error de formato. Por favor, ingresa un valor numérico válido.");
        }
    } while (!salir);

    return balance;
}