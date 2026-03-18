using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SlotMachine_Ejercicio3_
{
    public enum EMaquina : byte
    {
        TRI_SLOT = 3,
        FIVE_SLOT = 5
    }
    public class Maquina
    {
        #region Atributos
        // Nombre de la máquina
        public string? _Nombre { get; }
        // Cada máquina comienza con un saldo de 1000 monedas
        public int _Monedas { get; private set; } = 1000;
        // Definir el número de slots que tiene la máquina mediante el enum
        public EMaquina _Slots { get; } = EMaquina.TRI_SLOT; // Por defecto tienen 3 slots
        // Estado. Si tiene monedas está operativa, sino no
        public bool _Operativa { get; set; } = true;
        #endregion



        #region Constructores
        public Maquina()
        {
            _Nombre = "Máquina Tragaperras";
        }

        public Maquina(string nombre)
        {
            _Nombre = nombre;
        }

        public Maquina(string nombre, EMaquina slots) : this(nombre)
        {
            _Slots = slots;
        }
        #endregion



        #region Métodos
        public async Task<bool> Play(Jugador player)
        {
            byte size = (byte)this._Slots;
            byte count = 0;

            if (player._Saldo >= size) RecalcularSaldo(player, size, false); // Retirar las monedas del precio de la máquina
            else return await Program.ShowMessageFooter("💸 No tienes saldo suficiente.");

            // Tirar suerte
            for (byte i = 0; i < size; i++)
            {
                if (RandomExtension.NextBool(Random.Shared) == true) count++;
            }

            if (count == size || count == 0)
            {
                byte monedas = CalcularMonedas();
                RecalcularSaldo(player, monedas, true); // Ingresas las ganancias (el precio de la máquina más los intereses
                return await Program.ShowMessageFooter($"💰 ¡¡¡ENHORABUENA!!! Has ganado {monedas} monedas 💰");
            }
            else return await Program.ShowMessageFooter($"Lo sentimos, ha perdido {size} monedas");
        }

        private byte CalcularMonedas()
        {
            double multiplicadorAleatorio = Random.Shared.NextDouble() * 3;

            byte monedasBase = (byte)this._Slots;

            // Redondear hacia arriba, es decir, que si el cálculo sale 3.6, se devuelvan 4 monedas (o eso debería pasar)
            byte ganancia = (byte)(monedasBase + (byte)Math.Ceiling(monedasBase * multiplicadorAleatorio));

            return ganancia;
        }

        private void RecalcularSaldo(Jugador player, byte monedas, bool sumar)
        {
            if (sumar)
            {
                if (this._Monedas < monedas)
                {
                    // Que corte la operatividad
                    this._Operativa = false;
                }

                // Pero que le siga devolviendo al jugador sus monedas


                // TODO: sería interesante añadir que si la máquina no tiene monedas suficientes, se las quite a otra máquina
                // del mismo tipo para poder pagar la deuda que se podría considerar que tiene el casino.


                // Gana el jugador, pierde la máquina
                player._Saldo += monedas;
                this._Monedas -= monedas;
            }
            else
            {
                if (player._Saldo < monedas)
                {
                    // Impide que el jugador pueda seguir jugando hasta que 1) pague su deuda (si tiene) y 2) meta más dinero
                    player._Sigue = false;
                }

                // Pueda el jugador seguir o no, se le cobran las monedas, aunque se quede en negativo

                // Pierde el jugador, gana la máquina
                player._Saldo -= monedas;
                this._Monedas += monedas;
            }
        }
        #endregion
    }
}
