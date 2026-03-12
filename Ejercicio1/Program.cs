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


            // EJERCICIO 3
            Console.WriteLine("\nEscriba un día de la semana (ej. lunes): ");
            string? dia = Console.ReadLine().ToLower();
            string[] finesSemana = { "sabado", "sábado", "domingo" };
            if (dia != null)
            {
                bool flag = false;
                foreach (string d in finesSemana)
                {
                    if (string.Equals(dia, d))
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag) Console.WriteLine("El día introducido SÍ es fin de semana.");
                else Console.WriteLine("El dia introducido NO es fin de semana.");
            }



            // EJERCICIO 4
            for (int i = 1; i <= 100; i++)
            {
                if (i % 2 == 0) Console.WriteLine(i);
            }
        }
    }
}
