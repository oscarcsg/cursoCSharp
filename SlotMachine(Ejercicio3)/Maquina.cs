using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
        public string? Nombre { get; }
        // Cada máquina comienza con un saldo de 1000 monedas
        public int Monedas { get; private set; } = 1000;
        // Definir el número de slots que tiene la máquina mediante el enum
        public EMaquina Slots { get; } = EMaquina.TRI_SLOT; // Por defecto tienen 3 slots
        // Multiplicador de probabilidad de ganar de la máquina
        public double MultiplicadorProb { get; }
        // Multiplicador de premio
        public double MultiplicadorPrem { get; private set; }
        // Estado. Si tiene monedas está operativa, sino no
        public bool Operativa { get; set; } = true;

        public Dictionary<byte, string?> Simbolos = new()
        {
            { 1, "🍒" },
            { 2, "🍇" },
            { 3, "🍓" },
            { 4, "🔔" },
            { 5, "👑" },
            { 6, "🐉" },
            { 7, "💰" },
            { 8, "💎" },
        };
        #endregion



        #region Constructores
        private Maquina()
        {
            Nombre = "Máquina Tragaperras";
        }

        private Maquina(string nombre)
        {
            Nombre = nombre;
        }

        public Maquina(string nombre, EMaquina slots, double multiProb, double multiPrem) : this(nombre)
        {
            Slots = slots;
            MultiplicadorProb = multiProb;
            MultiplicadorPrem = multiPrem;
        }
        #endregion



        #region Métodos
        public async Task Play(Jugador player)
        {
            Dictionary<byte, byte> results = [];
            byte size = (byte)this.Slots;

            if (player.Saldo >= size) RecalcularSaldo(player, size, false); // Retirar las monedas del precio de la máquina
            else
            {
                await Program.ShowMessageFooter("💸 No tienes saldo suficiente.");
                return;
            }

            // Tirar suerte
            byte[] simbols = TirarSuerte(size);
            foreach (byte b in simbols)
            {
                if (results.TryGetValue(b, out byte value)) results[b] = ++value;
                else results.Add(b, 1);
            }



            // Comprobar resultados
            if (results.ContainsValue(5))
            {
                MultiplicadorPrem += 0.5;
                byte monedas = CalcularMonedas();
                RecalcularSaldo(player, monedas, true); // Ingresas las ganancias (el precio de la máquina más los intereses
                MultiplicadorPrem -= 0.5;
                await Program.ShowMessageFooter($"💰 ¡¡¡ENHORABUENA!!! Has ganado {monedas} monedas 💰");
            }
            else if (results.ContainsValue(3))
            {
                MultiplicadorPrem += 0.3;
                byte monedas = CalcularMonedas();
                RecalcularSaldo(player, monedas, true); // Ingresas las ganancias (el precio de la máquina más los intereses
                MultiplicadorPrem -= 0.3;
                await Program.ShowMessageFooter($"💰 ¡¡¡ENHORABUENA!!! Has ganado {monedas} monedas 💰");
            }
            else await Program.ShowMessageFooter($"Lo sentimos, ha perdido {size} monedas");
        }

        private byte CalcularMonedas()
        {
            double multiplicadorAleatorio = Random.Shared.NextDouble() * (byte)this.Slots;

            byte monedasBase = (byte)this.Slots;

            // Redondear hacia arriba, es decir, que si el cálculo sale 3.6, se devuelvan 4 monedas (o eso debería pasar)
            byte ganancia = (byte)(monedasBase + (byte)Math.Ceiling(monedasBase * multiplicadorAleatorio) * this.MultiplicadorPrem);

            return ganancia;
        }

        private void RecalcularSaldo(Jugador player, byte monedas, bool sumar)
        {
            if (sumar)
            {
                if (this.Monedas < monedas)
                {
                    // Que corte la operatividad
                    this.Operativa = false;
                }

                // Pero que le siga devolviendo al jugador sus monedas


                // TODO: sería interesante añadir que si la máquina no tiene monedas suficientes, se las quite a otra máquina
                // del mismo tipo para poder pagar la deuda que se podría considerar que tiene el casino.


                // Gana el jugador, pierde la máquina
                player.Saldo += monedas;
                this.Monedas -= monedas;
            }
            else
            {
                if (player.Saldo < monedas)
                {
                    // Impide que el jugador pueda seguir jugando hasta que 1) pague su deuda (si tiene) y 2) meta más dinero
                    player.Sigue = false;
                }

                // Pueda el jugador seguir o no, se le cobran las monedas, aunque se quede en negativo

                // Pierde el jugador, gana la máquina
                player.Saldo -= monedas;
                this.Monedas += monedas;
            }
        }

        private byte[] TirarSuerte(byte size)
        {
            byte[] values = new byte[size];

            // Calcular el primer simbolo
            values[0] = (byte)Random.Shared.Next(1, 9);

            for (int i = 1; i < size; i++)
            {
                // Ej. Hay un 0.8 (80%) de probabilidad de suerte
                // osea que hay 80 posibles numeros para sacar el mismo valor
                if (Random.Shared.NextDouble() <= MultiplicadorProb)
                {
                    // Si ha tenido SUERTE, entonces sale el mismo simbolo de la rueda anterior
                    values[i] = values[i - 1];
                }
                else
                {
                    values[i] = (byte)Random.Shared.Next(1, 9);
                }
            }

            return values;
        }
        #endregion
    }
}
