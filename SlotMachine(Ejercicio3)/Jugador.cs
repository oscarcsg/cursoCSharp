namespace SlotMachine_Ejercicio3_
{
    public class Jugador
    {
        #region Atributos
        // Nombre del jugador
        public string? _Nombre { get; set; }
        // Saldo del jugador
        public int _Saldo { get; set; } = 0;
        // Flag para saber si puede seguir jugando o no
        public bool _Sigue { get; set; } = true;
        #endregion



        #region Constructores
        public Jugador(string nombre)
        {
            _Nombre = nombre;
        }
        public Jugador(string nombre, int saldo) : this(nombre)
        {
            _Saldo = saldo;
        }
        #endregion



        #region Métodos
            
        #endregion
    }
}
