// See https://aka.ms/new-console-template for more information
using HttpClient client = new HttpClient();

while (true)
{
    Console.Write("Напиши выражение: ");
    string exp = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(exp))
    {
        Console.WriteLine("Expression cannot be empty.");
        continue;
    }

    if (!IsValidExpression(exp))
    {
        Console.WriteLine("Invalid expression format.");
        continue;
    }

    HttpResponseMessage response = await client.GetAsync($"http://localhost:5197/values/calculate?exp={Uri.EscapeDataString(exp)}");

    if (response.IsSuccessStatusCode)
    {
        string message = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Result: {message}");
    }
    else
    {
        string error = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error message: {error}");
    }
}

static bool IsValidExpression(string expression)
{
    int balance = 0;
    foreach (char c in expression)
    {
        if (c == '(')
        {
            balance++;
        }
        else if (c == ')')
        {
            balance--;
        }

        if (balance < 0)
        {
            return false;
        }
    }

    return balance == 0;
}