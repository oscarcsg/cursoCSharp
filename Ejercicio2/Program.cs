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

        // Enumeración de los niveles de dificultad
        public enum Dificultad : byte
        {
            // Estos 3 primeros niveles son con números del 1 al 100
            FACIL = 8,
            NORMAL = 5,
            DIFICIL = 3,

            DIOS = 3 // Este nivel genererá numeros del 1 al 255
        }

        static void Main(string[] args)
        {
            // Generar el número a encontrar
            byte aleatorio = 0,
                 intentosMax = 0,
                 currentIntentos = 0,
                 user = 0;

            Console.WriteLine("--- BIENVENIDO A ADIVINA EL NÚMERO. ---");

            // Solicitar al jugador la dificultad deseada y almacenarla
            var difSelec = SolicitarDificultad();
            // Sacar el # de intentos máximos dependiendo de la dificultad
            intentosMax = (byte)difSelec;
            // Generar el número a adivinar
            aleatorio = crearNumero(difSelec);

            if (difSelec == Dificultad.DIOS) Console.WriteLine("El número estará entre el 1 y el 255.\n");
            else Console.WriteLine("El número estará entre el 1 y el 100.\n");

            // BUCLE DE JUEGO
            do
            {
                Console.Write($"Tiene {intentosMax - currentIntentos} intentos restantes.\nEscriba un número: ");

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

                if (currentIntentos == intentosMax) break;

                // Mensajes de numero grande o pequeño
                if (user > aleatorio) Console.WriteLine("El número es más pequeño.");
                else if (user < aleatorio) Console.WriteLine("El número es más grande.");
            }
            while (user != aleatorio);

            // Esta comprobación no sería necesaria porque el bucle termina cuando el usuario ha acertado el número
            // pero la voy a hacer por robusted
            if (user == aleatorio) Console.WriteLine("¡¡¡ENHORABUENA!!! Has acertado el número.");
            else Console.WriteLine($"\nHas perdido. El número era el {aleatorio}."); // Este mensaje (por ahora) no sería necesario
        }

        private static byte crearNumero(Dificultad dif)
        {
            Random rand = new Random();
            byte num = 0, range = 100;

            if (dif == Dificultad.DIOS) range = byte.MaxValue;

            do
            {
                num = (byte) rand.Next();
            }
            while (num == 0 || num > range);

            return num;
        }

        private static Dificultad SolicitarDificultad()
        {
            bool flag = false;
            byte opcion = 0;
            do
            {
                Console.WriteLine("Seleccione la dificultad deseada:\n\t1.Fácil   2.Normal   3.Dificil   4.Dios");
                try
                {
                    opcion = byte.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("El valor introducido debe ser un número del 1 al 4.");
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        return Dificultad.FACIL;

                    case 2:
                        return Dificultad.NORMAL;

                    case 3:
                        return Dificultad.DIFICIL;

                    case 4:
                        return Dificultad.DIOS;

                    default:
                        Console.WriteLine("El valor introducido no es válido, vuelva a intentarlo.");
                        break;
                }
            }
            while(!flag);
            return Dificultad.NORMAL;
        }
    }
}
