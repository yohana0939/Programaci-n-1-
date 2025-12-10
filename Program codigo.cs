using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;


// PROYECTO FINAL - GESTOR DE GUARDERÍA DE MASCOTAS
//  Estudiante YOHANA MONTERO 2025-0939


namespace GuarderiaMascotas
{
    
    public abstract class Persona
    {
        public string Nombre { get; set; }

        protected Persona(string nombre)
        {
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
        }
    }

    public class Cliente : Persona
    {
        public string Mascota { get; set; }

        public Cliente(string nombre, string mascota) : base(nombre)
        {
            Mascota = mascota ?? throw new ArgumentNullException(nameof(mascota));
        }

        public override string ToString() => $"Cliente: {Nombre} | Mascota: {Mascota}";
    }

    
    public abstract class Servicio
    {
        public abstract string Nombre { get; }
        public abstract string Descripcion { get; }
    }

    public class Banio : Servicio
    {
        public override string Nombre => "Baño";
        public override string Descripcion => "Baño completo con limpieza y secado.";
    }

    public class Grooming : Servicio
    {
        public override string Nombre => "Grooming";
        public override string Descripcion => "Corte de pelo y arreglo estético.";
    }

    public class Paseo : Servicio
    {
        public override string Nombre => "Paseo";
        public override string Descripcion => "Paseo supervisado en parque.";
    }

    public class AlimentacionEspecial : Servicio
    {
        public override string Nombre => "Alimentación especial";
        public override string Descripcion => "Dieta especial indicada por el cliente.";
    }

    public class ServicioAdicional : Servicio
    {
        public override string Nombre => "Servicio adicional";
        public override string Descripcion => "Servicio personalizado según solicitud.";
    }

    
    public class Reserva
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string NombreCliente { get; set; }
        public string NombreMascota { get; set; }
        public DateTime Fecha { get; set; }
        public Servicio Servicio { get; set; }

        public Reserva(string cliente, string mascota, DateTime fecha, Servicio servicio)
        {
            NombreCliente = cliente;
            NombreMascota = mascota;
            Fecha = fecha;
            Servicio = servicio;
        }

        public override string ToString()
        {
            return $"[{Id}] Cliente: {NombreCliente} | Mascota: {NombreMascota} | Fecha: {Fecha:yyyy-MM-dd} | Servicio: {Servicio.Nombre}";
        }
    }


    public interface IClienteRepository
    {
        IEnumerable<Cliente> GetAll();
        Cliente FindByName(string nombre);
        void Add(Cliente cliente);
        void Remove(string nombre);
    }

    public interface IReservaRepository
    {
        IEnumerable<Reserva> GetAll();
        void Add(Reserva reserva);
        void RemoveByCliente(string cliente);
        IEnumerable<Reserva> GetByCliente(string cliente);
    }

    public class SqlClienteRepository : IClienteRepository
    {
        private readonly string _connectionString;

        public SqlClienteRepository(string cs)
        {
            _connectionString = cs;
        }

        public void Add(Cliente cliente)
        {
            using var con = new SqlConnection(_connectionString);
            con.Open();
            var cmd = new SqlCommand(
                "INSERT INTO Clientes (Nombre, Mascota) VALUES (@n, @m)", con);

            cmd.Parameters.AddWithValue("@n", cliente.Nombre);
            cmd.Parameters.AddWithValue("@m", cliente.Mascota);

            cmd.ExecuteNonQuery();
        }

        public IEnumerable<Cliente> GetAll()
        {
            var lista = new List<Cliente>();
            using var con = new SqlConnection(_connectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT Nombre, Mascota FROM Clientes", con);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                lista.Add(new Cliente(rd.GetString(0), rd.GetString(1)));
            }

            return lista;
        }

        public Cliente FindByName(string nombre)
        {
            using var con = new SqlConnection(_connectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT Nombre, Mascota FROM Clientes WHERE Nombre=@n", con);
            cmd.Parameters.AddWithValue("@n", nombre);

            using var rd = cmd.ExecuteReader();
            if (rd.Read())
                return new Cliente(rd.GetString(0), rd.GetString(1));

            return null;
        }

        public void Remove(string nombre)
        {
            using var con = new SqlConnection(_connectionString);
            con.Open();
            var cmd = new SqlCommand("DELETE FROM Clientes WHERE Nombre=@n", con);
            cmd.Parameters.AddWithValue("@n", nombre);
            cmd.ExecuteNonQuery();
        }
    }


    
    public class SqlReservaRepository : IReservaRepository
    {
        private readonly string _connectionString;

        public SqlReservaRepository(string cs)
        {
            _connectionString = cs;
        }

        public void Add(Reserva reserva)
        {
            using var con = new SqlConnection(_connectionString);
            con.Open();
            var cmd = new SqlCommand(
                "INSERT INTO Reservas (Id, NombreCliente, NombreMascota, Fecha, Servicio) VALUES (@i, @c, @m, @f, @s)", con);

            cmd.Parameters.AddWithValue("@i", reserva.Id);
            cmd.Parameters.AddWithValue("@c", reserva.NombreCliente);
            cmd.Parameters.AddWithValue("@m", reserva.NombreMascota);
            cmd.Parameters.AddWithValue("@f", reserva.Fecha);
            cmd.Parameters.AddWithValue("@s", reserva.Servicio.Nombre);

            cmd.ExecuteNonQuery();
        }

        public IEnumerable<Reserva> GetAll()
        {
            var lista = new List<Reserva>();

            using var con = new SqlConnection(_connectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT Id, NombreCliente, NombreMascota, Fecha, Servicio FROM Reservas ORDER BY Fecha", con);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                string servicio = rd.GetString(4);

                Servicio servObj = servicio switch
                {
                    "Baño" => new Banio(),
                    "Grooming" => new Grooming(),
                    "Paseo" => new Paseo(),
                    "Alimentación especial" => new AlimentacionEspecial(),
                    _ => new ServicioAdicional()
                };

                var r = new Reserva(rd.GetString(1), rd.GetString(2), rd.GetDateTime(3), servObj);
                r.Id = rd.GetGuid(0);
                lista.Add(r);
            }

            return lista;
        }

        public IEnumerable<Reserva> GetByCliente(string cliente)
        {
            var lista = new List<Reserva>();

            using var con = new SqlConnection(_connectionString);
            con.Open();
            var cmd = new SqlCommand(
                "SELECT Id, NombreCliente, NombreMascota, Fecha, Servicio FROM Reservas WHERE NombreCliente=@c", con);

            cmd.Parameters.AddWithValue("@c", cliente);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                string servicio = rd.GetString(4);

                Servicio servObj = servicio switch
                {
                    "Baño" => new Banio(),
                    "Grooming" => new Grooming(),
                    "Paseo" => new Paseo(),
                    "Alimentación especial" => new AlimentacionEspecial(),
                    _ => new ServicioAdicional()
                };

                var r = new Reserva(rd.GetString(1), rd.GetString(2), rd.GetDateTime(3), servObj);
                r.Id = rd.GetGuid(0);
                lista.Add(r);
            }

            return lista;
        }

        public void RemoveByCliente(string cliente)
        {
            using var con = new SqlConnection(_connectionString);
            con.Open();
            var cmd = new SqlCommand("DELETE FROM Reservas WHERE NombreCliente=@c", con);
            cmd.Parameters.AddWithValue("@c", cliente);
            cmd.ExecuteNonQuery();
        }
    }


    public class ClienteService
    {
        private readonly IClienteRepository _repo;

        public ClienteService(IClienteRepository repo)
        {
            _repo = repo;
        }

        public void RegistrarCliente(string nombre, string mascota)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del cliente es obligatorio.");
            if (string.IsNullOrWhiteSpace(mascota))
                throw new ArgumentException("El nombre de la mascota es obligatorio.");

            var cliente = new Cliente(nombre.Trim(), mascota.Trim());
            _repo.Add(cliente);
        }

        public IEnumerable<Cliente> ObtenerClientes() => _repo.GetAll();
        public Cliente BuscarPorNombre(string nombre) => _repo.FindByName(nombre);
        public void EliminarCliente(string nombre) => _repo.Remove(nombre);
    }


    public class ReservaService
    {
        private readonly IReservaRepository _repo;
        private readonly IClienteRepository _clienteRepo;

        public ReservaService(IReservaRepository repo, IClienteRepository clienteRepo)
        {
            _repo = repo;
            _clienteRepo = clienteRepo;
        }

        public Reserva CrearReserva(string nombreCliente, DateTime fecha, Servicio servicio)
        {
            var cliente = _clienteRepo.FindByName(nombreCliente);

            if (cliente == null)
                throw new InvalidOperationException("Cliente no encontrado.");

            if (fecha.Date < DateTime.Today)
                throw new ArgumentException("La fecha no puede ser en el pasado.");

            var reserva = new Reserva(cliente.Nombre, cliente.Mascota, fecha, servicio);
            _repo.Add(reserva);

            return reserva;
        }

        public IEnumerable<Reserva> ObtenerReservas() => _repo.GetAll();
        public IEnumerable<Reserva> ObtenerReservasPorCliente(string nombreCliente) => _repo.GetByCliente(nombreCliente);
    }

    class Program
    {
        static string connectionString = "YOUR_CONNECTION_STRING_HERE";

        static IClienteRepository clienteRepo = new SqlClienteRepository(connectionString);
        static IReservaRepository reservaRepo = new SqlReservaRepository(connectionString);

        static ClienteService clienteService = new ClienteService(clienteRepo);
        static ReservaService reservaService = new ReservaService(reservaRepo, clienteRepo);

        static void Main(string[] args)
        {
            Console.WriteLine("=== GESTOR DE GUARDERÍA DE MASCOTAS ===");

            bool salir = false;
            while (!salir)
            {
                try
                {
                    MostrarMenu();
                    var entrada = Console.ReadLine();

                    if (!int.TryParse(entrada, out int opcion))
                    {
                        Console.WriteLine("Seleccione una opción válida.");
                        continue;
                    }

                    switch (opcion)
                    {
                        case 1: RegistrarCliente(); break;
                        case 2: MostrarClientes(); break;
                        case 3: CrearReserva(); break;
                        case 4: MostrarReservas(); break;
                        case 5: EliminarCliente(); break;
                        case 6:
                            salir = true;
                            Console.WriteLine("Saliendo del sistema...");
                            break;

                        default:
                            Console.WriteLine("Opción inválida.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void MostrarMenu()
        {
            Console.WriteLine();
            Console.WriteLine("1. Registrar cliente");
            Console.WriteLine("2. Ver clientes");
            Console.WriteLine("3. Crear reserva");
            Console.WriteLine("4. Ver reservas");
            Console.WriteLine("5. Eliminar cliente");
            Console.WriteLine("6. Salir");
            Console.Write("Seleccione: ");
        }

        static void RegistrarCliente()
        {
            Console.Write("Nombre del cliente: ");
            string nombre = Console.ReadLine();

            Console.Write("Nombre de la mascota: ");
            string mascota = Console.ReadLine();

            clienteService.RegistrarCliente(nombre, mascota);
            Console.WriteLine("Cliente registrado.");
        }

        static void MostrarClientes()
        {
            var clientes = clienteService.ObtenerClientes().ToList();

            if (!clientes.Any())
            {
                Console.WriteLine("No hay clientes.");
                return;
            }

            foreach (var c in clientes)
                Console.WriteLine(c);
        }

        static void CrearReserva()
        {
            Console.Write("Nombre del cliente: ");
            string nombre = Console.ReadLine().Trim();

            Console.Write("Fecha (YYYY-MM-DD): ");
            string fechaTxt = Console.ReadLine();

            if (!DateTime.TryParseExact(fechaTxt, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
            {
                Console.WriteLine("Fecha inválida.");
                return;
            }

            Console.WriteLine("Servicios:");
            Console.WriteLine("1. Baño");
            Console.WriteLine("2. Grooming");
            Console.WriteLine("3. Paseo");
            Console.WriteLine("4. Alimentación especial");
            Console.WriteLine("5. Servicio adicional");
            Console.Write("Seleccione: ");

            int.TryParse(Console.ReadLine(), out int op);

            Servicio servicio = op switch
            {
                1 => new Banio(),
                2 => new Grooming(),
                3 => new Paseo(),
                4 => new AlimentacionEspecial(),
                5 => new ServicioAdicional(),
                _ => null
            };

            if (servicio == null)
            {
                Console.WriteLine("Servicio inválido.");
                return;
            }

            var r = reservaService.CrearReserva(nombre, fecha, servicio);
            Console.WriteLine("Reserva creada:");
            Console.WriteLine(r);
        }

        static void MostrarReservas()
        {
            var reservas = reservaService.ObtenerReservas().ToList();

            if (!reservas.Any())
            {
                Console.WriteLine("No hay reservas.");
                return;
            }

            foreach (var r in reservas)
                Console.WriteLine(r);
        }

        static void EliminarCliente()
        {
            Console.Write("Nombre del cliente: ");
            string nombre = Console.ReadLine();

            var reservas = reservaService.ObtenerReservasPorCliente(nombre).ToList();

            if (reservas.Any())
            {
                Console.WriteLine($"El cliente tiene {reservas.Count} reservas.");
                Console.Write("¿Eliminar también las reservas? (s/n): ");
                string resp = Console.ReadLine().ToLower();

                if (resp != "s" && resp != "si")
                {
                    Console.WriteLine("No se puede eliminar el cliente sin borrar sus reservas.");
                    return;
                }

                reservaRepo.RemoveByCliente(nombre);
            }

            clienteService.EliminarCliente(nombre);
            Console.WriteLine("Cliente eliminado.");
        }
    }
}