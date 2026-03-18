using Spectre.Console;
using System.Reflection.Emit;
using System.Text;

namespace SlotMachine_Ejercicio3_
{
    internal class Program
    {
        #region Atributos
        private const int WIDTH = 100;

        // Máquinas
        private static Maquina[] maquinas =
        {
            new Maquina("👑 Los tres reyes 👑", EMaquina.TRI_SLOT),
            new Maquina("♠ Los cinco naipes ♠", EMaquina.FIVE_SLOT)
        };
        #endregion

        #region MAIN
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var view = new Layout("Base")
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

            // HEADER
            view["Cabecera"].Update(
                new Panel(new Align(new Markup("[bold gold1] 🎰 CASINO NUEVO MÁLAGA 🎰 [/]"), HorizontalAlignment.Center))
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Gold1)
                );

            // FOOTER
            view["Pie"].Update(
                new Panel(new Align(new Markup("Texto Prueba"), HorizontalAlignment.Left))
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Gold1)
                    .Padding(2, 0 ,2, 0)
                );

            view["Maquina"].Update(new Panel("").Expand());
            view["Datos"].Update(new Panel("").Expand());
            view["Decisiones"].Update(new Panel("").Expand());

            AnsiConsole.AlternateScreen(() =>
            {
                AnsiConsole.Live(view)
                .Start(ctx =>
                {
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
                    Maquina? maquinaSeleccionada = new("", EMaquina.TRI_SLOT);
                    Jugador? jugador = null;



                    // --------------------------------- //
                    //           BUCLE DE JUEGO          //
                    // --------------------------------- //
                    while (jugando)
                    {
                        // Actualizar los paneles
                        ConfigurarPanelDecisiones(view, opciones, opcionSeleccionada);
                        
                        ConfigurarPanelDatos(view, maquinaSeleccionada, jugador);

                        ConfigurarPanelMaquina(view);

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
                        else if (tecla == ConsoleKey.Enter)
                        {
                            // Evaluar qué opciés es la que está marcada cuando se pulsó enter
                            switch (opcionSeleccionada)
                            {
                                // Cambiar de máquina
                                case 0:
                                    // Guardar la máquina seleccionada con la que jugar
                                    maquinaSeleccionada = CambiarMaquina(view, ctx);
                                    break;

                                // Tirar de la palanca
                                case 1:
                                    view["Decisiones"].Update(new Panel("[yellow]¡Girando rodillos!...[/]").Expand());
                                    ctx.Refresh();
                                    Thread.Sleep(1000); // SIMULAR POR AHORA
                                    break;

                                // Añadir saldo
                                case 2:
                                    AnadirSaldo(view, ctx, jugador);
                                    break;

                                // Cambiar de jugador
                                case 3:
                                    jugador = CambiarJugador(view, ctx);
                                    break;

                                // Salir del juego
                                case 4:
                                    jugando = false;
                                    break;
                            }
                        }
                    }
                });
            });
        }
        #endregion



        #region Métodos
        private static Maquina? CambiarMaquina(Layout view, LiveDisplayContext ctx)
        {
            // Valor para saber cuál de las máquinas está seleccionada por el jugador
            int opcionSeleccionada = 0;
            bool flag = false;
            Maquina? maquinaSeleccionada = null;

            while (!flag)
            {
                // Texto con todas las máquinas
                var lineasMaquinas = new List<string>();
                lineasMaquinas.Add("[yellow]¿A qué máquina quieres jugar?[/]\n");
                for (int i = 0; i < maquinas.Length; i++)
                {
                    if (i == opcionSeleccionada)
                    {
                        // Si es la opción actual, se recalca con un fondo blanco y un >
                        lineasMaquinas.Add($"[black on white] > {maquinas[i]._Nombre} [/]");
                    }
                    else
                    {
                        // Si no, texto plano
                        lineasMaquinas.Add($"   {maquinas[i]._Nombre}");
                    }
                }

                string maquinasTxt = string.Join("\n", lineasMaquinas);

                // Cambiar el panel para mostrar las máquinas que se pueden seleccionar
                view["Maquina"].Update(
                    new Panel(new Align(new Markup(maquinasTxt), HorizontalAlignment.Center, VerticalAlignment.Top))
                        .Border(BoxBorder.Rounded)
                        .Expand());

                ctx.Refresh();

                var tecla = Console.ReadKey(intercept: true).Key;

                if (tecla == ConsoleKey.UpArrow)
                {
                    // Subir en el panel (las máquinas) sin pasar de 0
                    if (opcionSeleccionada > 0) opcionSeleccionada--;
                }
                else if (tecla == ConsoleKey.DownArrow)
                {
                    // Bajar en el panel, siempre dentro de las opciones de las maquinas
                    if (opcionSeleccionada < maquinas.Length - 1) opcionSeleccionada++;
                }
                else if (tecla == ConsoleKey.Enter)
                {
                    flag = true;
                    // Seleccionar la máquina actual
                    maquinaSeleccionada = maquinas[opcionSeleccionada];
                }
            }
            // Devolver la máquina seleccionada
            return maquinaSeleccionada;
        }

        private static Jugador? CambiarJugador(Layout view, LiveDisplayContext ctx)
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

                view["Maquina"].Update(
                    new Panel(new Align(new Markup(msg), HorizontalAlignment.Center, VerticalAlignment.Top))
                        .Border(BoxBorder.Rounded)
                        .Expand());

                ctx.Refresh();

                var tecla = Console.ReadKey(intercept: true);
                var key = tecla.Key;
                char c = tecla.KeyChar;

                if (key == ConsoleKey.Backspace && nombre.Length > 0 && saldoStr.Length > 0)
                {
                    if (!isNombre) nombre = nombre.Remove(nombre.Length - 1);
                    else
                    {
                        saldoStr = saldoStr.Remove(saldoStr.Length - 1);
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

        private static void AnadirSaldo(Layout view, LiveDisplayContext ctx, Jugador? jugador)
        {
            if (jugador == null)
            {
                ShowMessageFooter(view, ctx, ":warning:  No hay un jugador registrado actualmente.");
                return;
            }

            bool flag = false,
                 good = true;

            int monedas = jugador._Saldo;
            string monedasStr = "0";

            while (!flag)
            {
                string msg;
                if (good) msg = $"¿Cuánto dinero quiere meter en su cuenta?\nCantidad: {monedasStr} monedas";
                else msg = $"No es posible, pruebe con otra cantidad.\nCantidad: {monedasStr} monedas";

                view["Maquina"].Update(
                    new Panel(new Align(new Markup(msg), HorizontalAlignment.Center, VerticalAlignment.Top))
                        .Border(BoxBorder.Rounded)
                        .Expand());

                ctx.Refresh();

                var tecla = Console.ReadKey(intercept: true);
                var key = tecla.Key;
                var c = tecla.KeyChar;

                if (key == ConsoleKey.Backspace && monedasStr.Length > 0)
                {
                    monedasStr = monedasStr.Remove(monedasStr.Length -1);
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
                    if (monedasStr.IndexOf("0") == 0)
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

            jugador?._Saldo = monedas;
        }

        private static void ConfigurarPanelDatos(Layout view, Maquina maquinaActual, Jugador jugadorActual)
        {
            string nombreJ = "", saldoJ = "0", nombreM = "", slotsM = "0";

            if (jugadorActual != null && 
                !string.IsNullOrEmpty(jugadorActual._Nombre))
            {
                nombreJ = jugadorActual._Nombre;
                if (jugadorActual._Saldo <= 0) saldoJ = "0 :warning: No puede jugar:warning:";
                else saldoJ = jugadorActual._Saldo.ToString();
            }

            if (maquinaActual != null &&
                !string.IsNullOrEmpty(maquinaActual._Nombre))
            {
                nombreM = maquinaActual._Nombre;
                slotsM = ((byte)maquinaActual._Slots).ToString();
            }

            string? txt = $"""
                Jugador: {nombreJ}
                Saldo: {saldoJ}

                Máquina seleccionada: {nombreM}
                Multiplicador: x{slotsM}
                """;

            view["Datos"].Update(
                new Panel(txt)
                    .Header("Estadísticas")
                    .Border(BoxBorder.Rounded)
                    .Expand());
        }

        private static void ConfigurarPanelDecisiones(Layout view, string[] deciciones, int opcionSeleccionada)
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

            view["Decisiones"].Update(
                new Panel(textoMenu)
                    .Header("Acciones (Usa las flechas y ENTER)")
                    .Border(BoxBorder.Rounded)
                    .Expand());
        }

        private static void ConfigurarPanelMaquina(Layout view)
        {
            view["Maquina"].Update(
                new Panel(new Align(new Markup("Seleccione una acción"), HorizontalAlignment.Center, VerticalAlignment.Top))
                    .Border(BoxBorder.Rounded)
                    .Expand());
        }
        #endregion

        #region Métodos de Utilidad
        private static void ShowMessageFooter(Layout view, LiveDisplayContext ctx, string? msg)
        {
            if (string.IsNullOrEmpty(msg)) throw new ArgumentNullException("El mensaje no puede ser nulo o estar vacío.");

            view["Pie"].Update(
                new Panel(new Align(new Markup(msg), HorizontalAlignment.Left))
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Gold1)
                    .Padding(2, 0, 2, 0)
                );

            ctx.Refresh();
        }
        #endregion
    }
}
