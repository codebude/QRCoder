using BenchmarkDotNet.Running;
using System.Reflection;

namespace QRCoderBenchmarks;

internal sealed class Program
{
    public static void Main()
    {
        // Get all benchmark classes from the assembly
        var benchmarkTypes = typeof(Program).Assembly.GetTypes()
            .Where(t => t.GetMethods().Any(m => m.GetCustomAttributes(typeof(BenchmarkDotNet.Attributes.BenchmarkAttribute), false).Length > 0))
            .OrderBy(t => t.Name)
            .ToList();

        if (benchmarkTypes.Count == 0)
        {
            Console.WriteLine("No benchmark classes found.");
            return;
        }

        Console.WriteLine("QRCoder Benchmarks");
        Console.WriteLine("==================");
        Console.WriteLine();
        Console.WriteLine("Available benchmark classes:");
        Console.WriteLine();

        for (int i = 0; i < benchmarkTypes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {benchmarkTypes[i].Name}");
        }

        Console.WriteLine($"{benchmarkTypes.Count + 1}. Run all benchmarks");
        Console.WriteLine();
        Console.Write("Select a benchmark to run (1-{0}): ", benchmarkTypes.Count + 1);

        var input = Console.ReadLine();
        if (!int.TryParse(input, out int selection) || selection < 1 || selection > benchmarkTypes.Count + 1)
        {
            Console.WriteLine("Invalid selection.");
            return;
        }

        Console.WriteLine();

        if (selection == benchmarkTypes.Count + 1)
        {
            // Run all benchmarks
            Console.WriteLine("Running all benchmarks...");
            Console.WriteLine();
            BenchmarkRunner.Run(typeof(Program).Assembly);
        }
        else
        {
            // Run selected benchmark
            var selectedType = benchmarkTypes[selection - 1];
            Console.WriteLine($"Running {selectedType.Name}...");
            Console.WriteLine();
            BenchmarkRunner.Run(selectedType);
        }
    }
}
