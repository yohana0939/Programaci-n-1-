using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace GuarderiaMascotas
{
    //Student s name : Yohana Montero
    // Student ID: 2025-0939
    // Gestor de Guardería de Mascotas

    public abstract class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        protected Persona(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }
    }

    public class Cliente : Persona
    {
        public Cliente(int id, string nombre) : base(id, nombre) { }
        public override string ToString() => $"{Id} - {Nombre}";
    }

    public class Mascota
    {
        public int IdMascota { get; set; }
        public int IdCliente { get; set; }
        public string NombreMascota { get; set; }

        public override string ToString() => $"{IdMascota} - {NombreMascota}";
    }

    public class Servicio
    {
        public int IdServicio { get; set; }
        public string Nombre { get; set; }

        public override string ToString() => $"{IdServicio} - {Nombre}";
    }

    public class Reserva
    {
        public string Cliente { get; set; }
        public string Mascota { get; set; }
        public DateTime Fecha { get; set; }
        public string Servicio { get; set; }

        public override string ToString() =>
            $"{Cliente} | {Mascota} | {Fecha:yyyy-MM-dd} | {Servicio}";
    }

    // ===================== REPOSITORIOS =====================

    public class SqlClienteRepository
    {
        private readonly string cs;
        public SqlClienteRepository(string cs) => this.cs = cs;

        public void Add(string nombre)
        {
            using var con = new SqlConnection(cs);
            con.Open();
            var cmd = new SqlCommand(
                "INSERT INTO Clientes (NombreCliente) VALUES (@n)", con);
            cmd.Parameters.AddWithValue("@n", nombre);
            cmd.ExecuteNonQuery();
        }

        public List<Cliente> GetAll()
        {
            var lista = new List<Cliente>();
            using var con = new SqlConnection(cs);
            con.Open();

            var cmd = new SqlCommand(
                "SELECT IdCliente, NombreCliente FROM Clientes", con);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                lista.Add(new Cliente(rd.GetInt32(0), rd.GetString(1)));

            return lista;
        }
    }

    public class SqlMascotaRepository
    {
        private readonly string cs;
        public SqlMascotaRepository(string cs) => this.cs = cs;

        public void Add(int idCliente, string nombre)
        {
            using var con = new SqlConnection(cs);
            con.Open();

            var cmd = new SqlCommand(
                "INSERT INTO Mascotas (IdCliente, NombreMascota) VALUES (@c,@n)", con);

            cmd.Parameters.AddWithValue("@c", idCliente);
            cmd.Parameters.AddWithValue("@n", nombre);
            cmd.ExecuteNonQuery();
        }

        public List<Mascota> GetByCliente(int idCliente)
        {
            var lista = new List<Mascota>();
            using var con = new SqlConnection(cs);
            con.Open();

            var cmd = new SqlCommand(
                "SELECT IdMascota, NombreMascota FROM Mascotas WHERE IdCliente=@c", con);
            cmd.Parameters.AddWithValue("@c", idCliente);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                lista.Add(new Mascota
                {
                    IdMascota = rd.GetInt32(0),
                    NombreMascota = rd.GetString(1),
                    IdCliente = idCliente
                });

            return lista;
        }
    }

    public class SqlServicioRepository
    {
        private readonly string cs;
        public SqlServicioRepository(string cs) => this.cs = cs;

        public List<Servicio> GetAll()
        {
            var lista = new List<Servicio>();
            using var con = new SqlConnection(cs);
            con.Open();

            var cmd = new SqlCommand(
                "SELECT IdServicio, NombreServicios FROM Servicios", con);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                lista.Add(new Servicio
                {
                    IdServicio = rd.GetInt32(0),
                    Nombre = rd.GetString(1)
                });

            return lista;
        }

        public Servicio GetById(int id)
        {
            using var con = new SqlConnection(cs);
            con.Open();

            var cmd = new SqlCommand(
                "SELECT IdServicio, NombreServicios FROM Servicios WHERE IdServicio=@id", con);
            cmd.Parameters.AddWithValue("@id", id);

            using var rd = cmd.ExecuteReader();
            if (rd.Read())
                return new Servicio
                {
                    IdServicio = rd.GetInt32(0),
                    Nombre = rd.GetString(1)
                };

            return null;
        }
    }

    public class SqlReservaRepository
    {
        private readonly string cs;
        public SqlReservaRepository(string cs) => this.cs = cs;

        public void Add(int idCliente, int idMascota, DateTime fecha, int idServicio)
        {
            using var con = new SqlConnection(cs);
            con.Open();

            var cmd = new SqlCommand(
                @"INSERT INTO Reservas (IdCliente, IdMascota, Fecha, IdServicio)
                  VALUES (@c,@m,@f,@s)", con);

            cmd.Parameters.AddWithValue("@c", idCliente);
            cmd.Parameters.AddWithValue("@m", idMascota);
            cmd.Parameters.AddWithValue("@f", fecha);
            cmd.Parameters.AddWithValue("@s", idServicio);
            cmd.ExecuteNonQuery();
        }

        public List<Reserva> GetAll()
        {
            var lista = new List<Reserva>();
            using var con = new SqlConnection(cs);
            con.Open();

            var cmd = new SqlCommand(
                @"SELECT c.NombreCliente, m.NombreMascota, r.Fecha, s.NombreServicios
                  FROM Reservas r
                  JOIN Clientes c ON r.IdCliente = c.IdCliente
                  JOIN Mascotas m ON r.IdMascota = m.IdMascota
                  JOIN Servicios s ON r.IdServicio = s.IdServicio", con);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                lista.Add(new Reserva
                {
                    Cliente = rd.GetString(0),
                    Mascota = rd.GetString(1),
                    Fecha = rd.GetDateTime(2),
                    Servicio = rd.GetString(3)
                });

            return lista;
        }
    }

    // ===================== PROGRAMA =====================

    class Program
    {
        static string cs =
            @"Server=(localdb)\MSSQLLocalDB;Database=GuarderiaMascotasDB;Trusted_Connection=True;";

        static SqlClienteRepository clienteRepo = new(cs);
        static SqlMascotaRepository mascotaRepo = new(cs);
        static SqlReservaRepository reservaRepo = new(cs);
        static SqlServicioRepository servicioRepo = new(cs);

        static void Main()
        {
            Console.WriteLine("GESTOR DE GUARDERÍA DE MASCOTAS");

            while (true)
            {
                Console.WriteLine(
                    "\n1. Registrar cliente" +
                    "\n2. Ver clientes" +
                    "\n3. Registrar mascota" +
                    "\n4. Crear reserva" +
                    "\n5. Ver reservas" +
                    "\n6. Ver servicios" +
                    "\n7. Salir"
                );

                Console.Write("Opción: ");
                var op = Console.ReadLine();

                if (op == "1")
                {
                    Console.Write("Nombre cliente: ");
                    clienteRepo.Add(Console.ReadLine());
                }
                else if (op == "2")
                {
                    clienteRepo.GetAll().ForEach(Console.WriteLine);
                }
                else if (op == "3")
                {
                    Console.Write("Id Cliente: ");
                    int idCliente = int.Parse(Console.ReadLine());

                    Console.Write("Nombre mascota: ");
                    mascotaRepo.Add(idCliente, Console.ReadLine());
                }
                else if (op == "4")
                {
                    Console.Write("Id Cliente: ");
                    int idCliente = int.Parse(Console.ReadLine());

                    var mascotas = mascotaRepo.GetByCliente(idCliente);
                    mascotas.ForEach(Console.WriteLine);

                    Console.Write("Id Mascota: ");
                    int idMascota = int.Parse(Console.ReadLine());

                    Console.Write("Fecha (yyyy-MM-dd): ");
                    DateTime fecha = DateTime.ParseExact(
                        Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    var servicios = servicioRepo.GetAll();
                    servicios.ForEach(Console.WriteLine);

                    Console.Write("Id Servicio: ");
                    int idServicio = int.Parse(Console.ReadLine());

                    reservaRepo.Add(idCliente, idMascota, fecha, idServicio);
                    Console.WriteLine("Reserva creada correctamente.");
                }
                else if (op == "5")
                {
                    reservaRepo.GetAll().ForEach(Console.WriteLine);
                }
                else if (op == "6")
                {
                    servicioRepo.GetAll().ForEach(Console.WriteLine);
                }
                else if (op == "7")
                {
                    break;
                }
            }
        }
    }
}
