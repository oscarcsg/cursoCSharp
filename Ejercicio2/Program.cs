namespace Ejercicio2
{
    internal class Program
    {

        // Creo la excepción , pero no la voy a usar porque no creo que tenga mucho sentido cortar el flujo cada vez que
        // el usuario escriba un número más grande o pequeño que el número a adivinar.
        public class NumeroException : Exception
        {
            public NumeroException(string msg) : base(msg) { }
        }

        static void Main(string[] args)
        {
            // Generar el número a encontrar
            byte aleatorio = crearNumero();

            const byte INTENTOS = 5;
            byte currentIntentos = 0;

            byte user = 0;


            Console.WriteLine("--- BIENVENIDO A ADIVINA EL NÚMERO. ---\nAdivina el número entre el 1 y el 100.\n");
            do
            {
                Console.Write($"Escriba un número: ");

                try
                {
                    user = byte.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    //throw new Exception("El valor introducido no es válido, debe ser un valor numérico entre 1 y 100.");
                    // No lanzo la excepción porque en este ejemplo concreto de programa no tendría sentido cortar el flujo del programa
                    Console.WriteLine("El valor introducido no es válido, debe ser un valor numérico entre 1 y 100.");
                    continue;
                }

                // No hace falta hacer comprobación de si el valor introducido está en los limites del byte porque eso ya lo controla try

                currentIntentos++;

                if (currentIntentos == INTENTOS) break;

                // Mensajes de numero grande o pequeño
                if (user > aleatorio) Console.WriteLine("El número es más pequeño.");
                else if (user < aleatorio) Console.WriteLine("El número es más grande.");
            }
            while (user != aleatorio);

            // Esta comprobación no sería necesaria porque el bucle termina cuando el usuario ha acertado el número
            // pero la voy a hacer por robusted
            if (user == aleatorio) Console.WriteLine("¡¡¡ENHORABUENA!!! Has acertado el número.");
            else Console.WriteLine($"Has perdido. El número era {aleatorio}."); // Este mensaje (por ahora) no sería necesario
        }

        private static byte crearNumero()
        {
            Random rand = new Random();
            byte num = 0;

            do
            {
                num = (byte) rand.Next();
            }
            while (num == 0 || num > 100);

            return num;
        }
    }
}
