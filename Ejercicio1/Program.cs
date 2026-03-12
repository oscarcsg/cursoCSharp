namespace Ejercicio1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // EJERCICIO 1
            Console.WriteLine("Ingrese su nombre: ");
            string? nombre = Console.ReadLine();
            Console.WriteLine("Ingrese su edad: ");
            int edad = int.Parse(Console.ReadLine());
            Console.WriteLine("Ingrese el nombre de su ciudad: ");
            string? ciudad = Console.ReadLine();
            Console.WriteLine($"Te llamas {nombre}, tienes {edad} años. Bienvenido a {ciudad}.");



            // EJERCICIO 2
            Console.WriteLine("\nEscriba un número: ");
            int num1 = int.Parse(Console.ReadLine());
            Console.WriteLine("Escriba otro número: ");
            int num2 = int.Parse(Console.ReadLine());
            Console.WriteLine($"El número mayor es el {Math.Max(num1, num2)}");
        }
    }
}
