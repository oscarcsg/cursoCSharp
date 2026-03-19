namespace SlotMachine_Ejercicio3_
{
    public class Jugador
    {
        #region Atributos
        // Nombre del jugador
        public string? Nombre { get; set; }
        // Saldo del jugador
        public int Saldo { get; set; } = 0;
        // Flag para saber si puede seguir jugando o no
        public bool Sigue { get; set; } = true;
        #endregion



        #region Constructores
        public Jugador(string? nombre)
        {
            Nombre = nombre;
        }
        public Jugador(string? nombre, int saldo) : this(nombre)
        {
            Saldo = saldo;
        }
        #endregion



        #region Métodos
            
        #endregion
    }
}
