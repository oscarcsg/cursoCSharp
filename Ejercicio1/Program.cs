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
        }
    }
}
