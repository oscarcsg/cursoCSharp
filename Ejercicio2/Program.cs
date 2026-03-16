namespace Ejercicio2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Generar el número a encontrar
            byte aleatorio = (byte)new Random().Next();

            byte user = 0;

            do
            {
                Console.Write($"Escriba un número: ");

                try
                {
                    user = byte.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    //throw new Exception("El valor introducido no es válido, debe ser un valor numérico entre 0 y 256.");
                    // No lanzo la excepción porque en este ejemplo concreto de programa no tendría sentido cortar el flujo del programa
                    Console.WriteLine("El valor introducido no es válido, debe ser un valor numérico entre 0 y 256.");
                    continue;
                }

                // No hace falta hacer comprobación de si el valor introducido está en los limites del byte porque eso ya lo controla try

                // Mensajes de numero grande o pequeño
                if (user > aleatorio) Console.WriteLine("El número es más pequeño.");
                else if (user < aleatorio) Console.WriteLine("El número es más grande.");
            }
            while (user != aleatorio);

            // Esta comprobación no sería necesaria porque el bucle termina cuando el usuario ha acertado el número
            // pero la voy a hacer por robusted
            if (user == aleatorio) Console.WriteLine("¡¡¡ENHORABUENA!!! Has acertado el número.");
            else Console.WriteLine("Has perdido."); // Este mensaje (por ahora) no sería necesario
        }
    }
}
