
using System.Collections.Generic;
using System;
using Parcial1.Users;
using Parcial1.Rooms;
using Parcial1.Reservations;
using Parcial1.TimeRepresentation;
using Parcial1.Validators;
using Parcial1.Rules;

namespace Parcial1.Facades
{
    public class Boundary
    {
        #region Properties
        public string Name {get; set;}
        public string Password {get; set;}
        public string Id { get; set; }
        public int Day { get; set; }
        public string Description { get; set; }
        public double Temp {get; set;}
        public int On {get; set;}
        public int Off {get; set;}
        public Classroom Room;
        public Time Start;
        public Time Finish;
        public Time Snap;
        public List<User> UserList;
        public List<Classroom> ClassroomList;
        public Validator IntValidate;
        public Validator StringValidate;
        public Validator DoubleValidate;
        public Validator DayValidate;
        public Validator TimeValidate;
        public Interactor Controller;
        #endregion

        #region Initialize
        public Boundary()
        {
            Room = new Classroom();
            Start = new Time();
            Finish = new Time();
            Snap = new Time();

            UserList = new List<User>();
            UserList.Add(new Admin("Leonardo", "admin"));
            UserList.Add(new Admin("Gustavo", "admin"));
            UserList.Add(new User("Miguel", "user"));
            UserList.Add(new User("Jhoan", "user"));

            ClassroomList = new List<Classroom>();
            ClassroomList.Add(new Classroom("101"));
            ClassroomList.Add(new Classroom("102"));
            ClassroomList.Add(new Classroom("103"));


            IntValidate = new Validator();
            IntValidate.RuleList.Add(new IntRule());

            StringValidate = new Validator();
            StringValidate.RuleList.Add(new StrRule());
            StringValidate.RuleList.Add(new LengthRule());

            DoubleValidate = new Validator();
            DoubleValidate.RuleList.Add(new DoubleRule());

            DayValidate = new Validator();
            DayValidate.RuleList.Add(new DayRule());

            TimeValidate = new Validator();
            TimeValidate.RuleList.Add(new TimeRule());

            Controller = new Interactor();
        }
        #endregion

        #region Methods
        public void Login()
        {
            string option;
            bool validate = false;
            bool result = false;
            int userIndex = 0;
            do
            {
                option = "0";
                Console.WriteLine("Bienvenido al sistema de reserva de salones");
                Console.WriteLine("Seleccione una opción:\n1. Ingresar\n0. Salir");
                Console.Write("> ");
                option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        Console.Write("Ingrese su usuario: ");
                        Name = Console.ReadLine();
                        StringValidate.Value = Name;
                        validate = StringValidate.ValidateField();
                        Console.Write("Ingrese su contraseña: ");
                        Password = Console.ReadLine();
                        StringValidate.Value = Password;
                        validate = validate && StringValidate.ValidateField();
                        UserList.ForEach(user =>
                        {
                            if(!(result))
                            {
                                result = user.Name == Name && user.Password == Password;
                                if(result)
                                {
                                    userIndex = UserList.IndexOf(user);
                                }
                            }
                        });
                        if(result)
                        {
                            LoggedMenu(userIndex);
                        }
                        break;
                        case "0":
                            break;
                    default:
                        Console.WriteLine("Opción invalida");
                        break;
                }
            } while (option != "0");
        }
        
        private void LoggedMenu(int userIndex)
        {
            string option = "0";
            bool check;
            do
            {
                Console.WriteLine("Bienvenido {0}, seleccione una opción:", Name);
                Console.WriteLine("1. Reservar");
                if(UserList[userIndex].GetType().Name == "Admin")
                {
                    Console.WriteLine("2. Editar parametros del aire acondicionado de un salón");
                    Console.WriteLine("3. Editar parametros de las luces de un salón");
                    Console.WriteLine("4. Editar el tiempo de apertura de un salon");
                    Console.WriteLine("5. Poner/Quitar en mantenimiento");
                }
                Console.WriteLine("6. Snapshot");
                Console.WriteLine("0. Cerrar sesión");
                Console.Write("> ");
                option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        check = ReservationData();
                        if(check)
                        {
                            Controller.Reserve(Room, Day, Start, Finish, 
                                                UserList[userIndex].Name, Description);
                            Console.WriteLine("Reservación exitosa");
                        }
                        break;
                        
                    case "2":
                        if(UserList[userIndex].GetType().Name == "Admin")
                        {
                            check = AirData();
                            if (check)
                            {
                                Controller.ModifyAir(Room, Temp, On, Off);
                                Console.WriteLine("Modificacion exitosa");
                            } 
                        }
                        break;

                    case "3":
                        if(UserList[userIndex].GetType().Name == "Admin")
                        {
                            check = LightData();
                            if (check)
                            {
                                Controller.ModifyLight(Room, On, Off);
                                Console.WriteLine("Modificacion exitosa");
                            }
                        }
                        break;

                    case "4":
                        if(UserList[userIndex].GetType().Name == "Admin")
                        {
                            check = OpenData();
                            if (check)
                            {
                                Controller.ModifyOpen(Room, On);
                                Console.WriteLine("Modificacion exitosa");
                            }
                        }
                        break;

                    case "5":
                        if(UserList[userIndex].GetType().Name == "Admin")
                        {
                            check = MaintenanceData();
                            if (check)
                            {
                                Controller.Maintenance(Room);
                                Console.WriteLine("Mantenimiento ordenado");
                            }
                        }
                        break;

                    case "6":
                        check = SnapshotData();
                        if (check)
                        {
                            Snapshot();
                        }
                        break;

                    case "0":
                        break;

                    default:
                        Console.WriteLine("Opcion invalida");
                        break;
                }

            } while (option != "0");
        }
        
        public bool ReservationData()
        {
            bool result;
            bool found = false;
            int newDay;
            int hour = 0;
            int minute = 0;
            Console.Write("Ingrese el día de su reserva: ");
            Int32.TryParse(Console.ReadLine(), out newDay);
            Day = newDay;
            DayValidate.Value = newDay;
            result = DayValidate.ValidateField();
            if(result)
            {
                PrintClassrooms();
                Console.Write("Ingresé el ID del salón a reservar: ");
                Id = Console.ReadLine();
                ClassroomList.ForEach(newRoom =>
                {
                    if(!(found))
                    {
                        found = newRoom.Id == Id;
                        if(found)
                        {
                            Room = newRoom;
                        }
                    }
                });

                if (found)
                {
                    if (Room.InMaintenance)
                    {
                        Console.WriteLine("Salon en mantenimiento");
                        result = false;
                    }
                    else
                    {
                        Console.WriteLine("Nota: se le pediran las horas y minutos por separado");
                        Console.WriteLine("Tiempo de inicio");
                        Console.Write("Hora: ");
                        Int32.TryParse(Console.ReadLine(), out hour);
                        Start.Hour = hour;
                        Console.Write("Minutos: ");
                        Int32.TryParse(Console.ReadLine(), out minute);
                        Start.Minute = minute;
                        TimeValidate.Value = Start;
                        result = result && TimeValidate.ValidateField();

                        Console.WriteLine("Tiempo de fin");
                        Console.Write("Hora: ");
                        Int32.TryParse(Console.ReadLine(), out hour);
                        Finish.Hour = hour;
                        Console.Write("Minutos: ");
                        Int32.TryParse(Console.ReadLine(), out minute);
                        Finish.Minute = minute;
                        TimeValidate.Value = Finish;
                        result = result && TimeValidate.ValidateField();

                        Console.Write("Descripción de la reserva (más de 3 caracteres): ");
                        Description = Console.ReadLine();
                        StringValidate.Value = Description;
                        result = result && StringValidate.ValidateField();

                        if (Start.Hour > Finish.Hour)
                        {
                            result = false;
                        }
                        else if (Start.Hour == Finish.Hour && Start.Minute >= Finish.Minute)
                        {
                            result = false;
                        }
                        Room.ReservationList.ForEach(reserve =>
                        {
                            if(TimeInRange(Start, reserve.Start, reserve.Finish) || 
                                TimeInRange(Finish, reserve.Start, reserve.Finish))
                            {
                                result = false;
                            }
                        });
                        if (!(result))
                        {
                            Console.WriteLine("Datos ingresados invalidos");
                        }
                    }
                    
                }
                else
                {
                    Console.WriteLine("Salón no encontrado");
                    result = false;
                }
            }
            else
            {
                Console.WriteLine("Día invalido");
            }
            return result;
            
        }
        
        public void PrintClassrooms()
        {
            int reserveCounter = 0;
            ClassroomList.ForEach(newRoom => 
            {
                Console.WriteLine("Salon: " + newRoom.Id);
                if(newRoom.ReservationList.Count > 0)
                {
                    
                    newRoom.ReservationList.ForEach(reserve => 
                    {
                        
                        if(reserve.Day == Day)
                        {
                            
                            Console.WriteLine("Reservado por: " + reserve.Name);
                            Console.WriteLine("Hora de inicio: " + PrintTime(reserve.Start));
                            Console.WriteLine("Hora de fin: " + PrintTime(reserve.Finish));
                            Console.WriteLine("Descripción: " + reserve.Description);
                            Console.WriteLine("==========================================");
                            reserveCounter++;
                        }
                        
                    });
                }
                if(reserveCounter == 0)
                {
                    Console.WriteLine("Libre todo el día");
                }
                reserveCounter = 0;
            });
        }
        
        public bool AirData()
        {
            bool result = true;

            Console.Write("Ingrese el ID del salon: ");
            Id = Console.ReadLine();
            bool found = false;
            ClassroomList.ForEach(newRoom =>
            {
                if (!(found))
                {
                    found = newRoom.Id == Id;
                    if (found)
                    {
                        Room = newRoom;
                    }
                }
            });

            if (!found)
            {
                Console.WriteLine("Salon no encontrado");
                result = false;
            }
            else
            {
                Console.Write("Ingrese la nueva temperatura: ");
                double newTemp;
                Double.TryParse(Console.ReadLine(), out newTemp);
                
                DoubleValidate.Value = newTemp;
                result = result && DoubleValidate.ValidateField();

                Temp = newTemp;

                Console.Write("Ingrese el tiempo para prender el aire: ");
                int newVal;
                Int32.TryParse(Console.ReadLine(), out newVal);

                IntValidate.Value = newVal;
                result = result && IntValidate.ValidateField();

                On = newVal;

                Console.Write("Ingrese el tiempo para apagar el aire: ");
                Int32.TryParse(Console.ReadLine(), out newVal);

                IntValidate.Value = newVal;
                result = result && IntValidate.ValidateField();

                Off = newVal;

                if (!result)
                {
                    Console.WriteLine("Datos invalidos");
                }
            }

            return result;
        }
        
        public bool LightData()
        {
            bool result = true;

            Console.Write("Ingrese el ID del salon: ");
            Id = Console.ReadLine();
            bool found = false;
            ClassroomList.ForEach(newRoom =>
            {
                if (!(found))
                {
                    found = newRoom.Id == Id;
                    if (found)
                    {
                        Room = newRoom;
                    }
                }
            });

            if (!found)
            {
                Console.WriteLine("Salon no encontrado");
                result = false;                
            }
            else
            {
                Console.Write("Ingrese el tiempo para prender las luces: ");
                int newVal;
                Int32.TryParse(Console.ReadLine(), out newVal);

                IntValidate.Value = newVal;
                result = result && IntValidate.ValidateField();

                On = newVal;

                Console.Write("Ingrese el tiempo para apagar las luces: ");
                Int32.TryParse(Console.ReadLine(), out newVal);

                IntValidate.Value = newVal;
                result = result && IntValidate.ValidateField();

                Off = newVal;

                if (!result)
                {
                    Console.WriteLine("Datos invalidos");
                }
            }

            return result;
        }

        public bool OpenData()
        {
            bool result = true;

            Console.Write("Ingrese el ID del salon: ");
            Id = Console.ReadLine();
            bool found = false;
            ClassroomList.ForEach(newRoom =>
            {
                if (!(found))
                {
                    found = newRoom.Id == Id;
                    if (found)
                    {
                        Room = newRoom;
                    }
                }
            });

            if (!found)
            {
                Console.WriteLine("Salon no encontrado");
                result = false;
            }
            else
            {
                Console.Write("Ingrese el tiempo para abrir el salon: ");
                int newVal;
                Int32.TryParse(Console.ReadLine(), out newVal);

                IntValidate.Value = newVal;
                result = result && IntValidate.ValidateField();

                On = newVal;

                if (!result)
                {
                    Console.WriteLine("Datos invalidos");
                }
            }

            return result;
        }
        
        public bool MaintenanceData()
        {
            bool result = true;

            Console.Write("Ingrese el ID del salon: ");
            Id = Console.ReadLine();
            bool found = false;
            ClassroomList.ForEach(newRoom =>
            {
                if (!(found))
                {
                    found = newRoom.Id == Id;
                    if (found)
                    {
                        Room = newRoom;
                    }
                }
            });

            if (!found)
            {
                Console.WriteLine("Salon no encontrado");
                result = false;
            }

            return result;
        }
        
        public bool SnapshotData()
        {   
            bool result = true;
            int hour;
            int minute;

            Console.Write("Ingrese el ID del salon: ");
            Id = Console.ReadLine();
            bool found = false;
            ClassroomList.ForEach(newRoom =>
            {
                if (!(found))
                {
                    found = newRoom.Id == Id;
                    if (found)
                    {
                        Room = newRoom;
                    }
                }
            });

            if (!found)
            {
                Console.WriteLine("Salon no encontrado");
                result = false;
            }
            else
            {
                Console.WriteLine("Nota: se le pediran las horas y minutos por separado");

                Console.Write("Dia: ");
                int newDay;
                Int32.TryParse(Console.ReadLine(), out newDay);
                Day = newDay;
                DayValidate.Value = Day;
                result = result && DayValidate.ValidateField();

                Console.WriteLine("Tiempo a verificar");
                Console.Write("Hora: ");
                Int32.TryParse(Console.ReadLine(), out hour);
                Snap.Hour = hour;
                Console.Write("Minutos: ");
                Int32.TryParse(Console.ReadLine(), out minute);
                Snap.Minute = minute;
                TimeValidate.Value = Snap;
                result = result && TimeValidate.ValidateField();

                if (!result)
                {
                    Console.WriteLine("Datos Invalidos");
                }
            }

            return result;
        }

        public bool TimeGreater(Time t1, Time t2)
        {
            if (t1.Hour > t2.Hour)
            {
                return true;
            }
            else if (t1.Hour == t2.Hour && t1.Minute > t2.Minute)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TimeInRange(Time toCheck, Time lower, Time upper)
        {
            return TimeGreater(toCheck, lower) && TimeGreater(upper, toCheck);
        }
        
        public void Snapshot()
        {
            Time switchOn = new Time();
            Time switchOff = new Time();

            Console.WriteLine("Salon: " + Room.Id);
            bool lights = false;
            bool air = false;
            bool open = false;
            bool inUse = false;
            Room.ReservationList.ForEach(reserve =>
            {
                if (reserve.Day == Day)
                {
                    switchOn.Hour = reserve.Start.Hour;
                    switchOn.Minute = reserve.Start.Minute - Room.OnLight;
                    
                    switchOff.Hour = reserve.Finish.Hour;
                    switchOff.Minute = reserve.Finish.Minute + Room.OffLight;

                    if (switchOn.Minute < 0)
                    {
                        switchOn.Hour--;
                        switchOn.Minute = switchOn.Minute - Room.OnLight + 60;
                    }
                    if (switchOff.Minute >= 60)
                    {
                        switchOn.Hour--;
                        switchOn.Minute = switchOn.Minute + Room.OffLight - 60;
                    }

                    if (TimeInRange(Snap, switchOn, switchOff))
                    {
                        lights = true;
                    }

                    switchOn.Hour = reserve.Start.Hour;
                    switchOn.Minute = reserve.Start.Minute - Room.OnAir;

                    switchOff.Hour = reserve.Finish.Hour;
                    switchOff.Minute = reserve.Finish.Minute + Room.OffAir;

                    if (switchOn.Minute < 0)
                    {
                        switchOn.Hour--;
                        switchOn.Minute = switchOn.Minute - Room.OnAir + 60;
                    }
                    if (switchOff.Minute >= 60)
                    {
                        switchOn.Hour--;
                        switchOn.Minute = switchOn.Minute + Room.OffAir - 60;
                    }

                    if (TimeInRange(Snap, switchOn, switchOff))
                    {
                        air = true;
                    }

                    switchOn.Hour = reserve.Start.Hour;
                    switchOn.Minute = reserve.Start.Minute - Room.Open;

                    if (switchOn.Minute < 0)
                    {
                        switchOn.Hour--;
                        switchOn.Minute = switchOn.Minute - Room.Open + 60;
                    }

                    if (TimeInRange(Snap, switchOn, reserve.Finish))
                    {
                        open = true;
                    }

                    if (TimeInRange(Snap, reserve.Start, reserve.Finish))
                    {
                        inUse = true;
                    }
                }
            });
            Console.WriteLine("Luces: " + (lights ? "prendidas" : "apagadas"));
            Console.WriteLine("Aire: " + (air ? "prendido" : "apagado"));
            Console.WriteLine("Temperatura: " + (air ? Convert.ToString(Room.AirTemp) : "ambiente"));
            Console.WriteLine("En mantenimiento: " + (Room.InMaintenance ? "si" : "no"));
            Console.WriteLine("Estado: " + (open ? "abierto" : "cerrado"));
            Console.WriteLine("Ocupado: " + (inUse ? "si" : "no"));
        }

        public string PrintTime(Time t)
        {
            string print = "";
            if(t.Hour < 10)
            {
                print += "0" + Convert.ToString(t.Hour);
            }
            else
            {
                print += Convert.ToString(t.Hour);
            }
            print += ":";
            if(t.Minute < 10)
            {
                print += "0" + Convert.ToString(t.Minute);
            }
            else
            {
                print += Convert.ToString(t.Minute);
            }
            return print;
        }
    }
        #endregion
}