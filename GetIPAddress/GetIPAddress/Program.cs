using System;
using System.Net;
using System.Net.NetworkInformation;

namespace GetIPAddress
{
    class Program
    {
        static void Main(string[] args)
        {
            // Required Network Objects
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            Console.WriteLine("Interface information for {0}.{1}     \n",
                    computerProperties.HostName, computerProperties.DomainName);

            // If no installed network interfaces
            if (nics == null || nics.Length < 1)
            {
                Console.WriteLine("\tNo network interfaces found.");
                return;
            }

            // Interfaces found
            Console.WriteLine("\tNumber of interfaces .................... : {0}", nics.Length);
            // Iterate through interface collection
            foreach (NetworkInterface adapter in nics)
            {
                // get IP address of interface
                IPInterfaceProperties properties = adapter.GetIPProperties();

                Console.WriteLine("\n");

                // Don't want loopback (127.0.0.1) or anything that returns null
                if (adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback && adapter.GetPhysicalAddress().ToString() != null && string.IsNullOrWhiteSpace(adapter.GetPhysicalAddress().ToString()) == false && string.IsNullOrEmpty(adapter.GetPhysicalAddress().ToString()) == false)
                {
                    Console.Write("\tInterface type .......................... : ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(adapter.NetworkInterfaceType);
                    Console.ResetColor();
                    Console.Write("\tPhysical (MAC) Address .................. : ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(adapter.GetPhysicalAddress().ToString());
                    Console.ResetColor();

                    // IP Address
                    UnicastIPAddressInformationCollection uniCast = properties.UnicastAddresses;
                    if (uniCast != null)
                    {
                        foreach (UnicastIPAddressInformation uni in uniCast)
                        {
                            IPAddress address;
                            bool blIsIPV4 = false;
                            if (IPAddress.TryParse(uni.Address.ToString(), out address))
                            {
                                switch (address.AddressFamily)
                                {
                                    case System.Net.Sockets.AddressFamily.InterNetwork:
                                        // we have IPv4
                                        blIsIPV4 = true;
                                        break;
                                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                                        // we have IPv6
                                        blIsIPV4 = false;
                                        break;
                                    default:
                                        blIsIPV4 = true;
                                        break;
                                }
                            }
                            if (blIsIPV4 == true)
                            {
                                Console.Write("\tIP Address .............................. : ");
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine(uni.Address);
                                Console.ResetColor();
                            }
                        }
                    }

                }
            }
        }
    }
}
