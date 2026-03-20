using System.Diagnostics;
using System.Timers;

namespace Ejercicio4_linq_
{
    public enum Sexo
    {
        Mujer,
        Hombre
    }

    public enum Especialidad
    {
        MedicinaGeneral,
        Pediatria,
        Neurologia,
        Cardiologia,
        Oftalmologia
    }

    public class Paciente
    {
        public int? Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public Sexo? Sexo { get; set; }
        public double? Temperatura { get; set; }
        public byte? RitmoCardiaco { get; set; }
        public Especialidad? Especialidad { get; set; }
        public byte? Edad { get; set; }

        public Paciente() { }

        public Paciente(int? id, string? nombre, string? apellido, Sexo? sexo, double? temperatura, byte? ritmoCardiaco, Especialidad? especialidad, byte? edad)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
            Sexo = sexo;
            Temperatura = temperatura;
            RitmoCardiaco = ritmoCardiaco;
            Especialidad = especialidad;
            Edad = edad;
        }

        public override string ToString()
        {
            return $"Paciente: {Nombre} {Apellido}, {Edad} años, {Sexo}, {Temperatura}ºC, {RitmoCardiaco}PPM, de {Especialidad}";
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var pacientes = new List<Paciente>()
            {
                // Medicina General
                new(1, "Ana", "García", Sexo.Mujer, 36.5, 75, Especialidad.MedicinaGeneral, 35),
                new(2, "Luis", "Fernández", Sexo.Hombre, 37.2, 80, Especialidad.MedicinaGeneral, 42),
                new(3, "María", "López", Sexo.Mujer, 39.1, 95, Especialidad.MedicinaGeneral, 28), // Fiebre alta
                new(4, "Carlos", "Martínez", Sexo.Hombre, 36.8, 70, Especialidad.MedicinaGeneral, 50),
                new(5, "Lucía", "Gómez", Sexo.Mujer, 36.4, 65, Especialidad.MedicinaGeneral, 60),
                new(6, "David", "Gil", Sexo.Hombre, 39.8, 100, Especialidad.MedicinaGeneral, 33), // Fiebre muy alta
                new(7, "Oscar", "Lorenzo", Sexo.Hombre, 39.8, 120, Especialidad.MedicinaGeneral, 17), // Fiebre muy alta
                new(8, "Paca", "Terreno", Sexo.Mujer, 36.4, 72, Especialidad.MedicinaGeneral, 20),
                new(9, "Francisca", "Iznate", Sexo.Mujer, 37.1, 81, Especialidad.MedicinaGeneral, 78),

                // Pediatría
                new(10, "Hugo", "Sánchez", Sexo.Hombre, 38.5, 110, Especialidad.Pediatria, 5), // Fiebre
                new(11, "Valeria", "Díaz", Sexo.Mujer, 37.0, 90, Especialidad.Pediatria, 8),
                new(12, "Mateo", "Pérez", Sexo.Hombre, 36.6, 100, Especialidad.Pediatria, 2),
                new(13, "Martina", "Ruiz", Sexo.Mujer, 39.5, 120, Especialidad.Pediatria, 4), // Fiebre alta, taquicardia
                new(14, "Daniel", "Alonso", Sexo.Hombre, 36.8, 85, Especialidad.Pediatria, 12),
                new(15, "Pedro", "Sanz", Sexo.Hombre, 37.5, 85, Especialidad.Pediatria, 10),

                // Neurología
                new(16, "Carmen", "Torres", Sexo.Mujer, 36.7, 68, Especialidad.Neurologia, 75),
                new(17, "Jorge", "Blanco", Sexo.Hombre, 36.5, 72, Especialidad.Neurologia, 65),
                new(18, "Elena", "Navarro", Sexo.Mujer, 37.1, 78, Especialidad.Neurologia, 55),
                new(19, "Raúl", "Marín", Sexo.Hombre, 36.9, 60, Especialidad.Neurologia, 80),
                new(20, "Isabel", "Molina", Sexo.Mujer, 36.6, 64, Especialidad.Neurologia, 70),
                new(21, "Alba", "Vázquez", Sexo.Mujer, 36.4, 66, Especialidad.Neurologia, 48),

                // Cardiología
                new(22, "Antonio", "Ortiz", Sexo.Hombre, 36.8, 115, Especialidad.Cardiologia, 58), // Taquicardia
                new(23, "Laura", "Delgado", Sexo.Mujer, 37.0, 55, Especialidad.Cardiologia, 45), // Bradicardia
                new(24, "Manuel", "Castro", Sexo.Hombre, 36.5, 95, Especialidad.Cardiologia, 62),
                new(25, "Sofía", "Rubio", Sexo.Mujer, 38.0, 105, Especialidad.Cardiologia, 71), // Fiebre y ritmo alto
                new(26, "Javier", "Serrano", Sexo.Hombre, 36.7, 88, Especialidad.Cardiologia, 50),
                new(27, "Natalia", "Domínguez", Sexo.Mujer, 36.7, 75, Especialidad.Cardiologia, 29),

                // Oftalmología
                new(28, "Paula", "Iglesias", Sexo.Mujer, 36.6, 70, Especialidad.Oftalmologia, 30),
                new(29, "Diego", "Romero", Sexo.Hombre, 36.8, 74, Especialidad.Oftalmologia, 40),
                new(30, "Sara", "Suárez", Sexo.Mujer, 37.2, 76, Especialidad.Oftalmologia, 25),
                new(31, "Alejandro", "Gallo", Sexo.Hombre, 36.5, 68, Especialidad.Oftalmologia, 38),
                new(32, "Marta", "Ramos", Sexo.Mujer, 36.9, 72, Especialidad.Oftalmologia, 52),
                new(33, "Pablo", "Giménez", Sexo.Hombre, 37.8, 82, Especialidad.Oftalmologia, 66)
            };

            PediatriaDiezAnios(pacientes);
            ProtocoloUrgencia(pacientes);
            ReasignarGeneral(pacientes);
            ContarPorEspecialidad(pacientes);
            PrioridadPorFiebreOCardiaco(pacientes);
            EdadMediaPorEspecialidad(pacientes);
        }

        #region Métodos
        private static void PediatriaDiezAnios(List<Paciente> lista)
        {
            var l = lista.Where(x => x.Edad < 10 && x.Especialidad == Especialidad.Pediatria).ToList();
            Console.WriteLine(" - Pacientes pediatria menores de 10 años (exclusivo)");
            l.ForEach(p => Console.WriteLine(p));
        }

        private static void ProtocoloUrgencia(List<Paciente> lista)
        {
            bool activar = lista.Any(x => x.Temperatura > 39.0 || x.RitmoCardiaco > 100);
            if (activar) Console.WriteLine("\nATENCIÓN: Se ha activado el protocolo de urgencias.");
            else Console.WriteLine("\nEl protocolo de urgencias no se ha activado.");
        }

        private static void ReasignarGeneral(List<Paciente> lista)
        {
            var reasig = lista.Where(p => p.Especialidad == Especialidad.Pediatria).ToList();
            reasig.ForEach(p => p.Especialidad = Especialidad.MedicinaGeneral);
            Console.WriteLine("\n - Reasignados a general");
            reasig.ForEach(p => Console.WriteLine(p));
        }

        private static void ContarPorEspecialidad(List<Paciente> lista)
        {
            Console.WriteLine("\n - Contar por especialidad (nota: se ha reasignado antes, no queda nadie en pediatria)");
            lista.GroupBy(p => p.Especialidad).ToList().ForEach(g => Console.WriteLine($"{g.Key}: {g.Count()} pacientes"));
        }

        private static void PrioridadPorFiebreOCardiaco(List<Paciente> lista)
        {
            var l = lista.Where(p => p.Temperatura > 39.0 || p.RitmoCardiaco > 100).ToList();
            Console.WriteLine("\n - Prioridad por fiebre o ritmo cardiaco alto");
            l.ForEach(p => Console.WriteLine($"El paciente {p.Nombre} {p.Apellido} (id: {p.Id}) tiene prioridad."));
        }

        private static void EdadMediaPorEspecialidad(List<Paciente> lista)
        {
            Console.WriteLine("\n - Edad media por especialidad (no hay de pediatria porque se reasignaron a general)");
            lista.GroupBy(p => p.Especialidad).ToList()
                 .ForEach(g => Console.WriteLine($"{g.Key}: {g.Average(p => p.Edad):F2} años de media."));
            // :F2 es un límite de 2 decimales
        }
        #endregion
    }
}
