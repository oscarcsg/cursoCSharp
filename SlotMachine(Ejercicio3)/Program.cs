using Spectre.Console;
using System.Text;

namespace SlotMachine_Ejercicio3_
{
    internal class Program
    {
        #region Atributos
        private const int _WIDTH = 100;

        public static bool GirarLibre { get; set; } = true;

        // Máquinas
        // Para que ganar no sea ultra complejo, voy a poner los multiplicadores alto
        private static readonly Maquina[] _maquinas =
        [
            // Máquinas de 3 slots
            new Maquina("👑 Los tres reyes 👑", EMaquina.TRI_SLOT, 0.52, 1.06),
            new Maquina("🍒 Las tres cerezas 🍒", EMaquina.TRI_SLOT, 0.45, 1.15),
            // Máquinas de 5 slots
            new Maquina("♠ Los cinco naipes ♠", EMaquina.FIVE_SLOT, 0.28, 1.34),
            new Maquina("🎰 Los cinco sietes 🎰", EMaquina.FIVE_SLOT, 0.14, 1.58)
        ];

        // Layouts de la interfaz
        private static readonly Layout _view = new Layout("Base")
                .SplitRows(
                    new Layout("Cabecera").Size(3),
                    new Layout("Cuerpo").Size(20).SplitColumns(
                        new Layout("Maquina").Ratio(6),
                        new Layout("Estadisticas").Ratio(4).SplitRows(
                            new Layout("Datos"),
                            new Layout("Decisiones")
                        )
                    ),
                    new Layout("Pie").Size(3)
                );

        private static LiveDisplayContext? ctx;
        #endregion

        #region MAIN
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Clear();

            // HEADER
            _view["Cabecera"].Update(
                new Panel(new Align(new Markup("[bold gold1] 🎰 CASINO NUEVO MÁLAGA 🎰 [/]"), HorizontalAlignment.Center))
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Gold1)
                );

            // FOOTER
            _view["Pie"].Update(
                new Panel(new Align(new Markup("Texto Prueba"), HorizontalAlignment.Left))
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Gold1)
                    .Padding(2, 0 ,2, 0)
                );

            _view["Maquina"].Update(new Panel("").Expand());
            _view["Datos"].Update(new Panel("").Expand());
            _view["Decisiones"].Update(new Panel("").Expand());

            await AnsiConsole.Live(_view)
                    .StartAsync(async context =>
                    {
                        ctx = context;
                        bool jugando = true;

                        string[] opciones = {
                            "🔀 Cambiar de tragaperras",
                            "🎰 Tirar de la palanca",
                            "💰 Aumentar saldo",
                            "👤 Cambiar de jugador",
                            "🚪 Salir del Casino"
                        };
                        // Por defecto está seleccionada la opcion de cambiar tragaperras
                        int opcionSeleccionada = 3;
                        Maquina? maquinaSeleccionada = null;
                        Jugador? jugador = null;



                        // --------------------------------- //
                        //           BUCLE DE JUEGO          //
                        // --------------------------------- //
                        while (jugando)
                        {
                            // Actualizar los paneles
                            ConfigurarPanelDecisiones(opciones, opcionSeleccionada);
                        
                            ConfigurarPanelDatos(maquinaSeleccionada, jugador);

                            ConfigurarPanelMaquina();

                            // Refrescar la ventana para mostrar los cambios
                            ctx.Refresh();

                            // Lectura de tecla para saber qué acción hacer
                            var tecla = Console.ReadKey(intercept: true).Key;

                            if (tecla == ConsoleKey.UpArrow)
                            {
                                // Subir en el menú sin pasar del 0
                                if (opcionSeleccionada > 0) opcionSeleccionada--;
                            }
                            else if (tecla == ConsoleKey.DownArrow)
                            {
                                // Bajar en el menú, sin pasar del límite de opciones
                                if (opcionSeleccionada < opciones.Length - 1) opcionSeleccionada++;
                            }
                            else if (tecla == ConsoleKey.Spacebar)
                            {
                                _ = TirarPalanca(GirarLibre, jugador, maquinaSeleccionada);
                            }
                            else if (tecla == ConsoleKey.Enter)
                            {
                                // Evaluar qué opciés es la que está marcada cuando se pulsó enter
                                switch (opcionSeleccionada)
                                {
                                    // Cambiar de máquina
                                    case 0:
                                        // Guardar la máquina seleccionada con la que jugar
                                        maquinaSeleccionada = CambiarMaquina();
                                        break;

                                    // Tirar de la palanca
                                    case 1:
                                        _ = TirarPalanca(GirarLibre, jugador, maquinaSeleccionada);
                                        break;

                                    // Añadir saldo
                                    case 2:
                                        await AnadirSaldo(jugador);
                                        break;

                                    // Cambiar de jugador
                                    case 3:
                                        jugador = CambiarJugador();
                                        break;

                                    // Salir del juego
                                    case 4:
                                        jugando = false;
                                        break;
                                }
                            }
                        }
                    });
        }
        #endregion



        #region Métodos
        private static Maquina? CambiarMaquina()
        {
            // Valor para saber cuál de las máquinas está seleccionada por el jugador
            int opcionSeleccionada = 0;
            bool flag = false;
            Maquina? maquinaSeleccionada = null;

            while (!flag)
            {
                // Texto con todas las máquinas
                var lineasMaquinas = new List<string>
                {
                    "[yellow]¿A qué máquina quieres jugar?[/]\n"
                };
                for (int i = 0; i < _maquinas.Length; i++)
                {
                    if (i == opcionSeleccionada)
                    {
                        // Si es la opción actual, se recalca con un fondo blanco y un >
                        lineasMaquinas.Add($"[black on white] > {_maquinas[i].Nombre} [/]");
                    }
                    else
                    {
                        // Si no, texto plano
                        lineasMaquinas.Add($"   {_maquinas[i].Nombre}");
                    }
                }

                string maquinasTxt = string.Join("\n", lineasMaquinas);

                // Cambiar el panel para mostrar las máquinas que se pueden seleccionar
                _view["Maquina"].Update(
                    new Panel(new Align(new Markup(maquinasTxt), HorizontalAlignment.Center, VerticalAlignment.Top))
                        .Border(BoxBorder.Rounded)
                        .Expand());

                ctx?.Refresh();

                var tecla = Console.ReadKey(intercept: true).Key;

                if (tecla == ConsoleKey.UpArrow)
                {
                    // Subir en el panel (las máquinas) sin pasar de 0
                    if (opcionSeleccionada > 0) opcionSeleccionada--;
                }
                else if (tecla == ConsoleKey.DownArrow)
                {
                    // Bajar en el panel, siempre dentro de las opciones de las maquinas
                    if (opcionSeleccionada < _maquinas.Length - 1) opcionSeleccionada++;
                }
                else if (tecla == ConsoleKey.Enter)
                {
                    flag = true;
                    // Seleccionar la máquina actual
                    maquinaSeleccionada = _maquinas[opcionSeleccionada];
                }
            }
            // Devolver la máquina seleccionada
            return maquinaSeleccionada;
        }

        private static Jugador? CambiarJugador()
        {
            bool flag = false;

            string? nombre = null;
            string? saldoStr = "10";
            int saldo = 10;
            bool isNombre = false;

            while (!flag)
            {
                string msg;
                if (!isNombre) msg = $"Escribe tu nombre de jugador (enter para enviar):\n{nombre}";
                else msg = $"Escribe cuánto saldo quieres tener: {saldo}";

                _view["Maquina"].Update(
                    new Panel(new Align(new Markup(msg), HorizontalAlignment.Center, VerticalAlignment.Top))
                        .Border(BoxBorder.Rounded)
                        .Expand());

                ctx?.Refresh();

                var tecla = Console.ReadKey(intercept: true);
                var key = tecla.Key;
                char c = tecla.KeyChar;

                if (key == ConsoleKey.Backspace && nombre?.Length > 0 && saldoStr.Length > 0)
                {
                    if (!isNombre) nombre = nombre.Remove(nombre.Length - 1);
                    else
                    {
                        // Esto elimina el último elemento (último caracter del string)
                        saldoStr = saldoStr[..^1];
                        if (saldoStr.Length == 0) saldo = 0;
                        else saldo = int.Parse(saldoStr);
                    }
                }
                else if (key == ConsoleKey.Enter)
                {
                    if (!isNombre)
                    {
                        if (!string.IsNullOrEmpty(nombre)) isNombre = true;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(saldoStr)) flag = true;
                    }
                }
                else if (char.IsDigit(c) || char.IsLetter(c))
                {
                    if (!isNombre) nombre += c;
                    else if (char.IsDigit(c))
                    {
                        saldoStr += c;
                        saldo = int.Parse(saldoStr);
                    }
                }
            }

            return new Jugador(nombre, int.Parse(saldoStr));
        }

        private static async Task AnadirSaldo(Jugador? jugador)
        {
            if (jugador == null)
            {
                _ = ShowMessageFooter(":warning:  No hay un jugador registrado actualmente.");
                return;
            }

            bool flag = false,
                 good = true;

            int monedas = jugador.Saldo;
            string monedasStr = "0";

            while (!flag)
            {
                string msg;
                if (good) msg = $"¿Cuánto dinero quiere meter en su cuenta?\nCantidad: {monedasStr} monedas";
                else msg = $"No es posible, pruebe con otra cantidad.\nCantidad: {monedasStr} monedas";

                _view["Maquina"].Update(
                    new Panel(new Align(new Markup(msg), HorizontalAlignment.Center, VerticalAlignment.Top))
                        .Border(BoxBorder.Rounded)
                        .Expand());

                ctx?.Refresh();

                var tecla = Console.ReadKey(intercept: true);
                var key = tecla.Key;
                var c = tecla.KeyChar;

                if (key == ConsoleKey.Backspace && monedasStr.Length > 0)
                {
                    monedasStr = monedasStr[..^1];
                }
                else if (key == ConsoleKey.Enter) {
                    if (!string.IsNullOrEmpty(monedasStr) && int.Parse(monedasStr) > 0)
                    {
                        flag = true;
                        monedas += int.Parse(monedasStr);
                    }
                }
                else if (char.IsDigit(c))
                {
                    if (monedasStr.StartsWith('0'))
                    {
                        monedasStr = "";
                    }
                    monedasStr += c;
                    try
                    {
                        int temp = int.Parse(monedasStr);
                    }
                    catch
                    {
                        monedasStr = "2147483647";
                        monedas = int.MaxValue;
                    }
                }
            }

            jugador?.Saldo = monedas;
        }

        private static void ConfigurarPanelDatos(Maquina maquinaActual, Jugador jugadorActual)
        {
            string nombreJ = "", saldoJ = "0", nombreM = "", multiMon = "0", multiProb = "0";

            if (jugadorActual != null && 
                !string.IsNullOrEmpty(jugadorActual.Nombre))
            {
                nombreJ = jugadorActual.Nombre;
                if (jugadorActual.Saldo <= 0) saldoJ = "0 :warning: No puede jugar:warning:";
                else saldoJ = jugadorActual.Saldo.ToString();
            }

            if (maquinaActual != null &&
                !string.IsNullOrEmpty(maquinaActual.Nombre))
            {
                nombreM = maquinaActual.Nombre;
                multiMon = (maquinaActual.MultiplicadorPrem).ToString();
                multiProb = double.Round(maquinaActual.MultiplicadorProb * 100).ToString();
            }

            string? txt = $"""
                Jugador: {nombreJ}
                Saldo: {saldoJ}

                Máquina seleccionada: {nombreM}
                Multiplicador: x{multiMon}
                Suerte: {multiProb}%
                """;

            _view["Datos"].Update(
                new Panel(txt)
                    .Header("Estadísticas")
                    .Border(BoxBorder.Rounded)
                    .Expand());
        }

        private static void ConfigurarPanelDecisiones(string[] deciciones, int opcionSeleccionada)
        {
            // Crear el texto para el menú con las opciones anteriores
            var lineasMenu = new List<string>();
            for (int i = 0; i < deciciones.Length; i++)
            {
                if (i == opcionSeleccionada)
                {
                    // Si es la opción actual, se recalca con un fondo blanco y un >
                    lineasMenu.Add($"[black on white] > {deciciones[i]} [/]");
                }
                else
                {
                    // Si no, simplemente texto plano
                    lineasMenu.Add($"   {deciciones[i]}");
                }
            }
            string textoMenu = string.Join("\n", lineasMenu);

            _view["Decisiones"].Update(
                new Panel(textoMenu)
                    .Header("Acciones (Usa las flechas y ENTER)")
                    .Border(BoxBorder.Rounded)
                    .Expand());
        }

        private static void ConfigurarPanelMaquina()
        {
            _view["Maquina"].Update(
                new Panel(new Align(new Markup("Seleccione una acción\nO pulse 'Espacio' para tirar rápidamente"), HorizontalAlignment.Center, VerticalAlignment.Top))
                    .Border(BoxBorder.Rounded)
                    .Expand());
        }

        private static async Task TirarPalanca(bool girarLibre, Jugador jugador, Maquina maquinaSeleccionada)
        {
            if (!girarLibre)
            {
                return;
            }
            else if (jugador == null)
            {
                _ = ShowMessageFooter(":warning:  No hay un jugador registrado actualmente.");
                return;
            }
            else if (maquinaSeleccionada == null)
            {
                _ = ShowMessageFooter(":warning:  Debe seleccionar una máquina primero.");
                return;
            }

            girarLibre = false;
            _view["Decisiones"].Update(new Panel("[yellow]¡Girando rodillos!...[/]").Expand());
            ctx?.Refresh();
            await maquinaSeleccionada.Play(jugador);
        }
        #endregion

        #region Métodos de Utilidad
        public static async Task ShowMessageFooter(string? msg)
        {
            if (string.IsNullOrEmpty(msg)) throw new ArgumentNullException("El mensaje no puede ser nulo o estar vacío.");

            GirarLibre = false;

            for (byte i = 1; i <= 2; i++)
            {
                if (i == 2) msg = "Centro de mensajes.";

                _view["Pie"].Update(
                new Panel(new Align(new Markup(msg), HorizontalAlignment.Left))
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Gold1)
                    .Padding(2, 0, 2, 0)
                );

                ctx?.Refresh();

                // Solo ejecutar la parada en la primera vuelta (en la que muestra el mensaje enviado)
                if (i != 2) await Task.Delay(1000); // 1 segundos
            }

            GirarLibre = true;
        }
        #endregion
    }
}
