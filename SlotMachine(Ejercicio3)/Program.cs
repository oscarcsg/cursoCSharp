namespace SlotMachine_Ejercicio3_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Jugador j = new Jugador("Oscar", 100);
            var maq = new Maquina("Máquina 1");

            for (byte i = 0; i < 5; i++)
            {
                Console.WriteLine();
                maq.Play(j);
                Console.WriteLine($"Jugador: {j._Saldo}; Máquina: {maq._Monedas}");
            }
        }
    }
}
