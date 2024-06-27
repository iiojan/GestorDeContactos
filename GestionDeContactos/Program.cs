using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GestionDeContactos
{
    internal class Program
    {
        //INTERFAZ PRINCIPAL ---------------------------------------------------------------
        static void Main()
        {
                Console.Clear();
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("              Gestor de Contactos              ");
                Console.WriteLine("-----------------------------------------------\n");
                Console.WriteLine("1. Agregar contacto\n");
                Console.WriteLine("2. Buscar contacto\n");
                Console.WriteLine("3. Eliminar contacto\n");
                Console.WriteLine("4. Listar contactos\n");
                Console.WriteLine("5. Guardar contactos\n");
                Console.WriteLine("6. Cargar contactos\n");
                Console.WriteLine("7. Salir\n");
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("Por favor seleccione una opción");

            var input = Console.ReadKey();

            switch (input.Key)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        ContactManager.addContact(); 
                    break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        ContactManager.searchContact();
                        break;
                    case ConsoleKey.D3:
                        Console.Clear();
                        ContactManager.removeContact();
                        break;
                    case ConsoleKey.D4:
                        Console.Clear();
                        ContactManager.listContact();
                        break;
                    case ConsoleKey.D5:
                        Console.Clear();
                        ContactManager.saveContacts();
                        break;
                    case ConsoleKey.D6:
                        Console.Clear();
                        ContactManager.loadContacts();
                        break;
                    case ConsoleKey.D7:
                        System.Environment.Exit(-1);
                        break;
                    default:
                        break;
                }


        }

        //CONTROLES PARA REGRESAR A INTERFAZ PRINCIPAL -------------------------------------
        public static void returnMainMenu()
        {
            Console.WriteLine("¿Desea volver al menú principal?\n");
            Console.WriteLine("1. Volver al menú principal");
            Console.WriteLine("2. Salir");
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Por favor seleccione una opción");

            switch (Console.ReadKey().Key) 
            {
                case ConsoleKey.D1: Main(); break;
                case ConsoleKey.D2: Environment.Exit(-1); break;
            }
        }
        
    }

    //CLASE EN DONDE SE DEFINE LA INFORMACIÓN DE LOS CONTACTOS -----------------------------//
    internal class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public override string ToString()
        {
            return $"Id: {Id}\n" +
                   $"Nombre: {Name}\n" +
                   $"Telefono: {Phone}\n" +
                   $"Correo Electronico: {Email}\n" +
                   $"-----------------------------------------------";
        }
    }

    //CLASE GESTORA DE CONTACTOS  ----------------------------------------------------------//
    internal class ContactManager
    {
        private static List<Contact> contacts = new List<Contact>();

        // Metodo para añadir contactos ----------------------------------------------------
        public static void addContact()
        {
                Console.WriteLine("Ingrese el ID del contacto:");
                string inputId = Console.ReadLine();
                Console.WriteLine("Ingrese el Nombre:");
                string inputName = Console.ReadLine();
                Console.WriteLine("Ingrese el Telefono:");
                string inputPhone = Console.ReadLine();
                Console.WriteLine("Ingrese el Correo Electronico:");
                string inputEmail = Console.ReadLine();

                if (inputId != null && inputName != null && inputPhone != null && inputEmail != null)
                {
                    Contact newContact = new Contact
                    {
                        Id = Convert.ToInt32(inputId), Name = inputName, Phone = inputPhone, Email = inputEmail
                    };
                contacts.Add(newContact);
                Console.WriteLine("-----------------------------------------------\n");
                Console.WriteLine("         Contacto creado exitosamente.         \n");
                Console.WriteLine("-----------------------------------------------");
                } 
                else
                {
                Console.WriteLine("\nDEBE LLENAR TODOS LOS DATOS ANTES DE GUARDAR!");
                }

                Program.returnMainMenu();
        }

        // Metodo para borrar contactos ----------------------------------------------------
        public static void removeContact()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("              Eliminar Contactos               ");
            Console.WriteLine("-----------------------------------------------\n");

            if (contacts.Count == 0)
            {
                Console.WriteLine("La lista de contactos está vacía");
                Program.returnMainMenu();
                return;
            }
            Console.WriteLine("Seleccione el ID del contacto que desea eliminar:");
            foreach (var contact in contacts)
            {
                Console.WriteLine($"ID: {contact.Id} - Nombre: {contact.Name} - Telefono: {contact.Phone} - Correo: {contact.Email}");
            }

            if (int.TryParse(Console.ReadLine(), out int eliminateId))
            {
                var contactToRemove = contacts.FirstOrDefault(c => c.Id == eliminateId);
                if (contactToRemove != null)
                {
                    contacts.Remove(contactToRemove);
                    Console.WriteLine($"El contacto con ID {eliminateId}, ha sido eliminado exitosamente.");
                }
                else
                {
                    Console.WriteLine($"No se encontró ningún contacto con el ID {eliminateId}.");
                }
            }
            else
            {
                Console.WriteLine("Por favor ingrese un número de ID válido.");
            }
            Program.returnMainMenu();
        }

        // Metodo para buscar contactos ----------------------------------------------------
        public static void searchContact()
        {
            List<Contact> filterResults = new List<Contact>();

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("               Filtrar Contactos               ");
            Console.WriteLine("-----------------------------------------------\n");

            if (contacts.Count == 0)
            {
                Console.WriteLine("No es posible realizar una busqueda si la lista de contactos está vacía.");
                Program.returnMainMenu();
                return;
            }

            Console.WriteLine("Por favor ingrese el nombre del contacto que desea buscar:");
            string searchByName = Console.ReadLine();

            foreach (var contact in contacts)
            {
                if (contact.Name.Contains(searchByName, StringComparison.OrdinalIgnoreCase))
                {
                    filterResults.Add(contact);
                }
            }
            if (filterResults.Count > 0)
            {
                Console.WriteLine($"Se han encontrado las siguientes coincidencias para '{searchByName}'");
                foreach (var result in filterResults)
                {
                    Console.WriteLine(result);
                }
            }
            else
            {
                Console.WriteLine($"No se han encontrado contactos con el nombre '{searchByName}'. Revisa que se encuentre bien escrito o prueba con otro nombre.");
            }

            Program.returnMainMenu();
        }

        // Metodo para listar contactos ----------------------------------------------------
        public static void listContact() 
        {

            if (contacts.Count > 0) 
            {
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("              Lista de Contactos               ");
                Console.WriteLine("-----------------------------------------------\n");
                foreach (var contact in contacts)
                {
                    Console.WriteLine(contact);
                }
                
            } 
            else
            {
                Console.WriteLine("No hay contactos guardados.");
            }

            Program.returnMainMenu();
        }

        // Metodo para guardar contactos ---------------------------------------------------
        public static void saveContacts() 
        {
            string file = "contacts.json";
            string JSON = JsonConvert.SerializeObject(contacts, Formatting.Indented);

            if (!contacts.Any())
            {
                Console.WriteLine("NO HAY CONTACTOS PARA ALMACENAR.");
                Program.returnMainMenu();
                return;
            }

            try
            {
                File.WriteAllText(file, JSON);
                Console.WriteLine($"Los contactos fueros guardados exitosamente en el archivo: {file}.");
            }
            catch (Exception err)
            {

                throw new ApplicationException($"[Error en la exportación de contactos]: {err.Message}");
            }

            Program.returnMainMenu();
        }

        // Metodo para cargar contactos ----------------------------------------------------
        public static void loadContacts()
        {
            string file = "contacts.json";

            if (!File.Exists(file))
            {
                Console.WriteLine("NO SE ENCONTRÓ EL ARCHIVO PARA CARGAR.");
                Program.returnMainMenu();
                return;
            }

            try
            {
                contacts = JsonConvert.DeserializeObject<List<Contact>>(File.ReadAllText(file));
                Console.WriteLine($"Los contactos del archivo {file}, fueros cargados exitosamente.");
            }
            catch (Exception err)
            {

                throw new ApplicationException($"[Error en la importación de contactos]: {err.Message}");
            }

            Program.returnMainMenu();
        }
    }
}
