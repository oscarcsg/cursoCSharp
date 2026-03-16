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



            // EJERCICIO 5
            Console.WriteLine("\nIntroduzca cuántos números aleatorios quiere obtener: ");
            int cantidad = int.Parse(Console.ReadLine());
            int[] arr = new int[cantidad];
            var rand = new Random();
            for (int i = 0; i < cantidad; i++) arr[i] = rand.Next();
            Console.WriteLine(string.Join(", ", arr));



            // EJERCICIO 6
            Console.WriteLine("\nIntroduzca cuántas filas quiere que tenga la pirámide: ");
            int filas = int.Parse(Console.ReadLine());
            Console.WriteLine("¿Qué caracter quiere usar para la pirámide?");
            string? c = Console.ReadLine();

            int cont = 0;
            Console.WriteLine("\n");
            // Bucle para mover entre filas
            for (int i = 0; i < filas; i++)
            {
                // Aumentar el contador de cuántas columnas debe tener cada fila en 1
                cont++;

                // Bucle para mover entre columnas de una fila
                for (int j = 0; j < cont; j++)
                {
                    // Controlar cuándo estamos en la última fila para escribir la línea completa
                    if (i == filas-1)
                    {
                        Console.Write(c);
                        /* Pasar a la siguiente columna (j) para evitar hacer comprobaciones de cuándo la columna actual
                         * es la primera o la última de la fila
                         */
                        continue;
                    }

                    // Si la columna actual es EXCLUSIVAMENTE la primera o la última de la fila, entonces pinta el caracter
                    if (j == 0 || j == i) Console.Write(c);
                    // Si no, pinta un espacio en blanco
                    else Console.Write(" ");

                }
                // Salto de línea para empezar en la siguiente fila
                Console.WriteLine();
            }
        }
    }
}
